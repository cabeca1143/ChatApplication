namespace Packets;

public class C2S_Connection : BasePacket
{
    public override PacketType Type => PacketType.S2C_Connection;

    public string UserName { get; set; } = "";

    public C2S_Connection() { }
    public C2S_Connection(string userName)
    {
        UserName = userName;
    }

    public override void Read(NetworkStream stream)
    {
        BinaryReader reader = new(stream);
        reader.ReadByte();
        UserName = Extensions.ReadString(reader);
    }

    public override void Write(MemoryStream stream)
    {
        BinaryWriter writer = new(stream, Encoding.UTF8, true);
        writer.WriteString(UserName);
    }
}