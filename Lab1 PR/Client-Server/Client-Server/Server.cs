using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client_Server
{
    internal abstract class Server
    {
        private static readonly List<Socket> Clients = new ();
        private static readonly Lock LockObj = new ();
    
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
                lock (LockObj)
                {
                    Clients.Add(clientSocket);
                }
                Console.WriteLine("Client connected: " + clientSocket.RemoteEndPoint);
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(clientSocket);
            }
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
                lock (LockObj)
                {
                    Clients.Remove(clientSocket);
                }
                Console.WriteLine("Client disconnected: " + clientSocket.RemoteEndPoint);
                clientSocket.Close();
            }
        }

        static void BroadcastMessage(string message, Socket sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            lock (LockObj)
            {
                foreach (Socket client in Clients)
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