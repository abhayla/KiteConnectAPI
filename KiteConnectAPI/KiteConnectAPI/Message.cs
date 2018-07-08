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
using System.Runtime.Serialization.Json;
using System.IO;

namespace KiteConnectAPI
{

    public static class Message
    {
        /// <summary>
        /// Gets subscribe string
        /// </summary>
        public static string subscribe
        {
            get { return "subscribe"; }
        }

        /// <summary>
        /// Gets unsubscribe string
        /// </summary>
        public static string unsubscribe
        {
            get { return "unsubscribe"; }
        }

        /// <summary>
        /// Gets mode string
        /// </summary>
        public static string mode
        {
            get { return "mode"; }
        }

        /// <summary>
        /// Gets ltp string
        /// </summary>
        public static string ltp
        {
            get { return "ltp"; }
        }

        /// <summary>
        /// Gets quote string
        /// </summary>
        public static string quote
        {
            get { return "quote"; }
        }

        /// <summary>
        /// Gets full string
        /// </summary>
        public static string full
        {
            get { return "full"; }
        }

    }


    [DataContract]
    internal class Message<T>
    {
        /// <summary>
        /// Gets or sets the action
        /// </summary>
        [DataMember(Name = "a")]
        public string a { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [DataMember(Name = "v")]
        public T v { get; set; }

        /// <summary>
        /// Gets or sets if the message is for subscribing quotes or not
        /// </summary>
        [IgnoreDataMember]
        public bool IsSubscribe { get; set; }

        /// <summary>
        /// Serialize the object to json string
        /// </summary>
        /// <returns>Json string of the object</returns>
        internal string Serialize()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(this.GetType(), new DataContractJsonSerializerSettings()
                {
                    KnownTypes = new Type[] { typeof(string), typeof(int[]), typeof(int) }
                });
            

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, this);
                byte[] bytes = ms.ToArray();
                return Encoding.UTF8.GetString(bytes);
            }
        }

    }
}
