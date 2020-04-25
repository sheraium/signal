using System;
using System.Collections.Concurrent;
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

        public virtual async Task<bool> GetBitAsync(string address)
        {
            foreach (var item in Blocks)
            {
                var (succeeded, value) = await item.Value.TryGetBitAsync(address);
                if (succeeded)
                {
                    return value;
                }
            }

            return false;
        }

        public virtual async Task<int> ReadWordAsync(string address)
        {
            foreach (var item in Blocks)
            {
                var (succeeded, value) = await item.Value.TryGetWordAsync(address);
                if (succeeded)
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
                var (succeeded, value) = await item.Value.TryGetWordsAsync(startAddress, length);
                if (succeeded)
                {
                    return value;
                }
            }

            throw new InvalidOperationException("no match block");
        }

        public virtual async Task SetBitAsync(string address, bool isOn)
        {
            foreach (var item in Blocks)
            {
                if (await item.Value.TrySetBitAsync(address, isOn))
                {
                    return;
                }
            }

            throw new InvalidOperationException("no match block");
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