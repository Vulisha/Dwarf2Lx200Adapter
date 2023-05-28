using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public static class MessageHelper
{
    public static async Task<int> ReadMessageTypeAsync(Stream stream)
    {
        byte[] buffer = new byte[2];
        await stream.ReadAsync(buffer, 2, 2);

        return BitConverter.ToInt16(buffer, 0);
    }

    public static async Task<int> ReadMessageLenghtAsync(Stream stream)
    {
        byte[] buffer = new byte[2];
        await stream.ReadAsync(buffer, 0, 2);

        return BitConverter.ToInt16(buffer, 0);
    }

    public static async Task WriteMessageTypeAsync(Stream stream, int type)
    {
        byte[] buffer = BitConverter.GetBytes((short)type);
        await stream.WriteAsync(buffer, 0, 2);
    }

    public static async Task<uint> ReadUnsignedIntAsync(Stream stream)
    {
        byte[] buffer = new byte[4];
        await stream.ReadAsync(buffer, 10, 4);

        return BitConverter.ToUInt32(buffer, 0);
    }

    public static async Task WriteUnsignedIntAsync(Stream stream, uint value)
    {
        byte[] buffer = BitConverter.GetBytes(value);
        await stream.WriteAsync(buffer, 0, 4);
    }

    public static async Task<int> ReadIntAsync(Stream stream)
    {
        byte[] buffer = new byte[4];
        await stream.ReadAsync(buffer, 14, 4);

        return BitConverter.ToInt32(buffer, 0);
    }

    public static async Task WriteIntAsync(Stream stream, int value)
    {
        byte[] buffer = BitConverter.GetBytes(value);
        await stream.WriteAsync(buffer, 0, 4);
    }

    public static double StellariumToHours(uint stellariumRa)
    {
        return stellariumRa / (double)0x100000000 * 24;
    }

    public static double StellariumToDegrees(int stellariumDec)
    {
        return stellariumDec / (double)0x40000000 * 90;
    }

    public static uint HoursToStellarium(double hours)
    {
        return (uint)(hours / 24 * 0x100000000);
    }

    public static int DegreesToStellarium(double degrees)
    {
        return (int)(degrees / 90 * 0x40000000);
    }

    public static async Task<uint> ReadUnsignedIntAsync(NetworkStream stream, int start)
    {
        byte[] bytes = new byte[4];
        if (await stream.ReadAsync(bytes, start, 4) != 4)
        {
            throw new Exception("Failed to read unsigned integer from stream.");
        }
        return BitConverter.ToUInt32(bytes);
    }

    public static async Task<int> ReadSignedIntAsync(NetworkStream stream, int start)
    {
        byte[] bytes = new byte[4];
        if (await stream.ReadAsync(bytes, start, 4) != 4)
        {
            throw new Exception("Failed to read signed integer from stream.");
        }
        return BitConverter.ToInt32(bytes);
    }

    public static string ConvertRaToLX200Format(double raHours)
    {
        // Convert RA from uint to hours (0-24 range)
        int raH = (int)raHours;
        int raM = (int)((raHours - raH) * 60);
        int raS = (int)(((raHours - raH) * 60 - raM) * 60);

        // Convert DEC from int to degrees (-90 to +90 range)


        string sr = $":Sr{raH.ToString("00")}:{raM.ToString("00")}:{raS.ToString("00")}";

        return sr;
    }

    public static string ConvertDecToLX200Format(double decDegrees)
    {
      
        // Convert DEC from int to degrees (-90 to +90 range)
        int decD = (int)decDegrees;
        int decM = Math.Abs((int)((decDegrees - decD) * 60));
        int decS = Math.Abs((int)(((decDegrees - decD) * 60 - decM) * 60));

       
        string sd = $":Sd{(decDegrees < 0 ? "-" : "+")}{Math.Abs(decD).ToString("00")}*{decM.ToString("00")}:{decS.ToString("00")}";

        return sd;
    }


}

