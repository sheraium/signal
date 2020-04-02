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

        [SetUp]
        public void SetUp()
        {
            _mplc = Substitute.For<IMPLCProvider>();
            _word = new Word(_mplc, "D100");
        }

        [Test]
        public void set_value()
        {
            _word.SetValue(Value);

            _mplc.Received().WriteWord(WordAddress, Value);
        }


        [Test]
        public void get_value()
        {
            _mplc.ReadWord(WordAddress).Returns(Value);

            var actual = _word.GetValue();

            actual.Should().Be(Value);
        }

        [Test]
        public void value()
        {
            _mplc.ReadWord(WordAddress).Returns(Value);

            var actual = _word.Value;

            actual.Should().Be(Value);
        }

        [Test]
        public void address()
        {
            var actual = _word.Address;

            actual.Should().Be(WordAddress);
        }

        [Test]
        public void get_bit()
        {
            _mplc.ReadWord(WordAddress).Returns(Value);

            var index = 0;
            var actual = _word.GetBit(index);

            actual.Should().BeTrue();
        }

        [Test]
        public void set_bit()
        {
            _mplc.ReadWord(WordAddress).Returns(1);

            var index = 1;
            _word.SetBit(index, true);

            _mplc.Received().WriteWord(Arg.Is(WordAddress), Arg.Is(3));
        }
    }
}