using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace KiteConnectAPI
{
    /// <summary>
    /// Base class for symbols
    /// </summary>
    [DataContract]
    public abstract class SymbolBase
    {
        /// <summary>
        /// Parses a line and assigns the values to the object
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <returns></returns>
        public abstract bool TryParse(string line);


        /// <summary>
        /// Gets or sets the trading symbol
        /// </summary>
        [DataMember(Name = "tradingsymbol")]
        public string tradingsymbol { get; set; }

    }
}
