using System.Threading.Tasks;

namespace MPLC
{
    public interface IMPLCProvider
    {
        bool GetBit(string address);

        Task<bool> GetBitAsync(string address);

        void SetBitOff(string address);

        Task SetBitOffAsync(string address);

        void SetBitOn(string address);

        Task SetBitOnAsync(string address);
    }
}