using System.Threading.Tasks;

namespace MPLC
{
    public interface IMPLCProvider
    {
        Task<bool> GetBitAsync(string address);

        Task<int> ReadWordAsync(string address);

        Task<int[]> ReadWordsAsync(string startAddress, int length);

        Task SetBitOffAsync(string address);

        Task SetBitOnAsync(string address);

        Task WriteWordAsync(string address, int value);

        Task WriteWordsAsync(string startAddress, int[] words);
    }
}