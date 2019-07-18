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
using SharpCharts.Base.Data;
using SharpCharts.Base.Order;
using KiteConnection;
using KiteConnectAPI;

namespace SharpCharts.Base.Connection
{
    public class KiteConnectConnection : ExternalConnectionBase, IKiteLogger
    {
        KiteViewModel viewModel;
        
        
        public override void Connect(IConnection connection)
        {
           
            string apiKey, secret;
            bool isHistoricalDataSubscribed;
            int maxReconnectionAttempts = 20;
            try
            {
                //cant reference the SharpCharts.User assembly as it is dynamically generated. 
                //thus we use dynamics to get the values
                dynamic options = this.Options;
                apiKey = options.ApiKey;
                secret = options.Secret;
                isHistoricalDataSubscribed = options.IsHistoricalDataSubscribed;
                maxReconnectionAttempts = options.MaxReconnectionAttempts;
            }
            catch (Exception ex)
            {
                Log($"{ex.Message}");
                SetConnectionState(ConnectionState.ConnectionFailed);
                return;
            }

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secret))
            {
                Log($"Connection failed : apiKey= {apiKey}, secret= {secret}");
                SetConnectionState(ConnectionState.ConnectionFailed);
                return;
            }

            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                this.DialogService.Show<KiteView>(this.viewModel = new KiteViewModel(this, apiKey, secret, isHistoricalDataSubscribed, maxReconnectionAttempts));
                try
                {
                    System.Windows.Threading.Dispatcher.Run();
                }
                catch (Exception ex)
                {
                    Log($"{ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Log($"{ex.InnerException.Message}");
                    }
                }
            });

            thread.IsBackground = true;
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();

        }

        
        public override void Disconnect()
        {
            if (this.viewModel == null)
            {
                SetConnectionState(ConnectionState.Disconnected);
            }

            this.viewModel.Disconnect();
        }

        public override async Task SubscribeLevel1(Instrument instrument)
        {
            if (this.viewModel == null)
                return;
            await this.viewModel.SubscribeLevel1(instrument).ConfigureAwait(false);
        }

        public override async Task UnsubscribeLevel1(Instrument instrument)
        {
            if (this.viewModel == null)
                return;

            await this.viewModel.UnsubscribeLevel1(instrument).ConfigureAwait(false);
        }

        public override async Task SubscribeHistoricalData(Instrument instrument, BuiltDataType builtDataType, BackfillPolicy backfillPolicy, DateTime startDate, DateTime endDate)
        {
            if (this.viewModel == null)
                return;

            await this.viewModel.SubscribeHistoricalData(instrument, builtDataType, backfillPolicy, startDate, endDate).ConfigureAwait(false);
        }

        
        public override async Task<StaticQuote> GetSnapQuote(Instrument instrument)
        {
            if (this.viewModel == null)
                return null;

            return await this.viewModel.GetSnapQuote(instrument).ConfigureAwait(false);
        }
        

        public override bool IsOrderFeed
        {
            get { return true; }
        }

        public override async Task SubmitOrder(IOrder order)
        {
            if (this.viewModel == null)
                return;

            //Automated stategies are not supported
            if (order.Strategy is SharpCharts.Base.SharpScript.StrategyBase)
                throw new Exception("Automated trading is not supported via the Kite API");

            await this.viewModel.SubmitOrder(order).ConfigureAwait(false);
        }

        public override async Task ChangeOrder(IOrder order)
        {
            if (this.viewModel == null)
                return;
            await this.viewModel.ChangeOrder(order).ConfigureAwait(false);
        }

        public override async Task CancelOrder(IOrder order)
        {
            if (this.viewModel == null)
                return;
            await this.viewModel.CancelOrder(order).ConfigureAwait(false);
        }


        #region Kite logger

        public void OnLog(string message)
        {
            Log(message, logCategory: Base.Log.LogCategory.Connection);
        }

        public void OnException(Exception ex)
        {
            ServerException sx = ex as ServerException;
            if (sx != null)
            {
                if (sx.ServerError != null)
                {
                    Log($"{sx.Type.Name}|{sx.ServerError.error_type}|{sx.ServerError.status} ({sx.Status}) : {sx.ServerError.message}", logCategory: Base.Log.LogCategory.Exception);

                    if (sx.ServerError.error_type == "TokenException" && sx.Type != typeof(Candle))
                    {
                        if (this.ConnectionState == ConnectionState.Connected)
                        {
                            this.viewModel?.Disconnect();
                        }
                    }

                }
            }
            else
            {
                Log(ex.Message, logCategory: Base.Log.LogCategory.Exception);
            }
        }

        #endregion


    }
}
