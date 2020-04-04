using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace MPLC
{
    public class DataProvider : IMPLCProvider
    {
        protected readonly ConcurrentDictionary<string, IDataBlock> Blocks = new ConcurrentDictionary<string, IDataBlock>();

        public virtual void AddBlock(IDataBlock block)
        {
            Blocks.TryAdd(block.Name, block);
        }

        public virtual bool GetBit(string address)
        {
            foreach (var item in Blocks)
            {
                if (item.Value.TryGetBit(address, out var value))
                {
                    return value;
                }
            }
            return false;
        }

        public virtual async Task<bool> GetBitAsync(string address)
        {
            foreach (var item in Blocks)
            {
                var (successed, value) = await item.Value.TryGetBitAsync(address);
                if (successed)
                {
                    return value;
                }
            }
            return false;
        }

        public virtual int ReadWord(string address)
        {
            foreach (var item in Blocks)
            {
                if (item.Value.TryGetWord(address, out var value))
                {
                    return value;
                }
            }
            throw new InvalidOperationException("no match block");
        }

        public virtual async Task<int> ReadWordAsync(string address)
        {
            foreach (var item in Blocks)
            {
                var (successed, value) = await item.Value.TryGetWordAsync(address);
                if (successed)
                {
                    return value;
                }
            }
            throw new InvalidOperationException("no match block");
        }

        public virtual int[] ReadWords(string startAddress, int length)
        {
            foreach (var item in Blocks)
            {
                if (item.Value.TryGetWords(startAddress, length, out var value))
                {
                    return value;
                }
            }
            throw new InvalidOperationException("no match block");
        }

        public virtual async Task<int[]> ReadWordsAsync(string startAddress, int length)
        {
            foreach (var item in Blocks)
            {
                var (successed, value) = await item.Value.TryGetWordsAsync(startAddress, length);
                if (successed)
                {
                    return value;
                }
            }
            throw new InvalidOperationException("no match block");
        }

        public virtual void SetBitOff(string address)
        {
            if (!Blocks.Any(item => item.Value.TrySetBitOff(address)))
            {
                throw new InvalidOperationException("no match block");
            }
        }

        public virtual async Task SetBitOffAsync(string address)
        {
            foreach (var item in Blocks)
            {
                if (await item.Value.TrySetBitOffAsync(address))
                {
                    return;
                }
            }
            throw new InvalidOperationException("no match block");
        }

        public virtual void SetBitOn(string address)
        {
            if (!Blocks.Any(item => item.Value.TrySetBitOn(address)))
            {
                throw new InvalidOperationException("no match block");
            }
        }

        public virtual async Task SetBitOnAsync(string address)
        {
            foreach (var item in Blocks)
            {
                if (await item.Value.TrySetBitOnAsync(address))
                {
                    return;
                }
            }
            throw new InvalidOperationException("no match block");
        }

        public virtual void WriteWord(string address, int value)
        {
            if (!Blocks.Any(item => item.Value.TrySetWord(address, value)))
            {
                throw new InvalidOperationException("no match block");
            }
        }

        public virtual async Task WriteWordAsync(string address, int value)
        {
            foreach (var item in Blocks)
            {
                if (await item.Value.TrySetWordAsync(address, value))
                {
                    return;
                }
            }
            throw new InvalidOperationException("no match block");
        }

        public virtual void WriteWords(string startAddress, int[] words)
        {
            if (!Blocks.Any(item => item.Value.TrySetWords(startAddress, words)))
            {
                throw new InvalidOperationException("no match block");
            }
        }

        public virtual async Task WriteWordsAsync(string startAddress, int[] words)
        {
            foreach (var item in Blocks)
            {
                if (await item.Value.TrySetWordsAsync(startAddress, words))
                {
                    return;
                }
            }
            throw new InvalidOperationException("no match block");
        }
    }
}