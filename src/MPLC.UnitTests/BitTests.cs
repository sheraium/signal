using System.Threading.Tasks;
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

        [Test]
        public async Task isOff()
        {
            _mplc.GetBitAsync(BitAddress).Returns(true);

            var actual = await _bit.IsOffAsync();

            actual.Should().BeFalse();
        }

        [Test]
        public async Task isOn()
        {
            _mplc.GetBitAsync(BitAddress).Returns(true);

            var actual = await _bit.IsOnAsync();

            actual.Should().BeTrue();
        }

        [Test]
        public async Task set_isOff()
        {
            var isOn = false;
            await _bit.SetAsync(isOn);

            await _mplc.Received().SetBitOffAsync(Arg.Is(BitAddress));
        }

        [Test]
        public async Task set_isOn()
        {
            var isOn = true;
            await _bit.SetAsync(isOn);

            await _mplc.Received().SetBitOnAsync(Arg.Is(BitAddress));
        }

        [Test]
        public async Task set_off()
        {
            await _bit.SetOffAsync();

            await _mplc.Received().SetBitOffAsync(Arg.Is(BitAddress));
        }

        [Test]
        public void set_on()
        {
            _bit.SetOnAsync();

            _mplc.Received().SetBitOnAsync(Arg.Is(BitAddress));
        }

        [SetUp]
        public void SetUp()
        {
            _mplc = Substitute.For<IMPLCProvider>();
            _bit = new Bit(_mplc, BitAddress);
        }
    }
}