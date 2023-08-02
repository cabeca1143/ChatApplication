namespace Packets;

public class UserMessage : BasePacket
{
    public override PacketType Type => PacketType.Message;
    
    public uint SenderID { get; private set; }
    public string Message { get; private set; } = "";

    public UserMessage() { }
    public UserMessage(string message, uint senderID)
    {
        Message = message;
        SenderID = senderID;
    }

    public override void Read(NetworkStream stream)
    {
        BinaryReader reader = new(stream);
        SenderID = reader.ReadUInt32();
        Message = Extensions.ReadString(reader);
    }

    public override void Write(MemoryStream stream)
    {
        BinaryWriter writer = new(stream, Encoding.UTF8, true);
        writer.Write(SenderID);
        writer.WriteString(Message);
    }
}