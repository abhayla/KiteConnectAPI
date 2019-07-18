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

namespace KiteConnection
{
    class KiteStaticQuotes : SharpCharts.Base.Data.StaticQuote
    {
        /*
        Dictionary`2 : {"status":"success","data":
            {
                "53718535":
                    {
                        "instrument_token":53718535,
                        "timestamp":"2018-07-13 20:46:12",
                        "last_trade_time":"2018-07-13 20:46:12",
                        "last_price":4868,
                        "last_quantity":3,
                        "buy_quantity":3173,
                        "sell_quantity":2827,
                        "volume":74038,
                        "average_price":4822.89,
                        "oi":17584,
                        "oi_day_high":18717,
                        "oi_day_low":13307,
                        "net_change":88,
                        "ohlc":{"open":4803,"high":4878,"low":4783,"close":4780},
                        "depth":{"buy":[{"price":4868,"quantity":4,"orders":2},{"price":4867,"quantity":112,"orders":22},{"price":4866,"quantity":119,"orders":27},{"price":4865,"quantity":78,"orders":34},{"price":4864,"quantity":4,"orders":3}],"sell":[{"price":4869,"quantity":55,"orders":18},{"price":4870,"quantity":197,"orders":40},{"price":4871,"quantity":41,"orders":16},{"price":4872,"quantity":28,"orders":13},{"price":4873,"quantity":26,"orders":11}]}}}}

        */

        public KiteConnectAPI.Quotes Quotes { get; set; }

        public override string TemplateName
        {
            get { return "KiteConnectConnection"; }
        }
    }
}
