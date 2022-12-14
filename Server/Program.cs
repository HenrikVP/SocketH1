// A C# Program for Server
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Program
    {
        public static List<User> UserList = new();
        // Main Method
        static void Main(string[] args){}

        public static void ExecuteServer()
        {
            int cursPos = 70;
            // Establish the local endpoint
            // for the socket. Dns.GetHostName
            // returns the name of the host
            // running the application.
            //IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = IPAddress.Parse("192.168.2.3");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);
            Console.CursorLeft = cursPos;
            Console.WriteLine("Starting server on: " + ipAddr.ToString());

            // Creation TCP/IP Socket using
            // Socket Class Constructor
            Socket listener = new Socket(ipAddr.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

            try
            {

                // Using Bind() method we associate a
                // network address to the Server Socket
                // All client that will connect to this
                // Server Socket must know this network
                // Address
                listener.Bind(localEndPoint);

                // Using Listen() method we create
                // the Client list that will want
                // to connect to Server
                listener.Listen(10);

                while (true)
                {
                    Console.CursorLeft = cursPos;
                    Console.WriteLine("Waiting connection ... ");

                    // Suspend while waiting for
                    // incoming connection Using
                    // Accept() method the server
                    // will accept connection of client
                    Socket clientSocket = listener.Accept();
                    IPEndPoint clientEndpoint = (IPEndPoint)clientSocket.RemoteEndPoint;
                    Console.CursorLeft = cursPos;
                    Console.WriteLine("Client IP: " + clientEndpoint.Address +
                        " Client Port: " + clientEndpoint.Port);

                    // Data buffer
                    byte[] bytes = new Byte[1024];
                    string data = null;

                    while (true)
                    {

                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes,
                                                0, numByte);

                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }
                    Console.CursorLeft = cursPos;
                    string[] dataArray = data.Split('#');
                    Console.WriteLine($"Text received -> {dataArray[1]} from user: {dataArray[0]} ");
                    byte[] message = Encoding.ASCII.GetBytes("Message Recieved :)");

                    User user = new User() { Name = dataArray[0], Ipaddress = clientEndpoint.Address.ToString() };

                    bool addUser = false;
                    foreach (User u in UserList)
                    {
                        if (u.Name == user.Name && u.Ipaddress == user.Ipaddress) Console.WriteLine("User already exists in list");
                        else addUser= true;
                    }
                    if (addUser == true) UserList.Add(user);
                    // Send a message to Client
                    // using Send() method
                    clientSocket.Send(message);

                    // Close client Socket using the
                    // Close() method. After closing,
                    // we can use the closed Socket
                    // for a new Client Connection
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();

                    ShowAddressList();
                }
            }

            catch (Exception e)
            {
                Console.CursorLeft = cursPos;
                Console.WriteLine(e.ToString());
            }
        }

        private static void ShowAddressList()
        {
            foreach (User user in UserList)
            {
                Console.WriteLine($"{user.Name} IP {user.Ipaddress}");
            }
        }
    }
}