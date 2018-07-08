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
    public class Position
    {
        

        /// <summary>
        /// Gets or sets the margin product applied to the position
        /// </summary>
        [DataMember(Name = "product")]
        public string product { get; set; }

        /// <summary>
        /// Gets or sets the quantity held previously and carried forward over night
        /// </summary>
        [DataMember(Name = "overnight_quantity")]
        public int overnight_quantity { get; set; }

        /// <summary>
        /// Gets or sets the exchange
        /// </summary>
        [DataMember(Name = "exchange")]
        public string exchange { get; set; }

        /// <summary>
        /// Gets or sets the net value of the sold quantities
        /// </summary>
        [DataMember(Name = "sell_value")]
        public double sell_value { get; set; }

        /// <summary>
        /// Gets or sets the last traded market price of the instrument
        /// </summary>
        [DataMember(Name = "last_price")]
        public double last_price { get; set; }

        /// <summary>
        /// Gets or sets the net buy amount m2m
        /// </summary>
        [DataMember(Name = "net_buy_amount_m2m")]
        public double net_buy_amount_m2m { get; set; }


        /// <summary>
        /// Gets or sets the exchange tradingsymbol of the instrument
        /// </summary>
        [DataMember(Name = "tradingsymbol")]
        public string tradingsymbol { get; set; }

        /// <summary>
        /// Gets or set the returns on the position; Profit and loss
        /// </summary>
        [DataMember(Name = "pnl")]
        public double pnl { get; set; }

        /// <summary>
        /// Gets or sets the quantity/lot size multiplier used for calculating P&Ls.
        /// </summary>
        [DataMember(Name = "multiplier")]
        public int multiplier { get; set; }

        /// <summary>
        /// Gets or sets the quantity sold off from the position
        /// </summary>
        [DataMember(Name = "sell_quantity")]
        public int sell_quantity { get; set; }

        /// <summary>
        /// Gets or sets the net value of the bought quantities
        /// </summary>
        [DataMember(Name = "buy_value")]
        public double buy_value { get; set; }


        /// <summary>
        /// Gets or sets the net sell amount m2m
        /// </summary>
        [DataMember(Name = "net_sell_amount_m2m")]
        public double net_sell_amount_m2m { get; set; }

        /// <summary>
        /// Gets or sets the average price at which the net position quantity was acquired
        /// </summary>
        [DataMember(Name = "average_price")]
        public double average_price { get; set; }

        /// <summary>
        /// Gets or sets the unrelaised intraday returns
        /// </summary>
        [DataMember(Name = "unrealised")]
        public double unrealised { get; set; }


        /// <summary>
        /// Gets or sets the net value of the position
        /// </summary>
        [DataMember(Name = "value")]
        public double value { get; set; }

        /// <summary>
        /// Gets or sets the average price at which quantities were bought
        /// </summary>
        [DataMember(Name = "buy_price")]
        public double buy_price { get; set; }

        /// <summary>
        /// Gets or sets the average price at which quantities were sold
        /// </summary>
        [DataMember(Name = "sell_price")]
        public double sell_price { get; set; }


        /// <summary>
        /// Gets or sets the mark to market returns (computed based on the last close and the last traded price)
        /// </summary>
        [DataMember(Name = "m2m")]
        public double m2m { get; set; }


        /// <summary>
        /// Gets or sets the numerical identifier issued by the exchange representing the instrument. Used for subscribing to live market data over WebSocket
        /// </summary>
        [DataMember(Name = "instrument_token")]
        public string instrument_token { get; set; }

        /// <summary>
        /// Gets or sets the closing price of the instrument from the last trading day
        /// </summary>
        [DataMember(Name = "close_price")]
        public double close_price { get; set; }

        /// <summary>
        /// Gets or sets the quantity held
        /// </summary>
        [DataMember(Name = "quantity")]
        public int quantity { get; set; }

        /// <summary>
        /// Gets or sets the quantity bought and added to the position
        /// </summary>
        [DataMember(Name = "buy_quantity")]
        public int buy_quantity { get; set; }


        /// <summary>
        /// Gets or sets the realised intraday returns
        /// </summary>
        [DataMember(Name = "realised")]
        public double realised { get; set; }

        /// <summary>
        /// Gets or sets the buy m2m
        /// </summary>
        [DataMember(Name = "buy_m2m")]
        public double buy_m2m { get; set; }

        /// <summary>
        /// Gets or sets the sell m2m
        /// </summary>
        [DataMember(Name = "sell_m2m")]
        public double sell_m2m { get; set; }

        /// <summary>
        /// Gets or sets the day buy quantity
        /// </summary>
        [DataMember(Name = "day_buy_quantity")]
        public int day_buy_quantity { get; set; }

        /// <summary>
        /// Gets or sets the day buy price
        /// </summary>
        [DataMember(Name = "day_buy_price")]
        public double day_buy_price { get; set; }

        /// <summary>
        /// Gets or sets the day buy value
        /// </summary>
        [DataMember(Name = "day_buy_value")]
        public double day_buy_value { get; set; }

        /// <summary>
        /// Gets or sets the day sell quantity
        /// </summary>
        [DataMember(Name = "day_sell_quantity")]
        public int day_sell_quantity { get; set; }

        /// <summary>
        /// Gets or sets the day sell price
        /// </summary>
        [DataMember(Name = "day_sell_price")]
        public double day_sell_price { get; set; }

        /// <summary>
        /// Gets or sets the day sell value
        /// </summary>
        [DataMember(Name = "day_sell_value")]
        public double day_sell_value { get; set; }

        public override string ToString()
        {
            return $"{this.tradingsymbol} ({this.exchange}-{this.product}) : {this.quantity} @ {this.average_price}";
        }

    }
}
