using System.Net.Sockets;
using System.Security.Cryptography;

namespace Packets;

public class Client
{
    public string Name { get; set; } = "";
    public uint ID;
    public TcpClient? TcpClient { get; }

    public Client(TcpClient client)
    {
        TcpClient = client;

        C2S_Connection connectionData = new();
        connectionData.Read(client.GetStream());

        Name = connectionData.UserName;

        ID = (uint)RandomNumberGenerator.GetInt32(int.MaxValue);

        Console.WriteLine($"User \"{Name}\" has Connected to the server!");
    }

    public Client(string name, uint id)
    {
        Name = name;
        ID = id;
    }

    public Client(string name)
    {
        Name = name;
    }
}