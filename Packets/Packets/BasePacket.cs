global using System.Text;
global using System.Net.Sockets;

namespace Packets;

public class BasePacket
{
    public virtual PacketType Type { get; } = PacketType.Unknown;
    public virtual void Read(NetworkStream stream) { }
    public virtual void Write(MemoryStream stream) { }
    private void WriteHeader(MemoryStream stream)
    {
        BinaryWriter writer = new(stream, Encoding.UTF8, true);
        writer.Write((byte)Type);
    }
    public byte[] GetBytes()
    {
        MemoryStream stream = new();
        WriteHeader(stream);
        Write(stream);
        return stream.ToArray();
    }
}