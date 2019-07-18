/*
        ARTHACHITRA MAKES NO REPRESENTATION ABOUT THE SUITABILITY OF THIS SOURCE CODE FOR ANY 
        PURPOSE. IT IS PROVIDED "AS IS" WITHOUT EXPRESS OR IMPLIED WARRANTY OF ANY KIND. ARTHACHITRA DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS SOURCE CODE, 
        INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY, NON-INFRINGEMENT, AND FITNESS FOR A PARTICULAR PURPOSE. IN NO EVENT SHALL ARTHACHITRA BE LIABLE FOR ANY 
        SPECIAL, INDIRECT, INCEDENTAL, OR CONSEQUENTIAL DAMAGES, OR ANY DAMAGES WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION, ARISING 
        OUT OF OR IN CONNECTION WITH THE USE OR PERFORMACE OF THIS SOURCE CODE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Net.WebSockets;

namespace KiteConnectAPI
{
    public class KiteClientWebSocket : Kite
    {
        private object locker = new object();


        string apiKey = string.Empty;
        string accessToken = string.Empty;
        IKiteLogger logger;
        System.Timers.Timer timer = null;
        int maxReconnectionAttempts = 20;
        int reconnectionAttempts = 0;
        bool isTimerBusy = false;
        int reconnectionTime = 1; //seconds
        DateTime lastUpdateTime;
        ClientWebSocket socket = null;
        Queue<Message<int[]>> queue = new Queue<Message<int[]>>();
        //in case of disconnection use this to subscribe again
        Dictionary<string, List<int>> subscribedQuotes = new Dictionary<string, List<int>>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Optional Logger</param>
        public KiteClientWebSocket(string apiKey, string accessToken, int maxReconnectionAttempts = 20, IKiteLogger logger = null)
        {
            this.maxReconnectionAttempts = Math.Max(1, maxReconnectionAttempts);
            this.apiKey = apiKey;
            this.accessToken = accessToken;
            this.logger = logger;
        }




        /// <summary>
        /// Connect to kite socket
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <returns></returns>
        public override async Task ConnectAsync()
        {

            try
            {
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new ArgumentNullException("Api key is null");
                }

                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new ArgumentNullException("Access token is null");
                }

            }
            catch (Exception ex)
            {
                try
                {
                    this.logger?.OnException(ex);
                }
                catch (Exception ex1)
                {

                }

                OnState(KiteConnectState.ConnectionFailed);
                return;
            }

            this.socket?.Dispose();
            this.socket = null;
            try
            {
                if (this.reconnectionAttempts++ >= this.maxReconnectionAttempts)
                {
                    try
                    {
                        this.logger?.OnLog("Max reconnection attempted reached");
                    }
                    catch (Exception ex)
                    {

                    }

                    OnState(KiteConnectState.ConnectionFailed);
                    return;
                }

                try
                {
                    this.logger?.OnLog($"Try connecting ({this.reconnectionAttempts}) to websocket ....");
                }
                catch (Exception ex1)
                {

                }

                lock (locker)
                {
                    this.queue.Clear();
                }

                this.socket = new ClientWebSocket();
                await this.socket.ConnectAsync(new Uri($"wss://ws.kite.trade?api_key={this.apiKey}&access_token={this.accessToken}"), CancellationToken.None).ConfigureAwait(false);
                ReceiveAsync();
                this.reconnectionAttempts = 0;

                //if disconnected then subscribed to quotes
                lock (locker)
                {
                    foreach (var item in this.subscribedQuotes)
                    {
                        this.queue.Enqueue(new Message<int[]>()
                        {
                            a = item.Key,
                            v = item.Value.ToArray(),
                            IsSubscribe = true
                        });
                    }
                }

                OnState(KiteConnectState.Connected);

            }
            catch (Exception ex)
            {
                try
                {
                    this.logger?.OnException(ex);
                }
                catch (Exception ex1)
                {

                }

                await Task.Delay(TimeSpan.FromSeconds(this.reconnectionTime * 15)).ConfigureAwait(false);

                await ConnectAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Disconnect from socket
        /// </summary>
        /// <returns></returns>
        public override async Task DisconnectAsync()
        {

            System.Timers.Timer tmp = this.timer;
            if (tmp != null)
            {
                tmp.Stop();
                tmp.Dispose();
            }

            ClientWebSocket tmpSocket = this.socket;
            if (tmpSocket != null && tmpSocket.State == WebSocketState.Open)
            {
                try
                {
                    await tmpSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    try
                    {
                        this.logger?.OnException(ex);
                    }
                    catch (Exception ex1)
                    {

                    }
                }
            }
        }


        private void InitializeTimer()
        {
            this.timer = new System.Timers.Timer();
            this.timer.Interval = reconnectionTime * 1000;
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Start();
        }





        private async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.isTimerBusy)
                return;

            this.isTimerBusy = true;

            if (this.socket?.State != WebSocketState.Open)
            {
                try
                {
                    this.logger?.OnLog($"Connection state = {this.socket?.State}");
                }
                catch (Exception ex)
                {

                }
            }


            if (this.socket?.State == WebSocketState.Aborted)
            //if (this.lastUpdateTime.AddSeconds(this.reconnectionTime * 20) < DateTime.UtcNow) //Abort state can take some time. Alt will be check time
            {
                OnState(KiteConnectState.ConnectionLost);
                this.logger?.OnLog($"SocketState ={this.socket?.State} ....");
                try
                {
                    await ConnectAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    try
                    {
                        this.logger?.OnException(ex);
                    }
                    catch (Exception ex1)
                    {

                    }
                }

                this.isTimerBusy = false;
                return;
            }


            List<Message<int[]>> tmp = new List<Message<int[]>>();

            lock (locker)
            {
                for (int i = 0; i < this.queue.Count; i++)
                {
                    tmp.Add(this.queue.Dequeue());
                }
            }

            for (int i = 0; i < tmp.Count; i++)
            {
                if (this.socket?.State != WebSocketState.Open)
                    break;

                Message<int[]> message = tmp[i];
                if (message == null)
                    continue;

                await this.SendMessageAsync(message.a, message.v, message.IsSubscribe).ConfigureAwait(false);

            }


            this.isTimerBusy = false;
        }

        
        private async void ReceiveAsync()
        {

            this.lastUpdateTime = DateTime.UtcNow;

            if (this.timer == null)
            {
                InitializeTimer();
            }

            try
            {
                ClientWebSocket tmpSocket = this.socket;
                if (tmpSocket == null)
                    return;

                while (tmpSocket.State == WebSocketState.Open || tmpSocket.State == WebSocketState.CloseSent)
                {
                    WebSocketReceiveResult result;
                    byte[] buffer = new byte[64 * 1024];
                    int offset = 0;

                    do
                    {
                        byte[] tmpBytes = new byte[64 * 1024];
                        result = await tmpSocket.ReceiveAsync(new ArraySegment<byte>(tmpBytes), CancellationToken.None).ConfigureAwait(false);
                        int count = result.Count;

                        if ((offset + count) > buffer.Length)
                        {
                            byte[] tmp = buffer;
                            buffer = new byte[buffer.Length + 64 * 1024];
                            Array.Copy(tmp, buffer, tmp.Length);
                        }

                        Array.Copy(tmpBytes, 0, buffer, offset, count);
                        offset += count;

                    } while (!result.EndOfMessage);


                    if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        byte[] data = new byte[offset];
                        Array.Copy(buffer, 0, data, 0, offset);

                        OnQuotes(data);
                    }
                    else if (result.MessageType == WebSocketMessageType.Text)
                    {

                        //parse the json msg
                        byte[] data = new byte[offset];
                        Array.Copy(buffer, 0, data, 0, offset);

                        Postback<OrderPostBack> orderPostback = Kite.ParseBytes<Postback<OrderPostBack>>(data, logger: this.logger);
                        if (orderPostback?.data != null)
                        {
                            OnPostback(orderPostback.data);
                        }

                        try
                        {
                            this.logger?.OnLog($"{Encoding.UTF8.GetString(data)}");
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                this.logger?.OnException(ex);
                            }
                            catch (Exception ex1)
                            {

                            }
                        }
                        
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        try
                        {
                            await tmpSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                this.logger?.OnException(ex);
                            }
                            catch (Exception ex1)
                            {

                            }
                        }

                        OnState(KiteConnectState.Disconnected);
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                try
                {
                    this.logger?.OnException(ex);
                }
                catch (Exception ex1)
                {

                }
            }
        }


        /// <summary>
        /// Send data subscription message
        /// </summary>
        /// <param name="mode">Mode</param>
        /// <param name="tokens">Symbol tokens</param>
        /// <param name="isSubscribe">Is data subscribe or unstubscribed. Default is subscribed</param>
        /// <returns></returns>
        private async Task SendMessageAsync(string mode, int[] tokens, bool isSubscribe = true)
        {

            if (tokens == null)
            {
                try
                {
                    this.logger?.OnException(new ArgumentNullException("token is null"));
                }
                catch (Exception ex)
                {

                }
                return;
            }

            if (string.IsNullOrWhiteSpace(mode))
            {
                try
                {
                    this.logger?.OnException(new ArgumentNullException("mode is null"));
                }
                catch (Exception ex)
                {

                }
                return;
            }

            Message<int[]> subscribeMsg = new Message<int[]>()
            {
                a = isSubscribe ? Message.subscribe : Message.unsubscribe,
                v = tokens
            };

            string actionString = string.Empty;
            try
            {
                actionString = subscribeMsg.Serialize();
            }
            catch (Exception ex)
            {
                try
                {
                    this.logger?.OnException(ex);
                }
                catch (Exception ex1)
                {

                }
                return;
            }

            if (string.IsNullOrEmpty(actionString))
            {
                try
                {
                    this.logger?.OnException(new ArgumentNullException("action json string is null"));
                }
                catch (Exception ex)
                {

                }
                return;
            }

            try
            {
                this.logger?.OnLog(actionString);
            }
            catch (Exception ex)
            {

            }


            string modeMsgString = string.Empty;

            if (isSubscribe)
            {
                Message<object[]> modeMsg = new Message<object[]>()
                {
                    a = Message.mode,
                    v = new object[] { mode, tokens }
                };




                try
                {
                    modeMsgString = modeMsg.Serialize();
                }
                catch (Exception ex)
                {
                    try
                    {
                        this.logger?.OnException(ex);
                    }
                    catch (Exception ex1)
                    {

                    }
                    return;
                }

                if (string.IsNullOrEmpty(modeMsgString))
                {
                    try
                    {
                        this.logger?.OnException(new ArgumentNullException("Mode json string is null"));
                    }
                    catch (Exception ex)
                    {

                    }
                    return;
                }

                try
                {
                    this.logger?.OnLog(modeMsgString);
                }
                catch (Exception ex)
                {

                }
            }

            try
            {
                await this.socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(actionString)), WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false); //send the subscription msg
                if (isSubscribe)
                {
                    await this.socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(modeMsgString)), WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false); //send the mode msg
                }
            }
            catch (Exception ex)
            {
                try
                {
                    this.logger?.OnException(ex);
                }
                catch (Exception ex1)
                {

                }
            }
        }

        /// <summary>
        /// Subscribe market data
        /// </summary>
        /// <param name="mode">Mode</param>
        /// <param name="tokens">Instrument tokens</param>
        /// <returns></returns>
        public override async Task Subscribe(string mode, int[] tokens)
        {
            if (string.IsNullOrEmpty(mode) || tokens == null)
                return;

            Message<int[]> msg = new Message<int[]>()
            {
                a = mode,
                v = tokens,
                IsSubscribe = true
            };

            lock (locker)
            {
                this.queue.Enqueue(msg);

                List<int> tokenList = null;
                this.subscribedQuotes.TryGetValue(mode, out tokenList);
                if (tokenList == null)
                {
                    this.subscribedQuotes.Add(mode, tokenList = new List<int>());
                }

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (!tokenList.Contains(tokens[i]))
                    {
                        tokenList.Add(tokens[i]);
                    }
                }

            }
        }

        /// <summary>
        /// Unsubscribe market data
        /// </summary>
        /// <param name="mode">Mode</param>
        /// <param name="tokens">Instrument tokens</param>
        /// <returns></returns>
        public override async Task Unsubscribe(string mode, int[] tokens)
        {
            if (string.IsNullOrEmpty(mode) || tokens == null)
                return;

            lock (locker)
            {
                this.queue.Enqueue(new Message<int[]>()
                {
                    a = mode,
                    v = tokens,
                    IsSubscribe = false
                });

                if (this.subscribedQuotes.TryGetValue(mode, out List<int> list))
                {
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (list.Contains(tokens[i]))
                        {
                            list.Remove(tokens[i]);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Gets if the socket is connected or not
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                WebSocket socket = this.socket;

                if (socket == null)
                    return false;

                return socket.State == WebSocketState.Open;
            }
        }



    }
}
