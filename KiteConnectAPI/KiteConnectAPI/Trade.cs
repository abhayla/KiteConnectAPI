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
using System.Runtime.Serialization;

namespace KiteConnectAPI
{
    [DataContract]
    public class Trade
    {
        /*
            {"status": "success", "data": [{
         * "product": "MIS",
         * "exchange": "NSE",
         * "order_id": "170210000303537",
         * "exchange_order_id": "1200000002360135",
         * "order_timestamp": "2017-02-10 12:30:04",
         * "trade_id": "50985534",
         * "exchange_timestamp": "2017-02-10 12:30:04",
         * "average_price": 45.55,
         * "tradingsymbol": "RPOWER",
         * "transaction_type": "BUY",
         * "instrument_token": 3906305,
         * "quantity": 1}
         * , {"product": "MIS", "exchange": "NSE", "order_id": "170210000424586", "exchange_order_id": "1200000003479661", "order_timestamp": "2017-02-10 14:24:28", "trade_id": "51484737", "exchange_timestamp": "2017-02-10 14:24:28", "average_price": 45.4, "tradingsymbol": "RPOWER", "transaction_type": "BUY", "instrument_token": 3906305, "quantity": 1}, {"product": "MIS", "exchange": "NSE", "order_id": "170210000435606", "exchange_order_id": "1200000003589299", "order_timestamp": "2017-02-10 14:31:09", "trade_id": "51519146", "exchange_timestamp": "2017-02-10 14:31:09", "average_price": 193.85, "tradingsymbol": "ONGC", "transaction_type": "BUY", "instrument_token": 633601, "quantity": 1}]}
        */

        /// <summary>
        /// Margin product applied to the order
        /// </summary>
        [DataMember(Name = "product")]
        public string product { get; set; }

        /// <summary>
        /// Gets or sets the exchange
        /// </summary>
        [DataMember(Name = "exchange")]
        public string exchange { get; set; }


        /// <summary>
        /// Gets or sets the unique order id
        /// </summary>
        [DataMember(Name = "order_id")]
        public string order_id { get; set; }

        /// <summary>
        /// Gets or sets the exchange generated order id
        /// </summary>
        [DataMember(Name = "exchange_order_id")]
        public string exchange_order_id { get; set; }

        /// <summary>
        /// Gets or sets the timestamp at which the order was registered by the API
        /// </summary>
        [DataMember(Name = "fill_timestamp")]
        public string fill_timestamp { get; set; }

        /// <summary>
        /// Gets or sets the exchange generated trade id
        /// </summary>
        [DataMember(Name = "trade_id")]
        public string trade_id { get; set; }

        /// <summary>
        /// Gets or sets the timestamp at which the order was registered by the exchange. Orders that don’t reach the exchange have null timestamps
        /// </summary>
        [DataMember(Name = "exchange_timestamp")]
        public string exchange_timestamp { get; set; }

        /// <summary>
        /// Gets or sets the order timestamp
        /// </summary>
        [DataMember(Name = "order_timestamp")]
        public string order_timestamp { get; set; }

        /// <summary>
        /// Gets or sets the price at which the was filled
        /// </summary>
        [DataMember(Name = "average_price")]
        public double average_price { get; set; }


        /// <summary>
        /// Gets or sets the exchange tradingsymbol 
        /// </summary>
        [DataMember(Name = "tradingsymbol")]
        public string tradingsymbol { get; set; }


        /// <summary>
        /// Gets or sets the transaction type (BUY or SELL)
        /// </summary>
        [DataMember(Name = "transaction_type")]
        public string transaction_type { get; set; }


        /// <summary>
        /// Gets or sets the numerical identifier issued by the exchange representing the instrument. Used for subscribing to live market data over WebSocket
        /// </summary>
        [DataMember(Name = "instrument_token")]
        public string instrument_token { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        [DataMember(Name = "quantity")]
        public int quantity { get; set; }

        public override string ToString()
        {
            return $"Id: {this.trade_id} Symbol: {this.tradingsymbol} ({this.exchange}-{this.product}) Action: {this.transaction_type} Quantity: {this.quantity} Price: {this.average_price}";
        }

    }
}
