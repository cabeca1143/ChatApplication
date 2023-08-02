using ChatClient.Net;
using Packets;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;

namespace ChatClient.MVVM.ViewModel
{
    internal class MainViewModel
    {
        public Client? User;
        public ObservableCollection<Client> Users { get; set; } = new();
        public ObservableCollection<string> Messages { get; set; } = new();
        public RelayCommand ConnectCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string ServerIP { get; set; } = "127.0.0.1";
        public Server Server { get; }
        public PacketManager PacketHandler { get; }

        public MainViewModel() 
        {
            Server = new(this);
            PacketHandler = new(this);
            Server.OnConnected += PacketHandler.HandleConnectedUser;
            Server.OnDisconnected += PacketHandler.HandleDisconnectedUser;
            Server.OnBroadcastInfo += PacketHandler.HandleBroadcastInfo;
            Server.OnUserMessage += PacketHandler.HandleReceiveUserMessage;
            Server.OnConnectionResponse += PacketHandler.HandleConnectionResponse;
            Server.OnServerShutDown+= OnServerShutDown;
            ConnectCommand = new RelayCommand(x => Server.ConnectToServer(UserName, ServerIP), x => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(ServerIP));
            SendMessageCommand = new RelayCommand(x => PacketHandler.HandleSendUserMessage(Message), x => !string.IsNullOrEmpty(Message) && Server.IsConnected);
        }

        void OnServerShutDown()
        {
            Messages.Add("[System] Connection Lost!");
        }
    }
}
