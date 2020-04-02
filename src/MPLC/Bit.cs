using System.Threading.Tasks;

namespace MPLC
{
    public class Bit : IDataType
    {
        private readonly IMPLCProvider _mplc;

        public Bit(IMPLCProvider mplc, string address)
        {
            _mplc = mplc;
            Address = address;
        }

        public double Value => IsOn() ? 1 : 0;
        public string Address { get; }

        public void SetOn()
        {
            _mplc.SetBitOn(Address);
        }

        public void SetOff()
        {
            _mplc.SetBitOff(Address);
        }

        public void Set(bool isOn)
        {
            if (isOn)
            {
                _mplc.SetBitOn(Address);
            }
            else
            {
                _mplc.SetBitOff(Address);
            }
        }

        public bool IsOn()
        {
            return _mplc.GetBit(Address);
        }

        public bool IsOff()
        {
            return !_mplc.GetBit(Address);
        }

        public async Task SetOnAsync()
        {
            await _mplc.SetBitOnAsync(Address);
        }

        public async Task SetOffAsync()
        {
            await _mplc.SetBitOffAsync(Address);
        }

        public async Task SetAsync(bool isOn)
        {
            if (isOn)
            {
                await _mplc.SetBitOnAsync(Address);
            }
            else
            {
                await _mplc.SetBitOffAsync(Address);
            }
        }

        public async Task<bool> IsOnAsync()
        {
            return await _mplc.GetBitAsync(Address);
        }

        public async Task<bool> IsOffAsync()
        {
            return !await _mplc.GetBitAsync(Address);
        }
    }
}