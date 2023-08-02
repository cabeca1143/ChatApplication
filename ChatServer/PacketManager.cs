using Packets;
using System.Net.Sockets;

namespace ChatServer
{
    public class PacketManager
    {
        public static readonly List<Client> Clients = new();

        internal static void BroadcastPacket(BasePacket packet)
        {
            foreach(var client in Clients)
            {
                try
                {
                    _ = client.TcpClient?.Client.Send(packet.GetBytes());
                }
                catch { }
            }
        }

        internal static void SendPacket(BasePacket packet, uint id)
        {
            Clients.Find(x => x.ID == id)?.TcpClient?.Client.Send(packet.GetBytes());
        }

        internal static void SendPacket(BasePacket packet, TcpClient client)
        {
            client.Client.Send(packet.GetBytes());
        }

        internal static void Notify_ConnectionResponse(uint userId)
        {
            SendPacket(new ConnectionResponse(userId), userId);
        }

        internal static void Notify_S2C_Connection(Client client)
        {
            S2C_Connection packet = new(client);
            foreach (var c in Clients)
            {
                if(c != client)
                {
                    SendPacket(packet, c.TcpClient!);
                }
            }
        }

        internal static void Notify_BroadcastInfo(uint? userId = null)
        {
            if(userId is null)
            {
                BroadcastPacket(new BroadcastInfo(Clients));
            }
            else
            {
                SendPacket(new BroadcastInfo(Clients), (uint)userId);
            }
        }

        internal static void Notify_UserMessage(UserMessage message)
        {
            BroadcastPacket(message);
            Console.WriteLine($"User {Clients.Where(x => x.ID == message.SenderID)?.FirstOrDefault()?.Name ?? ""} sent: {message.Message}");
        }

        internal static void Notify_Disconnection(uint id)
        {
            Client? client = Clients.Find(x => x.ID == id);
            if(client is not null)
            {
                string name = client.Name;
                Clients.RemoveAll(x => x.ID == id);
                BroadcastPacket(new Disconnection(id));
                Console.WriteLine($"User {name} disconnected!");
            }

        }
    }
}
