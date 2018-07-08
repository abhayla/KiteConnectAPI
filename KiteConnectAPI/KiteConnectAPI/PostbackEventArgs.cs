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

namespace KiteConnectAPI
{
    public class PostbackEventArgs : EventArgs
    {
        public PostbackEventArgs(OrderPostBack order)
        {
            this.Order = order;
        }

        /// <summary>
        /// Gets the postback object
        /// </summary>
        public OrderPostBack Order { get; private set; }
    }
}
