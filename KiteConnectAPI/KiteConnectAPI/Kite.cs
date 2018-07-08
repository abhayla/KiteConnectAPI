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
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Threading;

namespace KiteConnectAPI
{
    /// <summary>
    /// Requests urls's
    /// </summary>
    public abstract class Kite
    {
        private enum QueryMethod
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public static string version = "3";
        public static string versionHeader = "X-Kite-version";

        
        

        /// <summary>
        /// Get a http request
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="apiKey">api key</param>    
        /// <param name="accessToken">Access token</param>
        /// <param name="url">Url</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>T object</returns>
        public static T Get<T>(string apiKey, string accessToken, string url, IKiteLogger logger = null) 
        {
            Type type = typeof(T);
            bool isDictionary = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
            return QueryHttp<T>(QueryMethod.GET, apiKey, accessToken, url, logger: logger, isDictionary: isDictionary);
        }

        /// <summary>
        /// Post a http request
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="apiKey">Api key</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="url">Url</param>
        /// <param name="payload">Payload</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>T object</returns>
        public static T Post<T>(string apiKey, string accessToken, string url, string payload, IKiteLogger logger = null) 
        {
            return QueryHttp<T>(QueryMethod.POST, apiKey, accessToken, url, payload: payload, logger: logger);
        }

        /// <summary>
        /// Put a http request
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="apiKey">Api key</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="url">Url</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>T object</returns>
        public static T Put<T>(string apiKey, string accessToken, string url, string payload = null, IKiteLogger logger = null)
        {
            return QueryHttp<T>(QueryMethod.PUT, apiKey, accessToken, url, payload: payload, logger: logger);
        }


