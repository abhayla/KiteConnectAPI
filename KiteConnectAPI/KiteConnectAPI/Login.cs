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
    public class Login
    {
        /*
            {"status": "success", "data": {
         * "product": ["BO", "CO", "CNC", "MIS", "NRML"],
         * "user_id": "RJ0893", 
         * "order_type": ["LIMIT", "MARKET", "SL", "SL-M"],
         * "exchange": ["NFO", "NSE", "BSE", "BFO", "MCX", "CDS"],
         * "access_token": "xzt35nnagavht6rybr83i8gkbpklar0d",
         * "password_reset": false,
         * "user_type": "investor", 
         * "broker": "ZERODHA", 
         * "public_token": "328499a3f6d28593c69c505f7d3d8afe",
         * "member_id": "ZERODHA", 
         * "user_name": "JOYDEEP MITRA",
         * "email": "mitrajoydeep@gmail.com", 
         * "login_time": "2017-02-11 19:26:09"}}
        */

        /// <summary>
        /// Gets or sets the unique, permanent user id registered with the broker and the exchanges
        /// </summary>
        [DataMember(Name = "user_id")]
        public string user_id { get; set; }

        /// <summary>
        /// Gets or sets the user’s real name
        /// </summary>
        [DataMember(Name = "user_name")]
        public string user_name { get; set; }

        /// <summary>
        /// Gets or sets the user short name
        /// </summary>
        [DataMember(Name = "user_shortname")]
        public string user_shortname { get; set; }

        /// <summary>
        /// Gets or sets the user email id
        /// </summary>
        [DataMember(Name = "email")]
        public string email { get; set; }

        /// <summary>
        /// Gets or sets the user’s registered role at the broker
        /// </summary>
        [DataMember(Name = "user_type")]
        public string user_type { get; set; }

        /// <summary>
        /// Gets or sets the broker
        /// </summary>
        [DataMember(Name = "broker")]
        public string broker { get; set; }

        /// <summary>
        /// Gets or sets the exchanges enabled for trading on the user’s account
        /// </summary>
        [DataMember(Name = "exchanges")]
        public string[] exchanges { get; set; }


        /// <summary>
        /// Gets or sets the order product types (margin related) enabled for the user
        /// </summary>
        [DataMember(Name = "products")]
        public string[] products { get; set; }

        

        /// <summary>
        /// Gets or sets the order types enabled for the user
        /// </summary>
        [DataMember(Name = "order_types")]
        public string[] order_types { get; set; }

        /// <summary>
        /// Gets or sets the api key
        /// </summary>
        [DataMember(Name = "api_key")]
        public string api_key { get; set; }

        /// <summary>
        /// Gets or sets the token that is to be used with every request after authentication
        /// </summary>
        [DataMember(Name = "access_token")]
        public string access_token { get; set; }

        /// <summary>
        /// Gets or sets the token for public session validation where requests may be exposed to the public, for instance, browser WebSocket connections for streaming market data
        /// </summary>
        [DataMember(Name = "public_token")]
        public string public_token { get; set; }

        /// <summary>
        /// Gets or sets the refresh token
        /// </summary>
        [DataMember(Name = "refresh_token")]
        public string refresh_token { get; set; }

        /// <summary>
        /// Gets or sets the login time
        /// </summary>
        [DataMember(Name = "login_time")]
        public string login_time { get; set; }

        /// <summary>
        /// Gets or sets the avatar url
        /// </summary>
        [DataMember(Name = "avatar_url")]
        public string avatar_url { get; set; }


    }
}
