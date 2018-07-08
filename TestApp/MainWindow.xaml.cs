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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KiteConnectAPI;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IKiteLogger, INotifyPropertyChanged
    {
        private const string apiKey = "enter api key";
        private const string secret = "enter api secret";


        public event PropertyChangedEventHandler PropertyChanged;
        
        
        public MainWindow()
        {
            InitializeComponent();
            this.Logs = new ObservableCollection<string>();
            DataContext = this;
        }

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<string> Logs { get; private set; }

       
        private string connectStr = "Connect";

        public string ConnectStr
        {
            get { return connectStr; }
            set
            {
                connectStr = value;
                NotifyPropertyChanged();
            }
        }



        private int selectedTab = 0;

        public int SelectedTab
        {
            get { return selectedTab; }
            set
            {
                selectedTab = value;
                NotifyPropertyChanged();
            }
        }


        private Kite kite;
        private Login login;

        private void Log(string msg, [CallerMemberName]string methodName = null)
        {
            if (string.IsNullOrEmpty(msg))
                return;

            if (this.Dispatcher.CheckAccess())
            {
                this.Logs.Add($"{methodName}: {msg}");
            }
            else
            {
                this.Dispatcher.InvokeAsync(() =>
                {
                    Log(msg, methodName);
                });
            }
        }
       
        
        


        #region IKiteWrapper

        public void OnLog(string message)
        {
            Log(message);
        }

        public void OnException(Exception ex)
        {
            Log(ex.Message);
        }


        #endregion

        private void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs obj)
        {
            if (obj == null || obj.Uri == null || string.IsNullOrEmpty(obj.Uri.AbsoluteUri))
                return;

            if (obj.Uri.AbsoluteUri.Contains("127.0.0.1"))
            {

                obj.Cancel = true;
                OnLogin(obj.Uri.AbsoluteUri);
                return;
            }
            else if (obj.Uri.AbsoluteUri.Contains("res://ieframe.dll/navcancl.htm"))
            {
                this.Log("Login failed. Please make sure you have entered a valid kite connect api key and api secret");
                this.SelectedTab = 2;
                return;
            }
        }

        private void OnLogin(string absoluteUri)
        {
            string requestToken, checkSum;
            if (!Kite.IsValidLogin(absoluteUri, apiKey, secret, out requestToken, out checkSum))
            {
                this.OnLog("Failed to validate");
                return;
            }

            this.login = Kite.Post<Login>(string.Empty, string.Empty, KiteConnectAPI.Url.Token(), payload: Payload.Token(apiKey, requestToken, checkSum), logger: this);

            if (this.login == null)
            {
                this.OnLog("Failed to get access token");
                return;
            }

            this.SelectedTab = 1;
        }




        private void WebBrowser_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            WebBrowser wb = sender as WebBrowser;

            if (wb == null)
                return;

            FieldInfo field = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
                return;

            object obj = field.GetValue(wb);
            if (obj == null)
                return;

            obj.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, obj, new object[] { true });

            if (string.IsNullOrEmpty(apiKey))
            {
                return;
            }

            wb.Navigate(new Uri(KiteConnectAPI.Url.Login(apiKey, "3")));
        }

        private void Button_Connect(object sender, RoutedEventArgs e)
        {
            if (this.login == null)
                return;

            if (this.ConnectStr == "Connect")
            {
                this.kite = new KiteWebSocket(apiKey, this.login.access_token, login.public_token, maxReconnectionAttempts: 20, logger: this);
                this.kite.State += OnConnectionState;
                this.kite.Quotes += OnQuotes;
                this.kite.Postback += OnPostback;

                this.kite.ConnectAsync();
            }
            else if (this.ConnectStr == "Disconnect")
            {
                this.kite?.DisconnectAsync();
            }
            
        }

        private void OnConnectionState(SocketStateEventArgs obj)
        {
            if (obj.KiteConnectState == KiteConnectState.Connected)
            {
                this.ConnectStr = "Disconnect";
            }
            else if (obj.KiteConnectState == KiteConnectState.Disconnected)
            {

                string isLogout = Kite.Delete<string>(apiKey, login?.access_token, Url.Token(apiKey: apiKey, accessToken: login?.access_token), logger: this);
                Log($"logout = {isLogout}");

                this.ConnectStr = "Connect";
            }
            Log(obj.KiteConnectState.ToString());
        }

        private void OnQuotes(QuoteEventArgs obj)
        {
            if (obj.Data.Length == 1)
            {
                Log("ping ....");
                return;
            }

            BinaryQuotes[] quotes = BinaryQuotes.Parse(obj.Data);

            for (int i = 0; i < quotes.Length; i++)
            {
                Level2 l2 = quotes[i] as Level2;
                if (l2 == null)
                    continue;
                Log($"LTT= {l2.LastTradedTime}, TimeStamp= {l2.TimeStamp}");
            }


        }

        private void OnPostback(PostbackEventArgs obj)
        {
            Log($"Postback received {obj.Order}");
        }


        private void Button_SubscribeRT(object sender, RoutedEventArgs e)
        {
            this.kite?.Subscribe("full", new int[] { 738561 }); //Reliance
            //this.kite.Subscribe("quote", new int[] { 53767943, 53490183 }); //Crude, Silver
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.login == null)
                return;

            Symbol[] symbols = Kite.GetSymbols<Symbol>(apiKey, login.access_token, $"{Url.Instrument("MCX")}", logger: this);

            Position[] position = Kite.Get<Position[]>(apiKey, login.access_token, Url.Positions(), logger: this);
            Holding[] holding = Kite.Get<Holding[]>(apiKey, login.access_token, Url.Holdings(), logger: this);
            Order[] orders = Kite.Get<Order[]>(apiKey, login.access_token, Url.Orders(), logger: this);
            Trade[] trades = Kite.Get<Trade[]>(apiKey, login.access_token, Url.Trades(), logger: this);

            Dictionary<string, OHLCQuotes> quotes = Kite.Get<Dictionary<string, OHLCQuotes>>(apiKey, this.login.access_token, Url.OHLC(new string[] 
                {
                    "NSE:INFY",
                    "BSE:INFY",
                    "NSE:TVSMOTOR"
                }), logger: this);
        }

        OrderId orderId = null;

        private void Button_Buy(object sender, RoutedEventArgs e)
        {
            if (this.login == null)
                return;

            string tag = Guid.NewGuid().ToString().Replace("-", string.Empty);
            if (tag.Length > 8)
            {
                tag = tag.Substring(0, 8);
            }

            string payload = Payload.PlaceOrder("NSE", "RELIANCE", "BUY", "LIMIT", "MIS", 1, 970.0d, 0.0d);
            this.orderId = Kite.Post<OrderId>(apiKey, login.access_token, KiteConnectAPI.Url.PlaceOrder(), payload, logger: this);

        }

        private void Button_Modify(object sender, RoutedEventArgs e)
        {
            if (this.login == null || this.orderId == null)
                return;

            string payload = Payload.ModifyOrder("LIMIT", 2, 965.0d, 0.0d);
            OrderId response = Kite.Put<OrderId>(apiKey, login.access_token, Url.PlaceOrder(orderId: this.orderId.order_id), payload: payload, logger: this);
            Log($"modify: {response?.order_id}");
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            if (this.login == null || this.orderId == null)
                return;

            OrderId response = Kite.Delete<OrderId>(apiKey, login.access_token, Url.PlaceOrder(orderId: this.orderId.order_id), logger: this);
            Log($"cancel: {response?.order_id}");
            
        }
    }
}
