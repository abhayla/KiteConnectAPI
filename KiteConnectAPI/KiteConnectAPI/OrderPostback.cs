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
using System.Security.Cryptography;

namespace KiteConnectAPI
{
    [DataContract]
    public class OrderPostBack
    {
        /*
        {
            "type":"order",
            "data":
            {
                "placed_by":"RJ0893",
                "order_id":"180628001243893",
                "exchange_order_id":"1200000003982277",
                "parent_order_id":null,
                "status":"OPEN",
                "status_message":null,
                "order_timestamp":"2018-06-28 12:44:36",
                "exchange_update_timestamp":null,
                "exchange_timestamp":"2018-06-28 12:44:36",
                "variety":"regular",
                "exchange":"NSE",
                "tradingsymbol":"RELIANCE",
                "instrument_token":738561,
                "order_type":"LIMIT",
                "transaction_type":"BUY",
                "validity":"DAY",
                "product":"CNC",
                "quantity":1,
                "disclosed_quantity":0,
                "price":952.95,
                "trigger_price":0,
                "average_price":0,
                "filled_quantity":0,
                "pending_quantity":1,
                "cancelled_quantity":0,
                "market_protection":0,
                "meta":null,
                "tag":"387d7b00",
                "guid":"ZNnZ9ryuS2fmtdFJ",
                "account_id":"RJ0893",
                "unfilled_quantity":0,
                "app_id":106,
                "checksum":""
            }
         }
         */
        

        /// <summary>
        /// Gets or sets the id of the user that placed the order. This may different from the user’s id for orders placed outside of Kite, for instance, by dealers at the brokerage using dealer terminals.
        /// </summary>
        [DataMember(Name = "placed_by")]
        public string placed_by { get; set; }


        /// <summary>
        /// Gets or sets the order id
        /// </summary>
        [DataMember(Name = "order_id")]
        public string order_id { get; set; }

        /// <summary>
        /// Gets or sets the exchange order id
        /// </summary>
        [DataMember(Name = "exchange_order_id")]
        public string exchange_order_id { get; set; }

        /// <summary>
        /// Gets or sets the parent order id
        /// </summary>
        [DataMember(Name = "parent_order_id")]
        public string parent_order_id { get; set; }

        

        /// <summary>
        /// Gets or sets the current status of the order. Most common values or COMPLETE, REJECTED, CANCELLED, and OPEN. There may be other values as well.
        /// </summary>
        [DataMember(Name = "status")]
        public string status { get; set; }

        /// <summary>
        /// Gets or sets the textual description of the order’s status. Failed orders come with human readable explanation

        /// </summary>
        [DataMember(Name = "status_message")]
        public string status_message { get; set; }


        /// <summary>
        /// Gets or sets the order time
        /// </summary>
        [DataMember(Name = "order_timestamp")]
        public string order_timestamp { get; set; }

        /// <summary>
        /// Gets or sets the exchange update time
        /// </summary>
        [DataMember(Name = "exchange_update_timestamp")]
        public string exchange_update_timestamp { get; set; }

        /// <summary>
        /// Gets or sets the exchange time
        /// </summary>
        [DataMember(Name = "exchange_timestamp")]
        public string exchange_timestamp { get; set; }

        /// <summary>
        /// Gets or sets the variety
        /// </summary>
        [DataMember(Name = "variety")]
        public string variety { get; set; }

        /// <summary>
        /// Gets or sets the exchange tradingsymbol of the of the instrument
        /// </summary>
        [DataMember(Name = "tradingsymbol")]
        public string tradingsymbol { get; set; }

        /// <summary>
        /// Gets or sets the exchange
        /// </summary>
        [DataMember(Name = "exchange")]
        public string exchange { get; set; }

        /// <summary>
        /// Gets or sets the instrument token
        /// </summary>
        [DataMember(Name = "instrument_token")]
        public int instrument_token { get; set; }

        /// <summary>
        /// Gets or sets the order type
        /// </summary>
        [DataMember(Name = "order_type")]
        public string order_type { get; set; }

