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
using System.Globalization;

namespace KiteConnectAPI
{
    public static class Payload
    {

        /// <summary>
        /// Returns the payload for token
        /// </summary>
        /// <param name="api_key">Api Key</param>
        /// <param name="request_token">Request token</param>
        /// <param name="checksum">Checksum</param>
        /// <returns></returns>
        public static string Token(string api_key, string request_token, string checksum)
        {
            return $"api_key={api_key}&request_token={request_token}&checksum={checksum}";
        }

        /// <summary>
        /// Returns the payload for placing an new order
        /// </summary>
        /// <param name="exchange">Exchange</param>
        /// <param name="tradingsymbol">Trading symbol</param>
        /// <param name="transaction_type">Transaction type</param>
        /// <param name="order_type">Order type</param>
        /// <param name="product">Product type</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="price">Price</param>
        /// <param name="trigger_price">Trigger price</param>
        /// <param name="disclosed_quantity">Disclosed quantity</param>
        /// <param name="valididy">Validity</param>
        /// <param name="tag">Tag</param>
        /// <param name="stoploss">Stop loss value for BO orders</param>
        /// <param name="squareoff">Target value for BO orders</param>
        /// <param name="trailing_stop">Trailing stop for BO orders</param>
        /// <returns></returns>
        public static string PlaceOrder(string exchange, string tradingsymbol, string transaction_type, string order_type,  string product, int quantity, double price, 
            double trigger_price, int disclosed_quantity = 0, string valididy = "DAY", string tag = null, double stoploss = 0.0d, double squareoff = 0.0d, double trailing_stop = 0.0d)
        {
            return string.Format(CultureInfo.InvariantCulture, "exchange={0}&tradingsymbol={1}&transaction_type={2}&order_type={3}&product={4}&quantity={5}&price={6}&trigger_price={7}&disclosed_quantity={8}&validity={9}&tag={10}&&stoploss={11}&squareoff={12}&trailing_stoploss={13}",
                exchange, tradingsymbol, transaction_type, order_type, product, quantity, price, trigger_price, disclosed_quantity, valididy, tag, stoploss, squareoff, trailing_stop);
        }

        /// <summary>
        /// Returns the payload for modifying an order
        /// </summary>
        /// <param name="order_type">Order type</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="price">Price</param>
        /// <param name="trigger_price">Stop price</param>
        /// <param name="disclosed_quantity">Disclosed price</param>
        /// <param name="validity">Validity</param>
        /// <returns></returns>
        public static string ModifyOrder(string order_type, int quantity, double price, double trigger_price, int disclosed_quantity = 0, string validity = "DAY")
        {
            return string.Format(CultureInfo.InvariantCulture, "order_type={0}&quantity={1}&price={2}&trigger_price={3}&disclosed_quantity={4}&validity={5}",
                order_type, quantity, price, trigger_price, disclosed_quantity, validity);
        }

        /// <summary>
        /// Returns the payload for modifying a bracket order
        /// </summary>
        /// <param name="parent_order_id">Parent order id</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="price">Price</param>
        /// <param name="trigger_price">Trigger price</param>
        /// <returns></returns>
        public static string ModifyBracketOrder(string parent_order_id, int quantity, double price, double trigger_price)
        {
            return string.Format(CultureInfo.InvariantCulture, "parent_order_id={0}&quantity={1}&price={2}&trigger_price={3}",
                parent_order_id, quantity, price, trigger_price);
        }

        /// <summary>
        /// Returns the payload for modifying conditional order
        /// </summary>
        /// <param name="order_id">Order id</param>
        /// <param name="price">Price</param>
        /// <param name="trigger_price">Trigger price</param>
        /// <returns></returns>
        public static string ModifyConditionalOrder(string order_id, double price, double trigger_price)
        {
            return string.Format(CultureInfo.InvariantCulture, "order_id={0}&price={1}&trigger_price={2}",
                order_id, price, trigger_price);
        }

        /// <summary>
        /// Returns the payload to convert a position
        /// </summary>
        /// <param name="exchange">Exchange</param>
        /// <param name="tradingsymbol">Trading symbol</param>
        /// <param name="transaction_type">Transaction type</param>
        /// <param name="position_type">Position type</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="old_product">Old product</param>
        /// <param name="new_product">New product</param>
        /// <returns></returns>
        public static string ConvertPosition(string exchange, string tradingsymbol, string transaction_type, string position_type, int quantity, string old_product, string new_product)
        {
            return string.Format(CultureInfo.InvariantCulture, "exchange={0}&tradingsymbol={1}&transaction_type={2}&position_type={3}&quantity={4}&old_product={5}&new_product={6}",
                exchange, tradingsymbol, transaction_type, position_type, quantity, old_product, new_product);
        }

        /// <summary>
        /// Returns the payload for placing a mutual fund order
        /// </summary>
        /// <param name="tradingsymbol">Trading symbol</param>
        /// <param name="transaction_type">Transaction type</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="amount">Amount</param>
        /// <param name="tag">Tag</param>
        /// <returns></returns>
        public static string PlaceMfOrder(string tradingsymbol, string transaction_type, int quantity, double amount, string tag = null)
        {
            return string.Format(CultureInfo.InvariantCulture, "tradingsymbol={0}&transaction_type={1}&quantity={2}&amount={3}&tag={4}",
                tradingsymbol, transaction_type, quantity, amount, tag);
        }

        /// <summary>
        /// Returns teh payload to place a sip order
        /// </summary>
        /// <param name="tradingsymbol">Trading symbol</param>
        /// <param name="frequency">Frequency</param>
        /// <param name="instalment_day">Installment day</param>
        /// <param name="instalments">Installments</param>
        /// <param name="initial_amount">Initial amount</param>
        /// <param name="amount">Amount</param>
        /// <param name="tag">Tag</param>
        /// <returns></returns>
        public static string PlaceSipOrder(string tradingsymbol, string frequency, int instalment_day, int instalments, double initial_amount, double amount, string tag = null)
        {
            return string.Format(CultureInfo.InvariantCulture, "tradingsymbol={0}&frequency={1}&instalment_day={2}&instalments={3}&initial_amount={4}&amount={5}&tag={6}",
                tradingsymbol, frequency, instalment_day, instalments, initial_amount, amount, tag);
        }

        /// <summary>
        /// Returns the payload to modify a sim order
        /// </summary>
        /// <param name="frequency">Frequency</param>
        /// <param name="instalments">Installments</param>
        /// <param name="amount">Amount</param>
        /// <param name="status">Status</param>
        /// <param name="instalment_day">Installment day</param>
        /// <returns></returns>
        public static string ModifySipOrder(string frequency, int instalments, double amount, string status, int instalment_day)
        {
            return string.Format(CultureInfo.InvariantCulture, "frequency={0}&instalments={1}&amount={2}&status={3}&instalment_day={4}",
                frequency, instalments, amount, status, instalment_day);
        }


    }
}
