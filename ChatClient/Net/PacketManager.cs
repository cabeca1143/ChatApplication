using ChatClient.MVVM.ViewModel;
using Packets;
using System.Linq;

namespace ChatClient.Net;

class PacketManager
{
    MainViewModel _model;
    internal PacketManager(MainViewModel model)
    {
        _model = model;
    }

    internal void HandleConnectionResponse(ConnectionResponse response)
    {
        if (_model.Users.Any(x => x.ID == response.ID))
        {
            return;
        }
        _model.User!.ID = response.ID;
    }

    internal void HandleReceiveUserMessage(UserMessage packet)
    {
        Client? sender = _model.Users.ToList().Find(x => x.ID == packet.SenderID);
        _model.Messages.Add($"[{StringOrUnknown(sender?.Name)}]: {packet.Message}");
    }

    internal void HandleSendUserMessage(string message)
    {
        _model.Server.SendPacket(new UserMessage(message, _model.User!.ID));
    }

    internal void HandleBroadcastInfo(BroadcastInfo info)
    {
        foreach (Client client in info.Clients!)
        {
            _model.Users.Add(client);
        }
    }

    internal void HandleDisconnectedUser(Disconnection connectionInfo)
    {
        Client? user = _model.Users.Where(x => x.ID == connectionInfo.ID).FirstOrDefault()!;
        _ = _model.Users.Remove(user);
        _model.Messages.Add($"[System]: {StringOrUnknown(user?.Name)} Left!");
    }

    internal void HandleConnectedUser(S2C_Connection connectionInfo)
    {
        _model.Users.Add(connectionInfo.Client!);
        _model.Messages.Add($"[System]: {connectionInfo.Client!.Name} Has Joined!");
    }

    static string StringOrUnknown(string? value)
    {
        return value ?? "Unknown";
    }
}
