using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
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
        public void get_bit()
        {
            _blockB.TryGetBit("B0", out Arg.Any<bool>())
                .Returns(param =>
                {
                    param[1] = true;
                    return true;
                });

            var actual = _mplc.GetBit("B0");

            actual.Should().BeTrue();
        }

        [Test]
        public async Task get_bit_async()
        {
            _blockD.TryGetBitAsync("D10.1")
                .Returns((true, true));

            var actual = await _mplc.GetBitAsync("D10.1");

            actual.Should().BeTrue();
        }

        [Test]
        public void set_bit_on()
        {
            _blockB.TrySetBitOn("B0")
                .Returns(true);

            _mplc.SetBitOn("B0");

            _blockB.Received().TrySetBitOn(Arg.Is("B0"));
        }

        [Test]
        public async Task set_bit_on_async()
        {
            _blockB.TrySetBitOnAsync("B0")
                .Returns(true);

            await _mplc.SetBitOnAsync("B0");

            await _blockB.Received().TrySetBitOnAsync(Arg.Is("B0"));
        }

        [Test]
        public void set_bit_on_fail()
        {
            _blockB.TrySetBitOn("B0")
                .Returns(true);

            Action act = () => _mplc.SetBitOn("B1");

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        [Test]
        public void set_bit_off()
        {
            _blockB.TrySetBitOff("B0")
                .Returns(true);

            _mplc.SetBitOff("B0");

            _blockB.Received().TrySetBitOff(Arg.Is("B0"));
        }

        [Test]
        public async Task set_bit_off_async()
        {
            _blockB.TrySetBitOffAsync("B0")
                .Returns(true);

            await _mplc.SetBitOffAsync("B0");

            await _blockB.Received().TrySetBitOffAsync(Arg.Is("B0"));
        }

        [Test]
        public void set_bit_off_fail()
        {
            _blockB.TrySetBitOff("B0")
                .Returns(true);

            Action act = () => _mplc.SetBitOff("B1");

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        [Test]
        public void read_word()
        {
            _blockD.TryGetWord("D100", out Arg.Any<int>())
                .Returns(param =>
                {
                    param[1] = 10;
                    return true;
                });

            var actual = _mplc.ReadWord("D100");

            actual.Should().Be(10);
        }

        [Test]
        public async Task read_word_async()
        {
            _blockD.TryGetWordAsync("D100")
                .Returns((true, 10));

            var actual = await _mplc.ReadWordAsync("D100");

            actual.Should().Be(10);
        }

        [Test]
        public void read_word_fail()
        {
            _blockD.TryGetWord("D100", out Arg.Any<int>())
                .Returns(param =>
                {
                    param[1] = 10;
                    return true;
                });

            Action act = () => _mplc.ReadWord("D101");

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        [Test]
        public void write_word()
        {
            _blockD.TrySetWord("D100", Arg.Any<int>())
                .Returns(true);

            _mplc.WriteWord("D100", 10);
        }

        [Test]
        public async Task write_word_async()
        {
            _blockD.TrySetWordAsync("D100", Arg.Any<int>())
                .Returns(true);

            await _mplc.WriteWordAsync("D100", 10);
        }

        [Test]
        public void write_word_fail()
        {
            _blockD.TrySetWord("D100", Arg.Any<int>())
                .Returns(true);

            Action act = () => _mplc.WriteWord("D101", 10);

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        [Test]
        public void reads_word()
        {
            _blockD.TryGetWords("D100", 2, out Arg.Any<int[]>())
                .Returns(param =>
                {
                    param[2] = new[] { 10, 11 };
                    return true;
                });

            var actual = _mplc.ReadWords("D100", 2);

            actual.Should().BeEquivalentTo(new[] { 10, 11 });
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
            _blockD.TryGetWords("D100", 2, out Arg.Any<int[]>())
                .Returns(param =>
                {
                    param[2] = new[] { 10, 11 };
                    return true;
                });

            Action act = () => _mplc.ReadWord("D101");

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        [Test]
        public void writes_word()
        {
            _blockD.TrySetWords("D100", Arg.Any<int[]>())
                .Returns(true);

            _mplc.WriteWords("D100", new[] { 10, 11 });
        }

        [Test]
        public async Task write_words_async()
        {
            _blockD.TrySetWordsAsync("D100", Arg.Any<int[]>())
                .Returns(true);

            await _mplc.WriteWordsAsync("D100", new[] { 10, 11 });
        }

        [Test]
        public void write_words_fail()
        {
            _blockD.TrySetWords("D100", Arg.Any<int[]>())
                .Returns(true);

            Action act = () => _mplc.WriteWords("D101", new[] { 10, 11 });

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }
    }
}