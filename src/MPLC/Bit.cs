using System.Threading.Tasks;

namespace MPLC
{
    /// <summary>
    ///     1 bit
    /// </summary>
    /// <seealso cref="MPLC.IDataType" />
    public class Bit : IDataType
    {
        private readonly IMPLCProvider _mplc;

        public Bit(IMPLCProvider mplc, string address)
        {
            _mplc = mplc;
            Address = address;
        }

        public string Address { get; }

        public async Task<bool> IsOffAsync()
        {
            return !await _mplc.GetBitAsync(Address);
        }

        public Task<bool> IsOnAsync()
        {
            return _mplc.GetBitAsync(Address);
        }

        public Task SetAsync(bool isOn)
        {
            return _mplc.SetBitAsync(Address, isOn);
        }

        public Task SetOffAsync()
        {
            return _mplc.SetBitAsync(Address, false);
        }

        public Task SetOnAsync()
        {
            return _mplc.SetBitAsync(Address, true);
        }
    }
}