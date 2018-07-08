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
using WebSocket4Net;
using KiteConnectAPI;

namespace KiteConnectAPI
{
    public class KiteWebSocket : Kite
    {
        object locker = new object();

        WebSocket webSocket;
        IKiteLogger logger;
        string apiKey, accessToken, publicToken;
        int maxReconnectionAttempts;
        int reconnectionAttempts;
        DateTime lastUpdateTime = DateTime.MaxValue;
        System.Timers.Timer timer;
        Dictionary<string, List<int>> subscribedQuotes = new Dictionary<string, List<int>>();

        bool isDisconnected = false;
        bool isConnecting = false;

        public KiteWebSocket(string apiKey, string accessToken, string publicToken, int maxReconnectionAttempts = 20, IKiteLogger logger = null)
        {
            this.maxReconnectionAttempts = Math.Max(1, maxReconnectionAttempts);
            this.apiKey = apiKey;
            this.accessToken = accessToken;
            this.publicToken = publicToken;
            this.logger = logger;
        }

        public override async Task ConnectAsync()
        {

            if (this.reconnectionAttempts++ >= this.maxReconnectionAttempts)
            {
                await this.DisconnectAsync().ConfigureAwait(false);
                return;
            }
            if (this.isConnecting)
            {
                await Task.Delay(20000);
            }

            if (this.isDisconnected)
                return;

            this.webSocket = new WebSocket(Url.Websocket(this.apiKey, this.accessToken, this.publicToken)); // ($"wss://ws.kite.trade?api_key={this.apiKey}&access_token={this.accessToken}&public_token={this.publicToken}");
            this.webSocket.Opened += WebSocket_Opened;
            this.webSocket.Closed += WebSocket_Closed;
            this.webSocket.DataReceived += WebSocket_DataReceived;
            this.webSocket.MessageReceived += WebSocket_MessageReceived;
            this.webSocket.Error += WebSocket_Error;
            this.webSocket.Open();
            
        }


        public override async Task DisconnectAsync()
        {
            this.isDisconnected = true;
            this.isConnecting = false;
            this.webSocket?.Close();
        }

        private void WebSocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            try
            {
                this.logger?.OnException(e.Exception);
            }
            catch (Exception ex)
            {

            }
        }

        private void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Postback<OrderPostBack> postback = Kite.ParseString<Postback<OrderPostBack>>(e.Message, logger: this.logger);
            if (postback?.data != null)
            {
                OnPostback(postback.data);
            }
            else
            {
                try
                {
                    this.logger?.OnLog(e.Message);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void WebSocket_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.lastUpdateTime = DateTime.UtcNow;
            OnQuotes(e.Data);
        }

        private async void WebSocket_Closed(object sender, EventArgs e)
        {
            try
            {
                this.logger?.OnLog("Websocket connection closed ....");
            }
            catch (Exception ex)
            {

            }
            if (this.timer != null)
            {
                this.timer.Elapsed -= Timer_Elapsed;
                this.timer.Dispose();
                this.timer = null;
            }
            
            if (this.webSocket != null)
            {
                this.webSocket.Opened -= WebSocket_Opened;
                this.webSocket.Closed -= WebSocket_Closed;
                this.webSocket.DataReceived -= WebSocket_DataReceived;
                this.webSocket.MessageReceived -= WebSocket_MessageReceived;
                this.webSocket.Error -= WebSocket_Error;

                this.webSocket.Dispose();
                this.webSocket = null;
            }
            if (this.isDisconnected)
            {
                OnState(KiteConnectState.Disconnected);
            }
            else
            {
                await this.ConnectAsync().ConfigureAwait(false);
            }
        }

        private async void WebSocket_Opened(object sender, EventArgs e)
        {
            try
            {
                this.logger?.OnLog("Websocket connection opened ....");
            }
            catch (Exception ex)
            {

            }
            this.reconnectionAttempts = 0;
            this.isConnecting = false;
            this.lastUpdateTime = DateTime.MaxValue;

            if (this.timer != null)
            {
                this.timer.Elapsed -= Timer_Elapsed;
                this.timer.Dispose();
                this.timer = null;
            }

            this.timer = new System.Timers.Timer();
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Interval = 1000;
            this.timer.Start();
            
            OnState(KiteConnectState.Connected);

            for (int i = 0; i < this.subscribedQuotes.Count; i++)
            {
                await SendMessageAsync(subscribedQuotes.ElementAt(i).Key, subscribedQuotes.ElementAt(i).Value.ToArray()).ConfigureAwait(false);
            }
            
           
            
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.isConnecting)
                return;

            if (this.lastUpdateTime == DateTime.MaxValue)
                return;

            if (this.lastUpdateTime.AddSeconds(20) < DateTime.UtcNow)
            {
                this.isConnecting = true;
                try
                {
                    this.logger?.OnLog($"No ping detected ({this.webSocket?.State}). Try reconnecting ....");
                }
                catch (Exception ex)
                {

                }

                this.webSocket?.Close();

                

            }
        }

        public override bool IsConnected
        {
            get
            {
                WebSocket webSocket = this.webSocket;
                if (webSocket == null)
                    return false;

                return webSocket.State == WebSocketState.Open;
            }
        }

        public override async Task Subscribe(string mode, int[] tokens)
        {
            await SendMessageAsync(mode, tokens).ConfigureAwait(false);

            lock (locker)
            {
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


        public override async Task Unsubscribe(string mode, int[] tokens)
        {
            await SendMessageAsync(mode, tokens, isSubscribe: false).ConfigureAwait(false);

            lock (locker)
            {
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


            Message<object[]> modeMsg = new Message<object[]>()
            {
                a = Message.mode,
                v = new object[] { mode, tokens }
            };


            string modeMsgString = string.Empty;

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

            try
            {
                this.webSocket.Send(actionString);
                this.webSocket.Send(modeMsgString);
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
}
