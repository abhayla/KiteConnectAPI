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
using KiteConnectAPI;
using SharpCharts.Base.Common;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Navigation;
using System.Reflection;
using System.Windows.Input;
using SharpCharts.Base.Connection;
using SharpCharts.Base.Data;
using SharpCharts.Base.Order;
using System.IO;
using System.Globalization;

namespace KiteConnection
{
    public class KiteViewModel : ViewModelBase
    {
        private object orderLocker = new object();

        //connection Options
        string apiKey, secret;
        bool canDownloadSymbols = false;
        bool isHistoricalDataSubscribed;
        //

        TimeSpan timeZoneOffset = TimeSpan.Zero;
        KiteConnectConnection connection;
        Kite kite;
        Login login;

        Dictionary<string, IOrder> orders;
        List<Instrument> level1Subscriptions;
        Dictionary<uint, KiteQuotes> quotes; 
        Dictionary<string, List<KiteConnectAPI.Symbol>> symbols;
        List<string> trades;

        System.Windows.Threading.Dispatcher dispatcher;
        public KiteViewModel(KiteConnectConnection connection, string apiKey, string secret, bool isHistoricalDataSubscribed, bool canDownloadSymbols)
        {
            this.dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            this.connection = connection;
            this.canDownloadSymbols = canDownloadSymbols;
            this.apiKey = apiKey;
            this.isHistoricalDataSubscribed = isHistoricalDataSubscribed;
            this.secret = secret;

            this.ClosingCommand = new RelayCommand<CancelEventArgs>(OnClosing);
            this.ClosedCommand = new RelayCommand(OnClosed);
            this.WbLoadedCommand = new RelayCommand<WebBrowser>(OnWbLoaded);
            this.WbNavigatingCommand = new RelayCommand<NavigatingCancelEventArgs>(OnWbNavigating);
            this.WbLoadCompletedCommand = new RelayCommand<NavigationEventArgs>(OnWbLoadComplete);
            this.RefreshCommand = new RelayCommand<string>(OnRefresh);

            this.ConvertCommand = new RelayCommand(OnConvertPosition, CanConvertPosition);

            this.KiteOrders = new ObservableImmutableList<KiteConnectAPI.Order>();
            this.KiteTrades = new ObservableImmutableList<KiteConnectAPI.Trade>();
            this.KitePositions = new ObservableImmutableList<KiteConnectAPI.Position>();
            this.KiteFunds = new ObservableImmutableList<KFund>();
            this.KiteHoldings = new ObservableImmutableList<Holding>();

            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(this.KiteOrders, orderLocker);
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(this.KiteTrades, orderLocker);
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(this.KitePositions, orderLocker);
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(this.KiteFunds, orderLocker);
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(this.KiteHoldings, orderLocker);

            this.IsVisible = true;
            
        }


        public async Task SubscribeLevel1(Instrument instrument)
        {
            if (instrument == null)
                return;
            
            Symbol symbol = FromInstrument(instrument);
            if (symbol == null)
                throw new Exception($"No symbol found for instrument {instrument.DisplayName}");

            int token;
            if (!int.TryParse(symbol.instrument_token, NumberStyles.Any, CultureInfo.InvariantCulture, out token))
                throw new Exception($"Failed to parse token for symbol {symbol.tradingsymbol}|{symbol.exchange}");

            Kite kite = this.kite;
            if (kite == null)
                throw new Exception("Kite object is null");

            lock (orderLocker)
            {
                if (this.level1Subscriptions.Contains(instrument))
                    return;

                this.level1Subscriptions.Add(instrument);
                this.quotes.Add((uint)token, new KiteQuotes(instrument, symbol));

            }


            await kite.Subscribe(KiteConnectAPI.Message.full, new int[] { token }).ConfigureAwait(false);
        }

