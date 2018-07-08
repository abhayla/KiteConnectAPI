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
    public class Quotes : OHLCQuotes
    {
        
        /// <summary>
        /// Gets or sets the time stamp
        /// </summary>
        [DataMember(Name = "timestamp")]
        public string timestamp { get; set; }

        /// <summary>
        /// Gets or sets the last quantity
        /// </summary>
        [DataMember(Name = "last_quantity")]
        public int last_quantity { get; set; }

        /// <summary>
        /// Gets or sets the last traded time
        /// </summary>
        [DataMember(Name = "last_trade_time")]
        public string last_trade_time { get; set; }

        /// <summary>
        /// Gets or sets the average price
        /// </summary>
        [DataMember(Name = "average_price")]
        public double average_price { get; set; }

        /// <summary>
        /// Gets or sets the volume traded today
        /// </summary>
        [DataMember(Name = "volume")]
        public int volume { get; set; }

        /// <summary>
        /// Gets or sets the quantity bought today
        /// </summary>
        [DataMember(Name = "buy_quantity")]
        public int buy_quantity { get; set; }

        /// <summary>
        /// Gets or sets the quantity sold today
        /// </summary>
        [DataMember(Name = "sell_quantity")]
        public int sell_quantity { get; set; }

        /// <summary>
        /// Gets or sets the absolute change from the last traded price to last close price
        /// </summary>
        [DataMember(Name = "net_change")]
        public double net_change { get; set; }

        /// <summary>
        /// Gets or sets the open interest
        /// </summary>
        [DataMember(Name = "oi")]
        public double oi { get; set; }

        /// <summary>
        /// Gets or sets the days highest open interest
        /// </summary>
        [DataMember(Name = "oi_day_high")]
        public int oi_day_high { get; set; }

        /// <summary>
        /// Gets or sets the days lowest open interest
        /// </summary>
        [DataMember(Name = "oi_day_low")]
        public int oi_day_low { get; set; }


        /// <summary>
        /// Gets or sets the market depth
        /// </summary>
        [DataMember(Name = "depth")]
        public Depth depth { get; set; }

    }
}
