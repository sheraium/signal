using System.Threading.Tasks;

namespace MPLC
{
    public interface IDataBlock
    {
        string Name { get; }

        Task<(bool successed, bool value)> TryGetBitAsync(string address);

        Task<(bool successed, int value)> TryGetWordAsync(string address);

        Task<(bool successed, int[] words)> TryGetWordsAsync(string startAddress, int length);

        Task<bool> TrySetBitAsync(string address, bool isOn);

        Task<bool> TrySetWordAsync(string address, int value);

        Task<bool> TrySetWordsAsync(string startAddress, int[] words);
    }
}