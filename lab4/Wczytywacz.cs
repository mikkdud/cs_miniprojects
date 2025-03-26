using System;
using System.Collections.Generic;
using System.IO;

public class Wczytywacz<T>
{
    public List<T> WczytajListe(string path, Func<string[], T> generuj)
    {
        var lista = new List<T>();
        foreach (var linia in File.ReadLines(path))
        {
            if (string.IsNullOrWhiteSpace(linia)) continue;
            string[] pola = linia.Split(',');
            lista.Add(generuj(pola));
        }
        return lista;
    }
}
