//using RustyCon.Rcon;
using RustRcon.Containers;
using RustRcon.Entities;
using RustRcon.EventArgs;
using RustRcon.Messages;
using RustRcon.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Websocket.Client;

namespace RustRcon
{
    public class RustRconClient
    {
        #region Events14
        public event EventHandler<BanEventArgs>? BansReceived;
        public event EventHandler<ChatEventArgs>? ChatReceived;
        public event EventHandler<ConsoleEventArgs>? ConsoleReceived;
        public event EventHandler<OxidePluginsEventArgs>? OxidePluginsReceived;
        public event EventHandler<CarbonPluginsEventArgs>? CarbonPluginsReceived;
        public event EventHandler<PlayerListEventArgs>? PlayerListReceived;
        public event EventHandler<ServerInfoEventArgs>? ServerInfoReceived;
        public event EventHandler<UnprocessedEventArgs>? UnprocessedMessageReceived;
        public event EventHandler<ModFrameworkVersionEventArgs>? ModFrameworkVersionReceived;

        public event EventHandler<MessageReceivedEventArgs>? MessageReceived;
        public event EventHandler<ConnectionChangedEventArgs>? ConnectionChanged;
        #endregion

        #region Public Properties13
        public bool IsConnected { get => _socket == null ? false : _socket.IsRunning; }
        public virtual string Name { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        #endregion

        #region Private Variables12
        private object _outstandingRequestsLock = new object();
        private Dictionary<int, ParserBase> _outstandingRequests = new Dictionary<int, ParserBase>();
        private List<int> _debugRequests = new ();

        private List<ParserBase> _parsers = new List<ParserBase>();

        private WebsocketClient _socket;

        public List<WebRconMessageBase> MessageLog = new List<WebRconMessageBase>();
        #endregion

        #region Protected Properties11
        protected Sequence<int> _sequence = new Sequence<int>(0);
        #endregion

        #region Constructor10
        public RustRconClient(string name, string hostname, int port, string password)
        {
            Name = name;
            Hostname = hostname;
            Port = port;
            Password = password;

            var uri = new Uri($"ws://{hostname}:{port}/{password}");

            _socket = new WebsocketClient(uri);

            _socket.MessageReceived.Subscribe((message) => Socket_OnMessage(message));
            _socket.DisconnectionHappened.Subscribe((info) => Socket_OnClose(info));

            InitializeParsers();
        }
        #endregion

        #region WebRconClient Events9
        protected void OnMessageReceived(WebRconResponse response, bool handled)
        {
            ParserBase? responseParser = null;
            bool isDebug = _debugRequests.Contains(response.Identifier);
            if (isDebug)
            {
                _debugRequests.Remove(response.Identifier);
            }
            if (_outstandingRequests.TryGetValue(response.Identifier, out responseParser))
            {
                if (responseParser.TryParseMessage(response, out var parseMessage))
                {
                    if (parseMessage != null)
                    {
                        if (responseParser.FireEvent != null)
                        { 
                            responseParser.FireEvent(parseMessage);
                            if (!(responseParser is NoParser)) handled = true;
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        // couldn't find message parser
                        OnUnknownMessageReceived(response);
                    }
                }
                lock (_outstandingRequestsLock)
                {
                    _outstandingRequests.Remove(response.Identifier);
                }
            }
            else
            {
                OnUnknownMessageReceived(response);
            }

            //base.OnMessageReceived(response, handled);
        }
        #endregion

        #region Event Sources8
        protected void OnChatReceived(List<ChatMessage> chats)
        {
            ChatReceived?.Invoke(this, new ChatEventArgs(chats));
        }
        protected void OnConsoleReceived(List<ConsoleMessage> consoles)
        {
            ConsoleReceived?.Invoke(this, new ConsoleEventArgs(consoles));
        }
        protected void OnBansReceived(List<Ban> bans)
        {
            BansReceived?.Invoke(this, new BanEventArgs(bans));
        }
        protected void OnOxidePluginListReceived(List<OxidePlugin> oxidePlugins)
        {
            OxidePluginsReceived?.Invoke(this, new OxidePluginsEventArgs(oxidePlugins));
        }

        protected void OnCarbonPluginListReceived(List<CarbonPlugin> carbonPlugins)
        {
            CarbonPluginsReceived?.Invoke(this, new CarbonPluginsEventArgs(carbonPlugins));
        }

        protected void OnModFrameworkVersionReceived(ModFrameworkVersion version)
        {
            ModFrameworkVersionReceived?.Invoke(this, new ModFrameworkVersionEventArgs(version));
        }
        protected void OnPlayerListReceived(List<Player> playerList)
        {
            PlayerListReceived?.Invoke(this, new PlayerListEventArgs(playerList));
        }
        protected void OnServerInfoReceived(ServerInfo serverInfo)
        {
            ServerInfoReceived?.Invoke(this, new ServerInfoEventArgs(serverInfo));
        }

        protected void OnUnprocessedMessageReceived(Unprocessed unprocessed)
        {
            UnprocessedMessageReceived?.Invoke(this, new UnprocessedEventArgs(unprocessed));
        }
        protected void OnUnknownMessageReceived(WebRconResponse response)
        {
            var unprocessed = new Unprocessed
            { 
                Identifier = response.Identifier,
                Id = response.id,
                Message = response.Message,
                Type = response.Type,
                StackTrace = response.Stacktrace
            };
            OnUnprocessedMessageReceived(unprocessed);
        }
        #endregion

        #region Public Methods7
        public void GetBanList()
        {
            SendCommand("bans", _parsers.First(p => p is BanParser));
        }
        public void GetChatHistory(int lineCount)
        {
            SendCommand($"chat.tail {lineCount}", _parsers.First(p => p is ChatParser));
        }
        public void GetConsoleHistory(int lineCount)
        {
            SendCommand($"console.tail {lineCount}", _parsers.First(p => p is ConsoleParser));
        }
        public void GetOxidePlugins()
        {
            SendCommand("o.plugins", _parsers.First(p => p is OxidePluginListParser));
        }

        public void GetCarbonPlugins()
        {
            SendCommand("c.plugins", _parsers.First(p => p is CarbonPluginListParser));
        }
        public void GetPlayerList()
        {
            SendCommand("playerlist", _parsers.First(p => p is PlayerParser));
        }
        public void GetServerInfo()
        {
            SendCommand("serverinfo", _parsers.First(p => p is ServerInfoParser));
        }

        public void GetModFrameworkVersion()
        {
            // Unfortunately there's no single command to send to find out if oxide or carbon is installed
            // so we have to send both the Oxide and the Carbon version command to find out
            SendCommand("o.version", _parsers.First(p => p is OxideVersionParser));
            SendCommand("c.version", _parsers.First(p => p is CarbonVersionParser));
        }
        public void Send(string command)
        {
            WebRconRequest request = new WebRconRequest();
            request.Message = command;
            request.Identifier = this._sequence.GetValue();
            _debugRequests.Add(request.Identifier);
            SendRequest(request);
        }
        #endregion

        #region Private methods6
        private void SendCommand(string command, ParserBase parser)
        {
            WebRconRequest request = new WebRconRequest();
            request.Message = command;
            request.Identifier = this._sequence.GetValue();
            lock (_outstandingRequestsLock)
            {
                _outstandingRequests.Add(request.Identifier, parser);
            }
            SendRequest(request);
        }

        private void InitializeParsers()
        {
            _parsers.Add( new NoParser(message => OnUnprocessedMessageReceived((Unprocessed)message)));
            _parsers.Add(new ServerInfoParser(message => OnServerInfoReceived((ServerInfo)message)));
            _parsers.Add(new BanParser(message => OnBansReceived(((BanList)message).Bans)));
            _parsers.Add(new ChatParser(message => OnChatReceived(((ChatMessageList)message).Chats)));
            _parsers.Add(new ConsoleParser(message => OnConsoleReceived(((ConsoleMessageList)message).Consoles)));
            _parsers.Add(new PlayerParser(message => OnPlayerListReceived(((PlayerList)message).Players)));
            _parsers.Add(new OxidePluginListParser(message => OnOxidePluginListReceived(((OxidePluginList)message).Plugins)));
            _parsers.Add(new CarbonPluginListParser(message => OnCarbonPluginListReceived(((CarbonPluginList)message).Plugins)));
            _parsers.Add(new CarbonVersionParser(message => OnModFrameworkVersionReceived((ModFrameworkVersion)message)));
            _parsers.Add(new OxideVersionParser(message => OnModFrameworkVersionReceived((ModFrameworkVersion)message)));
        }
        #endregion







        #region Socket Events5
        private void Socket_OnMessage(ResponseMessage message)
        {
            if (message.MessageType == WebSocketMessageType.Text)
            {
                var response = JsonSerializer.Deserialize<WebRconResponse>(message.Text);
                if (response != null)
                {
                    MessageLog.Add(response);
                    OnMessageReceived(response, false);
                }
            }
        }

        private void Socket_OnClose(DisconnectionInfo disconnectionInfo)
        {
            OnConnectionChanged(false);
        }

        private void Socket_OnOpen(object? sender, System.EventArgs e)
        {
            OnConnectionChanged(true);
        }
        #endregion

        #region Private Methods4

        protected virtual void OnConnectionChanged(bool connected)
        {
            ConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs(connected));
        }

        #endregion

        #region Public Connection Methods3
        public bool Connect()
        {
            try
            {
                _socket.Start();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return false;
            }
            return true;
        }

        public void Disconnect()
        {
            try
            {
                if (_socket != null)
                {
                    if (_socket.IsRunning)
                        _socket.Stop(WebSocketCloseStatus.NormalClosure, string.Empty);
                }
            }
            catch (Exception ex) { }
        }

        public bool SendRequest(WebRconRequest request)
        {
            if (_socket != null && _socket.IsRunning)
            {
                request.Name = Name;
                var json = JsonSerializer.Serialize(request);

                MessageLog.Add(request);
                _socket.Send(json);
                return true;
            }
            return false;
        }
        #endregion

        #region Protected Methods2
        protected virtual bool SendMessage(WebRconRequest message)
        {
            return false;
        }

        //protected void OnMessageReceived(RconMessage message)
        //{
        //    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message, false));
        //}
        #endregion

    }
}
