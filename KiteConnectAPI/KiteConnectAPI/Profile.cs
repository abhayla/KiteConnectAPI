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
    public class Profile
    {
        /// <summary>
        /// Gets or sets the user’s registered role at the broker
        /// </summary>
        [DataMember(Name = "user_type")]
        public string user_type { get; set; }

        /// <summary>
        /// Gets or sets the user email id
        /// </summary>
        [DataMember(Name = "email")]
        public string email { get; set; }

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

    }
}
