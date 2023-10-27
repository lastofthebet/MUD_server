using MUD_server;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Diagnostics.Metrics;

public class MUD_Server
{
    public static int Main(String[] args)
    {
        int MaxCon = 10;
        StartServer(MaxCon);
        return 0;
    }

    public static void StartServer(int MaxCon)
    {
        Random rnd = new Random(); //random number generator for the stats
        int[] CharacterStats = { 0, 0, 0 }; //Dungeons delved, Monsters killed, Encounters completed
        CharacterStats[0] = (rnd.Next(1, 3));
        CharacterStats[1] = (rnd.Next(1, 3));
        CharacterStats[2] = (rnd.Next(1, 3));
        Console.WriteLine("Max number of clients is "+MaxCon);
        Console.WriteLine("Waiting for a connection...");
        Socket Handler = Connecter.StartConnection(MaxCon);
        int counter =0;
        while (true)
        {
            try
            {
                counter ++;
                if (counter == MaxCon)
                {
                    CharacterStats[0]++;
                    CharacterStats[1]++;
                    CharacterStats[2]++;
                }
                String responce = Connecter.recieveData(Handler);
                Console.WriteLine(responce);
                if (responce == "Shutdown")
                {
                    Connecter.Shutdown(Handler);
                    Console.WriteLine( "shutting down connection");
                    break;
                }
                else if (responce == "Request")
                {
                    Console.WriteLine("sending Request");
                    String data = "0"+CharacterStats[0].ToString()+"."+ "0" + CharacterStats[1].ToString()+ "."+"0" +  CharacterStats[2].ToString();
                    Console.WriteLine(data);
                    Connecter.SendData(Handler,data);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("recieving " + e.Message);
                Connecter.Shutdown(Handler);
                Console.WriteLine("shutting down connection");
                break;
            }
            //try
            //{
            //    String Message = (CharacterStats[0].ToString() + CharacterStats[1].ToString() + CharacterStats[2].ToString());
            //    Connecter.SendData(Handler, Message);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("sending "+e.Message);
            //    Connecter.Shutdown(Handler);
            //    Console.WriteLine("shutting down connection");
            //    break;
            //}
            Console.ReadKey();
        }
    }
}