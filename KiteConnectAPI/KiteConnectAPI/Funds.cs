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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KiteConnectAPI
{
    [DataContract]
    public class Funds
    {
        
        //"{\"status\":\"success\",\"data\":{\"enabled\":true,\"net\":3889.172,\"available\":{\"adhoc_margin\":0,\"cash\":3889.172,\"collateral\":0,\"intraday_payin\":0},\"utilised\":{\"debits\":0,\"exposure\":0,\"m2m_realised\":0,\"m2m_unrealised\":0,\"option_premium\":0,\"payout\":0,\"span\":0,\"holding_sales\":0,\"turnover\":0}}}"

        /*
            {"status": "success", "data": {
         * "available": {"adhoc_margin": 0.0, "collateral": 0.0, "intraday_payin": 0.0, "cash": 315819.74}, 
         * "net": 315819.74, 
         * "enabled": true, 
         * "utilised": {"m2m_unrealised": -0.0, "m2m_realised": -0.0, "debits": 0.0, "span": 0.0, "option_premium": 0.0, "holding_sales": 0.0, "exposure": 0.0, "turnover": 0.0}}}
        */

        /// <summary>
        /// Gets or sets the available margin
        /// </summary>
        [DataMember(Name = "available")]
        public AvailableFunds available { get; set; }

        /// <summary>
        /// Gets or sets the net margin
        /// </summary>
        [DataMember(Name = "net")]
        public double net { get; set; }

        /// <summary>
        /// Gets or sets if enabled or not
        /// </summary>
        [DataMember(Name = "enabled")]
        public bool enabled { get; set; }

        /// <summary>
        /// Gets or sets the utilised margin
        /// </summary>
        [DataMember(Name = "utilised")]
        public UtilizedFunds utilised { get; set; }
    }

    [DataContract]
    public class AvailableFunds
    {
        /// <summary>
        /// Gets or sets the adhoc margin
        /// </summary>
        [DataMember(Name = "adhoc_margin")]
        public double adhoc_margin { get; set; }

        /// <summary>
        /// Gets or sets the collateral
        /// </summary>
        [DataMember(Name = "collateral")]
        public double collateral { get; set; }

        /// <summary>
        /// Gets or sets the intraday payin
        /// </summary>
        [DataMember(Name = "intraday_payin")]
        public double intraday_payin { get; set; }

        /// <summary>
        /// Gets or sets the cash
        /// </summary>
        [DataMember(Name = "cash")]
        public double cash { get; set; }

    }

    [DataContract]
    public class UtilizedFunds
    {
        /// <summary>
        /// Gets or sets the unrealised m2m
        /// </summary>
        [DataMember(Name = "m2m_unrealised")]
        public double m2m_unrealised { get; set; }

        /// <summary>
        /// Gets or sets the realised m2m
        /// </summary>
        [DataMember(Name = "m2m_realised")]
        public double m2m_realised { get; set; }

        /// <summary>
        /// Gets or sets the debits
        /// </summary>
        [DataMember(Name = "debits")]
        public double debits { get; set; }

        /// <summary>
        /// Gets or sets the span
        /// </summary>
        [DataMember(Name = "span")]
        public double span { get; set; }

        /// <summary>
        /// Gets or sets the option premium
        /// </summary>
        [DataMember(Name = "option_premium")]
        public double option_premium { get; set; }

        /// <summary>
        /// Gets or sets the holding sales
        /// </summary>
        [DataMember(Name = "holding_sales")]
        public double holding_sales { get; set; }

        /// <summary>
        /// Gets or sets the exposure
        /// </summary>
        [DataMember(Name = "exposure")]
        public double exposure { get; set; }

        /// <summary>
        /// Gets or sets the turnover
        /// </summary>
        [DataMember(Name = "turnover")]
        public double turnover { get; set; }

        /// <summary>
        /// Gets or sets the payout
        /// </summary>
        [DataMember(Name = "payout")]
        public double payout { get; set; }
    }
}
