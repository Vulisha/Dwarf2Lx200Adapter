using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class StellariumRemoteServer
{
    private TcpListener _listener;
    private TelescopeController _controller;

    public StellariumRemoteServer(TelescopeController controller, int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _controller = controller;
    }

    public async Task StartAsync()
    {
        _listener.Start();

        while (true)
        {
            TcpClient client = await _listener.AcceptTcpClientAsync();
            _ = Task.Run(() => HandleClientAsync(client));
        }
    }

    public async Task StopAsync()
    {
        _listener.Stop();
    }

    public async Task HandleClientAsync(TcpClient client)
    {
        using (NetworkStream stream = client.GetStream())
        {
            //int messageTyoe = await MessageHelper.ReadMessageTypeAsync(stream);
            int messageLength = await MessageHelper.ReadMessageLenghtAsync(stream);
            byte[] fullMessage = await ReadFullMessageAsync(stream, messageLength);
            uint ra = BitConverter.ToUInt32(fullMessage, 10);
            int dec = BitConverter.ToInt32(fullMessage, 14);

            // Convert the values to LX200 format and send the command
            var raInHours = MessageHelper.StellariumToHours(ra);
            var decInDegrees = MessageHelper.StellariumToDegrees(dec);  

            await _controller.HandleCommand(MessageHelper.ConvertRaToLX200Format(raInHours));
            await _controller.HandleCommand(MessageHelper.ConvertDecToLX200Format(decInDegrees));
            await _controller.HandleCommand($":MS#");

       }
    }
    public async Task<byte[]> ReadFullMessageAsync(NetworkStream stream, int length)
    {
        byte[] buffer = new byte[length];
        int bytesRead = 0;
        while (bytesRead < length)
        {
            bytesRead += await stream.ReadAsync(buffer, bytesRead, length - bytesRead);
        }
        return buffer;
    }


    public async Task SendCurrentPositionAsync(TcpClient client, double raInHours, double decInDegrees)
    {
        var stream = client.GetStream();

        // Convert RA and Dec to Stellarium format
        var ra = (uint)(raInHours / 24 * uint.MaxValue);
        var dec = (int)(decInDegrees / 90 * int.MaxValue);

        // Send the message
        await MessageHelper.WriteMessageTypeAsync(stream, 0);
        await MessageHelper.WriteUnsignedIntAsync(stream, ra);
        await MessageHelper.WriteIntAsync(stream, dec);
        await MessageHelper.WriteIntAsync(stream, 0); // status
    }
}





//using System.Net.Sockets;
//using System.Net;
//using System.Text;

//public class StellariumRemoteServer
//{
//    private readonly TcpListener _listener;
//    private readonly TelescopeController _telescopeController;

//    public StellariumRemoteServer( TelescopeController controller, int port)
//    {
//        _listener = new TcpListener(IPAddress.Any, port);
//        _telescopeController = controller;
//    }


//    public async Task StartAsync()
//    {

//        _listener.Start();
//        Console.WriteLine("Stellarium Server started...");

//        while (true)
//        {
//            try
//            {
//                TcpClient client = await _listener.AcceptTcpClientAsync();
//                Console.WriteLine("Accepted connection request from: " + client.Client.RemoteEndPoint);

//                // Process the client in a separate task
//                Task.Run(() => HandleClientAsync(client));
//            }
//            catch (Exception ex)
//            {
//                // Log any exceptions
//                Console.WriteLine("Exception occurred: " + ex.ToString());
//            }
//        }
//    }
//    public async Task StopAsync()
//    {
//        _listener.Stop();
//    }
//        private async Task HandleClientAsync(TcpClient client)
//    {
//        using (client)
//        {
//            Console.WriteLine("Processing request...");
//            NetworkStream stream = client.GetStream();
//            StreamReader reader = new StreamReader(stream, System.Text.Encoding.ASCII);

//            try
//            {
//                reader.BaseStream.ReadTimeout = 5000; // Set a timeout of 5 seconds

//                while (true)
//                {
//                    string line = await reader.ReadLineAsync();
//                    Console.WriteLine("Received: " + line);

//                    string response = _telescopeController.HandleCommand(line).Result;

//                    byte[] data = Encoding.ASCII.GetBytes(response + "\r\n");
//                    await stream.WriteAsync(data, 0, data.Length);
//                    Console.WriteLine("Sent: " + response);
//                }
//            }
//            catch (IOException ex)
//            {
//                // This exception will be thrown when ReadLineAsync times out
//                Console.WriteLine("ReadLineAsync timed out: " + ex.ToString());
//            }
//            catch (Exception ex)
//            {
//                // Log any other exceptions
//                Console.WriteLine("Exception occurred: " + ex.ToString());
//            }
//        }
//    }
//}
