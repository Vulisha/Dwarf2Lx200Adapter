using System.Net;
using System.Net.Sockets;
using System.Text;
public class LX200Server
{
    private readonly IPAddress _ipAddress;
    private readonly int _port;
    private readonly TelescopeController _telescopeController;

    public LX200Server(int port)
    {
        _ipAddress = IPAddress.Any;
        _port = port;
        _telescopeController = TelescopeController.Instance;
    }

    public async Task StartAsync()
    {
        var listener = new TcpListener(_ipAddress, _port);
        listener.Start();

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        using (var stream = client.GetStream())
        using (var reader = new StreamReader(stream, Encoding.ASCII))
        using (var writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true })
        {
            var buffer = new char[1];
            var command = new StringBuilder();

            while (await reader.ReadAsync(buffer, 0, 1) > 0)
            {
                char receivedChar = buffer[0];

                if (receivedChar == '#')
                {
                    // Command terminator received, process the command.
                    string response = await _telescopeController.HandleCommand(command.ToString());
                    if(command.ToString() != ":GR" && command.ToString() != ":GD")
                    Console.WriteLine(command.ToString());

                    if (!string.IsNullOrEmpty(response))
                    {
                        await writer.WriteAsync(response);
                    }


                    command.Clear();
                }
                else
                {
                    command.Append(receivedChar);
                }
            }
        }
    }

}
