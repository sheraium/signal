using System.Threading.Tasks;

namespace MPLC
{
    public interface IDataBlock
    {
        string Name { get; }

        //bool TryGetBit(string address, out bool value);

        Task<(bool successed, bool value)> TryGetBitAsync(string address);

        //bool TryGetWord(string address, out int value);

        Task<(bool successed, int value)> TryGetWordAsync(string address);

        //bool TryGetWords(string startAddress, int length, out int[] words);

        Task<(bool successed, int[] words)> TryGetWordsAsync(string startAddress, int length);

        //bool TrySetBitOff(string address);

        Task<bool> TrySetBitOffAsync(string address);

        //bool TrySetBitOn(string address);

        Task<bool> TrySetBitOnAsync(string address);

        //bool TrySetWord(string address, int value);

        Task<bool> TrySetWordAsync(string address, int value);

        //bool TrySetWords(string startAddress, int[] words);

        Task<bool> TrySetWordsAsync(string startAddress, int[] words);
    }
}