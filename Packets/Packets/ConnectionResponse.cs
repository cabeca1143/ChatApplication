namespace Packets;

public class ConnectionResponse : BasePacket
{
    public override PacketType Type => PacketType.ConnectionResponse;

    public uint ID { get; set; }

    public ConnectionResponse() { }
    public ConnectionResponse(uint id)
    {
        ID = id;
    }

    public override void Read(NetworkStream stream)
    {
        BinaryReader reader = new(stream);
        ID = reader.ReadUInt32();
    }

    public override void Write(MemoryStream stream)
    {
        BinaryWriter writer = new(stream, Encoding.UTF8, true);
        writer.Write(ID);
    }
}