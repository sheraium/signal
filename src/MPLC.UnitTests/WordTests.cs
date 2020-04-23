using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace MPLC.UnitTests
{
    [TestFixture]
    public class WordTests
    {
        private const int Value = 1;
        private const string WordAddress = "D100";
        private IMPLCProvider _mplc;
        private Word _word;

        [Test]
        public async Task get_bit()
        {
            _mplc.ReadWordAsync(WordAddress).Returns(Value);

            var index = 0;
            var actual = await _word.GetBitAsync(index);

            actual.Should().BeTrue();
        }

        [Test]
        public async Task get_value()
        {
            _mplc.ReadWordAsync(WordAddress).Returns(Value);

            var actual = await _word.GetValueAsync();

            actual.Should().Be(Value);
        }

        [Test]
        public async Task set_bit()
        {
            _mplc.ReadWordAsync(WordAddress).Returns(1);

            var index = 1;
            await _word.SetBitAsync(index, true);

            await _mplc.Received().WriteWordAsync(Arg.Is(WordAddress), Arg.Is(3));
        }

        [Test]
        public void set_value()
        {
            _word.SetValueAsync(Value);

            _mplc.Received().WriteWordAsync(WordAddress, Value);
        }

        [SetUp]
        public void SetUp()
        {
            _mplc = Substitute.For<IMPLCProvider>();
            _word = new Word(_mplc, "D100");
        }
    }
}