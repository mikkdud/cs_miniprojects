using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;

class CsvLoader
{
    // Zadanie 1: Wczytanie danych z pliku CSV
    public static (List<string> headers, List<List<string?>> rows) LoadCsv(string filePath, char separator)
    {
        var headers = new List<string>();
        var rows = new List<List<string?>>();

        using (var reader = new StreamReader(filePath))
        {
            if (!reader.EndOfStream)
            {
                string? headerLine = reader.ReadLine();
                if (headerLine != null)
                {
                    headers = headerLine.Split(separator).ToList();
                }
            }

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line != null)
                {
                    var values = line.Split(separator);
                    var row = new List<string?>();
                    foreach (var value in values)
                    {
                        row.Add(string.IsNullOrWhiteSpace(value) ? null : value);
                    }
                    rows.Add(row);
                }
            }
        }

        return (headers, rows);
    }

    // Zadanie 2: Analiza typów kolumn i nullowalności
    public static List<(string columnName, string columnType, bool allowsNull)> AnalyzeColumns(List<string> headers, List<List<string?>> rows)
    {
        var result = new List<(string, string, bool)>();

        for (int col = 0; col < headers.Count; col++)
        {
            bool allowsNull = false;
            bool allInt = true;
            bool allDouble = true;

            foreach (var row in rows)
            {
                if (col >= row.Count || row[col] == null)
                {
                    allowsNull = true;
                    continue;
                }

                string value = row[col]!;
                if (!int.TryParse(value, out _)) allInt = false;
                if (!double.TryParse(value, out _)) allDouble = false;
            }

            string type;
            if (allInt)
                type = "INTEGER";
            else if (allDouble)
                type = "REAL";
            else
                type = "TEXT";

            result.Add((headers[col], type, allowsNull));
        }

        return result;
    }

    // Zadanie 3: Tworzenie tabeli w SQLite
    public static void CreateTableFromMetadata(SqliteConnection connection, string tableName, List<(string columnName, string columnType, bool allowsNull)> columns)
    {
        var columnDefs = columns.Select(col =>
            $"\"{col.columnName}\" {col.columnType} {(col.allowsNull ? "" : "NOT NULL")}"
        );

        var createTableSql = $"CREATE TABLE IF NOT EXISTS \"{tableName}\" (\n" +
                             string.Join(",\n", columnDefs) +
                             "\n);";

        using var command = connection.CreateCommand();
        command.CommandText = createTableSql;
        command.ExecuteNonQuery();

        Console.WriteLine($"Tabela \"{tableName}\" została utworzona.\n");
    }

    // Zadanie 4: Wstawianie danych do tabeli
    public static void InsertData(SqliteConnection connection, string tableName, List<string> headers, List<List<string?>> rows)
    {
        foreach (var row in rows)
        {
            var columnList = string.Join(", ", headers.Select(h => $"\"{h}\""));
            var parameterList = string.Join(", ", headers.Select((h, i) => $"@p{i}"));

            var cmdText = $"INSERT INTO \"{tableName}\" ({columnList}) VALUES ({parameterList})";

            using var cmd = connection.CreateCommand();
            cmd.CommandText = cmdText;

            for (int i = 0; i < headers.Count; i++)
            {
                var value = (i < row.Count) ? row[i] : null;
                cmd.Parameters.AddWithValue($"@p{i}", (object?)value ?? DBNull.Value);
            }

            cmd.ExecuteNonQuery();
        }

        Console.WriteLine($"Dane zostały wstawione do tabeli \"{tableName}\".\n");
    }

    // Zadanie 5: Wyświetlenie danych z tabeli
    public static void PrintTable(SqliteConnection connection, string tableName)
    {
        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT * FROM \"{tableName}\"";

        using var reader = cmd.ExecuteReader();

        // Nagłówki
        for (int i = 0; i < reader.FieldCount; i++)
        {
            Console.Write(reader.GetName(i) + "\t");
        }
        Console.WriteLine();

        // Dane
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var val = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString();
                Console.Write(val + "\t");
            }
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string path = "dane.csv";
        char separator = ',';
        string tableName = "CsvTable";

        if (!File.Exists(path))
        {
            Console.WriteLine($"Plik \"{path}\" nie istnieje.");
            return;
        }

        Console.WriteLine("========== ZADANIE 1: Wczytywanie danych z pliku ==========");
        var (headers, data) = CsvLoader.LoadCsv(path, separator);

        Console.WriteLine("Kolumny:");
        Console.WriteLine(string.Join(" | ", headers));

        Console.WriteLine("\nDane:");
        foreach (var row in data)
        {
            Console.WriteLine(string.Join(" | ", row.Select(v => v ?? "NULL")));
        }

        Console.WriteLine("\n========== ZADANIE 2: Analiza kolumn ==========");
        var analysis = CsvLoader.AnalyzeColumns(headers, data);
        foreach (var col in analysis)
        {
            Console.WriteLine($"{col.columnName} — {col.columnType}, NULL: {(col.allowsNull ? "TAK" : "NIE")}");
        }

        Console.WriteLine("\n========== ZADANIE 3: Tworzenie tabeli w SQLite ==========");
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "baza.db" };

        using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
        connection.Open();

        CsvLoader.CreateTableFromMetadata(connection, tableName, analysis);

        Console.WriteLine("========== ZADANIE 4: Wstawianie danych ==========");
        CsvLoader.InsertData(connection, tableName, headers, data);

        Console.WriteLine("========== ZADANIE 5: Wyświetlanie danych z bazy ==========");
        CsvLoader.PrintTable(connection, tableName);
    }
}
