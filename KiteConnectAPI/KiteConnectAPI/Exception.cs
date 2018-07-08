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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KiteConnectAPI
{
    /// <summary>
    /// Raised when the server returns an exception to a http request
    /// </summary>
    public class ServerException : Exception
    {
        public ServerException(ServerError serverError, WebExceptionStatus status, Type type) : base(serverError.message)
        {
            this.ServerError = serverError;
            this.Type = type;
        }

        /// <summary>
        /// Gets the server error object
        /// </summary>
        public ServerError ServerError { get; private set; }

        /// <summary>
        /// Gets the type of the calling type
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Gets the web exception status
        /// </summary>
        public WebExceptionStatus Status { get; private set; }

    }

    /// <summary>
    /// Raised when the backend OMS is down and the websocket fails to connect
    /// </summary>
    public class BadGatewayException : Exception
    {
        public BadGatewayException(string errorMsg) : base(errorMsg)
        { }

    }
}
