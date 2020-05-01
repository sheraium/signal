using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace MPLC.UnitTests
{
    [TestFixture]
    public class WordsTests
    {
        private const string StartAddress = "D100";
        private IMPLCProvider _mplc;
        private Words _words;

        [SetUp]
        public void SetUp()
        {
            _mplc = Substitute.For<IMPLCProvider>();
            _words = new Words(_mplc, StartAddress, 2);
        }

        [Test]
        public async Task get_data()
        {
            GivenData(new[] { 1, 2 });
            await DataShouldBe(new[] { 1, 2 });
        }

        [Test]
        public async Task set_data()
        {
            WhenSetData(new[] { 1, 2 });
            await MPLCShouldWriteWords(new[] { 1, 2 });
        }

        private async Task DataShouldBe(int[] expected)
        {
            var actual = await _words.GetDataAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        private void GivenData(int[] data)
        {
            _mplc.ReadWordsAsync(StartAddress, 2).Returns(data);
        }

        private async Task MPLCShouldWriteWords(int[] data)
        {
            await _mplc.Received().WriteWordsAsync(StartAddress, Arg.Is<int[]>(x =>
                x[0] == data[0] &&
                x[1] == data[1]));
        }

        private void WhenSetData(int[] data)
        {
            _words.SetDataAsync(data);
        }
    }
}