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
            _mplc.ReadWordsAsync(StartAddress, 2).Returns(new[] { 1, 2 });

            var actual = await _words.GetDataAsync();

            actual.Should().BeEquivalentTo(new[] { 1, 2 });
        }

        [Test]
        public void set_data()
        {
            _words.SetDataAsync(new[] { 1, 2 });

            _mplc.Received().WriteWordsAsync(StartAddress, Arg.Is<int[]>(x =>
                x[0] == 1 &&
                x[1] == 2));
        }
    }
}