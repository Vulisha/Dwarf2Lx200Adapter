namespace Dwarf2Lx200Adapter
{
    internal static class Dwarf2Client
    {
        public static async Task<int> init(double lon, double lat, string date, string path)
        {
            var webSocketClient = new WebSocketClient();

            await webSocketClient.ConnectAsync();

            await webSocketClient.UpdateDateTimeAsync(date);

            // Perform the "correction" request
            int correctionCode = await webSocketClient.SendCorrectionAsync(lon, lat, date, path);

            Console.WriteLine(correctionCode);

            await webSocketClient.DisconnectAsync();
            
        return correctionCode;

        }

        public static async Task<int> Goto(double lon, double lat, double ra, double dec, string date, string path)
        {
            var webSocketClient = new WebSocketClient();

            await webSocketClient.ConnectAsync();

            // Perform the "Start goto" request
            int startGotoCode = await webSocketClient.StartGotoAsync(ra, dec, lon, lat, date, path);

            // Handle the "Start goto" response
            if (startGotoCode != 0)
            {
                return startGotoCode;
            }
            Console.WriteLine(startGotoCode);

            await webSocketClient.DisconnectAsync();

            return 0;

        }

        public static async void UpdateDateTime()
        {
            var webSocketClient = new WebSocketClient();

            await webSocketClient.UpdateDateTimeAsync();
        }
    }
}