        public async Task SubscribeHistoricalData(Instrument instrument, BuiltDataType builtDataType, BackfillPolicy backfillPolicy, DateTime startDate, DateTime endDate)
        {
            if (!this.isHistoricalDataSubscribed)
                return;

            Symbol symbol = FromInstrument(instrument);
            if (symbol == null)
                throw new Exception($"No symbol found for instrument {instrument.DisplayName}");

            int token;
            if (!int.TryParse(symbol.instrument_token, NumberStyles.Any, CultureInfo.InvariantCulture, out token))
                throw new Exception($"No token found for symbol {symbol.tradingsymbol}|{symbol.exchange}");

            string interval = string.Empty;

            switch (builtDataType)
            {
                case BuiltDataType.Minute:
                    interval = "minute";
                    break;
                case BuiltDataType.Day:
                    interval = "day";
                    break;
                default:
                    throw new Exception($"DataType {builtDataType} not supported");
            }

            Candle candle = Kite.Get<Candle>(this.apiKey, this.login.access_token, KiteConnectAPI.Url.Candles(token, interval, startDate, endDate,
                backfillPolicy == BackfillPolicy.Continuous && instrument.InstrumentDefination.InstrumentType == InstrumentType.Futures));

            if (candle == null)
            {
                throw new Exception($"No data found for instrument {instrument}");
            }

            for (int i = 0; i < candle.candles.GetLength(0); i++)
            {
                DateTime date;
                if (!DateTime.TryParse(candle.candles[i, 0], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                    continue;

                date = date.Add(this.timeZoneOffset);

                double open;
                if (!double.TryParse(candle.candles[i, 1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out open))
                    continue;

                double high;
                if (!double.TryParse(candle.candles[i, 2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out high))
                    continue;

                double low;
                if (!double.TryParse(candle.candles[i, 3], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out low))
                    continue;

                double close;
                if (!double.TryParse(candle.candles[i, 4], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out close))
                    continue;

                int volume;
                if (!int.TryParse(candle.candles[i, 5], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out volume))
                    continue;


                if (builtDataType == BuiltDataType.Minute)
                {
                    ExternalConnectionBase.AppendMinuteData(instrument, date, open, high, low, close, volume, 0L, backfillPolicy);
                }
                else if (builtDataType == BuiltDataType.Day)
                {
                    ExternalConnectionBase.AppendDayData(instrument, date, open, high, low, close, volume, 0L, backfillPolicy);
                }
            }

            //and notify
            ExternalConnectionBase.NotifyHistoricalDataDownloadEnd(instrument, builtDataType, backfillPolicy, startDate, endDate);

        }

        /*
         * Deferred for next release
        public async Task<StaticQuote> GetSnapQuote(Instrument instrument)
        {
            if (instrument == null)
                return null;

            Symbol symbol = FromInstrument(instrument);
            if (symbol == null)
            {
                this.connection?.OnLog($"No symbol found for instrument {instrument.DisplayName}");
                return null;
            }

            Dictionary<string, KiteConnectAPI.Quotes> quotes = Kite.Get<Dictionary<string, KiteConnectAPI.Quotes>>(this.apiKey, this.login?.access_token, 
                KiteConnectAPI.Url.Quote(new string[] { $"{symbol.instrument_token}" }), logger: this.connection);
            
            if (quotes != null && quotes.Count > 0)
            {
                Quotes q = quotes.ElementAt(0).Value;
                if (q == null)
                    return null;

                StaticQuote quote = new StaticQuote()
                {
                    LastTradedPrice = q.last_price,
                    Open = q.ohlc.open,
                    High = q.ohlc.high,
                    Low = q.ohlc.low,
                    PreviousClose = q.ohlc.close,
                    VWAP = q.average_price,
                    TotalVolume = q.volume,
                    OpenInterest = (int)q.oi,
                    LastTradedTime = this.ParseTime(q.timestamp),
                };

                DepthItem[] bids = q.depth.buy;

                if (bids != null)
                {
                    quote.BidPrices = new double[bids.Length];
                    quote.BidVolumes = new int[bids.Length];

                    for (int i = 0; i < bids.Length; i++)
                    {
                        if (bids[i] == null)
                            continue;

                        quote.BidPrices[i] = bids[i].price;
                        quote.BidVolumes[i] = bids[i].quantity;
                    }
                }


                DepthItem[] asks = q.depth.sell;

                if (asks != null)
                {
                    quote.AskPrices = new double[asks.Length];
                    quote.AskVolumes = new int[asks.Length];

                    for (int i = 0; i < asks.Length; i++)
                    {
                        if (asks[i] == null)
                            continue;

                        quote.AskPrices[i] = asks[i].price;
                        quote.AskVolumes[i] = asks[i].quantity;
                    }
                }


                return quote;
            }

            return null;

        }
        */

        public async Task SubmitOrder(IOrder order)
        {
            if (order == null || order.Instrument == null)
                return;

            if (this.login == null)
                throw new ArgumentNullException("Login info is null");

            if (order.Instrument.InstrumentDefination.InstrumentType == InstrumentType.Index)
                throw new Exception("Index cannot be traded");

            
            string tag = Guid.NewGuid().ToString().Replace("-", string.Empty);
            if (tag.Length > 8)
            {
                tag = tag.Substring(0, 8);
            }
            
            string payload = FromOrder(order, tag);

            if (string.IsNullOrEmpty(payload))
            {
                throw new Exception("Failed to submit order");
            }

            this.orders.Add(tag, order);
            ExternalConnectionBase.SetId(order, tag);

            KiteConnectAPI.OrderId response = Kite.Post<KiteConnectAPI.OrderId>(this.apiKey, this.login?.access_token, KiteConnectAPI.Url.PlaceOrder(variety: GetSegment(order.ProductType)), payload, logger: this.connection);

            if (response == null)
            {
                throw new Exception($"Failed to get orderId for order {order}");
            }

            if (!string.IsNullOrEmpty(response.order_id))
            {
                ExternalConnectionBase.SetOrderId(order, response.order_id);
            }
        }

        public async Task ChangeOrder(IOrder order)
        {
            string orderId = order.OrderId as string;

            if (string.IsNullOrEmpty(orderId))
                throw new Exception($"OrderId is null for order {order}");

            string payload = FromOrder(order);

            if (string.IsNullOrEmpty(payload))
                throw new Exception($"Payload is null for order {order}");

            string variety = GetSegment(order.ProductType);

            KiteConnectAPI.OrderId response = Kite.Put<KiteConnectAPI.OrderId>(this.apiKey, this.login?.access_token, KiteConnectAPI.Url.PlaceOrder(variety: variety, orderId: orderId), payload: payload, logger: this.connection);

            if (response == null)
            {
                throw new Exception($"Failed to modify order {order}");
            }

        }

        public async Task CancelOrder(IOrder order)
        {
            string orderId = order.OrderId as string;
            if (string.IsNullOrEmpty(orderId))
                throw new Exception($"OrderId is null for order {order}");

            string variety = GetSegment(order.ProductType);
            KiteConnectAPI.OrderId response = Kite.Delete<KiteConnectAPI.OrderId>(this.apiKey, this.login?.access_token, KiteConnectAPI.Url.PlaceOrder(variety: variety, orderId: orderId), logger: this.connection);

            if (response == null)
                throw new Exception($"Failed to cancel order {order}");
        }

        public void Disconnect()
        {
            Disconnect(ConnectionState.Disconnected);
        }

        private void Disconnect(ConnectionState connectionState)
        {

            Kite kite = this.kite;
            if (kite != null)
            {
                kite.DisconnectAsync();
                kite.Postback -= Kite_Postback;
                kite.State -= Kite_State;
                kite.Quotes -= Kite_Quotes;
                this.kite = null;
            }

            if (!string.IsNullOrEmpty(this.apiKey))
            {
                if (!string.IsNullOrEmpty(this.login?.access_token))
                {
                    bool isLogout = Kite.Delete<bool>(string.Empty, string.Empty, $"{KiteConnectAPI.Url.Token(apiKey: apiKey, accessToken: this.login.access_token)}", logger: this.connection);
                }
            }

            this.IsClosed = true;

            this.connection?.SetConnectionState(connectionState);
        }


        private void OnRefresh(string obj)
        {
            switch (obj)
            {
                case "Orders":
                    GetOrders();
                    break;
                case "Trades":
                    GetTrades();
                    break;
                case "Positions":
                    GetPosition();
                    break;
                case "Accounts":
                    GetFunds();
                    break;
                case "Holdings":
                    GetHoldings();
                    break;
                default:
                    break;
            }
        }


        #region WebBrowser

        private void OnWbNavigating(NavigatingCancelEventArgs obj)
        {
            if (obj == null || obj.Uri == null || string.IsNullOrEmpty(obj.Uri.AbsoluteUri))
                return;

            if (obj.Uri.AbsoluteUri.Contains("127.0.0.1"))
            {

                //"http://127.0.0.1/?request_token=mVi5J61z0n26LIsYioCycR2cfKYxH5b2&action=login&status=success"
                obj.Cancel = true;
                OnLogin(obj.Uri.AbsoluteUri);
                return;
            }
            else if (obj.Uri.AbsoluteUri.Contains("res://ieframe.dll/navcancl.htm"))
            {
                this.Disconnect(ConnectionState.ConnectionFailed);
                return;
            }
        }

        private void OnLogin(string absoluteUri)
        {
            if (this.connection == null)
                return;

            string requestToken, checkSum;
            if (!Kite.IsValidLogin(absoluteUri, this.apiKey, this.secret, out requestToken, out checkSum))
            {
                this.connection.OnLog("Failed to validate");
                this.Disconnect(ConnectionState.ConnectionFailed);
                this.IsClosed = true;
                return;
            }

            this.KiteFunds.Clear();
            this.KiteOrders.Clear();
            this.KiteTrades.Clear();
            this.KitePositions.Clear();
            
            this.IsVisible = false;
            this.IsBusy = true;
            this.BusyText = "Fetching token ....";

            this.login = Kite.Post<Login>(string.Empty, string.Empty, KiteConnectAPI.Url.Token(), payload: Payload.Token(this.apiKey, requestToken, checkSum), logger: this.connection);

            if (this.login == null)
            {
                this.connection.OnLog("Failed to get access token");
                this.Disconnect(ConnectionState.ConnectionFailed);
                IsClosed = true;
                return;
            }

            if (this.login.exchanges == null || this.login.exchanges.Length == 0)
            {
                this.connection.OnLog("No exchange found");
                this.Disconnect(ConnectionState.ConnectionFailed);
                this.IsClosed = false;
                return;
            }

            if (string.IsNullOrEmpty(this.login.user_id))
            {
                this.connection.OnLog("No user id found");
                this.Disconnect(ConnectionState.ConnectionFailed);
                this.IsClosed = true;
                return;
            }

            OrderType[] orderTypes = new OrderType[0];

            for (int i = 0; i < this.login.order_types.Length; i++)
            {
                OrderType orderType = ToOrderType(this.login.order_types[i]);

                orderTypes = SharpCharts.Base.Common.Globals.AddToArray<OrderType>(orderTypes, orderType);
            }

            ProductType[] productTypes = new ProductType[0];
            for (int i = 0; i < this.login.products.Length; i++)
            {
                ProductType productType = ToProductType(this.login.products[i]);
                if (Array.IndexOf<ProductType>(productTypes, productType) < 0)
                {
                    productTypes = SharpCharts.Base.Common.Globals.AddToArray<ProductType>(productTypes, productType);
                }
            }

            ExternalConnectionBase.CreateAccount(this.connection, this.login.user_id.Trim(), "Kite: " + this.login.user_name, orderTypes, new TimeInForce[] { TimeInForce.DAY, TimeInForce.IOC },
                productTypes, new Type[] { typeof(NoneOrderManager), typeof(OneCancelsOther), typeof(CoverOrder) }, false, false, true);


            this.orders = new Dictionary<string, IOrder>();
            this.symbols = new Dictionary<string, List<Symbol>>();
            this.trades = new List<string>();
            this.quotes = new Dictionary<uint, KiteQuotes>();
            this.level1Subscriptions = new List<Instrument>();

            this.timeZoneOffset = SharpCharts.Base.Common.Globals.GetTimeZoneOffset(SharpCharts.Base.Common.TimeZone.IndiaStandardTime);

            Task.Run(async () =>
            {
                for (int i = 0; i < this.login.exchanges.Length; i++)
                {
                    this.BusyText = $"Downloading symbols {this.login.exchanges[i]} ....";
                    GetSymbols(this.login.exchanges[i]);
                }

                this.BusyText = "Fetching orders ....";
                GetOrders();

                this.BusyText = "Fetching trades ....";
                GetTrades();

                this.BusyText = "Fetching positions ....";
                GetPosition();

                this.BusyText = "Fetching holdings ....";
                GetHoldings();

                this.BusyText = "Fetching funds ....";
                GetFunds();

                this.BusyText = "Connecting to Kite sockets ....";
                this.kite = new KiteWebSocket(this.apiKey, this.login?.access_token, this.login?.public_token, logger: this.connection);
                this.kite.State += Kite_State;
                this.kite.Quotes += Kite_Quotes;
                this.kite.Postback += Kite_Postback;

                await this.kite.ConnectAsync().ConfigureAwait(false);
            });

        }

        private void Kite_Postback(PostbackEventArgs args)
        {
            
            Symbol symbol = GetSymbolByTradingSymbol(args.Order.exchange, args.Order.tradingsymbol);
            if (symbol == null)
            {
                this.connection.OnLog($"No symbol found for postback {args.Order}");
                return;
            }

            bool isAddedToList = false;
            IOrder order = null;
            lock (orderLocker)
            {
                if (this.orders == null)
                    return;

                string tag = GetTag(args.Order.tag, args.Order.order_id, args.Order.parent_order_id);
                if (!this.orders.TryGetValue(tag, out order))
                {
                    order = ToOrder(args.Order, symbol);

                    if (order == null)
                        return;

                    isAddedToList = true;

                    this.orders.Add(tag, order);
                }
            }

            if (order != null)
            {

                if (!isAddedToList && args.Order.status == "UPDATE" && order.FilledQuantity > ToQuantity(args.Order.filled_quantity, symbol.lot_size))
                    return;


                if (!isAddedToList && args.Order.status == "UPDATE" && args.Order.filled_quantity == 0)
                    return;

                //Order may get filled, however an stale postback may come through
                if (!isAddedToList && order.IsClosed)
                    return;
                               
                OrderState orderState = ToOrderState(args.Order.status, args.Order.filled_quantity);
                ExternalConnectionBase.UpdateOrder(order, ParseTime(args.Order.order_timestamp), orderState, ToQuantity(args.Order.filled_quantity, symbol.lot_size),
                    ToQuantity(args.Order.pending_quantity, symbol.lot_size), args.Order.price, args.Order.trigger_price, args.Order.average_price, isAddedToList, false);
            }
        }

        private void Kite_Quotes(QuoteEventArgs obj)
        {
            if (obj.Data.Length == 1)
                return;

            if (this.quotes == null)
                return;

            
            BinaryQuotes[] binaryQuotes = BinaryQuotes.Parse(obj.Data);
            if (binaryQuotes == null || binaryQuotes.Length == 0)
                return;

            for (int i = 0; i < binaryQuotes.Length; i++)
            {
                BinaryQuotes bq = binaryQuotes[i];
                if (bq == null)
                    continue;
                
                if (this.quotes.TryGetValue(bq.Token, out KiteQuotes quote))
                {
                    quote.Process(bq);
                }
            }


        }

        private void Kite_State(SocketStateEventArgs obj)
        {
            switch (obj.KiteConnectState)
            {
                case KiteConnectState.Connected:

                    if (this.connection.ConnectionState == ConnectionState.Connecting)
                    {
                        this.IsBusy = false;
                    }
                    else
                    {
                        //Update the orders etc
                        GetOrders();
                        GetTrades();
                        GetPosition();
                    }

                    this.connection?.SetConnectionState(ConnectionState.Connected);

                    break;
                case KiteConnectState.Disconnected:
                    //disconnected by user
                    break;
                case KiteConnectState.ConnectionLost:

                    this.connection?.SetConnectionState(ConnectionState.ConnectionLost);

                    if (this.quotes != null)
                    {
                        for (int i = 0; i < this.quotes.Count; i++)
                        {
                            KiteQuotes quote = this.quotes.ElementAt(i).Value;
                            if (quote == null)
                                continue;

                            quote.HasInitialized = false;
                        }
                    }

                    break;
                case KiteConnectState.ConnectionFailed:

                    this.Disconnect(ConnectionState.ConnectionFailed);

                    break;
                default:
                    break;
            }
        }

        private void OnWbLoadComplete(NavigationEventArgs obj)
        {

        }

        private void OnWbLoaded(WebBrowser wb)
        {
            if (wb == null)
                return;

            FieldInfo field = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
                return;

            object obj = field.GetValue(wb);
            if (obj == null)
                return;

            obj.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, obj, new object[] { true });

            if (string.IsNullOrEmpty(this.apiKey))
            {
                this.IsClosed = true;
                return;
            }

            this.Url = KiteConnectAPI.Url.Login(this.apiKey, "3");
        }

        #endregion


        private void GetSymbols(string exchange)
        {
            if (string.IsNullOrEmpty(exchange))
                return;

            Dictionary<string, List<Symbol>> tmp = this.symbols;
            if (tmp == null)
                return;

            if (tmp.ContainsKey(exchange))
            {
                tmp.Remove(exchange);
            }

            List<Symbol> list = new List<Symbol>();


            string filename = $"{SharpFolder.Misc}Kite_{exchange}.csv";
            if (!this.canDownloadSymbols && File.Exists(filename))
            {
                this.connection.OnLog($"Reading symbol for exchange {exchange}");


                using (StreamReader reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        Symbol symbol = new Symbol();
                        if (symbol.TryParse(line))
                        {
                            list.Add(symbol);
                        }
                    }
                }

                
            }
            else
            {
                Symbol[] symbols = Kite.GetSymbols<Symbol>(this.apiKey, this.login?.access_token, KiteConnectAPI.Url.Instrument(exchange: exchange), filepath: filename, logger: this.connection);
                if (symbols != null)
                {
                    list.AddRange(symbols);
                }
            }

            if (list.Count > 0)
            {
                tmp.Add(exchange, list);
            }
        }

        private string GetTag(string tag, string order_id, string parent_order_id)
        {
            if (string.IsNullOrEmpty(parent_order_id))
            {
                if (string.IsNullOrEmpty(tag))
                    return order_id;
                return tag;
            }
            else
            {
                return order_id;
            }
        }

        private DateTime ParseTime(string date)
        {
            //"2015-12-20 15:01:43"
            if (DateTime.TryParseExact(date, new string[] 
                {
                    "yyyy-MM-dd HH:mm:ss",  //order timestamp
                    "yyyy-MM-dd"            //expiry date
                }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
            {
                return time;
            }

            if (DateTime.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
            {
                return time;
            }

            return DateTime.Now;
        }

        private Symbol GetSymbolByTradingSymbol(string exchange, string tradingSymbol)
        {
            Dictionary<string, List<KiteConnectAPI.Symbol>> tmp = this.symbols;
            if (tmp == null)
                return null;

            if (tmp.TryGetValue(exchange, out List<Symbol> list))
            {
                return list.Where(s => s.tradingsymbol == tradingSymbol).FirstOrDefault();
            }

            return null;
        }

        private Symbol GetSymbolByToken(string exchange, string token)
        {
            Dictionary<string, List<KiteConnectAPI.Symbol>> tmp = this.symbols;
            if (tmp == null)
                return null;

            if (tmp.TryGetValue(exchange, out List<Symbol> list))
            {
                return list.Where(s => s.instrument_token == token).FirstOrDefault();
            }

            return null;
        }

        private Symbol GetSymbol(string exchange, string tradingSymbol, string token)
        {
            Symbol symbol = GetSymbolByTradingSymbol(exchange, tradingSymbol);
            if (symbol == null)
            {
                return GetSymbolByToken(exchange, token);
            }

            return symbol;
        }

        private Instrument ToInstrument(Symbol symbol)
        {
            if (symbol == null)
                return null;

            string name = Symbol.TryParseName(symbol.exchange, symbol.tradingsymbol, out bool isWeekly);

            InstrumentType instrumentType = ToInstrumentType(symbol.instrument_type, symbol.segment);

            Exchange exchange = ToExchange(symbol.exchange);

            if (instrumentType == InstrumentType.Stocks || instrumentType == InstrumentType.Index)
            {
                return ExternalConnectionBase.GetInstrument(this.dispatcher, null, name, instrumentType, exchange, Currency.INR, symbol.tick_size, true);
            }
            else
            {
                DateTime expiryDate = ParseTime(symbol.expiry);

                if (instrumentType == InstrumentType.Options)
                {
                    OptionType optionType = ToOptionType(symbol.instrument_type);
                    double strikePrice = symbol.strike;

                    return ExternalConnectionBase.GetInstrument(this.dispatcher, null, name, name, instrumentType, exchange, Currency.INR, symbol.tick_size, expiryDate, 
                        symbol.lot_size, true, isWeekly, optionType, strikePrice);
                }
                else if (instrumentType == InstrumentType.Futures)
                {
                    return ExternalConnectionBase.GetInstrument(this.dispatcher, null, name, instrumentType, exchange, Currency.INR, symbol.tick_size, expiryDate, symbol.lot_size, true, isWeekly);
                }
                
            }

            return null;

            
        }

        private Symbol FromInstrument(Instrument instrument)
        {
            if (instrument == null)
                return null;

            string exchange = FromExchange(instrument.InstrumentDefination.Exchange, instrument.InstrumentDefination.InstrumentType);
            if (string.IsNullOrEmpty(exchange))
                return null;

            string symbol = ExternalConnectionBase.GetSymbolByDataFeedProvider(instrument, DataFeedProvider.External);
            if (string.IsNullOrEmpty(symbol))
            {
                this.connection.OnLog($"No symbol defined for instrument {instrument.DisplayName}");
                return null;
            }

            if (instrument.InstrumentDefination.InstrumentType == InstrumentType.Futures)
            {
                symbol = string.Format(CultureInfo.InvariantCulture, "{0}{1:yyMMM}FUT", symbol, instrument.ExpiryDate);
            }
            else if (instrument.InstrumentDefination.InstrumentType == InstrumentType.Options)
            {
                string optionType = FromOptionType(instrument.OptionType);
                if (instrument.InstrumentDefination.IsWeeklyContract)
                {
                    symbol = string.Format(CultureInfo.InvariantCulture, "{0}{1:ddMMMyy}{2}{3}", symbol, instrument.ExpiryDate, instrument.StrikePrice, optionType);
                }
                else
                {
                    symbol = string.Format(CultureInfo.InvariantCulture, "{0}{1:yyMMM}{2}{3}", symbol, instrument.ExpiryDate, instrument.StrikePrice, optionType);
                }
            }

            
            return GetSymbol(exchange, symbol.ToUpper(), string.Empty);
        }

        /// <summary>
        /// Gets the string payload of an order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="tag">Entry order tag string</param>
        /// <returns></returns>
        private string FromOrder(IOrder order, string tag = null)
        {
            KiteConnectAPI.Symbol symbol = FromInstrument(order.Instrument);
            if (symbol == null)
                throw new Exception($"No symbol found for instrument {order.Instrument}");

            string orderType = FromOrderType(order.OrderType);
            if (string.IsNullOrEmpty(orderType))
                throw new Exception("Unsupported orderType");

            string duration = FromTimeInForce(order.TimeInForce);
            if (string.IsNullOrEmpty(duration))
                throw new Exception("Unsupported timeInForce");

            string orderAction = FromOrderAction(order.OrderAction);

            string product = FromProductType(order.ProductType, order.Instrument.InstrumentDefination.InstrumentType);

            StringBuilder sb = new StringBuilder();
            sb.Append($"exchange={symbol.exchange}&tradingsymbol={symbol.tradingsymbol}&transaction_type={orderAction}&order_type={orderType}&product={product}&quantity={FromQuantity(order.Quantity, symbol.lot_size)}&price={order.LimitPrice}&validity={duration}");

            if (order.OrderType == OrderType.StopLimit || order.OrderType == OrderType.StopMarket)
            {
                sb.Append($"&trigger_price={order.StopPrice}");
            }

            if (order.Instrument.InstrumentDefination.InstrumentType == InstrumentType.Stocks && order.DisclosedQuantity > 0)
            {
                sb.Append($"&disclosed_quantity={FromQuantity(order.DisclosedQuantity, symbol.lot_size)}");
            }

            if (!string.IsNullOrEmpty(tag))
            {
                sb.Append($"&tag={tag}");

                if (order.ProductType == ProductType.OCO)
                {

                    if (order.OrderType == OrderType.Market || order.OrderType == OrderType.StopMarket)
                        throw new Exception($"{order.OrderType} orderType is not supported with productType {order.ProductType}");

                    OneCancelsOther oco = order.Strategy as OneCancelsOther;
                    if (oco == null)
                        throw new ArgumentNullException("No OCO order template found");

                    double ocoStop = CalculatePrice(oco.CalculationMode, order.LimitPrice, oco.StopLoss, order.Instrument.InstrumentDefination.TickSize);
                    double ocoTarget = CalculatePrice(oco.CalculationMode, order.LimitPrice, oco.Target, order.Instrument.InstrumentDefination.TickSize);

                    sb.Append($"&stoploss={ocoStop}&squareoff={ocoTarget}&trailing_stoploss={(oco.IsTrailing ? oco.TrailingTicks * order.Instrument.InstrumentDefination.TickSize : 0)}");

                }
                else if (order.ProductType == ProductType.CO)
                {
                    if (order.OrderType == OrderType.StopLimit || order.OrderType == OrderType.StopMarket)
                        throw new Exception($"{order.OrderType} orderType is not supported with productType {order.ProductType}");

                    CoverOrder co = order.Strategy as CoverOrder;
                    if (co == null)
                        throw new ArgumentNullException("No CO order template found");
                    
                    sb.Append($"&trigger_price={co.StopLoss}");
                }
            }

            return sb.ToString();
        }


        private IOrder ToOrder(OrderPostBack order, Symbol symbol)
        {
            return ToOrder(order.tag, order.order_id, order.parent_order_id, order.account_id, symbol, order.transaction_type, order.order_type, order.validity, 
                order.product, order.variety, order.quantity, order.price, order.trigger_price, order.disclosed_quantity);
        }

        private IOrder ToOrder(KiteConnectAPI.Order order, Symbol symbol)
        {
            return ToOrder(order.tag, order.order_id, order.parent_order_id, this.login?.user_id, symbol, order.transaction_type, order.order_type, order.validity, 
                order.product, order.variety, order.quantity, order.price, order.trigger_price, order.disclosed_quantity);
        }

        private IOrder ToOrder(string tag, string order_id, string parent_order_id, string accountId, Symbol symbol, string transaction_type, string order_type, string validity, 
            string product, string variety, int quantity, double price, double trigger_price, int disclosed_quantity)
        {
            if (string.IsNullOrEmpty(order_id))
                return null;
            
            Account account = ExternalConnectionBase.GetAccount(accountId);
            if (account == null)
            {
                this.connection.OnLog($"No account found for order {order_id}");
                return null;
            }

            Instrument instrument = ToInstrument(symbol);
            if (instrument == null)
            {
                this.connection.OnLog($"No instrument found for order {order_id}");
                return null;
            }

            OrderAction orderAction = ToOrderAction(transaction_type);
            OrderType orderType = ToOrderType(order_type);
            TimeInForce timeInForce = ToTimeInForce(validity);
            ProductType productType = ToProductType(product, variety);


            IOrder order = ExternalConnectionBase.CreateOrder(GetTag(tag, order_id, parent_order_id), transaction_type, account, instrument, orderAction, orderType,
                ToQuantity(quantity, symbol.lot_size), price, trigger_price, string.Empty,
                ToQuantity(disclosed_quantity, symbol.lot_size), timeInForce, productType);

            if (order != null)
            {
                if (!string.IsNullOrEmpty(order_id))
                {
                    ExternalConnectionBase.SetOrderId(order, order_id);
                }

                if (!string.IsNullOrEmpty(parent_order_id))
                {
                    ExternalConnectionBase.SetParentOrderId(order, parent_order_id);
                }
            }

            return order;


        }

        private double CalculatePrice(CalculationMode calculationMode, double limitPrice, double value, double tickSize)
        {
            switch (calculationMode)
            {
                case CalculationMode.Ticks:
                    return value * tickSize;
                case CalculationMode.Percent:
                    return (int)Math.Round((limitPrice * value / 100) / tickSize, MidpointRounding.AwayFromZero) * tickSize;
                default:
                    return value;
            }
        }


        private OptionType ToOptionType(string instrument_type)
        {

            switch (instrument_type)
            {
                case "PE":
                case "PA":
                    return OptionType.Put;
                default:
                    return OptionType.Call;
            }
        }

        private string FromOptionType(OptionType optionType)
        {
            switch (optionType)
            {
                case OptionType.Put:
                    return "PE";
                default:
                    return "CE";
            }
        }

        private Exchange ToExchange(string exchange)
        {
            if (string.IsNullOrEmpty(exchange))
                return Exchange.Unknown;

            exchange = exchange.ToUpper();

            switch (exchange)
            {
                case "NSE":
                case "NFO":
                    return Exchange.NSE;
                case "MCX":
                    return Exchange.MCX;
                case "CDS":
                    return Exchange.NSECDS;
                case "BSE":
                case "BFO":
                    return Exchange.BSE;
                case "MCXSX":
                    return Exchange.MCXSX;
                default:
                    return Exchange.Unknown;
            }
        }
        

        private string FromExchange(Exchange exchange, InstrumentType instrumentType)
        {
            switch (exchange)
            {
                case Exchange.NSE:
                    if (instrumentType == InstrumentType.Futures || instrumentType == InstrumentType.Options)
                        return "NFO";
                    else return "NSE";
                case Exchange.BSE:
                    if (instrumentType == InstrumentType.Futures || instrumentType == InstrumentType.Options)
                        return "BFO";
                    else return "BSE";
                case Exchange.MCX:
                    return "MCX";
                case Exchange.NSECDS:
                    return "CDS";
                case Exchange.MCXSX:
                    return "MCXSX";
                default:
                    return string.Empty;
            }
        }

        private InstrumentType ToInstrumentType(string instrument_type, string segment)
        {
            switch (instrument_type)
            {
                case "FUT":
                    return InstrumentType.Futures;
                case "CE":
                case "CA":
                case "PA":
                case "PE":
                    return InstrumentType.Options;
                default:
                    if (segment.Contains("INDICES"))
                        return InstrumentType.Index;

                    return InstrumentType.Stocks;
            }
        }

        

        private OrderAction ToOrderAction(string transaction_type)
        {
            if (transaction_type == "SELL")
            {
                return OrderAction.Sell;
            }
            else
                return OrderAction.Buy;
        }

        private string FromOrderAction(OrderAction orderAction)
        {
            switch (orderAction)
            {
                case OrderAction.Sell:
                case OrderAction.SellShort:
                    return "SELL";
                default:
                    return "BUY";
            }
        }

        internal static OrderType ToOrderType(string order_type)
        {
            switch (order_type)
            {
                case "MARKET":
                    return OrderType.Market;
                case "LIMIT":
                    return OrderType.Limit;
                case "SL":
                    return OrderType.StopLimit;
                case "SL-M":
                    return OrderType.StopMarket;
                default:
                    return OrderType.Unknown;
            }
        }

        private static string FromOrderType(OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.Market:
                    return "MARKET";
                case OrderType.Limit:
                    return "LIMIT";
                case OrderType.StopMarket:
                    return "SL-M";
                case OrderType.StopLimit:
                    return "SL";
                case OrderType.Unknown:
                default:
                    return string.Empty;
            }
        }


        private TimeInForce ToTimeInForce(string validity)
        {
            switch (validity)
            {
                case "DAY":
                    return TimeInForce.DAY;
                case "IOC":
                    return TimeInForce.IOC;
                default:
                    return TimeInForce.Unknown;
            }
        }

        private static string FromTimeInForce(TimeInForce tif)
        {
            switch (tif)
            {
                case TimeInForce.DAY:
                    return "DAY";
                case TimeInForce.IOC:
                    return "IOC";
                default:
                    return string.Empty;
            }
        }

        private int ToQuantity(int quantity, int lotSize)
        {
            return  quantity / Math.Max(1, lotSize);
        }

        private int FromQuantity(int quantity, int lot_size)
        {
            return Math.Max(1, quantity * lot_size);
        }
        

        private ProductType ToProductType(string product, string variety = null)
        {
            if (variety == "bo")
            {
                return ProductType.OCO;
            }
            else if (variety == "co")
            {
                return ProductType.CO;
            }
            else
            {

                if (string.IsNullOrEmpty(product))
                    return ProductType.Unknown;

                product = product.ToUpper();

                switch (product)
                {
                    case "CNC":
                    case "NRML":
                        return ProductType.Cash;
                    case "MIS":
                        return ProductType.Margin;
                    case "BO":
                        return ProductType.OCO;
                    case "CO":
                        return ProductType.CO;
                    default:
                        return ProductType.Unknown;
                }
            }
        }

        private string FromProductType(ProductType productType, InstrumentType instrumentType)
        {
            switch (productType)
            {
                case ProductType.Cash:
                    if (instrumentType == InstrumentType.Futures || instrumentType == InstrumentType.Options)
                        return "NRML";
                    return "CNC";
                default:
                    return "MIS";
            }
        }

        private string GetSegment(ProductType productType)
        {
            switch (productType)
            {
                case ProductType.OCO:
                    return "bo";
                case ProductType.CO:
                    return "co";
                default:
                    return "regular";
            }
        }

        //https://kite.trade/forum/discussion/comment/1901/#Comment_1901
        private OrderState ToOrderState(string state, int filledQuantity)
        {
            if (string.IsNullOrEmpty(state))
                return OrderState.Unknown;

            state = state.ToUpper();

            switch (state)
            {
                case "OPEN":
                case "MODIFIED":
                case "TRIGGER PENDING":
                case "UPDATE":
                    if (filledQuantity > 0)
                    {
                        return OrderState.PartFilled;
                    }
                    return OrderState.Working;
                case "OPEN PENDING":
                case "VALIDATION PENDING":
                    return OrderState.PendingSubmit;
                case "MODIFY PENDING":
                case "MODIFY VALIDATION PENDING":
                case "PUT ORDER REQUEST RECEIVED":
                    return OrderState.PendingChange;
                case "CANCEL PENDING":
                case "CANCEL VALIDATION PENDING":
                    return OrderState.PendingCancel;
                case "CANCELLED":
                    return OrderState.Cancelled;
                case "REJECTED":
                    return OrderState.Rejected;
                case "COMPLETE":
                    return OrderState.Filled;
                default:
                    return OrderState.Unknown;
            }
        }

        private void GetOrders()
        {
            Login login = this.login;
            if (login == null || string.IsNullOrEmpty(this.apiKey))
                return;

            if (this.orders == null)
                return;

            KiteConnectAPI.Order[] kiteOrders = Kite.Get<KiteConnectAPI.Order[]>(this.apiKey, login.access_token, KiteConnectAPI.Url.Orders(), logger: this.connection);

            if (kiteOrders == null)
                return;

            this.KiteOrders.Clear();

            for (int i = 0; i < kiteOrders.Length; i++)
            {
                KiteConnectAPI.Order kOrder = kiteOrders[i];
                if (kOrder == null)
                    continue;

                this.KiteOrders.DoAdd(o => kOrder);

                
                Symbol symbol = GetSymbolByTradingSymbol(kOrder.exchange, kOrder.tradingsymbol);
                if (symbol == null)
                {
                    this.connection.OnLog($"No symbol found for order {kOrder}");
                    continue;
                }

                bool isAddedToList = false;
                IOrder order = null;
                lock (orderLocker)
                {
                    if (this.orders == null)
                        continue;

                    if (!this.orders.TryGetValue(GetTag(kOrder.tag, kOrder.order_id, kOrder.parent_order_id), out order))
                    {
                        order = ToOrder(kOrder, symbol);

                        if (order == null)
                            continue;

                        isAddedToList = true;

                        this.orders.Add(GetTag(kOrder.tag, kOrder.order_id, kOrder.parent_order_id), order);
                    }
                }

                if (order != null)
                {
                                        
                    OrderState orderState = ToOrderState(kOrder.status, kOrder.filled_quantity);
                    ExternalConnectionBase.UpdateOrder(order, ParseTime(kOrder.order_timestamp), orderState, ToQuantity(kOrder.filled_quantity, symbol.lot_size), 
                        ToQuantity(kOrder.pending_quantity, symbol.lot_size), kOrder.price, kOrder.trigger_price, kOrder.average_price, isAddedToList, true);
                }
            }
            
        }

        private void GetTrades()
        {
            Login login = this.login;
            if (login == null || string.IsNullOrEmpty(this.apiKey))
                return;

            KiteConnectAPI.Trade[] trades = Kite.Get<KiteConnectAPI.Trade[]>(this.apiKey, login.access_token, KiteConnectAPI.Url.Trades(), logger: this.connection);
            if (trades == null)
                return;

            this.KiteTrades.Clear();

            for (int i = 0; i < trades.Length; i++)
            {
                KiteConnectAPI.Trade kTrade = trades[i];
                if (kTrade == null)
                    continue;

                this.KiteTrades.DoAdd(t => kTrade);

                lock (orderLocker)
                {
                    if (this.trades == null)
                        continue;

                    if (this.trades.Contains(kTrade.trade_id))
                    {
                        continue;
                    }
                    else
                    {
                        this.trades.Add(kTrade.trade_id);
                    }
                }


                IOrder order = this.orders.Where(kvp => kvp.Value.OrderId as string == kTrade.order_id).FirstOrDefault().Value;
                if (order == null)
                {
                    this.connection.OnLog($"Failed to find order for trade {kTrade}");    
                }
                else
                {
                    Symbol symbol = GetSymbol(kTrade.exchange, kTrade.tradingsymbol, kTrade.instrument_token);
                    if (symbol == null)
                    {
                        this.connection.OnLog($"Failed to get symbol for trade {kTrade}");
                        return;
                    }

                    ExternalConnectionBase.FillTrade(order, kTrade.trade_id, ParseTime(kTrade.order_timestamp), ToQuantity(kTrade.quantity, symbol.lot_size), kTrade.average_price);
                }
            }
            
        }

        private void GetPosition()
        {
            Login login = this.login;
            if (login == null || string.IsNullOrEmpty(this.apiKey))
                return;

            KiteConnectAPI.PositionItem positionItem = Kite.Get<KiteConnectAPI.PositionItem>(this.apiKey, login.access_token, KiteConnectAPI.Url.Positions(), logger: this.connection);

            if (positionItem == null || positionItem.net == null)
                return;

            this.KitePositions.Clear();

            for (int i = 0; i < positionItem.net.Length; i++)
            {
                KiteConnectAPI.Position kPosition = positionItem.net[i];
                if (kPosition == null)
                    continue;

                this.KitePositions.DoAdd(p => kPosition);

                Account account = ExternalConnectionBase.GetAccount(login.user_id);
                if (account == null)
                {
                    this.connection.OnLog($"Failed to get account for position {kPosition}");
                    continue;
                }

                Symbol symbol = GetSymbol(kPosition.exchange, kPosition.tradingsymbol, kPosition.instrument_token);
                if (symbol == null)
                {
                    this.connection.OnLog($"Failed to get symbol for position {kPosition}");
                    continue;
                }

                Instrument instrument = ToInstrument(symbol);
                if (instrument == null)
                {
                    this.connection.OnLog($"Failed to get instrument for position {kPosition}");
                    continue;
                }

                ProductType productType = ToProductType(kPosition.product);

                IPosition position = ExternalConnectionBase.AdddOrGetPosition(account, instrument, productType);
                if (position != null)
                {
                    ExternalConnectionBase.UpdatePosition(position, ToQuantity(kPosition.quantity, symbol.lot_size), kPosition.average_price, kPosition.realised);
                }
            }

        }

        private void GetHoldings()
        {
            Login login = this.login;
            if (login == null || string.IsNullOrEmpty(this.apiKey))
                return;

            Holding[] holdings = Kite.Get<Holding[]>(this.apiKey, login.access_token, KiteConnectAPI.Url.Holdings(), logger: this.connection);
            if (holdings == null)
                return;

            this.KiteHoldings.Clear();

            for (int i = 0; i < holdings.Length; i++)
            {
                if (holdings[i] == null)
                    continue;

                this.KiteHoldings.DoAdd(h => holdings[i]);
            }
            
        }

        private void GetFunds()
        {
            Login login = this.login;
            if (login == null || string.IsNullOrEmpty(this.apiKey))
                return;

            this.KiteFunds.Clear();

            KiteConnectAPI.Margins margins = KiteConnectAPI.Kite.Get<KiteConnectAPI.Margins>(this.apiKey, login.access_token, KiteConnectAPI.Url.Margins(), logger: this.connection);

            if (margins == null)
                return;

            this.KiteFunds.DoAdd(f => new KFund("equity", margins.equity));
            this.KiteFunds.DoAdd(f => new KFund("commodity", margins.commodity));
            
        }

        private void OnClosing(CancelEventArgs args)
        {
            if (!this.IsClosed)
                args.Cancel = true;

            System.Windows.Data.BindingOperations.DisableCollectionSynchronization(this.KiteOrders);
            System.Windows.Data.BindingOperations.DisableCollectionSynchronization(this.KiteTrades);
            System.Windows.Data.BindingOperations.DisableCollectionSynchronization(this.KitePositions);
            System.Windows.Data.BindingOperations.DisableCollectionSynchronization(this.KiteFunds);
            System.Windows.Data.BindingOperations.DisableCollectionSynchronization(this.KiteHoldings);
        }

        private void OnClosed()
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvokeShutdown(System.Windows.Threading.DispatcherPriority.Background);
        }


        public ObservableImmutableList<KiteConnectAPI.Order> KiteOrders { get; private set; } 
        public ObservableImmutableList<KiteConnectAPI.Trade> KiteTrades { get; private set; } 
        public ObservableImmutableList<KiteConnectAPI.Position> KitePositions { get; private set; } 
        public ObservableImmutableList<Holding> KiteHoldings { get; private set; } 
        public ObservableImmutableList<KFund> KiteFunds { get; private set; }



        private string url;

        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                NotifyPropertyChanged("Url");
            }
        }


        private bool isClosed;

        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                isClosed = value;
                NotifyPropertyChanged("IsClosed");
            }
        }

