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
    public class MfInstrument
    {
        /// <summary>
        /// Gets or sets the trading symbol
        /// </summary>
        [DataMember(Name = "tradingsymbol")]
        public string tradingsymbol { get; set; }

        /// <summary>
        /// Gets or sets the amc
        /// </summary>
        [DataMember(Name = "amc")]
        public string amc { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [DataMember(Name = "name")]
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the purchase allowed
        /// </summary>
        [DataMember(Name = "purchase_allowed")]
        public int purchase_allowed { get; set; }

        /// <summary>
        /// Gets or sets the redemption allowed
        /// </summary>
        [DataMember(Name = "redemption_allowed")]
        public int redemption_allowed { get; set; }

        /// <summary>
        /// Gets or sets the minimum purchase amout
        /// </summary>
        [DataMember(Name = "minimum_purchase_amount")]
        public double minimum_purchase_amount { get; set; }

        /// <summary>
        /// Gets or sets the purchase amout multiplier
        /// </summary>
        [DataMember(Name = "purchase_amount_multiplier")]
        public double purchase_amount_multiplier { get; set; }

        /// <summary>
        /// Gets or sets the minimum additional pruchase amount
        /// </summary>
        [DataMember(Name = "minimum_additional_purchase_amount")]
        public double minimum_additional_purchase_amount { get; set; }

        /// <summary>
        /// Gets or sets the minimum redemption quantity
        /// </summary>
        [DataMember(Name = "minimum_redemption_quantity")]
        public double minimum_redemption_quantity { get; set; }

        /// <summary>
        /// Gets or sets the redemption quantity multiplier
        /// </summary>
        [DataMember(Name = "redemption_quantity_multiplier")]
        public double redemption_quantity_multiplier { get; set; }

        /// <summary>
        /// Gets or sets the dividend type
        /// </summary>
        [DataMember(Name = "dividend_type")]
        public string dividend_type { get; set; }

        /// <summary>
        /// Gets or sets the scheme type
        /// </summary>
        [DataMember(Name = "scheme_type")]
        public string scheme_type { get; set; }

        /// <summary>
        /// Gets or sets the plan
        /// </summary>
        [DataMember(Name = "plan")]
        public string plan { get; set; }

        /// <summary>
        /// Gets or sets the settlement type
        /// </summary>
        [DataMember(Name = "settlement_type")]
        public string settlement_type { get; set; }

        /// <summary>
        /// Gets or sets the last price
        /// </summary>
        [DataMember(Name = "last_price")]
        public double last_price { get; set; }

        /// <summary>
        /// Gets or sets the last price date
        /// </summary>
        [DataMember(Name = "last_price_date")]
        public string last_price_date { get; set; }

    }
}
