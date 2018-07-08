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
    public class MfOrder
    {
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
        /// Gets or sets the trading symbol
        /// </summary>
        [DataMember(Name = "tradingsymbol")]
        public string tradingsymbol { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        [DataMember(Name = "status")]
        public string status { get; set; }

        /// <summary>
        /// Gets or sets the status message
        /// </summary>
        [DataMember(Name = "status_message")]
        public string status_message { get; set; }

        /// <summary>
        /// Gets or sets the amount
        /// </summary>
        [DataMember(Name = "amount")]
        public double amount { get; set; }

        /// <summary>
        /// Gets or sets the folio
        /// </summary>
        [DataMember(Name = "folio")]
        public string folio { get; set; }

        /// <summary>
        /// Gets or sets the fund
        /// </summary>
        [DataMember(Name = "fund")]
        public string fund { get; set; }

        /// <summary>
        /// Gets or sets the order timestamp
        /// </summary>
        [DataMember(Name = "order_timestamp")]
        public string order_timestamp { get; set; }

        /// <summary>
        /// Gets or sets the exchange timestamp
        /// </summary>
        [DataMember(Name = "exchange_timestamp")]
        public string exchange_timestamp { get; set; }

        /// <summary>
        /// Gets or sets the settlement id
        /// </summary>
        [DataMember(Name = "settlement_id")]
        public string settlement_id { get; set; }

        /// <summary>
        /// Gets or sets the transaction type
        /// </summary>
        [DataMember(Name = "transaction_type")]
        public string transaction_type { get; set; }

        /// <summary>
        /// Gets or sets the variety
        /// </summary>
        [DataMember(Name = "variety")]
        public string variety { get; set; }

        /// <summary>
        /// Gets or sets the purchase type
        /// </summary>
        [DataMember(Name = "purchase_type")]
        public string purchase_type { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        [DataMember(Name = "quantity")]
        public double quantity { get; set; }

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        [DataMember(Name = "price")]
        public double price { get; set; }

        /// <summary>
        /// Gets or sets the last price
        /// </summary>
        [DataMember(Name = "last_price")]
        public double last_price { get; set; }

        /// <summary>
        /// Gets or sets the average price
        /// </summary>
        [DataMember(Name = "average_price")]
        public double average_price { get; set; }

        /// <summary>
        /// Gets or sets who placed the order
        /// </summary>
        [DataMember(Name = "placed_by")]
        public string placed_by { get; set; }

        /// <summary>
        /// Gets or sets the tag
        /// </summary>
        [DataMember(Name = "tag")]
        public string tag { get; set; }
    }
}