        private bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                NotifyPropertyChanged("IsVisible");
            }
        }

        #region BUSY

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }


        private string busyText;

        public string BusyText
        {
            get { return busyText; }
            set
            {
                busyText = value;
                NotifyPropertyChanged("BusyText");
            }
        }


        #endregion



        #region Position converter


        private void OnConvertPosition()
        {
            if (this.login == null)
                return;

            KiteConnectAPI.Position position = this.SelectedPosition;

            if (position == null)
            {
                MessageBox.Show("Please select a position");
                return;
            }

            if (Array.IndexOf<string>(this.Products, this.NewProduct) < 0)
            {
                MessageBox.Show("Invalid product");
                return;
            }

            if (Array.IndexOf<string>(this.PositionTypes, this.PositionType) < 0)
            {
                MessageBox.Show("Invalid position type");
                return;
            }


            if (position.sell_quantity == position.buy_quantity)
            {
                MessageBox.Show("No quantity to convert. Buy Quantity == Sell Quantity");
                return;
            }


            string transaction_type = "BUY";

            if (position.quantity < 0)
            {
                transaction_type = "SELL";
            }



            string position_type = this.PositionType;

            string new_product = this.NewProduct;

            int convertQty = Math.Min(this.ConvertQuantity, Math.Abs(position.quantity));

            bool isSuccess = Kite.Put<bool>(this.apiKey, this.login?.access_token, KiteConnectAPI.Url.Positions(),
                payload: Payload.ConvertPosition(position.exchange, position.tradingsymbol, transaction_type, position_type, convertQty, position.product, new_product), 
                logger: this.connection);

            if (isSuccess)
            {
                GetPosition();
            }

            MessageBox.Show(isSuccess.ToString());
        }

        private bool CanConvertPosition()
        {
            return this.SelectedPosition != null && !string.IsNullOrEmpty(this.NewProduct) && !string.IsNullOrEmpty(this.PositionType);
        }



        private KiteConnectAPI.Position selectedPosition;

        public KiteConnectAPI.Position SelectedPosition
        {
            get { return selectedPosition; }
            set
            {
                selectedPosition = value;
                NotifyPropertyChanged("SelectedPosition");
            }
        }



        public string[] Products
        {
            get
            {
                return new string[] { "MIS", "CNC", "NRML" };
            }
        }

        private string newProduct = "MIS";

        public string NewProduct
        {
            get { return newProduct; }
            set
            {
                newProduct = value;
                NotifyPropertyChanged("NewProduct");
            }
        }

        public string[] PositionTypes
        {
            get
            {
                return new string[] { "overnight", "day" };
            }
        }

        private string positionType = "day";

        public string PositionType
        {
            get { return positionType; }
            set
            {
                positionType = value;
                NotifyPropertyChanged("PositionType");
            }
        }

        private int convertQuantity = 1;

        public int ConvertQuantity
        {
            get { return convertQuantity; }
            set
            {
                convertQuantity = Math.Max(1, value);
                NotifyPropertyChanged("ConvertQuantity");
            }
        }


        public ICommand ConvertCommand { get; private set; }

        #endregion

        public ICommand ClosingCommand { get; private set; } 
        public ICommand ClosedCommand { get; private set; } 
        public ICommand WbLoadedCommand { get; private set; }
        public ICommand WbNavigatingCommand { get; private set; }
        public ICommand WbLoadCompletedCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }


        

    }
}
