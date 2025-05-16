using System.Net;
using System.Net.Sockets;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            DateTime utcTime = GetNtpTime("pool.ntp.org");
            Console.WriteLine($"UTC Time: {utcTime:yyyy-MM-dd HH:mm:ss}");

            DateTime baseUtcTime = utcTime;

            while (true)
            {
                TimeSpan elapsed = DateTime.UtcNow - baseUtcTime;
                utcTime = baseUtcTime.Add(elapsed);

                Console.Write("Enter time zone (e.g., GMT+2 or GMT-5), or 'exit' to quit: ");
                string input = Console.ReadLine();

                if (input.Trim().ToLower() == "exit")
                {
                    Console.WriteLine("Exiting application...");
                    break;
                }

                if (!TryParseTimeZone(input, out int offsetHours))
                {
                    Console.WriteLine("Invalid input. Use GMT+X or GMT-X (X is 0 to 11), or 'exit' to quit.");
                    continue;
                }

                DateTime zoneTime = utcTime.AddHours(offsetHours);
                Console.WriteLine($"Time in {input}: {zoneTime:yyyy-MM-dd HH:mm:ss}\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    static DateTime GetNtpTime(string ntpServer)
    {
        byte[] ntpData = new byte[48];
        ntpData[0] = 0x1B; // Client mode, version 3

        using (var udpClient = new UdpClient())
        {
            udpClient.Connect(ntpServer, 123);
            udpClient.Send(ntpData, ntpData.Length);

            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            ntpData = udpClient.Receive(ref remoteEndPoint);
        }

        // Extract seconds from bytes 40-43 (transmit timestamp)
        uint seconds = ((uint)ntpData[40] << 24) | ((uint)ntpData[41] << 16) |
                      ((uint)ntpData[42] << 8) | ntpData[43];

        // Convert from NTP epoch (1900) to DateTime (1970)
        seconds -= 2208988800; // Difference between 1900 and 1970 in seconds
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);
    }

    static bool TryParseTimeZone(string input, out int offsetHours)
    {
        offsetHours = 0;

        if (string.IsNullOrWhiteSpace(input) || !input.StartsWith("GMT") || input.Length < 4)
            return false;

        string signAndNumber = input.Substring(3);
        if (!signAndNumber.StartsWith("+") && !signAndNumber.StartsWith("-"))
            return false;

        string sign = signAndNumber.Substring(0, 1);
        string number = signAndNumber.Substring(1);

        if (!int.TryParse(number, out offsetHours))
            return false;

        if (offsetHours < 0 || offsetHours > 11)
            return false;

        if (sign == "-")
            offsetHours = -offsetHours;

        return true;
    }
}