using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace MPLC.UnitTests
{
    [TestFixture]
    public class DWordTests
    {
        private IMPLCProvider _mplc;
        private DWord _dWord;
        private const string StartAddress = "D100";

        [SetUp]
        public void SetUp()
        {
            _mplc = Substitute.For<IMPLCProvider>();
            _dWord = new DWord(_mplc, StartAddress);
        }

        [Test]
        public void set_value()
        {
            _dWord.SetValue(65536);

            _mplc.Received().WriteWords(StartAddress, Arg.Is<int[]>(x =>
                x[0] == 0 &&
                x[1] == 1));
        }

        [Test]
        public void get_value()
        {
            _mplc.ReadWords(StartAddress, 2).Returns(new[] { 0, 1 });

            var actual = _dWord.GetValue();

            actual.Should().Be(65536);
        }
    }
}