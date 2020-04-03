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
        public void set_data()
        {
            _words.SetData(new[] { 1, 2 });

            _mplc.Received().WriteWords(StartAddress, Arg.Is<int[]>(x =>
                x[0] == 1 &&
                x[1] == 2));
        }

        [Test]
        public void get_data()
        {
            _mplc.ReadWords(StartAddress, 2).Returns(new[] { 1, 2 });

            var actual = _words.GetData();

            actual.Should().BeEquivalentTo(new[] { 1, 2 });
        }
    }
}