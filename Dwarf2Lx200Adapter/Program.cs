using Dwarf2Lx200Adapter;

Console.WriteLine("Hello, World!");

var lx200Server = new LX200Server("127.0.0.1", 9999);


var telescopeController = TelescopeController.Instance;
telescopeController.CoordinatesReceived += async (sender, coordinates) =>
{
    Console.WriteLine($"RA: {coordinates.RA}, DEC: {coordinates.DEC}");
    //await restClient.SendRaDecAsync(coordinates.RA, coordinates.DEC);
};

// Simulate periodic updates to the RA and DEC values.



await lx200Server.StartAsync();
