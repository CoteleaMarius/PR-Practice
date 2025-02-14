using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client_Server
{
    internal abstract class Server
    {
        private static List<Socket> _clients = new List<Socket>();
        private static object _lockObj = new object();
    
        public static void Main(string[] args)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            serverSocket.Bind(new IPEndPoint(ipAddress, 9000));
            serverSocket.Listen(5);
        
            Console.WriteLine("Server started. Waiting for connections...");

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                lock (_lockObj)
                {
                    _clients.Add(clientSocket);
                }
                Console.WriteLine("Client connected: " + clientSocket.RemoteEndPoint);
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(clientSocket);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        static void HandleClient(object obj)
        {
            Socket clientSocket = (Socket)obj;
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int received = clientSocket.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine("Received: " + message);
                    BroadcastMessage(message, clientSocket);
                }
            }
            catch
            {
                lock (_lockObj)
                {
                    _clients.Remove(clientSocket);
                }
                Console.WriteLine("Client disconnected: " + clientSocket.RemoteEndPoint);
                clientSocket.Close();
            }
        }

        static void BroadcastMessage(string message, Socket sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            lock (_lockObj)
            {
                foreach (Socket client in _clients)
                {
                    if (client != sender)
                    {
                        client.Send(data);
                    }
                }
            }
        }
    }
}