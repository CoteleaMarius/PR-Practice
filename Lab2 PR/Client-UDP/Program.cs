using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client_UDP;

internal abstract class NetworkChat
{
    private static Socket? _groupSocket;
    private static Socket? _directSocket;
    private static readonly IPAddress GroupAddress = IPAddress.Parse("224.0.0.2");
    private const int GroupPort = 3222;
    private const int DirectPort = 3111;
    private static IPAddress? _localIp;
    private static EndPoint? _groupEndPoint;
    private static IPAddress? _recipientIp;
    private static HashSet<IPAddress> _connectedUsers = new();

    static void Main()
    {
        Console.Write("Introduceți adresa IP locală (ex: 192.168.1.100): ");
        string? userIp = Console.ReadLine();
        if (!IPAddress.TryParse(userIp, out _localIp))
        {
            Console.WriteLine("IP invalid! Se va folosi configurarea implicită.");
            _localIp = IPAddress.Any;
        }

        _groupSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _groupSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        _groupSocket.Bind(new IPEndPoint(_localIp, GroupPort));
        _groupSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(GroupAddress, _localIp));
        _groupEndPoint = new IPEndPoint(GroupAddress, GroupPort);

        _directSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _directSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        _directSocket.Bind(new IPEndPoint(_localIp, DirectPort));

        Thread groupListener = new Thread(ReceiveGroupMessages);
        groupListener.Start();
        Thread directListener = new Thread(ReceiveDirectMessages);
        directListener.Start();

        Console.WriteLine("Chat-ul a fost inițiat.");
        Console.WriteLine("Comenzi disponibile:");
        Console.WriteLine("/privat IP - selectează un destinatar pentru mesaje directe");
        Console.WriteLine("/public - revino la chatul public");

        while (true)
        {
            string? input = Console.ReadLine();
            if (input != null) TransmitMessage(input);
        }
    }

    static void TransmitMessage(string? input)
    {
        try
        {
            byte[] packet;

            if (input != null && input.StartsWith("/privat "))
            {
                string ipString = input.Substring(8).Trim();
                if (IPAddress.TryParse(ipString, out _recipientIp))
                {
                    Console.WriteLine($"Mesajele vor fi trimise privat către {_recipientIp}");
                }
                else
                {
                    Console.WriteLine("Adresă IP invalidă!");
                }
                return;
            }
            else if (input == "/public")
            {
                _recipientIp = null;
                Console.WriteLine("Mesajele vor fi transmise în chatul public");
                return;
            }

            string formattedMsg = $"{_localIp}: {input}";
            packet = Encoding.UTF8.GetBytes(formattedMsg);

            if (_recipientIp == null)
            {
                _groupSocket?.SendTo(packet, _groupEndPoint);
                Console.WriteLine("Mesaj trimis în grup (multicast)");
            }
            else
            {
                EndPoint privateEndPoint = new IPEndPoint(_recipientIp, DirectPort);
                _directSocket?.SendTo(packet, privateEndPoint);
                Console.WriteLine($"Mesaj privat trimis către {_recipientIp}");
            }
        }
        catch (Exception err)
        {
            Console.WriteLine($"Eroare la trimitere: {err.Message}");
        }
    }

    static void ReceiveGroupMessages()
    {
        byte[] buffer = new byte[1024];
        EndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            while (true)
            {
                int receivedBytes = _groupSocket.ReceiveFrom(buffer, ref remoteEp);
                string receivedMsg = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                IPAddress senderIp = ((IPEndPoint)remoteEp).Address;
                
                if (_connectedUsers.Add(senderIp))
                {
                    Console.WriteLine("Lista utilizatorilor conectați:");
                    foreach (var ip in _connectedUsers)
                    {
                        Console.WriteLine(ip);
                    }
                }
                
                Console.WriteLine(receivedMsg);
            }
        }
        catch (Exception err)
        {
            Console.WriteLine($"Eroare la recepție (grup): {err.Message}");
        }
    }

    static void ReceiveDirectMessages()
    {
        byte[] buffer = new byte[1024];
        EndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            while (true)
            {
                int receivedBytes = _directSocket.ReceiveFrom(buffer, ref remoteEp);
                string receivedMsg = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                IPAddress senderIp = ((IPEndPoint)remoteEp).Address;
                
                if (_connectedUsers.Add(senderIp))
                {
                    Console.WriteLine("Lista utilizatorilor conectați:");
                    foreach (var ip in _connectedUsers)
                    {
                        Console.WriteLine(ip);
                    }
                }
                
                Console.WriteLine($"[PRIVAT] {receivedMsg}");
            }
        }
        catch (Exception err)
        {
            Console.WriteLine($"Eroare la recepție (privat): {err.Message}");
        }
    }
}