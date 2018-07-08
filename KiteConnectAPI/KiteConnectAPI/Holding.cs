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
    public class Holding
    {
        /*
            Holding[] = {"status": "success", "data": [{
         * "product": "CNC",
         * "day_change": 1434.999999999998,
         * "exchange": "NSE",
         * "price": 0, 
         * "last_price": 56.05,
         * "day_change_percentage": 3.7962962962962914,
         * "collateral_quantity": 0,
         * "pnl": 4660.0000000000255,
         * "close_price": 54.0,
         * "average_price": 49.3928571428571, 
         * "tradingsymbol": "PNBGILTS",
         * "collateral_type": null,
         * "t1_quantity": 0,
         * "instrument_token": 2236417,
         * "isin": "INE859A01011",
         * "realised_quantity": 700,
         * "quantity": 700
         * }, {"product": "CNC", "day_change": -5838.13999999997, "exchange": "NSE", "price": 0, "last_price": 12218.29, "day_change_percentage": -1.3858757062146823, "collateral_quantity": 0, "pnl": 20975.85999999905, "close_price": 12390.0, "average_price": 11601.3529411765, "tradingsymbol": "SBIN-N5", "collateral_type": null, "t1_quantity": 0, "instrument_token": 5722625, "isin": "INE062A08058", "realised_quantity": 34, "quantity": 34}, {"product": "CNC", "day_change": 7.499999999998863, "exchange": "NSE", "price": 0, "last_price": 850.9, "day_change_percentage": 0.017631501616218306, "collateral_quantity": 0, "pnl": 2053.666666666646, "close_price": 850.75, "average_price": 809.826666666667, "tradingsymbol": "HINDUNILVR", "collateral_type": null, "t1_quantity": 0, "instrument_token": 356865, "isin": "INE030A01027", "realised_quantity": 50, "quantity": 50}, {"product": "CNC", "day_change": -818.9999999999714, "exchange": "NSE", "price": 0, "last_price": 1030.15, "day_change_percentage": -0.25175502299684427, "collateral_quantity": 0, "pnl": 33601.250000000044, "close_price": 1032.75, "average_price": 923.479365079365, "tradingsymbol": "RELIANCE", "collateral_type": null, "t1_quantity": 0, "instrument_token": 738561, "isin": "INE002A01018", "realised_quantity": 315, "quantity": 315}, {"product": "CNC", "day_change": 100.0, "exchange": "NSE", "price": 0, "last_price": 15.15, "day_change_percentage": 3.4129692832764507, "collateral_quantity": 0, "pnl": 1880.0, "close_price": 14.65, "average_price": 5.75, "tradingsymbol": "SHREERAMA", "collateral_type": null, "t1_quantity": 0, "instrument_token": 1952513, "isin": "INE879A01019", "realised_quantity": 200, "quantity": 200}, {"product": "CNC", "day_change": -496.0799999999981, "exchange": "NSE", "price": 0, "last_price": 11580.92, "day_change_percentage": -0.16448275862068903, "collateral_quantity": 0, "pnl": 16347.920000001148, "close_price": 11600.0, "average_price": 10952.153846153802, "tradingsymbol": "SBIN-N2", "collateral_type": null, "t1_quantity": 0, "instrument_token": 5253121, "isin": "INE062A08025", "realised_quantity": 26, "quantity": 26}, {"product": "CNC", "day_change": -274.9999999999986, "exchange": "NSE", "price": 0, "last_price": 33.35, "day_change_percentage": -1.6224188790560388, "collateral_quantity": 0, "pnl": 1050.0000000000007, "close_price": 33.9, "average_price": 31.25, "tradingsymbol": "SJVN", "collateral_type": null, "t1_quantity": 0, "instrument_token": 4834049, "isin": "INE002L01015", "realised_quantity": 500, "quantity": 500}, {"product": "CNC", "day_change": 146.25, "exchange": "NSE", "price": 0, "last_price": 469.25, "day_change_percentage": 0.6974248927038627, "collateral_quantity": 0, "pnl": -3806.2499999999864, "close_price": 466.0, "average_price": 553.833333333333, "tradingsymbol": "WIPRO", "collateral_type": null, "t1_quantity": 0, "instrument_token": 969473, "isin": "INE075A01022", "realised_quantity": 45, "quantity": 45}, {"product": "CNC", "day_change": 14000.0, "exchange": "NSE", "price": 0, "last_price": 411.5, "day_change_percentage": 0.857843137254902, "collateral_quantity": 0, "pnl": 121299.35, "close_price": 408.0, "average_price": 381.1751625, "tradingsymbol": "TVSMOTOR", "collateral_type": null, "t1_quantity": 0, "instrument_token": 2170625, "isin": "INE494B01023", "realised_quantity": 4000, "quantity": 4000}, {"product": "CNC", "day_change": -280.00000000000114, "exchange": "NSE", "price": 0, "last_price": 45.3, "day_change_percentage": -0.7667031763417337, "collateral_quantity": 0, "pnl": -1150.0, "close_price": 45.65, "average_price": 46.7375, "tradingsymbol": "RPOWER", "collateral_type": null, "t1_quantity": 0, "instrument_token": 3906305, "isin": "INE614G01033", "realised_quantity": 800, "quantity": 800}, {"product": "CNC", "day_change": -173.4200000000019, "exchange": "NSE", "price": 0, "last_price": 2790, "day_change_percentage": -0.4758609373104991, "collateral_quantity": 0, "pnl": 36270.0, "close_price": 2803.34, "average_price": 0.0, "tradingsymbol": "SGBAUG24-GB", "collateral_type": null, "t1_quantity": 0, "instrument_token": 4717825, "isin": "IN0020160027", "realised_quantity": 13, "quantity": 13}, {"product": "CNC", "day_change": 0.0, "exchange": "NSE", "price": 0, "last_price": 1000, "day_change_percentage": 0.0, "collateral_quantity": 0, "pnl": 0.0, "close_price": 1000.0, "average_price": 1000.0, "tradingsymbol": "LIQUIDBEES", "collateral_type": null, "t1_quantity": 0, "instrument_token": 2817537, "isin": "INF732E01037", "realised_quantity": 746, "quantity": 746}]}
        */

        
    /// <summary>
    /// Gets or sets the margin product applied to the holding
    /// </summary>
    [DataMember(Name = "product")]
        public string product { get; set; }

        /// <summary>
        /// Gets or sets the change
        /// </summary>
        [DataMember(Name = "day_change")]
        public string day_change { get; set; }

        /// <summary>
        /// Gets or sets the exchange
        /// </summary>
        [DataMember(Name = "exchange")]
        public string exchange { get; set; }

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        [DataMember(Name = "price")]
        public double price { get; set; }

        /// <summary>
        /// Gets or sets the last traded market price of the instrument
        /// </summary>
        [DataMember(Name = "last_price")]
        public double last_price { get; set; }

        /// <summary>
        /// Gets or sets the percentage change
        /// </summary>
        [DataMember(Name = "day_change_percentage")]
        public double day_change_percentage { get; set; }

        /// <summary>
        /// Gets or sets the quantity used as collateral
        /// </summary>
        [DataMember(Name = "collateral_quantity")]
        public int collateral_quantity { get; set; }

        /// <summary>
        /// Gets or sets the net returns on the stock; Profit and loss
        /// </summary>
        [DataMember(Name = "pnl")]
        public double pnl { get; set; }

        /// <summary>
        /// Gets or sets the close price
        /// </summary>
        [DataMember(Name = "close_price")]
        public double close_price { get; set; }

        /// <summary>
        /// Gets or sets the average price at which the net holding quantity was acquired
        /// </summary>
        [DataMember(Name = "average_price")]
        public double average_price { get; set; }

        /// <summary>
        /// Gets or sets the exchange tradingsymbol of the instrument
        /// </summary>
        [DataMember(Name = "tradingsymbol")]
        public string tradingsymbol { get; set; }

        /// <summary>
        /// Gets or sets the type of collateral
        /// </summary>
        [DataMember(Name = "collateral_type")]
        public string collateral_type { get; set; }

        /// <summary>
        /// Gets or sets the quanity on T+1 day after order execution. Stocks are delivered T+2
        /// </summary>
        [DataMember(Name = "t1_quantity")]
        public int t1_quantity { get; set; }

        /// <summary>
        /// Gets or sets the instrument token
        /// </summary>
        [DataMember(Name = "instrument_token")]
        public string instrument_token { get; set; }

        /// <summary>
        /// Gets or sets the standard ISIN representing stocks listed on multiple exchanges
        /// </summary>
        [DataMember(Name = "isin")]
        public string isin { get; set; }

        /// <summary>
        /// Gets or sets the realized quantity
        /// </summary>
        [DataMember(Name = "realised_quantity")]
        public int realised_quantity { get; set; }

        /// <summary>
        /// Gets or sets the quantity held
        /// </summary>
        [DataMember(Name = "quantity")]
        public int quantity { get; set; }

    }
}
