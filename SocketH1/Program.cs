using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Program
    {

        // Main Method
        public static void Main(string[] args)
        {
            ThreadStart childref = new ThreadStart(Server.Program.ExecuteServer);
            Console.WriteLine("In Main: Creating the Child thread");
            Thread childThread = new Thread(childref);
            childThread.Start();

            //We let main thread(client) sleep a bit so server can start up
            Thread.Sleep(1000);
            ExecuteClient();
        }

        // ExecuteClient() Method
        static void ExecuteClient()
        {
            try
            {

                // Establish the remote endpoint
                // for the socket. This example
                // uses port 11111 on the local
                // computer.
                Console.CursorLeft = 0;
                Console.Write("Input server IP (default 192.168.1.2): ");
                string ip = Console.ReadLine();
                if (ip == "") ip = "192.168.1.2";
                Console.CursorLeft = 0;
                Console.Write("Message: ");
                string input = Console.ReadLine();
                //IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = IPAddress.Parse(ip);
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

                // Creation TCP/IP Socket using
                // Socket Class Constructor
                Socket sender = new Socket(ipAddr.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    // Connect Socket to the remote
                    // endpoint using method Connect()
                    sender.Connect(localEndPoint);

                    // We print EndPoint information
                    // that we are connected
                    Console.CursorLeft = 0;
                    Console.WriteLine("Socket connected to -> {0} ",
                                sender.RemoteEndPoint.ToString());

                    // Creation of message that
                    // we will send to Server
                    byte[] messageSent = Encoding.ASCII.GetBytes(input + "<EOF>");
                    int byteSent = sender.Send(messageSent);

                    // Data buffer
                    byte[] messageReceived = new byte[1024];

                    // We receive the message using
                    // the method Receive(). This
                    // method returns number of bytes
                    // received, that we'll use to
                    // convert them to string
                    int byteRecv = sender.Receive(messageReceived);
                    Console.CursorLeft = 0;
                    Console.WriteLine("Message from Server -> {0}",
                        Encoding.ASCII.GetString(messageReceived,
                                                    0, byteRecv));

                    // Close Socket using
                    // the method Close()
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

                // Manage of Socket's Exceptions
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }
    }
}