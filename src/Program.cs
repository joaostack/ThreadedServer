using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ThreadedServer
{
    class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 3000);
            this.listenThread = new Thread(ListenForClients);
            this.listenThread.Start();
        }

        private void ListenForClients()
        {
            // start srver
            this.tcpListener.Start();

            while (true)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");
                var th = new Thread(() => ProcessData(client));
                th.Start();
            }
        }

        private void ProcessData(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream networkStream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int bytesRead = 0;
                    bytesRead = networkStream.Read(buffer);

                    ASCIIEncoding encoder = new ASCIIEncoding();
                    string message = encoder.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"{tcpClient.Client.RemoteEndPoint}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{tcpClient.Client.RemoteEndPoint} disconnected.");
                Console.WriteLine(ex.Message);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var svr = new Server();
        }
    }
}
