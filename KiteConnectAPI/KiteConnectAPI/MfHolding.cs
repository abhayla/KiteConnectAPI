using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace KiteConnectAPI
{
    [DataContract]
    public class MfHolding
    {
        /// <summary>
        /// Gets or sets the folio
        /// </summary>
        [DataMember(Name = "folio")]
        public string folio { get; set; }

        /// <summary>
        /// Gets or sets the fund
        /// </summary>
        [DataMember(Name = "fund")]
        public string fund { get; set; }

        /// <summary>
        /// Gets or sets the trading symbol
        /// </summary>
        [DataMember(Name = "tradingsymbol")]
        public string tradingsymbol { get; set; }

        /// <summary>
        /// Gets or sets the average price
        /// </summary>
        [DataMember(Name = "average_price")]
        public double average_price { get; set; }

        /// <summary>
        /// Gets or sets the last price
        /// </summary>
        [DataMember(Name = "last_price")]
        public double last_price { get; set; }
        
        /// <summary>
        /// Gets or sets the pnl
        /// </summary>
        [DataMember(Name = "pnl")]
        public double pnl { get; set; }

        /// <summary>
        /// Gets or sets the last price date
        /// </summary>
        [DataMember(Name = "last_price_date")]
        public string last_price_date { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        [DataMember(Name = "quantity")]
        public double quantity { get; set; }

    }
}

