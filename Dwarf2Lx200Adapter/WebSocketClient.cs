using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Net.Http;

public class WebSocketClient
{
    private readonly Uri _serverUri = new Uri("ws://127.0.0.1:9900");
    private ClientWebSocket _clientWebSocket;

    public async Task ConnectAsync()
    {
        _clientWebSocket = new ClientWebSocket();
        await _clientWebSocket.ConnectAsync(_serverUri, CancellationToken.None);
    }

    public async Task SendMessageAsync<T>(T message)
    {
        string json = JsonSerializer.Serialize(message); 
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        var segment = new ArraySegment<byte>(buffer);

        await _clientWebSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async Task<T> ReceiveMessageAsync<T>()
    {
        var buffer = new ArraySegment<byte>(new byte[8192]);
        WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
        string json = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
        
        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task DisconnectAsync()
    {
        await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect", CancellationToken.None);
    }
    public async Task TurnOnCameraAsync()
    {
        var request = new
        {
            Interface = 10000,
            CamId = 0
        };

        await SendMessageAsync(request);
    }


    public async Task UpdateDateTimeAsync()
    {
        string dateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        string url = $"http://127.0.0.1:8092/date?date={dateTime}";

        using var httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            // Handle the error
        }
    }

    public async Task UpdateDateTimeAsync(string dateTime)
    {
        string url = $"http://127.0.0.1:8092/date?date={dateTime}";

        using var httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            // Handle the error
        }
    }

    public async Task<int> SendCorrectionAsync(double lon, double lat, string date, string path)
    {
        var request = new
        {
            Interface = 11205,
            CamId = 0,
            Lon = lon,
            Lat = lat,
            Date = date,
            Path = path
        };

        await SendMessageAsync(request);

        var response = await ReceiveMessageAsync<dynamic>();
        int code = response.code;

        return code;
    }

    public async Task<int> StartGotoAsync(double ra, double dec, double lon, double lat, string date, string path)
    {
        var request = new
        {
            Interface = 11203,
            CamId = 0,
            Ra = ra,
            Dec = dec,
            Lon = lon,
            Lat = lat,
            Date = date,
            Path = path
        };

        await SendMessageAsync(request);

        var response = await ReceiveMessageAsync<dynamic>();
        int code = response.code;

        return code;
    }

}
