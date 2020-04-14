using System.Threading.Tasks;

namespace MPLC
{
    /// <summary>
    /// 32 bit integer
    /// </summary>
    /// <seealso cref="MPLC.IDataType" />
    public class DWord : IDataType
    {
        private readonly string _address;
        private readonly IMPLCProvider _mplc;

        public DWord(IMPLCProvider mplc, string address)
        {
            _mplc = mplc;
            _address = address;
        }

        public string Address => _address;
        public double Value => GetValue();

        public int GetValue()
        {
            var words = _mplc.ReadWords(_address, 2);
            return words[0] + words[1] * 65536;
        }

        public async Task<int> GetValueAsync()
        {
            var words = await _mplc.ReadWordsAsync(_address, 2);
            return words[0] + words[1] * 65536;
        }

        public void SetValue(int value)
        {
            _mplc.WriteWords(_address, new[] { value % 65536, value / 65536 });
        }

        public Task SetValueAsync(int value)
        {
            return _mplc.WriteWordsAsync(_address, new[] { value % 65536, value / 65536 });
        }
    }
}