        /// <summary>
        /// Gets or sets the transaction type
        /// </summary>
        [DataMember(Name = "transaction_type")]
        public string transaction_type { get; set; }

        /// <summary>
        /// Gets or sets the validity
        /// </summary>
        [DataMember(Name = "validity")]
        public string validity { get; set; }

        /// <summary>
        /// Gets or sets the product
        /// </summary>
        [DataMember(Name = "product")]
        public string product { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        [DataMember(Name = "quantity")]
        public int quantity { get; set; }

        /// <summary>
        /// Gets or sets the disclosed quantity
        /// </summary>
        [DataMember(Name = "disclosed_quantity")]
        public int disclosed_quantity { get; set; }

        /// <summary>
        /// Gets or sets the limit price
        /// </summary>
        [DataMember(Name = "price")]
        public double price { get; set; }

        /// <summary>
        /// Gets or sets the stop price
        /// </summary>
        [DataMember(Name = "trigger_price")]
        public double trigger_price { get; set; }

        /// <summary>
        /// Gets or sets the average price
        /// </summary>
        [DataMember(Name = "average_price")]
        public double average_price { get; set; }

        

        /// <summary>
        /// Gets or sets the filled quantity
        /// </summary>
        [DataMember(Name = "filled_quantity")]
        public int filled_quantity { get; set; }

        /// <summary>
        /// Gets or sets the pending quantity
        /// </summary>
        [DataMember(Name = "pending_quantity")]
        public int pending_quantity { get; set; }

        /// <summary>
        /// Gets or sets the cancelled quantity
        /// </summary>
        [DataMember(Name = "cancelled_quantity")]
        public int cancelled_quantity { get; set; }

        /// <summary>
        /// Gets or sets the market protection
        /// </summary>
        [DataMember(Name = "market_protection")]
        public double market_protection { get; set; }

        /// <summary>
        /// Gets or sets the meta
        /// </summary>
        [DataMember(Name = "meta")]
        public string meta { get; set; }

        /// <summary>
        /// Gets or sets the tag assigned by the client
        /// </summary>
        [DataMember(Name = "tag")]
        public string tag { get; set; }

        /// <summary>
        /// Gets or sets the guid
        /// </summary>
        [DataMember(Name = "guid")]
        public string guid { get; set; }


        /// <summary>
        /// Gets or sets the account id
        /// </summary>
        [DataMember(Name = "account_id")]
        public string account_id { get; set; }

        /// <summary>
        /// Gets or sets the remaining quantity
        /// </summary>
        [DataMember(Name = "unfilled_quantity")]
        public int unfilled_quantity { get; set; }

        /// <summary>
        /// Gets or sets the app id
        /// </summary>
        [DataMember(Name = "app_id")]
        public int app_id { get; set; }

        
        

        /// <summary>
        /// Gets or sets the checksum
        /// </summary>
        [DataMember(Name = "checksum")]
        public string checksum { get; set; }

        

        

        /// <summary>
        /// Validates if the postback is submitted by the client
        /// </summary>
        /// <param name="secret">Api secret</param>
        /// <returns></returns>
        public static bool IsApiSubmitted(OrderPostBack op, string secret)
        {
            if (op == null)
                return false;
            
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(op.order_id) || string.IsNullOrEmpty(op.order_timestamp))
                return false;

            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(string.Format("{0}{1}{2}", op.order_id, op.order_timestamp, secret)));

                foreach (var item in result)
                {
                    sb.Append(item.ToString("x2"));
                }
            }

            return op.checksum == sb.ToString();
        }


        public override string ToString()
        {
            return $"tag={this.tag}, orderId={this.order_id}, parent={this.parent_order_id}, symbol={this.tradingsymbol}, exchange={this.exchange}, action={this.transaction_type}, status={this.status}, qty={this.quantity}, filled={this.filled_quantity}, pending={this.pending_quantity}, cancelled={this.cancelled_quantity}, price={this.price}, stop={this.trigger_price}";
        }

    }
}
