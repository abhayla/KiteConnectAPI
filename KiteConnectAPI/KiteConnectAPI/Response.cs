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
    public abstract class Response
    {
        /// <summary>
        /// Gets or sets the status
        /// </summary>
        [DataMember(Name = "status")]
        public string status { get; set; }
    }

    [DataContract]
    public class Response<T> : Response
    {

        /// <summary>
        /// Gets or sets the response data
        /// </summary>
        [DataMember(Name = "data")]
        public T data { get; set; }
    }


    [DataContract]
    public class ServerError : Response
    {

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        [DataMember(Name = "message")]
        public string message { get; set; }

        /// <summary>
        /// Gets or sets the error type
        /// </summary>
        [DataMember(Name = "error_type")]
        public string error_type { get; set; }

    }
}