        /// <summary>
        /// Delete a http request
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="apiKey">Api key</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="url">Url</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>T object</returns>
        public static T Delete<T>(string apiKey, string accessToken, string url, IKiteLogger logger = null) 
        {
            return QueryHttp<T>(QueryMethod.DELETE, apiKey, accessToken, url, logger: logger);
        }

        
        /// <summary>
        /// The http request method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">Query method</param>
        /// <param name="apiKey">Api key</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="url">Url</param>
        /// <param name="payload">Payload</param>
        /// <param name="logger">Optional, logger</param>
        /// <param name="isDictionary">Is json string is a dictionary object. Default value is false</param>
        /// <returns></returns>
        private static T QueryHttp<T>(QueryMethod method, string apiKey, string accessToken, string url, string payload = null, IKiteLogger logger = null, bool isDictionary = false)
        {
            if (!Enum.IsDefined(typeof(QueryMethod), method))
            {
                return default(T);
            }

            if (string.IsNullOrEmpty(url))
                return default(T);

            try
            {
                logger?.OnLog($"{method}|apiKey={apiKey}, accessToken= {accessToken}, Url={url}, Payload= {payload}");
            }
            catch (Exception ex)
            {

            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToString().ToUpper();
                request.AllowAutoRedirect = true;

                request.Headers.Add(versionHeader, version);
                
                request.Headers.Add("Authorization", $"token {apiKey}:{accessToken}");
                
                if (method == QueryMethod.POST || method == QueryMethod.PUT)
                {

                    request.ContentType = "application/x-www-form-urlencoded";

                    if (!string.IsNullOrEmpty(payload))
                    {
                        byte[] postData = Encoding.ASCII.GetBytes(payload);
                        request.ContentLength = postData.Length;

                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(postData, 0, postData.Length);
                        }
                    }
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //if logger is not null we will log the json string
                        if (logger != null)
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                Response<T> obj = ParseString<Response<T>>(reader.ReadToEnd(), logger: logger, methodName: typeof(T).Name, isDictionary: isDictionary);

                                if (obj == null)
                                    return default(T);

                                return obj.data;
                            }
                        }
                        else //we serialize straight away
                        {
                            DataContractJsonSerializer serializer = null;
                            if (isDictionary)
                            {
                                serializer = new DataContractJsonSerializer(typeof(Response<T>), new DataContractJsonSerializerSettings()
                                {
                                    UseSimpleDictionaryFormat = true
                                });
                            }
                            else
                            {
                                serializer = new DataContractJsonSerializer(typeof(Response<T>));
                            }

                            Response<T> obj = serializer.ReadObject(response.GetResponseStream()) as Response<T>;

                            if (obj == null)
                            {
                                return default(T);
                            }

                            return obj.data;
                        }
                    }
                }
            }
            catch (WebException we)
            {
                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ServerError));
                    ServerError se = serializer.ReadObject(we.Response.GetResponseStream()) as ServerError;
                    if (se != null)
                    {
                        try
                        {
                            logger?.OnLog($"{se.error_type} | {se.status} | {se.message}");
                            
                        }
                        catch (Exception ex)
                        {

                        }

                        try
                        {
                            logger?.OnException(new ServerException(se, we.Status, typeof(T)));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        logger?.OnException(ex);
                    }
                    catch (Exception ex1)
                    {

                    }
                }
                
            }
            catch (Exception ex)
            {
                try
                {
                    logger?.OnException(ex);
                }
                catch (Exception ex1)
                {

                }
            }

            return default(T);

        }


        /// <summary>
        /// Parse the json string
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="str">Json string</param>
        /// <param name="logger">Optional logger</param>
        /// <param name="methodName">Optional, name of calling method</param>
        /// <param name="isDictionary">If json string is a dictionary object or not. Default value is false</param>
        /// <returns></returns>
        public static T ParseString<T>(string str, IKiteLogger logger = null, [CallerMemberName]string methodName = null, bool isDictionary = false)
        {
            if (string.IsNullOrEmpty(str))
                return default(T);

            try
            {
                logger?.OnLog($"{methodName} : {str}");
            }
            catch (Exception ex)
            {

            }


            return ParseBytes<T>(Encoding.UTF8.GetBytes(str), logger: logger, methodName: methodName, isDictionary: isDictionary);
        }

        /// <summary>
        /// Parse bytes to json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes">Bytes to be parsed</param>
        /// <param name="logger">Optional, logger</param>
        /// <param name="methodName">Optional, calling method name</param>
        /// <param name="isDictionary">Option, Is json object is a dictionary</param>
        /// <returns>Returns T</returns>
        public static T ParseBytes<T>(byte[] bytes, IKiteLogger logger = null, [CallerMemberName]string methodName = null, bool isDictionary = false)
        {
            if (bytes == null)
                return default(T);

            try
            {
                T obj = default(T);
                
                //using dataContractSerializer
                DataContractJsonSerializer serializer = null;
                if (true)
                {
                    
                    serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings()
                    {
                        UseSimpleDictionaryFormat = true
                    });
                }
                else
                {
                    serializer = new DataContractJsonSerializer(typeof(T));
                }
                
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    ms.Position = 0;
                    obj = (T)serializer.ReadObject(ms);
                }

                /*
                 * using Newtonsoft
                T obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore
                });
                */

                if (obj == null)
                {
                    try
                    {
                        logger?.OnLog($"{methodName} : Failed to parse data");
                    }
                    catch (Exception ex)
                    {

                    }
                    return default(T);
                }

                return obj;
            }
            catch (Exception ex)
            {
                try
                {
                    logger?.OnException(ex);
                }
                catch (Exception ex1)
                {

                }
                return default(T);
            }
        }


        
        /// <summary>
        /// Downloads the symbol list
        /// </summary>
        /// <param name="apiKey">Api key</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="exchange">Queried exchange. If set to null will download the all available symbols</param>
        /// <param name="filepath">If specified, will save the downloaded data to the specified path</param>
        /// <param name="logger">Optional logger</param>
        /// <returns></returns>
        public static T[] GetSymbols<T>(string apiKey, string accessToken, string url, string filepath = null, IKiteLogger logger = null) where T : SymbolBase, new()
        {
            if (string.IsNullOrEmpty(url))
                return null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add(versionHeader, version);
                request.Headers.Add("Authorization", $"token {apiKey}:{accessToken}");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response?.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            List<T> tmp = new List<T>();
                            List<string> tmpLines = new List<string>();

                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();

                                T obj = new T();
                                if (obj.TryParse(line))
                                {
                                    tmp.Add(obj as T);
                                    tmpLines.Add(line);
                                }
                            }
                            

                            if (!string.IsNullOrWhiteSpace(filepath))
                            {
                                string folder = Path.GetDirectoryName(filepath);
                                if (Directory.Exists(folder))
                                {
                                    if (File.Exists(filepath))
                                        File.Delete(filepath);

                                    File.AppendAllLines(filepath, tmpLines);
                                }
                            }

                            if (tmp.Count > 0)
                            {
                                return tmp.ToArray();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    logger?.OnException(ex);
                }
                catch (Exception ex1)
                {

                }
            }

            return null;
        }

        
        /// <summary>
        /// Validates the url to determine a valid login
        /// </summary>
        /// <param name="requestUrl">request url</param>
        /// <param name="apiKey">Api key</param>
        /// <param name="secret">Api secret</param>
        /// <param name="requestToken">Request token</param>
        /// <param name="checksum">checksum</param>
        /// <returns>Returns true if the login is a success. Also parse the requestToken and the checksum</returns>
        public static bool IsValidLogin(string url, string apiKey, string secret, out string requestToken, out string checksum)
        {
            requestToken = string.Empty;
            checksum = string.Empty;

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secret))
                return false;

            int idx = url.IndexOf('?');
            if (idx < 0)
                return false;

            var token = url.Substring(idx);
            var query = HttpUtility.ParseQueryString(token);
            token = query.Get("request_token");

            if (string.IsNullOrEmpty(token))
                return false;

            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes($"{apiKey}{token}{secret}"));

                foreach (var item in result)
                {
                    sb.Append(item.ToString("x2"));
                }
            }

            var check = sb.ToString();
            if (string.IsNullOrEmpty(check))
                return false;

            checksum = check;
            requestToken = token;

            return true;
        }

        

        //*********************** Kite object *************************************************************

        /// <summary>
        /// Connect to streaming socket
        /// </summary>
        /// <returns></returns>
        public abstract Task ConnectAsync();
        /// <summary>
        /// Disconnect from streaming socket
        /// </summary>
        /// <returns></returns>
        public abstract Task DisconnectAsync();
        /// <summary>
        /// Subscribed to quotes
        /// </summary>
        /// <param name="mode">Mode</param>
        /// <param name="tokens">Instrument token</param>
        /// <returns></returns>
        public abstract Task Subscribe(string mode, int[] tokens);
        /// <summary>
        /// Unsubscribe from quotes
        /// </summary>
        /// <param name="mode">mode</param>
        /// <param name="tokens">Instrument token</param>
        /// <returns></returns>
        public abstract Task Unsubscribe(string mode, int[] tokens);
        /// <summary>
        /// Gets if streaming socket is connected or not
        /// </summary>
        public abstract bool IsConnected { get; }

        /// <summary>
        /// On state change
        /// </summary>
        /// <param name="state"></param>
        protected virtual void OnState(KiteConnectState state)
        {
            this.State?.Invoke(new SocketStateEventArgs(state));
        }

        /// <summary>
        /// On new postback
        /// </summary>
        /// <param name="order"></param>
        protected virtual void OnPostback(OrderPostBack order)
        {
            this.Postback?.Invoke(new PostbackEventArgs(order));
        }

        /// <summary>
        /// On new quotes
        /// </summary>
        /// <param name="data"></param>
        protected virtual void OnQuotes(byte[] data)
        {
            this.Quotes?.Invoke(new QuoteEventArgs(data));
        }

        /// <summary>
        /// Raises the Postback event
        /// </summary>
        public event Action<PostbackEventArgs> Postback;
        
        /// <summary>
        /// Raises on incoming market data
        /// </summary>
        public event Action<QuoteEventArgs> Quotes;

        /// <summary>
        /// Raises on connection status
        /// </summary>
        public event Action<SocketStateEventArgs> State;





    }
}
