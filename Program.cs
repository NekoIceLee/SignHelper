// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

if (File.Exists("prefixes.txt") == false)
{
    File.Create("prefixes.txt").Close();
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