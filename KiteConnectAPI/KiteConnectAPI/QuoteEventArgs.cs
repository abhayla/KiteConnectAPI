using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteConnectAPI
{
    public class QuoteEventArgs : EventArgs
    {
        public QuoteEventArgs(byte[] data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Gets the binary data
        /// </summary>
        public byte[] Data { get; private set; }
    }
}
