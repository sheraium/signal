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
        private const bool On = true;
        private const bool Off = false;
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
        public async Task blockB_get_bit()
        {
            GivenBlockB("B0", On);
            await BitShouldBe("B0", On);
        }

        [Test]
        public async Task blockB_set_bit_off()
        {
            await WhenBlockBSetBit("B0", Off);
            await BlockShouldSetBit("B0", Off);
        }

        [Test]
        public void blockB_set_bit_off_fail()
        {
            SetBitShouldThrowException("B0", Off);
        }

        [Test]
        public async Task blockB_set_bit_on()
        {
            await WhenBlockBSetBit("B0", On);
            await BlockShouldSetBit("B0", On);
        }

        [Test]
        public void blockB_set_bit_on_fail()
        {
            SetBitShouldThrowException("B0", On);
        }

        [Test]
        public async Task blockD_get_bit()
        {
            GivenBlockDBit("D10.1", On);
            await BitShouldBe("D10.1", On);
        }

        [Test]
        public async Task blockD_read_word()
        {
            GivenBlockDWord("D100", 10);
            await WordShouldBe("D100", 10);
        }

        [Test]
        public async Task blockD_read_words()
        {
            GivenBlockDWords("D100", 2, new[] { 10, 11 });
            await WordsShouldBe("D100", 2, new[] { 10, 11 });
        }

        [Test]
        public async Task blockD_write_word()
        {
            GivenSetWordSuccess("D100");
            await WhenMPLCWriteWord("D100", 10);
            await BlockDShouldBeSetWord("D100", 10);
        }

        [Test]
        public void blockD_write_word_fail()
        {
            GivenSetWordSuccess("D100");
            MPLCWriteWordShouldThrowException("D101", 10);
        }

        [Test]
        public async Task blockD_write_words()
        {
            GivenSetWordsSuccess("D100");
            await WhenMPLCWriteWords("D100", new[] { 10, 11 });
            await BlockDShouldBeSetWords("D100", new[] { 10, 11 });
        }

        [Test]
        public void blockD_write_words_fail()
        {
            GivenSetWordsSuccess("D100");
            MPLCWriteWordsShouldThrowException("D101", new[] { 10, 11 });
        }

        private async Task BitShouldBe(string address, bool expected)
        {
            var actual = await _mplc.GetBitAsync(address);
            actual.Should().Be(expected);
        }

        private async Task BlockDShouldBeSetWord(string address, int value)
        {
            await _blockD.Received(1).TrySetWordAsync(Arg.Is(address), Arg.Is(value));
        }

        private async Task BlockDShouldBeSetWords(string address, int[] data)
        {
            await _blockD.Received(1).TrySetWordsAsync(
                Arg.Is(address),
                Arg.Is<int[]>(x => x[0] == data[0] && x[1] == data[1]));
        }

        private async Task BlockShouldSetBit(string address, bool value)
        {
            await _blockB.Received(1).TrySetBitAsync(Arg.Is(address), Arg.Is(value));
        }

        private void GivenBlockB(string address, bool value)
        {
            _blockB.TryGetBitAsync(address)
                .Returns((true, value));
        }

        private void GivenBlockDBit(string address, bool value)
        {
            _blockD.TryGetBitAsync(address)
                .Returns((true, value));
        }

        private void GivenBlockDWord(string address, int value)
        {
            _blockD.TryGetWordAsync(address)
                .Returns((true, value));
        }

        private void GivenBlockDWords(string startAddress, int length, int[] data)
        {
            _blockD.TryGetWordsAsync(startAddress, length)
                .Returns((true, data));
        }

        private void GivenSetWordsSuccess(string startAddress)
        {
            _blockD.TrySetWordsAsync(startAddress, Arg.Any<int[]>())
                .Returns(true);
        }

        private void GivenSetWordSuccess(string address)
        {
            _blockD.TrySetWordAsync(address, Arg.Any<int>())
                .Returns(true);
        }

        private void MPLCWriteWordShouldThrowException(string address, int value)
        {
            Func<Task> act = async () => await _mplc.WriteWordAsync(address, value);

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        private void MPLCWriteWordsShouldThrowException(string startAddress, int[] data)
        {
            Func<Task> act = async () => await _mplc.WriteWordsAsync(startAddress, data);

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        private void SetBitShouldThrowException(string address, bool isOn)
        {
            Func<Task> act = async () => await _mplc.SetBitAsync(address, isOn);

            act.Should().Throw<InvalidOperationException>()
                .Where(e => e.Message.Contains("no match block"));
        }

        private async Task WhenBlockBSetBit(string address, bool isOn)
        {
            _blockB.TrySetBitAsync(address, isOn)
                .Returns(true);

            await _mplc.SetBitAsync(address, isOn);
        }

        private async Task WhenMPLCWriteWord(string address, int value)
        {
            await _mplc.WriteWordAsync(address, value);
        }

        private async Task WhenMPLCWriteWords(string startAddress, int[] data)
        {
            await _mplc.WriteWordsAsync(startAddress, data);
        }

        private async Task WordShouldBe(string address, int expected)
        {
            var actual = await _mplc.ReadWordAsync(address);

            actual.Should().Be(expected);
        }

        private async Task WordsShouldBe(string startAddress, int length, int[] expected)
        {
            var actual = await _mplc.ReadWordsAsync(startAddress, length);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}