using System.Linq;
using System.Threading.Tasks;

namespace MPLC
{
    public class Words
    {
        private readonly int _length;
        private readonly IMPLCProvider _mplc;
        private readonly string _startAddress;

        public Words(IMPLCProvider mplc, string startAddress, int length)
        {
            _mplc = mplc;
            _startAddress = startAddress;
            _length = length >= 1 ? length : 1;
        }

        public int[] GetData()
        {
            return _mplc.ReadWords(_startAddress, _length);
        }

        public Task<int[]> GetDataAsync()
        {
            return _mplc.ReadWordsAsync(_startAddress, _length);
        }

        public void SetData(int[] data)
        {
            _mplc.WriteWords(_startAddress, data.Take(_length).ToArray());
        }

        public Task SetDataAsync(int[] data)
        {
            return _mplc.WriteWordsAsync(_startAddress, data.Take(_length).ToArray());
        }
    }
}