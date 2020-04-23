using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace MPLC.UnitTests
{
    [TestFixture]
    public class DataProviderTests
    {
        private DataProvider _mplc;
        private IDataBlock _blockB;
        private IDataBlock _blockD;

        [SetUp]
        public void SetUp()
        {
            _mplc = new DataProvider();
            _blockB = Substitute.For<IDataBlock>();
            _blockB.Name.Returns("BRange");
            _mplc.AddBlock(_blockB);
            _blockD = Substitute.For<IDataBlock>();
            _blockD.Name.Returns("DRange");
            _mplc.AddBlock(_blockD);
        }

        [Test]
        public async Task get_bit()
        {
            _blockD.TryGetBitAsync("D10.1")
                .Returns((true, true));

            var actual = await _mplc.GetBitAsync("D10.1");

            actual.Should().BeTrue();
        }

        [Test]
        public async Task set_bit_on()
        {
            _blockB.TrySetBitOnAsync("B0")
                .Returns(true);

            await _mplc.SetBitOnAsync("B0");

            await _blockB.Received().TrySetBitOnAsync(Arg.Is("B0"));
        }

        [Test]
        public void set_bit_on_fail()
        {
            _blockB.TrySetBitOnAsync("B0")
                .Returns(true);

            Func<Task> act = async () => await _mplc.SetBitOnAsync("B1");

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        [Test]
        public async Task set_bit_off()
        {
            _blockB.TrySetBitOffAsync("B0")
                .Returns(true);

            await _mplc.SetBitOffAsync("B0");

            await _blockB.Received().TrySetBitOffAsync(Arg.Is("B0"));
        }

        [Test]
        public void set_bit_off_fail()
        {
            _blockB.TrySetBitOffAsync("B0")
                .Returns(true);

            Func<Task> act = async () => await _mplc.SetBitOffAsync("B1");

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        [Test]
        public async Task read_word()
        {
            _blockD.TryGetWordAsync("D100")
                .Returns((true, 10));

            var actual = await _mplc.ReadWordAsync("D100");

            actual.Should().Be(10);
        }

        [Test]
        public void read_word_fail()
        {
            _blockD.TryGetWordAsync("D101")
                .Returns((false, 10));

            Func<Task> act = async () => await _mplc.ReadWordAsync("D101");

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        [Test]
        public async Task write_word()
        {
            _blockD.TrySetWordAsync("D100", Arg.Any<int>())
                .Returns(true);

            await _mplc.WriteWordAsync("D100", 10);
        }

        [Test]
        public void write_word_fail()
        {
            _blockD.TrySetWordAsync("D100", Arg.Any<int>())
                .Returns(true);

            Func<Task> act = async () => await _mplc.WriteWordAsync("D101", 10);

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        [Test]
        public async Task reads_word_async()
        {
            _blockD.TryGetWordsAsync("D100", 2)
                .Returns((true, new[] { 10, 11 }));

            var actual = await _mplc.ReadWordsAsync("D100", 2);

            actual.Should().BeEquivalentTo(new[] { 10, 11 });
        }

        [Test]
        public void reads_word_fail()
        {
            _blockD.TryGetWordsAsync("D100", 2)
                .Returns((false, new[] { 10, 11 }));

            Func<Task> act = async () => await _mplc.ReadWordAsync("D101");

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        [Test]
        public async Task write_words()
        {
            _blockD.TrySetWordsAsync("D100", Arg.Any<int[]>())
                .Returns(true);

            await _mplc.WriteWordsAsync("D100", new[] { 10, 11 });
        }

        [Test]
        public void write_words_fail()
        {
            _blockD.TrySetWordsAsync("D100", Arg.Any<int[]>())
                .Returns(true);

            Func<Task> act = async () => await _mplc.WriteWordsAsync("D101", new[] { 10, 11 });

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }
    }
}