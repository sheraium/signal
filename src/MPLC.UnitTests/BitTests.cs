using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace MPLC.UnitTests
{
    public class BitTests
    {
        private const string BitAddress = "D100.1";
        private Bit _bit;
        private IMPLCProvider _mplc;

        [SetUp]
        public void SetUp()
        {
            _mplc = Substitute.For<IMPLCProvider>();
            _bit = new Bit(_mplc, BitAddress);
        }

        [Test]
        public void set_on()
        {
            _bit.SetOn();

            _mplc.Received().SetBitOn(Arg.Is(BitAddress));
        }

        [Test]
        public void set_off()
        {
            _bit.SetOff();

            _mplc.Received().SetBitOff(Arg.Is(BitAddress));
        }

        [Test]
        public void set_isOn()
        {
            var isOn = true;
            _bit.Set(isOn);

            _mplc.Received().SetBitOn(Arg.Is(BitAddress));
        }

        [Test]
        public void set_isOff()
        {
            var isOn = false;
            _bit.Set(isOn);

            _mplc.Received().SetBitOff(Arg.Is(BitAddress));
        }


        [Test]
        public void isOn()
        {
            _mplc.GetBit(BitAddress).Returns(true);

            var actual = _bit.IsOn();

            actual.Should().BeTrue();
        }


        [Test]
        public void isOff()
        {
            _mplc.GetBit(BitAddress).Returns(true);

            var actual = _bit.IsOff();

            actual.Should().BeFalse();
        }


        [Test]
        public void value()
        {
            _mplc.GetBit(BitAddress).Returns(true);

            var actual = _bit.Value;

            actual.Should().Be(1);
        }


        [Test]
        public void address()
        {
            var actual = _bit.Address;

            actual.Should().Be(BitAddress);
        }
    }
}