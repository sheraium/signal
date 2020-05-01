using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace MPLC.UnitTests
{
    [TestFixture]
    public class BitTests
    {
        private const string BitAddress = "D100.1";
        private const bool Off = false;
        private const bool On = true;
        private Bit _bit;
        private IMPLCProvider _mplc;

        [SetUp]
        public void SetUp()
        {
            _mplc = Substitute.For<IMPLCProvider>();
            _bit = new Bit(_mplc, BitAddress);
        }

        [Test]
        public async Task bit_is_off()
        {
            GivenBit(BitAddress, Off);
            await BitShouldBe(Off);
        }

        [Test]
        public async Task bit_is_on()
        {
            GivenBit(BitAddress, On);
            await BitShouldBe(On);
        }

        [Test]
        public async Task set_bit_off()
        {
            await WhenSetOff();
            await MPLCShouldBeSetBit(BitAddress, Off);
        }

        [Test]
        public async Task set_it_off()
        {
            await WhenSetBit(Off);
            await MPLCShouldBeSetBit(BitAddress, Off);
        }

        [Test]
        public async Task set_it_on()
        {
            await WhenSetBit(On);
            await MPLCShouldBeSetBit(BitAddress, On);
        }

        [Test]
        public async Task set_on()
        {
            await WhenSetOn();
            await MPLCShouldBeSetBit(BitAddress, On);
        }

        private async Task BitShouldBe(bool expected)
        {
            var actual = await _bit.IsOnAsync();

            actual.Should().Be(expected);
        }

        private void GivenBit(string address, bool isOn)
        {
            _mplc.GetBitAsync(address).Returns(isOn);
        }

        private async Task MPLCShouldBeSetBit(string address, bool isOn)
        {
            await _mplc.Received().SetBitAsync(Arg.Is(address), Arg.Is(isOn));
        }

        private async Task WhenSetBit(bool isOn)
        {
            await _bit.SetAsync(isOn);
        }

        private async Task WhenSetOff()
        {
            await _bit.SetOffAsync();
        }

        private async Task WhenSetOn()
        {
            await _bit.SetOnAsync();
        }
    }
}