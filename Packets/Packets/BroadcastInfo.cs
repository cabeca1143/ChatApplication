namespace Packets;

public class BroadcastInfo : BasePacket
{
    public override PacketType Type => PacketType.BroadcastInfo;

    public Client[]? Clients { get; private set; }

    public BroadcastInfo() { }
    public BroadcastInfo(int size)
    {
        Clients = new Client[size];
    }
    public BroadcastInfo(List<Client> clients)
    {
        Clients = clients.ToArray();
    }

    public override void Read(NetworkStream stream)
    {
        BinaryReader reader = new(stream);
        Clients = new Client[reader.ReadInt32()];
        for (int i = 0; i < Clients.Length; i++)
        {
            Clients[i] = new(Extensions.ReadString(reader), reader.ReadUInt32());
        }
    }

    public override void Write(MemoryStream stream)
    {
        BinaryWriter writer = new(stream, Encoding.UTF8, true);
        writer.Write(Clients?.Length ?? 0);
        if (Clients is not null)
        {
            foreach (Client client in Clients)
            {
                writer.WriteString(client.Name);
                writer.Write(client.ID);
            }
        }
    }
}