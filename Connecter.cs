using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MUD_server
{
    internal class Connecter
    {

        public static Socket StartConnection(int Maxcon)
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            // Get Host IP Address that is used to establish a connection
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses
            // Create a Socket that will use Tcp protocol
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // A Socket must be associated with an endpoint using the Bind method
            listener.Bind(localEndPoint);
            // Specify how many requests a Socket can listen before it gives Server busy response.
            listener.Listen(Maxcon);
            Socket handler = listener.Accept();
            return handler;
        }
        public static String recieveData (Socket handler)
        {
            try
            { // Incoming data from the client.
                string data = null;
                byte[] bytes = null;
                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    return data;
                }
            }
            catch (Exception e) { 
                throw e; 
            }
        }
        public static void SendData(Socket handler,string data)
        {
            byte[] message = Encoding.ASCII.GetBytes(data);
            handler.Send(message);

        }
        public static void Shutdown(Socket handler)
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }


        
        
    }
}
