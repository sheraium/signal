namespace MPLC
{
    public interface IMPLCProvider
    {
        void SetBitOn(string address);
        void SetBitOff(string address);
        bool GetBit(string address);
    }
}