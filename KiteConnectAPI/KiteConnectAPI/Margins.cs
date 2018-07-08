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
    public class Margins
    {
        //"{\"status\":\"success\",\"data\":{\"equity\":{\"enabled\":true,\"net\":3889.172,\"available\":{\"adhoc_margin\":0,\"cash\":3889.172,\"collateral\":0,\"intraday_payin\":0},\"utilised\":{\"debits\":0,\"exposure\":0,\"m2m_realised\":0,\"m2m_unrealised\":0,\"option_premium\":0,\"payout\":0,\"span\":0,\"holding_sales\":0,\"turnover\":0}},\"commodity\":{\"enabled\":true,\"net\":0.001,\"available\":{\"adhoc_margin\":0,\"cash\":0.001,\"collateral\":0,\"intraday_payin\":0},\"utilised\":{\"debits\":0,\"exposure\":0,\"m2m_realised\":0,\"m2m_unrealised\":0,\"option_premium\":0,\"payout\":0,\"span\":0,\"holding_sales\":0,\"turnover\":0}}}}"

        /// <summary>
        /// Gets or sets the equity fund value
        /// </summary>
        [DataMember(Name = "equity")]
        public Funds equity { get; set; }

        /// <summary>
        /// Gets or sets the commodity fund value
        /// </summary>
        [DataMember(Name = "commodity")]
        public Funds commodity { get; set; }

    }
}
