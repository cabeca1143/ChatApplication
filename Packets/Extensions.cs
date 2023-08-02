namespace Packets;

public static class Extensions
{
    public static string ReadString(this BinaryReader reader) => Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadInt32()));
    public static void WriteString(this BinaryWriter reader, string? text)
    {
        reader.Write(text?.Length ?? 0);
        reader.Write(Encoding.UTF8.GetBytes(text ?? string.Empty));
    }
}