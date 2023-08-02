using System.Net;
using System.Net.Sockets;
using ChatServer;
using Packets;

public class Server
{
    const string IP = "127.0.0.1";
    const int Port = 25566;
    static readonly TcpListener listener = new(IPAddress.Parse(IP), Port);

    static void Main()
    {
        listener.Start();
        Console.WriteLine("Server has Started!");

        while (true)
        {
            var tcpClient = listener.AcceptTcpClient();
            if (tcpClient is not null)
            {
                Client client = new(tcpClient);
                PacketManager.Clients.Add(client);

                PacketManager.Notify_ConnectionResponse(client.ID);
                PacketManager.Notify_BroadcastInfo(client.ID);
                PacketManager.Notify_S2C_Connection(client);
                Task.Run(() => Listen(client.TcpClient!.GetStream(), client.ID));
            }
        }
    }

    static void Listen(NetworkStream stream, uint id)
    {
        while (true)
        {
            try
            {
                switch ((PacketType)stream.ReadByte())
                {
                    case PacketType.Message:
                        {
                            UserMessage packet = new();
                            packet.Read(stream);
                            PacketManager.Notify_UserMessage(packet);
                        }
                        break;
                }
            }
            catch
            {
                PacketManager.Notify_Disconnection(id);
            }
        }
    }
}