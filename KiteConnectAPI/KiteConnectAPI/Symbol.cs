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
using System.Text.RegularExpressions;

namespace KiteConnectAPI
{
    /// <summary>
    /// Symbol for equity & fno
    /// </summary>
    [DataContract]
    public class Symbol : SymbolBase
    {

        /// <summary>
        /// Parses the name from trading symbol
        /// </summary>
        /// <param name="exchange">exchange</param>
        /// <param name="tradingSymbol">Trading symbol</param>
        /// <param name="isWeekly">Is weekly contract</param>
        /// <returns></returns>
        public static string TryParseName(string exchange, string tradingSymbol, out bool isWeekly)
        {
            isWeekly = false;

            switch (exchange)
            {
                case "NSE":
                case "BSE":
                    return tradingSymbol;
                case "NFO":
                case "CDS":
                case "MCX":
                case "MCXSX":

                    if (Regex.IsMatch(tradingSymbol, @"\d{2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)FUT$"))
                    {
                        return ParseSymbol(tradingSymbol, @"\d{2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)FUT$");
                    }
                    else if (Regex.IsMatch(tradingSymbol, @"\d{2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(\d+\.\d+|\d+)(CE|PE|CA|PA)$"))
                    {
                        return ParseSymbol(tradingSymbol, @"\d{2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(\d+\.\d+|\d+)(CE|PE|CA|PA)$");
                    }
                    else if (Regex.IsMatch(tradingSymbol, @"\d{2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)FUTW\d{1}$"))
                    {
                        isWeekly = true;
                        return ParseSymbol(tradingSymbol, @"\d{2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)FUTW\d{1}$");
                    }
                    else if (Regex.IsMatch(tradingSymbol, @"\d{2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(\d+\.\d+|\d+)(CE|PE|CA|PA)W\d{1}$"))
                    {
                        isWeekly = true;
                        return ParseSymbol(tradingSymbol, @"\d{2}(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(\d+\.\d+|\d+)(CE|PE|CA|PA)W\d{1}$");
                    }
                    break;
                case "BFO":
                    if (Regex.IsMatch(tradingSymbol, @"(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)\d{4}$"))
                    {
                        return ParseSymbol(tradingSymbol, @"(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)\d{4}$");
                    }
                    else if (Regex.IsMatch(tradingSymbol, @"\d{2}(C|P)\d+$"))
                    {
                        return ParseSymbol(tradingSymbol, @"\d{2}(C|P)\d+$");
                    }
                    break;
            }

            return tradingSymbol;
        }

        private static string ParseSymbol(string tradingSymbol, string pattern)
        {
            if (string.IsNullOrEmpty(tradingSymbol) || string.IsNullOrEmpty(pattern))
                return tradingSymbol;

            var match = Regex.Match(tradingSymbol, pattern);

            if (string.IsNullOrEmpty(match.Value))
                return tradingSymbol;

            int idx = tradingSymbol.LastIndexOf(match.Value);
            return tradingSymbol.Substring(0, idx);
        }

        /// <summary>
        /// Parses the line of the csv dump and assigns the value to the object
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Boolean value depending if the parse is successful or not</returns>
        public override bool TryParse(string line)
        {
            /*
                instrument_token, exchange_token, tradingsymbol, name, last_price, expiry, strike, tick_size, lot_size, instrument_type, segment, exchange
                408065,1594,INFY,INFOSYS,0,,,0.05,1,EQ,NSE,NSE
                5720322,22345,NIFTY15DECFUT,,78.0,2015 - 12 - 31,,0.05,75,FUT,NFO - FUT,NFO
                5720578,22346,NIFTY159500CE,,23.0,2015 - 12 - 31,9500,0.05,75,CE,NFO - OPT,NFO
                645639,SILVER15DECFUT,,7800.0,2015 - 12 - 31,,1,1,FUT,MCX,MCX
            */


            if (string.IsNullOrEmpty(line))
                return false;

            string[] array = line.Split(',');
            if (array.Length != 12)
                return false;

            string instrumentToken = array[0];
            if (string.IsNullOrEmpty(instrumentToken))
                return false;

            string exchangeToken = array[1];

            string tradingSymbol = array[2];
            if (string.IsNullOrEmpty(tradingSymbol))
                return false;

            string name = array[3];

            double lastPrice;
            double.TryParse(array[4], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out lastPrice);

            string expiry = array[5];

            double strike;
            double.TryParse(array[6], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out strike);

            double tickSize;
            if (!double.TryParse(array[7], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out tickSize))
                return false;

            int lotSize;
            int.TryParse(array[8], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out lotSize);

            string instrumentType = array[9];
            if (string.IsNullOrEmpty(instrumentType))
                return false;
            string segment = array[10];
            if (string.IsNullOrEmpty(segment))
                return false;
            string exchange = array[11];
            if (string.IsNullOrEmpty(exchange))
                return false;

            this.instrument_token = instrumentToken;
            this.exchange_token = exchangeToken;
            this.tradingsymbol = tradingSymbol;
            this.name = name;
            this.last_price = lastPrice;
            this.expiry = expiry;
            this.strike = strike;
            this.tick_size = tickSize;
            this.lot_size = lotSize;
            this.instrument_type = instrumentType;
            this.segment = segment;
            this.exchange = exchange;

            return true;
        }

        
        /// <summary>
        /// Gets or sets the instrument token
        /// </summary>
        [DataMember(Name = "instrument_token")]
        public string instrument_token { get; set; }

        /// <summary>
        /// Gets or sets the exchange token
        /// </summary>
        [DataMember(Name = "exchange_token")]
        public string exchange_token { get; set; }

        
        /// <summary>
        /// Gets or sets the instrument name
        /// </summary>
        [DataMember(Name = "name")]
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the last price
        /// </summary>
        [DataMember(Name = "last_price")]
        public double last_price { get; set; }

        /// <summary>
        /// Gets or sets the expiry
        /// </summary>
        [DataMember(Name = "expiry")]
        public string expiry { get; set; }

        /// <summary>
        /// Gets or sets the strike price
        /// </summary>
        [DataMember(Name = "strike")]
        public double strike { get; set; }

        /// <summary>
        /// Gets or sets the tick size
        /// </summary>
        [DataMember(Name = "tick_size")]
        public double tick_size { get; set; }

        /// <summary>
        /// Gets or sets the lot size
        /// </summary>
        [DataMember(Name = "lot_size")]
        public int lot_size { get; set; }

        /// <summary>
        /// Gets or sets the instrument type
        /// </summary>
        [DataMember(Name = "instrument_type")]
        public string instrument_type { get; set; }

        /// <summary>
        /// Gets or sets the segment
        /// </summary>
        [DataMember(Name = "segment")]
        public string segment { get; set; }

        /// <summary>
        /// Gets or sets the exchange
        /// </summary>
        [DataMember(Name = "exchange")]
        public string exchange { get; set; }

    }
}
