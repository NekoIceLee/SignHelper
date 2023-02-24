// See https://aka.ms/new-console-template for more information


using Microsoft.Data.Sqlite;

Console.WriteLine("Hello, World!");

if (File.Exists("data.db") == false)
{
    using (var connection = new SqliteConnection("Data Source=data.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText =
            @"
            CREATE TABLE userLog(
                id TEXT NOT NULL,
                time TEXT NOT NULL
            );
            ";
        command.ExecuteNonQuery();
    }
}


SignHelper.HelperService service = new SignHelper.HelperService(getPrefix().ToArray());
service.Init();
while (true)
{

}

IEnumerable<string> getPrefix()
{
    var f = new StreamReader(File.OpenRead("prefixes.txt"));
    var line = f.ReadLine();
    while (line is not null)
    {
        yield return line;
        line = f.ReadLine();
    }
}