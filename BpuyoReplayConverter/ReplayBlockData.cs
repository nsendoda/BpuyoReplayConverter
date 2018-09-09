using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpuyoReplayConverter
{
    class ReplayBlockData
    {
        public byte[] data;
        public bool invalid_memory;
        public uint size;

        public ReplayBlockData() { }

        public void Resize(uint block_size)
        {
            size = block_size;
            data = new byte[size];
        }
    }
}
