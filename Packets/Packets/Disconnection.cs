namespace Packets;

public class Disconnection : BasePacket
{
    public override PacketType Type => PacketType.Disconnection;

    public uint ID { get; private set; }

    public Disconnection() { }
    public Disconnection(uint iD)
    {
        ID = iD;
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