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
    public abstract class BinaryQuotes
    {

        public static ushort Reverse(ushort value)
        {
            return (ushort)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        public static uint Reverse(uint value)
        {
            return (value & 0x000000FFU) << 24 |
                (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 |
                (value & 0xFF000000U) >> 24;
        }

        public static BinaryQuotes[] Parse(byte[] data)
        {
            if (data == null)
                return null;

            if (data.Length < 2)
                return null;

            short noOfPackets = BitConverter.ToInt16(data, 0);

            List<BinaryQuotes> list = new List<BinaryQuotes>();
            int position = 2;

            while (true)
            {
                if (data.Length < position + 2)
                    break;

                ushort bytesLen = BitConverter.IsLittleEndian ? Reverse(BitConverter.ToUInt16(data, position)) : BitConverter.ToUInt16(data, position);
                position += 2;

                if (data.Length < position + bytesLen)
                    break;

                byte[] b = new byte[bytesLen];
                Array.Copy(data, position, b, 0, b.Length);


                BinaryQuotes quote = null;

                if (bytesLen == 8)
                {
                    quote = new Ltp();

                }
                else if (bytesLen == 32)
                {
                    quote = new Indices();
                }
                else if (bytesLen == 44)
                {
                    quote = new Level1();
                }
                else if (bytesLen == 184)
                {
                    quote = new Level2();
                }

                if (quote != null)
                {
                    quote.Processs(b);

                    list.Add(quote);
                }

                position += bytesLen;
            }

            if (list.Count == 0)
                return null;

            return list.ToArray();

        }

        
        public uint Token { get; set; }
        public uint LastTradedPrice { get; set; }

        public abstract void Processs(byte[] bytes);
    }

    public class Ltp : BinaryQuotes
    {
        public override void Processs(byte[] bytes)
        {
            if (bytes.Length < 8)
                return;

            if (BitConverter.IsLittleEndian)
            {
                this.Token = Reverse(BitConverter.ToUInt32(bytes, 0));
                this.LastTradedPrice = Reverse(BitConverter.ToUInt32(bytes, 4));
            }
            else
            {
                this.Token = BitConverter.ToUInt32(bytes, 0);
                this.LastTradedPrice = BitConverter.ToUInt32(bytes, 4);
            }
        }

        public override string ToString()
        {
            return string.Format("Token = {0}, Ltp = {1}", this.Token, this.LastTradedPrice);
        }
    }

    public class Indices : Ltp
    {
        public uint HighPrice { get; set; }
        public uint LowPrice { get; set; }
        public uint OpenPrice { get; set; }
        public uint ClosePrice { get; set; }
        public uint Change { get; set; }
        public uint TimeStamp { get; set; }

        public override void Processs(byte[] bytes)
        {
            if (bytes.Length < 32)
                return;

            base.Processs(bytes);

            if (BitConverter.IsLittleEndian)
            {
                this.HighPrice = Reverse(BitConverter.ToUInt32(bytes, 8));
                this.LowPrice = Reverse(BitConverter.ToUInt32(bytes, 12));
                this.OpenPrice = Reverse(BitConverter.ToUInt32(bytes, 16));
                this.ClosePrice = Reverse(BitConverter.ToUInt32(bytes, 20));
                this.Change = Reverse(BitConverter.ToUInt32(bytes, 24));
                this.TimeStamp = Reverse(BitConverter.ToUInt32(bytes, 28));
            }
            else
            {
                this.HighPrice = BitConverter.ToUInt32(bytes, 8);
                this.LowPrice = BitConverter.ToUInt32(bytes, 12);
                this.OpenPrice = BitConverter.ToUInt32(bytes, 16);
                this.ClosePrice = BitConverter.ToUInt32(bytes, 20);
                this.Change = BitConverter.ToUInt32(bytes, 24);
                this.TimeStamp = BitConverter.ToUInt32(bytes, 28);
            }

        }

        public override string ToString()
        {
            return base.ToString() + string.Format(", Change={1}", this.Change);
        }
    }

    public class Level1 : Ltp
    {
        public uint LastTradedQuantity { get; set; }
        public uint AvgTradedPrice { get; set; }
        public uint TotalVolume { get; set; }
        public uint TotalBuyQuantity { get; set; }
        public uint TotalSellQuantity { get; set; }
        public uint OpenPrice { get; set; }
        public uint HighPrice { get; set; }
        public uint LowPrice { get; set; }
        public uint ClosePrice { get; set; }

        public override void Processs(byte[] bytes)
        {
            if (bytes.Length < 44)
                return;

            base.Processs(bytes);

            if (BitConverter.IsLittleEndian)
            {
                this.LastTradedQuantity = Reverse(BitConverter.ToUInt32(bytes, 8));
                this.AvgTradedPrice = Reverse(BitConverter.ToUInt32(bytes, 12));
                this.TotalVolume = Reverse(BitConverter.ToUInt32(bytes, 16));
                this.TotalBuyQuantity = Reverse(BitConverter.ToUInt32(bytes, 20));
                this.TotalSellQuantity = Reverse(BitConverter.ToUInt32(bytes, 24));
                this.OpenPrice = Reverse(BitConverter.ToUInt32(bytes, 28));
                this.HighPrice = Reverse(BitConverter.ToUInt32(bytes, 32));
                this.LowPrice = Reverse(BitConverter.ToUInt32(bytes, 36));
                this.ClosePrice = Reverse(BitConverter.ToUInt32(bytes, 40));
            }
            else
            {
                this.LastTradedQuantity = BitConverter.ToUInt32(bytes, 8);
                this.AvgTradedPrice = BitConverter.ToUInt32(bytes, 12);
                this.TotalVolume = BitConverter.ToUInt32(bytes, 16);
                this.TotalBuyQuantity = BitConverter.ToUInt32(bytes, 20);
                this.TotalSellQuantity = BitConverter.ToUInt32(bytes, 24);
                this.OpenPrice = BitConverter.ToUInt32(bytes, 28);
                this.HighPrice = BitConverter.ToUInt32(bytes, 32);
                this.LowPrice = BitConverter.ToUInt32(bytes, 36);
                this.ClosePrice = BitConverter.ToUInt32(bytes, 40);
            }
        }

        public override string ToString()
        {
            return base.ToString() + string.Format(", ATP = {0}", this.AvgTradedPrice);
        }
    }

    public class Level2 : Level1
    {
        public uint LastTradedTime { get; set; }
        public uint OpenInterest { get; set; }
        public uint OpenInterestHigh { get; set; }
        public uint OpenInterestLow { get; set; }
        public uint TimeStamp { get; set; }

        public MarketDepth[] BidLevels { get; set; }
        public MarketDepth[] AskLevels { get; set; }

        public override void Processs(byte[] bytes)
        {
            if (bytes.Length < 184)
                return;

            base.Processs(bytes);

            

            if (BitConverter.IsLittleEndian)
            {
                this.LastTradedTime = Reverse(BitConverter.ToUInt32(bytes, 44));
                this.OpenInterest = Reverse(BitConverter.ToUInt32(bytes, 48));
                this.OpenInterestHigh = Reverse(BitConverter.ToUInt32(bytes, 52));
                this.OpenInterestLow = Reverse(BitConverter.ToUInt32(bytes, 56));
                this.TimeStamp = Reverse(BitConverter.ToUInt32(bytes, 60));
            }
            else
            {
                this.LastTradedTime = BitConverter.ToUInt32(bytes, 44);
                this.OpenInterest = BitConverter.ToUInt32(bytes, 48);
                this.OpenInterestHigh = BitConverter.ToUInt32(bytes, 52);
                this.OpenInterestLow = BitConverter.ToUInt32(bytes, 56);
                this.TimeStamp = BitConverter.ToUInt32(bytes, 60);
            }

            int position = 64;

            this.BidLevels = new MarketDepth[5];
            this.AskLevels = new MarketDepth[5];
            for (int i = 0; i < 10; i++)
            {
                byte[] b = new byte[12];
                Array.Copy(bytes, position, b, 0, b.Length);

                MarketDepth md = new MarketDepth();
                md.process(b);

                if (i < 5)
                {
                    this.BidLevels[i] = md;
                }
                else
                {
                    this.AskLevels[i - 5] = md;
                }

                position += 12;
            }
        }

        public override string ToString()
        {
            if (this.BidLevels != null && this.BidLevels.Length > 0)
            {
                return base.ToString() + string.Format(", BidAsk = {0}-{1}", this.BidLevels[0].Price, this.AskLevels[0].Price);
            }
            else return "no L2";
        }
    }

    public class MarketDepth
    {
        public uint Quantity { get; set; }
        public uint Price { get; set; }
        public uint Orders { get; set; }

        public void process(byte[] bytes)
        {
            if (bytes.Length != 12)
                return;

            if (BitConverter.IsLittleEndian)
            {
                this.Quantity = BinaryQuotes.Reverse(BitConverter.ToUInt32(bytes, 0));
                this.Price = BinaryQuotes.Reverse(BitConverter.ToUInt32(bytes, 4));
                this.Orders = BinaryQuotes.Reverse(BitConverter.ToUInt32(bytes, 8));
            }
            else
            {
                this.Quantity = BitConverter.ToUInt32(bytes, 0);
                this.Price = BitConverter.ToUInt32(bytes, 4);
                this.Orders = BitConverter.ToUInt32(bytes, 8);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Price, this.Quantity);
        }
    }
}
