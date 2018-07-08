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
using KiteConnectAPI;
using SharpCharts.Base.Data;
using SharpCharts.Base.Connection;
using SharpCharts.Base.Dom;

namespace KiteConnection
{
    public class KiteQuotes
    {

        Symbol symbol;
        TimeSpan utcOffset = TimeSpan.Zero;

        uint totalVolume, openInterest, high, low, open, ltp, vwap, prClose;
        
        uint[] bids = new uint[5];
        uint[] asks = new uint[5];
        uint[] bidVolume = new uint[5];
        uint[] askVolume = new uint[5];

        public KiteQuotes(Instrument instrument, Symbol symbol)
        {
            this.utcOffset = SharpCharts.Base.Common.Globals.GetUTCTimeZoneOffset(SharpCharts.Base.Common.TimeZone.IndiaStandardTime);
            this.Instrument = instrument;
            this.symbol = symbol;
        }

        public void Process(BinaryQuotes bq)
        {
            if (this.Instrument == null)
                return;

            Level2 l2 = bq as Level2;
            if (l2 != null)
            {
                DateTime time = GetTime(l2.TimeStamp);

                DateTime ltt = GetTime(l2.LastTradedTime);
                

                if (this.open != l2.OpenPrice)
                {
                    this.open = l2.OpenPrice;
                    ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(l2.OpenPrice), 0L, 0L, MarketDataType.Open);
                }

                if (this.high != l2.HighPrice)
                {
                    this.high = l2.HighPrice;
                    ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(l2.HighPrice), 0L, 0L, MarketDataType.High);
                }

                if (this.low != l2.LowPrice)
                {
                    this.low = l2.LowPrice;
                    ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(l2.LowPrice), 0L, 0L, MarketDataType.Low);
                }

                if (this.prClose != l2.ClosePrice)
                {
                    this.prClose = l2.ClosePrice;
                    ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(l2.ClosePrice), 0L, 0L, MarketDataType.PreviousClose);
                }

                if (this.vwap != l2.AvgTradedPrice)
                {
                    this.vwap = l2.AvgTradedPrice;
                    ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(l2.AvgTradedPrice), 0L, 0L, MarketDataType.VWAP);
                }

                if (this.openInterest != l2.OpenInterest)
                {
                    this.openInterest = l2.OpenInterest;
                    ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, 0.0d, 0L, l2.OpenInterest, MarketDataType.OpenInterest);
                }

                for (int i = 0; i < 5; i++)
                {
                    if (l2.AskLevels != null && l2.AskLevels.Length > i && l2.AskLevels[i] != null)
                    {
                        if (l2.AskLevels[i].Price != asks[i] || l2.AskLevels[i].Quantity != askVolume[i])
                        {
                            askVolume[i] = l2.AskLevels[i].Quantity;
                            asks[i] = l2.AskLevels[i].Price;

                            if (i == 0)
                            {
                                ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(l2.AskLevels[i].Price), l2.AskLevels[i].Quantity, 0L, MarketDataType.Ask);
                            }

                            ExternalConnectionBase.AppendL2Data(this.Instrument, time, i, GetPrice(l2.AskLevels[i].Price), l2.AskLevels[i].Quantity, (l2.AskLevels[i].Quantity <= 0 ? Operation.Delete : Operation.Added), MarketDataType.Ask);

                        }
                    }

                    if (l2.BidLevels != null && l2.BidLevels.Length >= 5)
                    {
                        if (l2.BidLevels != null && l2.BidLevels.Length > i && l2.BidLevels[i] != null)
                        {
                            if (l2.BidLevels[i].Price != bids[i] || l2.BidLevels[i].Quantity != bidVolume[i])
                            {
                                bidVolume[i] = l2.BidLevels[i].Quantity;
                                bids[i] = l2.BidLevels[i].Price;

                                if (i == 0)
                                {
                                    ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(l2.BidLevels[i].Price), l2.BidLevels[i].Quantity, 0L, MarketDataType.Bid);
                                }

                                ExternalConnectionBase.AppendL2Data(this.Instrument, time, i, GetPrice(l2.BidLevels[i].Price), l2.BidLevels[i].Quantity, (l2.BidLevels[i].Quantity <= 0 ? Operation.Delete : Operation.Added), MarketDataType.Bid);
                            }
                        }
                    }
                }

                //Previous trading date is stamped as the Last Traded Time of the pre-opening (9:00AM - 9:07AM) quotes. Thus we ignore those quotes
                if (ltt.Date != time.Date)
                {
                    this.HasInitialized = true;
                    this.totalVolume = 0;
                    return;
                }

                //and append the last
                if (this.HasInitialized)
                {
                    if (l2.TotalVolume > this.totalVolume)
                    {
                        ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, ltt, GetPrice(l2.LastTradedPrice), l2.TotalVolume - this.totalVolume, 0L, MarketDataType.Last);
                        this.totalVolume = l2.TotalVolume;
                        ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, 0.0d, l2.TotalVolume, 0L, MarketDataType.TotalVolume);
                    }
                }
                else
                {
                    this.HasInitialized = true;
                    if (DateTime.UtcNow.Add(this.utcOffset).Date == time.Date)
                        this.totalVolume = l2.TotalVolume;
                    else
                        this.totalVolume = 0;
                    ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, 0.0d, l2.TotalVolume, 0L, MarketDataType.TotalVolume);
                }
                
            }
            else
            {
                Indices indices = bq as Indices;
                
                if (indices != null)
                {
                    DateTime time = GetTime(indices.TimeStamp);

                    if (this.open != indices.OpenPrice)
                    {
                        this.open = indices.OpenPrice;
                        ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(indices.OpenPrice), 0L, 0L, MarketDataType.Open);
                    }

                    if (this.high != indices.HighPrice)
                    {
                        this.high = indices.HighPrice;
                        ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(indices.HighPrice), 0L, 0L, MarketDataType.High);
                    }

                    if (this.low != indices.LowPrice)
                    {
                        this.low = indices.LowPrice;
                        ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(indices.LowPrice), 0L, 0L, MarketDataType.Low);
                    }

                    if (this.prClose != indices.ClosePrice)
                    {
                        this.prClose = indices.ClosePrice;
                        ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(indices.ClosePrice), 0L, 0L, MarketDataType.PreviousClose);
                    }


                    if (this.HasInitialized)
                    {
                        if (this.ltp != indices.LastTradedPrice)
                        {
                            this.ltp = indices.LastTradedPrice;
                            ExternalConnectionBase.AppendRealTimeQuotes(this.Instrument, time, GetPrice(indices.LastTradedPrice), 0L, 0L, MarketDataType.Last);
                        }
                    }
                    else
                    {
                        this.ltp = indices.LastTradedPrice;
                        this.HasInitialized = true;
                    }
                }
            }
        }

        private DateTime GetTime(uint time)
        {
            return new DateTime(1970, 1, 1).AddSeconds(time).Add(this.utcOffset);
        }

        private double GetPrice(uint value)
        {
            if (this.Instrument.InstrumentDefination.Exchange == Exchange.NSECDS)
            {
                return (double)value / 10000000.0d;
            }

            return (double)value / 100.0d;
        }

        public Instrument Instrument { get; private set; }
        public bool HasInitialized { get; set; }

    }
}
