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

        [Test]
        public async Task get_value()
        {
            _mplc.ReadWordsAsync(StartAddress, 2).Returns(new[] { 0, 1 });

            var actual = await _dWord.GetValueAsync();

            actual.Should().Be(65536);
        }

        [Test]
        public void set_value()
        {
            _dWord.SetValueAsync(65536);

            _mplc.Received().WriteWordsAsync(StartAddress, Arg.Is<int[]>(x =>
                x[0] == 0 &&
                x[1] == 1));
        }

        [SetUp]
        public void SetUp()
        {
            _mplc = Substitute.For<IMPLCProvider>();
            _dWord = new DWord(_mplc, StartAddress);
        }
    }
}