// See https://aka.ms/new-console-template for more information



using SignHelper;

bool exit = false;
Console.WriteLine("SignService Opened.");

SignHelper.HelperService service = new SignHelper.HelperService();
MySQLAPI.Init();
service.Init();
while (!exit)
{
    Thread.Sleep(1);
}
