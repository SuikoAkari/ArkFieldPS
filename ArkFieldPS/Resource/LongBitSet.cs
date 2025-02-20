using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkFieldPS.Resource
{
    /// <summary>
    /// Utility class for supporting long bit set
    /// </summary>
    public class LongBitSet
    {
        ulong[] bits;
        int bitCount;

        public ulong[] Bits => bits;
        public int BitCount => bitCount;
        public LongBitSet(int bitCount)
        {
            InitializeWithBitCount(bitCount);
        }

        void InitializeWithBitCount(int bitCount)
        {
            this.bitCount = bitCount;
            int cnt = (bitCount / 64);
            if (bitCount % 64 != 0)
                cnt += 1;
            bits = new ulong[cnt];
        }

        public LongBitSet(ICollection<int> values)
        {
            int max = values.Max();
            InitializeWithBitCount(max);
            foreach(var i in values)
            {
                SetBit(i, true);
            }
        }

        public LongBitSet(ulong[] bits)
        {
            this.bits = bits;
            this.bitCount = bits.Length * 64;
        }

        public bool this[int bit]
        {
            get
            {
                return IsBitSet(bit);
            }
            set
            {
                SetBit(bit, value);
            }
        }

        public void SetBit(int bit, bool val)
        {
            if (bit > bitCount)
                throw new IndexOutOfRangeException();
            int remaining = bit % 64;
            int idx = bit / 64;
            ref ulong mask = ref bits[idx];
            if (val)
            {
                mask |= ((ulong)1 << remaining);
            }
            else
            {
                mask &= ~((ulong)1 << remaining);
            }
        }

        public bool IsBitSet(int bit)
        {
            if (bit > bitCount)
                throw new IndexOutOfRangeException();
            int remaining = bit % 64;
            int idx = bit / 64;
            ulong mask = bits[idx];
            ulong test = (ulong)1 << remaining;
            return (mask & test) == test;
        }

        public void And(LongBitSet other)
        {
            int cnt = Math.Min(bits.Length, other.bits.Length);
            for (int i = 0; i < cnt; i++)
            {
                bits[i] &= other.bits[i];
            }
        }

        public void Or(LongBitSet other)
        {
            int cnt = Math.Min(bits.Length, other.bits.Length);
            for (int i = 0; i < cnt; i++)
            {
                bits[i] |= other.bits[i];
            }
        }

        public void ClearBits(LongBitSet other)
        {
            int cnt = Math.Min(bits.Length, other.bits.Length);
            for (int i = 0; i < cnt; i++)
            {
                bits[i] &= ~other.bits[i];
            }
        }

        public int CountTrueBits()
        {
            int cnt = bitCount;
            int res = 0;
            for (int i = 0; i < cnt; i++)
            {
                if (IsBitSet(i))
                    res++;
            }

            return res;
        }

        public void Clear()
        {
            int cnt = bits.Length;
            for (int i = 0; i < cnt; i++)
            {
                bits[i] = 0;
            }
        }

        public void ConvertToIntValues(List<int> values)
        {
            for (int i = 0; i < bitCount; i++)
            {
                if (IsBitSet(i))
                    values.Add(i);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            LongBitSet other = obj as LongBitSet;
            if (other != null)
            {
                if (bitCount == other.bitCount)
                {
                    var cnt = bits.Length;
                    for (int i = 0; i < cnt; i++)
                    {
                        if (bits[i] != other.bits[i])
                            return false;
                    }
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
