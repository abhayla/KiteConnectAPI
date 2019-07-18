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
    public class Candle
    {
        //{"status": "success", "data": {
        //    "candles": [["2016-12-30T10:00:00+0530",3675,3676,3669,3675,388],["2016-12-30T10:01:00+0530",3675,3677,3674,3676,428]]}}
        
        //day candles
        //{"status": "success", "data": {
        //  "candles": [["2016-12-30T09:15:00+0530",8141.45,8256.35,8141.45,8239,111825],["2017-01-02T09:15:00+0530",8236.2,8269.75,8206.3,8247,56100],["2017-01-03T09:15:00+0530",8253.3,8278.2,8214,8248.4,150300],["2017-01-04T09:15:00+0530",8261.1,8273.45,8241.75,8257.8,29175],["2017-01-05T09:15:00+0530",8290.6,8348,8289.35,8339.6,102750]]}}

        /// <summary>
        /// Gets or sets the candle array
        /// </summary>
        [DataMember(Name = "candles")]
        public string[][] candles { get; set; }

    }
}
