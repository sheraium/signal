using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace MPLC.UnitTests
{
    [TestFixture]
    public class DWordTests
    {
        private const string StartAddress = "D100";
        private DWord _dWord;
        private IMPLCProvider _mplc;

        [SetUp]
        public void SetUp()
        {
            _mplc = Substitute.For<IMPLCProvider>();
            _dWord = new DWord(_mplc, StartAddress);
        }

        [Test]
        public async Task get_value()
        {
            GivenDword(StartAddress, 65536);
            await ValueShouldBe(65536);
        }

        [Test]
        public async Task set_value()
        {
            WhenSetValue(65536);
            await MPLCShouldWriteWords(StartAddress, new[] { 0, 1 });
        }

        private void GivenDword(string startAddress, int value)
        {
            _mplc.ReadWordsAsync(startAddress, 2).Returns(new[] { value % 65536, value / 65536 });
        }

        private async Task MPLCShouldWriteWords(string address, int[] value)
        {
            await _mplc.Received().WriteWordsAsync(address, Arg.Is<int[]>(x =>
                x[0] == value[0] &&
                x[1] == value[1]));
        }

        private async Task ValueShouldBe(int expected)
        {
            var actual = await _dWord.GetValueAsync();

            actual.Should().Be(expected);
        }

        private void WhenSetValue(int value)
        {
            _dWord.SetValueAsync(value);
        }
    }
}