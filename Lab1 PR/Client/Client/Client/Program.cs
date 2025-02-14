using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal abstract class Client
    {
        private static string? _username;
        public static void Main(string[] args)
        {
            Console.Write("Enter your username: ");
            _username = Console.ReadLine();

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000));
            Console.WriteLine("Connected to the server.");
            
            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start(clientSocket);
            
            while (true)
            {
                string? message = Console.ReadLine();
                string formattedMessage = _username + ": " + message;
                byte[] data = Encoding.UTF8.GetBytes(formattedMessage);
                clientSocket.Send(data);
            }
        }
        
        static void ReceiveMessages(object obj)
        {
            Socket clientSocket = (Socket)obj;
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int received = clientSocket.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine(message);
                }
            }
            catch
            {
                Console.WriteLine("Disconnected from server.");
                clientSocket.Close();
            }
        }
    }
}