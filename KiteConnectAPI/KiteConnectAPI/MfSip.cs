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
    public class MfSip
    {
        /// <summary>
        /// Gets or sets the sip id
        /// </summary>
        [DataMember(Name = "sip_id")]
        public string sip_id { get; set; }

        /// <summary>
        /// Gets or sets the order id
        /// </summary>
        [DataMember(Name = "order_id")]
        public string order_id { get; set; }

        /// <summary>
        /// Gets or sets the trading symbol
        /// </summary>
        [DataMember(Name = "tradingsymbol")]
        public string tradingsymbol { get; set; }

        /// <summary>
        /// Gets or sets the fund
        /// </summary>
        [DataMember(Name = "fund")]
        public string fund { get; set; }

        /// <summary>
        /// Gets or sets the dividend type
        /// </summary>
        [DataMember(Name = "dividend_type")]
        public string dividend_type { get; set; }

        /// <summary>
        /// Gets or sets the transaction type
        /// </summary>
        [DataMember(Name = "transaction_type")]
        public string transaction_type { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        [DataMember(Name = "status")]
        public string status { get; set; }

        /// <summary>
        /// Gets or sets the time created
        /// </summary>
        [DataMember(Name = "created")]
        public string created { get; set; }

        /// <summary>
        /// Gets or sets the frequency
        /// </summary>
        [DataMember(Name = "frequency")]
        public string frequency { get; set; }

        /// <summary>
        /// Gets or sets the installment amount
        /// </summary>
        [DataMember(Name = "instalment_amount")]
        public double instalment_amount { get; set; }

        /// <summary>
        /// Gets or sets the installments
        /// </summary>
        [DataMember(Name = "instalments")]
        public int instalments { get; set; }

        /// <summary>
        /// Gets or sets the last installment date
        /// </summary>
        [DataMember(Name = "last_instalment")]
        public string last_instalment { get; set; }

        /// <summary>
        /// Gets or sets the pending installments
        /// </summary>
        [DataMember(Name = "pending_instalments")]
        public int pending_instalments { get; set; }

        /// <summary>
        /// Gets or sets the installment date
        /// </summary>
        [DataMember(Name = "instalment_date")]
        public int instalment_date { get; set; }

        /// <summary>
        /// Gets or sets the tag
        /// </summary>
        [DataMember(Name = "tag")]
        public string tag { get; set; }

    }
}
