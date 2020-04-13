using System;
using System.Collections;
using System.Threading.Tasks;

namespace MPLC
{
    /// <summary>
    /// 16 bit integer
    /// </summary>
    /// <seealso cref="MPLC.IDataType" />
    public class Word : IDataType
    {
        private readonly string _address;
        private readonly IMPLCProvider _mplc;

        public Word(IMPLCProvider mplc, string address)
        {
            _mplc = mplc;
            _address = address;
        }

        public string Address => _address;

        public double Value => GetValue();

        public bool GetBit(int index)
        {
            if (index < 0 || index >= 16) return false;
            return new BitArray(BitConverter.GetBytes(GetValue())).Get(index);
        }

        public async Task<bool> GetBitAsync(int index)
        {
            if (index < 0 || index >= 16) return false;
            return new BitArray(BitConverter.GetBytes(await GetValueAsync())).Get(index);
        }

        public int GetValue()
        {
            return _mplc.ReadWord(_address);
        }

        public async Task<int> GetValueAsync()
        {
            return await _mplc.ReadWordAsync(_address);
        }

        public void SetBit(int index, bool isOn)
        {
            if (index < 0 || index >= 16) return;
            var bitArray = new BitArray(BitConverter.GetBytes(GetValue()));
            bitArray.Set(index, isOn);
            var bytes = new byte[4];
            bitArray.CopyTo(bytes, 0);
            SetValue(BitConverter.ToUInt16(bytes, 0));
        }

        public async Task SetBitAsync(int index, bool isOn)
        {
            if (index < 0 || index >= 16) return;
            var bitArray = new BitArray(BitConverter.GetBytes(await GetValueAsync()));
            bitArray.Set(index, isOn);
            var bytes = new byte[4];
            bitArray.CopyTo(bytes, 0);
            SetValue(BitConverter.ToUInt16(bytes, 0));
        }

        public void SetValue(int value)
        {
            _mplc.WriteWord(_address, value);
        }

        public async Task SetValueAsync(int value)
        {
            await _mplc.WriteWordAsync(_address, value);
        }
    }
}