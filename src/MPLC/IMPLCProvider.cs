using System.Threading.Tasks;

namespace MPLC
{
    public interface IMPLCProvider
    {
        bool GetBit(string address);

        Task<bool> GetBitAsync(string address);

        int ReadWord(string address);

        Task<int> ReadWordAsync(string address);

        void SetBitOff(string address);

        Task SetBitOffAsync(string address);

        void SetBitOn(string address);

        Task SetBitOnAsync(string address);

        void WriteWord(string address, int value);

        Task WriteWordAsync(string address, int value);
    }
}