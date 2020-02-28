namespace MPLC
{
    public class Bit : IDataType
    {
        private readonly IMPLCProvider _mplc;
        private readonly string _address;

        public Bit(IMPLCProvider mplc, string address)
        {
            _mplc = mplc;
            _address = address;
        }

        public bool IsOn => _mplc.GetBit(_address);
        public bool IsOff => !_mplc.GetBit(_address);
        public double Value => IsOn ? 1 : 0;
        public string Address => _address;

        public void SetOn()
        {
            _mplc.SetBitOn(_address);
        }

        public void SetOff()
        {
            _mplc.SetBitOff(_address);
        }

        public void Set(bool isOn)
        {
            if (isOn)
            {
                _mplc.SetBitOn(_address);
            }
            else
            {
                _mplc.SetBitOff(_address);
            }
        }
    }
}