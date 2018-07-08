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
    public class Parameter
    {
        /*
            {"status": "success", "data": {
         * "product": ["NRML", "MIS", "CNC", "CO", "BO"],
         * "order_type": ["MARKET", "LIMIT", "SL", "SL-M"], 
         * "exchange": ["NSE", "BSE", "NFO", "CDS", "MCX", "MCXSX", "BFO"], 
         * "order_variety": ["general", "amo", "bo", "co"],
         * "position_type": ["day", "overnight"],
         * "segment": ["equity", "commodity"], 
         * "validity": ["DAY", "IOC", "GTC", "AMO"],
         * "transaction_type": ["BUY", "SELL"]}}
        */

        /// <summary>
        /// Gets or sets the products
        /// </summary>
        [DataMember(Name = "product")]
        public string[] product { get; set; }

        /// <summary>
        /// Gets or sets the order type
        /// </summary>
        [DataMember(Name = "order_type")]
        public string[] order_type { get; set; }

        /// <summary>
        /// Gets or sets exchanges
        /// </summary>
        [DataMember(Name = "exchange")]
        public string[] exchange { get; set; }

        /// <summary>
        /// Gets or sets order varieties
        /// </summary>
        [DataMember(Name = "order_variety")]
        public string[] order_variety { get; set; }

        /// <summary>
        /// Gets or sets the position type
        /// </summary>
        [DataMember(Name = "position_type")]
        public string[] position_type { get; set; }

        /// <summary>
        /// Gets or sets the segments
        /// </summary>
        [DataMember(Name = "segment")]
        public string[] segment { get; set; }

        /// <summary>
        /// Gets or sets the validities , duration or timeInForce
        /// </summary>
        [DataMember(Name = "validity")]
        public string[] validity { get; set; }

        /// <summary>
        /// Gets or sets the transaction types
        /// </summary>
        [DataMember(Name = "transaction_type")]
        public string[] transaction_type { get; set; }

    }
}
