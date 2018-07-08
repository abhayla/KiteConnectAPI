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
    public class Order
    {

        /*
            {"status": "success", "data": [
         * {"exchange_order_id": "1200000002360135", 
         * "disclosed_quantity": 0, 
         * "market_protection": 0, 
         * "tradingsymbol": "RPOWER",
         * "tag": "4ebb4f78",
         * "order_type": "LIMIT",
         * "trigger_price": 0,
         * "cancelled_quantity": 0,
         * "instrument_token": 3906305,
         * "status_message": null,
         * "status": "COMPLETE",
         * "product": "CNC",
         * "exchange": "NSE",
         * "order_id": "170210000303537",
         * "price": 46,
         * "pending_quantity": 0,
         * "validity": "DAY",
         * "placed_by": "RJ0893",
         * "order_timestamp": "2017-02-10 12:30:04",
         * "parent_order_id": null,
         * "exchange_timestamp": "2017-02-10 12:30:04", 
         * "average_price": 45.55,
         * "variety": "regular",
         * "transaction_type": "BUY",
         * "filled_quantity": 1,
         * "quantity": 1}, 
         * {"exchange_order_id": "1200000002395330", "disclosed_quantity": 0, "market_protection": 0, "tradingsymbol": "RPOWER", "tag": "4389969c", "order_type": "LIMIT", "trigger_price": 0, "cancelled_quantity": 1, "instrument_token": 3906305, "status_message": null, "status": "CANCELLED", "product": "CNC", "exchange": "NSE", "order_id": "170210000307481", "price": 45.6, "pending_quantity": 1, "validity": "DAY", "placed_by": "RJ0893", "order_timestamp": "2017-02-10 13:21:57", "parent_order_id": null, "exchange_timestamp": "2017-02-10 13:21:57", "average_price": 0, "variety": "regular", "transaction_type": "SELL", "filled_quantity": 0, "quantity": 1}, {"exchange_order_id": "1200000003479661", "disclosed_quantity": 0, "market_protection": 0, "tradingsymbol": "RPOWER", "tag": "44c44fac", "order_type": "LIMIT", "trigger_price": 0, "cancelled_quantity": 0, "instrument_token": 3906305, "status_message": null, "status": "COMPLETE", "product": "MIS", "exchange": "NSE", "order_id": "170210000424586", "price": 45.95, "pending_quantity": 0, "validity": "DAY", "placed_by": "RJ0893", "order_timestamp": "2017-02-10 14:24:28", "parent_order_id": null, "exchange_timestamp": "2017-02-10 14:24:28", "average_price": 45.4, "variety": "regular", "transaction_type": "BUY", "filled_quantity": 1, "quantity": 1}, {"exchange_order_id": "1200000003589299", "disclosed_quantity": 0, "market_protection": 0, "tradingsymbol": "ONGC", "tag": null, "order_type": "LIMIT", "trigger_price": 0, "cancelled_quantity": 0, "instrument_token": 633601, "status_message": null, "status": "COMPLETE", "product": "MIS", "exchange": "NSE", "order_id": "170210000435606", "price": 193.85, "pending_quantity": 0, "validity": "DAY", "placed_by": "RJ0893", "order_timestamp": "2017-02-10 14:31:09", "parent_order_id": null, "exchange_timestamp": "2017-02-10 14:31:09", "average_price": 193.85, "variety": "regular", "transaction_type": "BUY", "filled_quantity": 1, "quantity": 1}, {"exchange_order_id": "1200000003772313", "disclosed_quantity": 0, "market_protection": 0, "tradingsymbol": "RPOWER", "tag": "6adbd93c", "order_type": "LIMIT", "trigger_price": 0, "cancelled_quantity": 0, "instrument_token": 3906305, "status_message": null, "status": "OPEN", "product": "MIS", "exchange": "NSE", "order_id": "170210000455555", "price": 45.55, "pending_quantity": 1, "validity": "DAY", "placed_by": "RJ0893", "order_timestamp": "2017-02-10 14:46:55", "parent_order_id": null, "exchange_timestamp": "2017-02-10 14:46:55", "average_price": 0, "variety": "regular", "transaction_type": "SELL", "filled_quantity": 0, "quantity": 1}]}
        */


    /// <summary>
    /// Gets or sets the exchange generated order id. Orders that don’t reach the exchange have null ids
    /// </summary>
    [DataMember(Name = "exchange_order_id")]
        public string exchange_order_id { get; set; }

        /// <summary>
        /// Gets or sets the disclosed quantity
        /// </summary>
        [DataMember(Name = "disclosed_quantity")]
        public int disclosed_quantity { get; set; }

        /// <summary>
        /// Gets or sets the market protection
        /// </summary>
        [DataMember(Name = "market_protection")]
        public int market_protection { get; set; }

        /// <summary>
        /// Exchange tradingsymbol of the of the instrument
        /// </summary>
        [DataMember(Name = "tradingsymbol")]
        public string tradingsymbol { get; set; }

        /// <summary>
        /// Custom tag of the order as assigned by the client
        /// </summary>
        [DataMember(Name = "tag")]
        public string tag { get; set; }

        /// <summary>
        /// Gets or sets the order type (LIMIT, MARKET, SL, SL-M)
        /// </summary>
        [DataMember(Name = "order_type")]
        public string order_type { get; set; }

        /// <summary>
        /// Trigger price (for SL, SL-M, CO orders)
        /// </summary>
        [DataMember(Name = "trigger_price")]
        public double trigger_price { get; set; }

        /// <summary>
        /// Gets or sets the cancelled quantity
        /// </summary>
        [DataMember(Name = "cancelled_quantity")]
        public int cancelled_quantity { get; set; }

        /// <summary>
        /// Gets or sets the numerical identifier issued by the exchange representing the instrument. Used for subscribing to live market data over WebSocket
        /// </summary>
        [DataMember(Name = "instrument_token")]
        public int instrument_token { get; set; }

        /// <summary>
        /// Gets or sets textual description of the order’s status. Failed orders come with human readable explanation
        /// </summary>
        [DataMember(Name = "status_message")]
        public string status_message { get; set; }

        /// <summary>
        /// Gets or sets the current status of the order. Most common values or COMPLETE, REJECTED, CANCELLED, and OPEN. There may be other values as well.
        /// PUT ORDER REQ RECEIVED, VALIDATION PENDING
        /// </summary>
        [DataMember(Name = "status")]
        public string status { get; set; }

        /// <summary>
        /// Gets or sets the margin product applied to the order
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
        /// Gets or sets the price at which the order was placed (LIMIT orders)
        /// </summary>
        [DataMember(Name = "price")]
        public double price { get; set; }


        /// <summary>
        /// Gets or sets the pending quantity to be filled (OPEN orders)
        /// </summary>
        [DataMember(Name = "pending_quantity")]
        public int pending_quantity { get; set; }

        /// <summary>
        /// Gets or sets the order validity
        /// </summary>
        [DataMember(Name = "validity")]
        public string validity { get; set; }

        /// <summary>
        /// Gets or sets the id of the user that placed the order. This may different from the user’s id for orders placed outside of Kite, for instance, by dealers at the brokerage using dealer terminals
        /// </summary>
        [DataMember(Name = "placed_by")]
        public string placed_by { get; set; }

        /// <summary>
        /// Gets or sets the timestamp at which the order was registered by the API
        /// </summary>
        [DataMember(Name = "order_timestamp")]
        public string order_timestamp { get; set; }

        /// <summary>
        /// Gets or sets the order id of the parent order (only applicable in case of multi-legged orders like BO and CO)
        /// </summary>
        [DataMember(Name = "parent_order_id")]
        public string parent_order_id { get; set; }

        /// <summary>
        /// Gets or sets the timestamp at which the order was registered by the exchange. Orders that don’t reach the exchange have null timestamps
        /// </summary>
        [DataMember(Name = "exchange_timestamp")]
        public string exchange_timestamp { get; set; }

        /// <summary>
        /// Gets or sets the average price at which the order was executed (only for COMPLETE orders)
        /// </summary>
        [DataMember(Name = "average_price")]
        public double average_price { get; set; }

        /// <summary>
        /// Gets or sets the variety (regular, bo, co)
        /// </summary>
        [DataMember(Name = "variety")]
        public string variety { get; set; }

        /// <summary>
        /// Gets or sets the transaction type (BUY, SELL)
        /// </summary>
        [DataMember(Name = "transaction_type")]
        public string transaction_type { get; set; }


        /// <summary>
        /// Gets or sets the filled quantity (OPEN orders)
        /// </summary>
        [DataMember(Name = "filled_quantity")]
        public int filled_quantity { get; set; }


        /// <summary>
        /// Gets or sets the quantity ordered
        /// </summary>
        [DataMember(Name = "quantity")]
        public int quantity { get; set; }


        public override string ToString()
        {
            return $"Id: {this.order_id} Symbol: {this.tradingsymbol} ({this.exchange}-{this.product}-{this.variety}) Action: {this.transaction_type} Status: {this.status} Quantity: {this.quantity} ({this.filled_quantity}) Price: {this.price} Stop: {this.trigger_price}";
        }
    }
}
