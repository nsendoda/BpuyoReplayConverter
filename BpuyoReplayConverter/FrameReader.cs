using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpuyoReplayConverter
{
    class FrameStream
    {
        public int match_count;
        public int hand_count;
        public int[] current_puyos;
        public int[] next_puyos;
        public int x;
        public int rotate;
        public Mode mode;

        public FrameStream(in byte[] decompressed_block_data, int frame_i, int player_i)
        {
            int base_index = frame_i * 2538 + player_i * 1269;

            int match_index = base_index + 1225;
            int hand_index = base_index + 1157;
            int current_parent_index = base_index + 1113;
            int current_child_index = base_index + 1114;
            int next_parent_index = base_index + 1115;
            int next_child_index = base_index + 1116;
            int x_index = base_index + 1123;
            int rotate_index = base_index + 1126;

            match_count = BitConverter.ToInt16(decompressed_block_data, match_index);
            hand_count = decompressed_block_data[hand_index];
            current_puyos = new int[2]{ decompressed_block_data[current_parent_index], decompressed_block_data[current_child_index]};
            next_puyos = new int[2]{decompressed_block_data[next_parent_index], decompressed_block_data[next_child_index]};
            x = decompressed_block_data[x_index];
            rotate = decompressed_block_data[rotate_index];
            mode = (Mode)decompressed_block_data[base_index];

        }

        public bool Invalid()
        {
            if (hand_count == 0 || current_puyos[0] == 0 || current_puyos[1] == 0) return true;
            return false;
        }

    }
}
