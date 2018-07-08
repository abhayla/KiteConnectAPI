using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Globalization;

namespace KiteConnectAPI
{
    /// <summary>
    /// Mutual fund symbol
    /// </summary>
    [DataContract]
    public class MfSymbol : SymbolBase
    {

        /// <summary>
        /// Parses a line from the csv dump of a mutual fund
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Boolean value depending if the parse is successful or not</returns>
        public override bool TryParse(string line)
        {
            /*
                tradingsymbol,amc,name,purchase_allowed,redemption_allowed,minimum_purchase_amount,purchase_amount_multiplier,minimum_additional_purchase_amount,minimum_redemption_quantity,redemption_quantity_multiplier,dividend_type,scheme_type,plan,settlement_type,last_price,last_price_date
                INF846K01DP8,AXISMUTUALFUND_MF,Axis Equity Fund - Direct Plan - Growth,1,1,5000.0,1.0,100.0,1.0,0.001,growth,equity,direct,T3,20.09,2016-11-11
                INF846K01EW2,AXISMUTUALFUND_MF,Axis Long Term Equity Fund - Direct Growth,1,1,500.0,500.0,500.0,1.0,0.001,growth,elss,direct,T3,33.0425,2016-11-11
                INF174K01LS2,KOTAKMAHINDRAMF,Kotak Select Focus Fund- Direct Plan - Growth,1,1,5000.0,1.0,1000.0,0.001,0.001,growth,equity,direct,T3,26.549,2016-11-11
                INF174K01336,KOTAKMAHINDRAMF,Kotak Select Focus Fund-Growth,1,1,5000.0,0.01,1000.0,0.001,0.001,growth,equity,regular,T3,25.635,2016-11-11

            */

            if (string.IsNullOrEmpty(line))
                return false;

            string[] array = line.Split(',');

            if (array.Length != 16)
                return false;

            string tradingSymbol = array[0];
            string @amc = array[1];
            string @name = array[2];

            int purchaseAllowed;
            if (!int.TryParse(array[3], NumberStyles.Any, CultureInfo.InvariantCulture, out purchaseAllowed))
                return false;

            int redemptionAllowed;
            if (!int.TryParse(array[4], NumberStyles.Any, CultureInfo.InvariantCulture, out redemptionAllowed))
                return false;

            double minimumPurchaseAmt;
            if (!double.TryParse(array[5], NumberStyles.Any, CultureInfo.InvariantCulture, out minimumPurchaseAmt))
                return false;

            double purchaseAmtMultiplier;
            if (!double.TryParse(array[6], NumberStyles.Any, CultureInfo.InvariantCulture, out purchaseAmtMultiplier))
                return false;

            double minimumAdditionalPurchaseAmt;
            if (!double.TryParse(array[7], NumberStyles.Any, CultureInfo.InvariantCulture, out minimumAdditionalPurchaseAmt))
                return false;

            double minimumRedeptionQuantity;
            if (!double.TryParse(array[8], NumberStyles.Any, CultureInfo.InvariantCulture, out minimumRedeptionQuantity))
                return false;

            double redemptionQuantityMultiplier;
            if (!double.TryParse(array[9], NumberStyles.Any, CultureInfo.InvariantCulture, out redemptionQuantityMultiplier))
                return false;

            string dividendType = array[10];
            string schemeType = array[11];
            string @plan = array[12];
            string settlementType = array[13];

            double lastPrice;
            double.TryParse(array[14], NumberStyles.Any, CultureInfo.InvariantCulture, out lastPrice);

            string lastPriceDate = array[15];

            this.tradingsymbol = tradingSymbol;
            this.amc = @amc;
            this.name = @name;
            this.purchase_allowed = purchaseAllowed;
            this.redemption_allowed = redemptionAllowed;
            this.minimum_purchase_amount = minimumPurchaseAmt;
            this.purchase_amount_multiplier = purchaseAmtMultiplier;
            this.minimum_additional_purchase_amount = minimumAdditionalPurchaseAmt;
            this.minimum_redemption_quantity = minimumRedeptionQuantity;
            this.redemption_quantity_multiplier = redemptionQuantityMultiplier;
            this.dividend_type = dividendType;
            this.scheme_type = schemeType;
            this.plan = @plan;
            this.settlement_type = settlementType;
            this.last_price = lastPrice;
            this.last_price_date = lastPriceDate;

            return true;
        }

        
        /// <summary>
        /// Gets or sets the amc
        /// </summary>
        [DataMember(Name = "amc")]
        public string amc { get; set; }

        /// <summary>
        /// Gets or sets the mf name
        /// </summary>
        [DataMember(Name = "name")]
        public string name { get; set; }

        /// <summary>
        /// Gets or sets if purchased is allowed or not.
        /// </summary>
        [DataMember(Name = "purchase_allowed")]
        public int purchase_allowed { get; set; }

        /// <summary>
        /// Gets or sets if redemption is allowed or not
        /// </summary>
        [DataMember(Name = "redemption_allowed")]
        public int redemption_allowed { get; set; }

        /// <summary>
        /// Gets or sets the minimum purchase amout
        /// </summary>
        [DataMember(Name = "minimum_purchase_amount")]
        public double minimum_purchase_amount { get; set; }


        /// <summary>
        /// Gets or sets the puchase amount multiplier
        /// </summary>
        [DataMember(Name = "purchase_amount_multiplier")]
        public double purchase_amount_multiplier { get; set; }

        /// <summary>
        /// Gets or sets the minimum additional purchase amount
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
        /// Gets or sets teh dividend type
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
