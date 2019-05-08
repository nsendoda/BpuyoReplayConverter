using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpuyoReplayConverter
{
    // 解凍されたフレームのリプレイブロックから必要なデータを解読し、格納するクラス
    class FrameStream
    {
        public int match_count;
        public int hand_count;
        public int[] field;
        public int[] now_puyos;
        public int[] next_puyos;
        public int x;
        public int rotate;
        public Mode mode;

        public FrameStream(in byte[] decompressed_block_data, int frame_i, int player_i)
        {
            // 情報が始まる位置を求める
            int base_index = frame_i * 2538 + player_i * 1269;

            // Debug(decompressed_block_data, base_index);

            int match_index = base_index + 1225;
            int hand_index = base_index + 1157;
            int field_index = base_index + 20;
            int current_parent_index = base_index + 1113;
            int current_child_index = base_index + 1114;
            int next_parent_index = base_index + 1115;
            int next_child_index = base_index + 1116;
            int x_index = base_index + 1123;
            int rotate_index = base_index + 1126;

            // 解凍されたブロックから読み取り
            match_count = BitConverter.ToInt16(decompressed_block_data, match_index);
            hand_count = decompressed_block_data[hand_index];
            field = ReadField(decompressed_block_data, field_index);
            now_puyos = new int[BpuyoParameter.KumipuyoSize] { decompressed_block_data[current_parent_index], decompressed_block_data[current_child_index] };
            next_puyos = new int[BpuyoParameter.KumipuyoSize] { decompressed_block_data[next_parent_index], decompressed_block_data[next_child_index] };
            x = decompressed_block_data[x_index];
            rotate = decompressed_block_data[rotate_index];
            mode = (Mode)decompressed_block_data[base_index];
        }

        // 無効なデータが含まれたフレームの時、trueを返す
        public bool Invalid()
        {
            if (hand_count == 0 || now_puyos[0] == 0 || now_puyos[1] == 0) return true;
            return false;
        }

        private int[] ReadField(in byte[] decompressed_block_data, int field_index)
        {
            int[] field = new int[BpuyoParameter.FieldSize];
            for (int i = 0; i < BpuyoParameter.FieldSize; i++)
            {
                field[i] = decompressed_block_data[field_index + i];
            }
            return field;
        }

        // 全データ標準出力用
        private void Debug(in byte[] decompressed_block_data, int base_index)
        {
            for (int i = 0; i < 1200; i++)
            {
                System.Diagnostics.Debug.Write(decompressed_block_data[base_index + i].ToString());
                if (i % 6 == 5)
                    System.Diagnostics.Debug.Write("\r\n");
                else
                    System.Diagnostics.Debug.Write(' ');
            }
        }
    }
}
