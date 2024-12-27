namespace YazilimAcademy.ABPRaffleApp.GoogleDrive;

public sealed record SpreadsheetDto
{
    public string Id { get; set; }
    public string Name { get; set; }

    public SpreadsheetDto(string id, string name)
    {
        Id = id;
        Name = name;
    }
}