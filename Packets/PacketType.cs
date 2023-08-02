namespace Packets;

public enum PacketType : byte
{
    Unknown = 0,
    S2C_Connection = 1,
    C2S_Connection = 2,
    Disconnection = 3,
    BroadcastInfo = 4,
    Message = 5,
    ConnectionResponse = 6
}