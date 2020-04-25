using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace MPLC.UnitTests
{
    [TestFixture]
    public class WordTests
    {
        private const string WordAddress = "D100";
        private const bool On = true;
        private IMPLCProvider _mplc;
        private Word _word;

        [SetUp]
        public void SetUp()
        {
            _mplc = Substitute.For<IMPLCProvider>();
            _word = new Word(_mplc, "D100");
        }

        [Test]
        public async Task get_bit()
        {
            GivenWord(WordAddress, 1);
            await BitShouldBe(0, On);
        }

        [Test]
        public async Task get_value()
        {
            GivenWord(WordAddress, 1);
            await ValueShouldBe(1);
        }

        [Test]
        public async Task set_bit()
        {
            await WhenSetBit(0, On);
            await MPLCShouldWriteWord(WordAddress, 1);
        }

        [Test]
        public async Task set_value()
        {
            WhenSet(1);
            await MPLCShouldWriteWord(WordAddress, 1);
        }

        private async Task BitShouldBe(int index, bool expected)
        {
            var actual = await _word.GetBitAsync(index);

            actual.Should().Be(expected);
        }

        private void GivenWord(string address, int value)
        {
            _mplc.ReadWordAsync(address).Returns(value);
        }

        private async Task MPLCShouldWriteWord(string address, int value)
        {
            await _mplc.Received().WriteWordAsync(Arg.Is(address), Arg.Is(value));
        }

        private async Task ValueShouldBe(int expected)
        {
            var actual = await _word.GetValueAsync();

            actual.Should().Be(expected);
        }

        private void WhenSet(int value)
        {
            _word.SetValueAsync(value);
        }

        private async Task WhenSetBit(int index, bool isOn)
        {
            await _word.SetBitAsync(index, isOn);
        }
    }
}