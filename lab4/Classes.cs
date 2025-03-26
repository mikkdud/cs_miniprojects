public class Region
{
    public string RegionID;
    public string RegionDescription;

    public Region(string regionID, string regionDescription)
    {
        RegionID = regionID;
        RegionDescription = regionDescription;
    }

    public override string ToString() => RegionDescription;
}


public class Territory
{
    public string TerritoryID;
    public string TerritoryDescription;
    public string RegionID;

    public Territory(string territoryID, string territoryDescription, string regionID)
    {
        TerritoryID = territoryID;
        TerritoryDescription = territoryDescription;
        RegionID = regionID;
    }

    public override string ToString() => TerritoryDescription;
}


public class EmployeeTerritory
{
    public string EmployeeID;
    public string TerritoryID;

    public EmployeeTerritory(string employeeID, string territoryID)
    {
        EmployeeID = employeeID;
        TerritoryID = territoryID;
    }

    public override string ToString() => $"Employee {EmployeeID}, Territory {TerritoryID}";
}


public class Employee
{
    public string EmployeeID;
    public string LastName;
    public string FirstName;

    public Employee(string employeeID, string lastName, string firstName)
    {
        EmployeeID = employeeID;
        LastName = lastName;
        FirstName = firstName;
    }

    public override string ToString() => $"{FirstName} {LastName}";
}