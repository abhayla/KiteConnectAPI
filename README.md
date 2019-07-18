# Kite Connect API v3
.Net client of the [Kite Connect API](https://kite.trade/) version 3

## Requirements
- .Net Framework 4.5.1 and above
- Windows 7 and above

## Dependencies
- [WebSocket4Net](https://www.nuget.org/packages/WebSocket4Net) (Version 0.15.2)

## Usage

### HTTP Requests

The Kite Connect API provides a simple way to interact with the Kite interface. All the http requests can be made by 5 methods. While the first 4 methods as listed below are used to make Get, Post, Put and Delete calls the GetSymbols() method is called to get the CSV dumps of the instruments.

```csharp

Kite.Get<T>(string apiKey, string accessToken, string url, IKiteLogger logger = null)

Kite.Post<T>(string apiKey, string accessToken, string url, string payload, IKiteLogger logger = null)

Kite.Put<T>(string apiKey, string accessToken, string url, string payload = null, IKiteLogger logger = null)

Kite.Delete<T>(string apiKey, string accessToken, string url, IKiteLogger logger = null)

Kite.GetSymbols<T>(string apiKey, string accessToken, string url, string filepath = null, IKiteLogger logger = null) where T : SymbolBase, new()

```
An usage of the same will be like

```csharp
//get the token
Login this.login = Kite.Post<Login>(string.Empty, string.Empty, KiteConnectAPI.Url.Token(), payload: Payload.Token(apiKey, requestToken, checkSum), logger: this);

//download the symbols
Symbol[] symbols = Kite.GetSymbols<Symbol>(apiKey, login?.access_token, Url.Instrument("NSE"), logger: this);

//get quotes
Dictionary<string, OHLCQuotes> quotes = Kite.Get<Dictionary<string, OHLCQuotes>>(apiKey, this.login?.access_token, Url.OHLC(new string[] 
                {
                    "NSE:INFY",
                    "BSE:INFY",
                    "NSE:TVSMOTOR"
                }), logger: this);
                
//call to get the order list
Order[] orders = Kite.Get<Order[]>(apiKey, login?.access_token, Url.Orders());

//call to place an order
string payload = Payload.PlaceOrder("NSE", "RELIANCE", "BUY", "LIMIT", "MIS", 1, 970.0d, 0.0d);
OrderId orderId = Kite.Post<OrderId>(apiKey, login?.access_token, KiteConnectAPI.Url.PlaceOrder(), payload, logger: this);

//call to get the trade list
Trade[] trades = Kite.Get<Trade[]>(apiKey, login?.access_token, Url.Trades());
```

### Connecting to the socket

For streaming quotes and order postbacks user can easily connect with the websocket.

```csharp
//crete the kite object
Kite kite = new KiteWebSocket(apiKey, this.login.access_token, login.public_token, maxReconnectionAttempts: 20, logger: this);
//subscribe to events
kite.State += OnConnectionState;
kite.Quotes += OnQuotes;
kite.Postback += OnPostback;

//and connect to the socket
kite.ConnectAsync();

//subscribe to quote(s)
kite.Subscribe(Message.subscribe, new int[] { 738561 }); //Reliance

//unsubscribe from quote(s)
kite.Unsubscribe(Message.unsubscribe, new int[] { 738561});

//disconnect from the kite socket
kite.DisconnectAsync();

//listen to the event
private void OnQuotes(QuoteEventArgs args)
{
	//parse the data
	BinaryQuotes[] quotes = BinaryQuotes.Parse(args.Data);
	//do other stuff
}
```

While a basic usage is demonstrated in the [Test App](TestApp) along with this project, including the oAuth login flow, a detailed implementation can be found in [Kite Connect](KiteConnect) project. The Kite Connect project is part of the [external connection](http://www.arthachitra.com/support/ac1/Help/html/81a233b3-ab6f-487b-833a-a0b81fe5b51d.htm) and connects to our standalone charting and trading platform [ArthaChitra](http://www.arthachitra.com). For more details regarding the Kite Conection Connection please refer [here](http://www.arthachitra.com/support/forum/viewtopic.php?f=28&t=61)

