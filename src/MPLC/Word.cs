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

        public async Task<bool> GetBitAsync(int index)
        {
            if (index < 0 || index >= 16) return false;
            return new BitArray(BitConverter.GetBytes(await GetValueAsync())).Get(index);
        }

        public Task<int> GetValueAsync()
        {
            return _mplc.ReadWordAsync(_address);
        }

        public async Task SetBitAsync(int index, bool isOn)
        {
            if (index < 0 || index >= 16) return;
            var bitArray = new BitArray(BitConverter.GetBytes(await GetValueAsync()));
            bitArray.Set(index, isOn);
            var bytes = new byte[4];
            bitArray.CopyTo(bytes, 0);
            await SetValueAsync(BitConverter.ToUInt16(bytes, 0));
        }

        public Task SetValueAsync(int value)
        {
            return _mplc.WriteWordAsync(_address, value);
        }
    }
}