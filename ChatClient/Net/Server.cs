using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ChatClient.MVVM.ViewModel;
using Packets;
namespace ChatClient.Net;

class Server
{
    private readonly TcpClient _client;
    private readonly MainViewModel _model;
    private readonly Dictionary<PacketType, BasePacket> Packets = new()
    {
        {PacketType.S2C_Connection, new S2C_Connection() },
        {PacketType.Disconnection, new Disconnection() },
        {PacketType.BroadcastInfo, new BroadcastInfo() },
        {PacketType.Message, new UserMessage() },
        {PacketType.ConnectionResponse, new ConnectionResponse() }
    };

    public bool IsConnected => _client.Connected;

    public event Action<S2C_Connection>? OnConnected;
    public event Action<Disconnection>? OnDisconnected;
    public event Action<BroadcastInfo>? OnBroadcastInfo;
    public event Action<UserMessage>? OnUserMessage;
    public event Action<ConnectionResponse>? OnConnectionResponse;
    public event Action? OnServerShutDown;

    public Server(MainViewModel model)
    {
        _model = model;
        _client = new TcpClient();
    }

    public void ConnectToServer(string userName, string serverIP)
    {
        if (!IsConnected)
        {
            _client.Connect(serverIP, 25566);
            SendPacket(new C2S_Connection(userName));
            _model.User = new(userName);
            ReadPackets();
        }
    }

    public void SendPacket(BasePacket packet)
    {
        _client.Client.Send(packet.GetBytes());
    }

    public void ReadPackets()
    {
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    PacketType type = (PacketType)_client.GetStream().ReadByte();
                    if (Packets.TryGetValue(type, out BasePacket? packet))
                    {
                        packet?.Read(_client.GetStream());

                        switch (type)
                        {
                            case PacketType.S2C_Connection:
                                {
                                    Application.Current.Dispatcher.Invoke(OnConnected, packet);
                                }
                                break;
                            case PacketType.Disconnection:
                                {
                                    Application.Current.Dispatcher.Invoke(OnDisconnected, packet);
                                }
                                break;
                            case PacketType.BroadcastInfo:
                                {
                                    Application.Current.Dispatcher.Invoke(OnBroadcastInfo, packet);
                                }
                                break;
                            case PacketType.Message:
                                {
                                    Application.Current.Dispatcher.Invoke(OnUserMessage, packet);
                                }
                                break;
                            case PacketType.ConnectionResponse:
                                {
                                    Application.Current.Dispatcher.Invoke(OnConnectionResponse, packet);
                                }
                                break;
                            default:
                                break;
                        }

                    }
                }
                catch
                {
                    Application.Current.Dispatcher.Invoke(OnServerShutDown);
                    return;
                }
            }
        });
    }
}
