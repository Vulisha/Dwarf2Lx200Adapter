using Dwarf2Lx200Adapter;


ApplicationConfiguration.IPAddress = args.Length > 0 ? args[0] : "127.0.0.1"; // Default to "127.0.0.1" if no IP address is provided
string comPortName = args.Length > 1 ? args[1] : null; // COM port name is the second argument, if provided

var lx200Server = new LX200Server(9999);

var telescopeController = TelescopeController.Instance;
telescopeController.CoordinatesReceived += async (sender, coordinates) =>
{
    Console.WriteLine($"RA: {coordinates.RA}, DEC: {coordinates.DEC}");
    //await restClient.SendRaDecAsync(coordinates.RA, coordinates.DEC);
};

SerialPortListener serialPortListener = null;
if (comPortName != null)
{
    serialPortListener = new SerialPortListener(comPortName, telescopeController);
    serialPortListener.StartAsync();
}
var stellariumRemoteServer = new StellariumRemoteServer(telescopeController, 10015);
Task lx200ServerTask = lx200Server.StartAsync();
Task rts2ServerTask = stellariumRemoteServer.StartAsync();
Console.WriteLine("Hello, Dwarf2!");
// Wait for both tasks to complete.
await Task.WhenAll(lx200ServerTask, rts2ServerTask);
await stellariumRemoteServer.StopAsync();

Console.WriteLine("Bye, Dwarf2!");
