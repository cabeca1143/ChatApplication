namespace Packets;

public class S2C_Connection : BasePacket
{
    public override PacketType Type => PacketType.S2C_Connection;

    public Client? Client { get; set; }

    public S2C_Connection() { }
    public S2C_Connection(Client client)
    {
        Client = client;
    }

    public override void Read(NetworkStream stream)
    {
        BinaryReader reader = new(stream);
        Client = new(Extensions.ReadString(reader), reader.ReadUInt32());
    }

    public override void Write(MemoryStream stream)
    {
        BinaryWriter writer = new(stream, Encoding.UTF8, true);
        writer.WriteString(Client?.Name);
        writer.Write(Client?.ID ?? 0);
    }
}