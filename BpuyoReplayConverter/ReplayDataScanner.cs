using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpuyoReplayConverter
{
    public class ReplayDataScanner
    {
        enum Mode : int { CHAIN = 4, PUT = 5, WAIT };

        private String puyofu_text;
        private String first_player_name;
        private String second_player_name;
        private PlayerState target_player; // 0なら1P, 1なら2P

        private void Init()
        {
            puyofu_text = String.Empty;
            first_player_name = String.Empty;
            second_player_name = String.Empty;
            target_player = new PlayerState();
        }

        // ファイルのヘッダを読んで飛ばす
        // @note プレイヤー名をフィールドにセットしている
        private void ReadAndSkipPrdFileHeader(System.IO.FileStream replay_stream)
        {
            const int kFileHeaderSize = 112;
            const int kFirstSkipSize = 16;
            const int kResultSize = 2; // read twice(1P, 2P)
            const int kSecondSkipSize = 60;
            const int kPlayerNameSize = 16; // read twice(1P, 2P)

            byte[] skip_buffer = new byte[kFileHeaderSize];
            byte[] result_buffer = new byte[kResultSize];
            byte[] name_buffer = new byte[kPlayerNameSize];

            replay_stream.Read(skip_buffer, 0, kFirstSkipSize);
            replay_stream.Read(result_buffer, 0, kResultSize);
            puyofu_text += BitConverter.ToInt16(result_buffer, 0).ToString() + '\n';
            replay_stream.Read(result_buffer, 0, kResultSize);
            puyofu_text += BitConverter.ToInt16(result_buffer, 0).ToString() + '\n';
            replay_stream.Read(skip_buffer, 0, kSecondSkipSize);
            replay_stream.Read(name_buffer, 0, kPlayerNameSize);
            first_player_name = System.Text.Encoding.UTF8.GetString(name_buffer);
            first_player_name = first_player_name.TrimEnd('\0');
            puyofu_text += first_player_name + '\n';
            int second_size = replay_stream.Read(name_buffer, 0, kPlayerNameSize);
            second_player_name = System.Text.Encoding.UTF8.GetString(name_buffer);
            second_player_name = second_player_name.TrimEnd('\0');
            puyofu_text += second_player_name + '\n';

        }

        // 120フレームブロックのヘッダを読んで飛ばす
        private void ReadAndSkipPrd120BlockHeader(System.IO.FileStream replay_stream, ReplayBlockData replay_block)
        {
            byte[] invalid_memory = new byte[1];
            byte[] block_size = new byte[4];
            byte[] meta_data = new byte[2];// 78DA

            replay_stream.Read(invalid_memory, 0, 1);
            replay_stream.Read(block_size, 0, 4);
            replay_stream.Read(meta_data, 0, 2);

            replay_block.invalid_memory = BitConverter.ToBoolean(invalid_memory, 0);
            replay_block.Resize(BitConverter.ToUInt32(block_size, 0));

            replay_stream.Read(replay_block.data, 0, (int)replay_block.size - 2);
        }

        // リプレイブロックを解凍したものを返す
        private byte[] DecompressReplayBlock(ReplayBlockData replay_block)
        {
            const int kOneFrameBytePerPlayer = 1269;
            const int kUncompressedDataSize = kOneFrameBytePerPlayer * BpuyoParameter.BlockFrameSize * BpuyoParameter.PlayerNumber;
            byte[] uncompressed_data = new byte[kUncompressedDataSize];
            //  プレイヤー１人分のデータが1フレームにつき1269バイト、それが1P,2Pずつ交互に、120フレーム分並んで
            System.IO.MemoryStream memory_stream = new System.IO.MemoryStream(replay_block.data);
            System.IO.Compression.DeflateStream deflate_stream = new System.IO.Compression.DeflateStream(memory_stream, System.IO.Compression.CompressionMode.Decompress);
            deflate_stream.Read(uncompressed_data, 0, kUncompressedDataSize);
            memory_stream.Close();
            deflate_stream.Close();
            return uncompressed_data;
        }

        // 一手ごとに記録
        private void ConvertDecompressedBlockDataToRecord(byte[] decompressed_block_data, RecordGame record_game)
        {
            for (int flame_i = 0; flame_i < BpuyoParameter.BlockFrameSize; flame_i++)
            {
                int player_i;
                if (target_player.IsFirstPlayer()) player_i = 0;
                else player_i = 1;

                int base_index = flame_i * 2538 + player_i * 1269;

                int match_index = base_index + 1225;
                int hand_index = base_index + 1157;
                int current_parent_index = base_index + 1113;
                int current_child_index = base_index + 1114;
                int next_parent_index = base_index + 1115;
                int next_child_index = base_index + 1116;
                int x_index = base_index + 1123;
                int rotate_index = base_index + 1126;

                int match_count = BitConverter.ToInt16(decompressed_block_data, match_index);
                int hand_count = decompressed_block_data[hand_index];
                if(record_game.match.Count < match_count)
                {
                    record_game.match.Add(new List<RecordOnePut>());
                    record_game.match.Last().Add(new RecordOnePut());
                }
                if(record_game.match.Last().Count < hand_count)
                {
                    record_game.match.Last().Add(new RecordOnePut());
                }
                RecordOnePut record = record_game.match.Last().Last();
                Mode mode_player = (Mode)decompressed_block_data[base_index];

                if(mode_player == Mode.CHAIN){

                }else if (mode_player == Mode.PUT)
                {
                    if (record.IsSetRecord()) continue;
                    record.hand_count = hand_count;
                    record.current_kumipuyo[0] = decompressed_block_data[current_parent_index];
                    record.current_kumipuyo[1] = decompressed_block_data[current_child_index];
                    record.next_kumipuyo[0] = decompressed_block_data[next_parent_index];
                    record.next_kumipuyo[1] = decompressed_block_data[next_child_index];
                    record.put_pattern.Set(decompressed_block_data[x_index], decompressed_block_data[rotate_index]);
                }

            }
        }

        private void ReadAndSkipPrdContents(System.IO.FileStream replay_stream, RecordGame record_game)
        {
            ReplayBlockData replay_block = new ReplayBlockData();
            while(replay_stream.Length - replay_stream.Position > 0)
            {
                ReadAndSkipPrd120BlockHeader(replay_stream, replay_block);

                if (replay_block.invalid_memory == true) continue;

                byte[] unzip = DecompressReplayBlock(replay_block);

                ConvertDecompressedBlockDataToRecord(unzip, record_game);
            }

            for(int match_i = 0; match_i < record_game.match.Count; match_i++)
            {
                puyofu_text += "match: " + match_i.ToString() + "\n";
                //                for(int hand_i = 0; hand_i < record_game.match[match_i].Count; hand_i++)
                foreach(RecordOnePut rec in record_game.match[match_i])
                {
                    puyofu_text += rec.ToString();
                }
            }

        }

        // 調査対象のプレイヤーのぷよ譜を返す
        // @note 調査対象のプレイヤーが対戦に存在しない場合は空文字列を返す
        public String ReadAndTraceData(String replay_file_path, String input_player_name)
        {
            Init(); // 返す文字列を初期化, 調査するプレイヤー名のset
            // 最低限の試合数と手数
            // @suspicion vectorみたいに増やせるの？
            RecordGame record_game = new RecordGame();

            using (
                System.IO.FileStream replay_stream = new System.IO.FileStream(replay_file_path, System.IO.FileMode.Open,
                    System.IO.FileAccess.Read))
            {
                ReadAndSkipPrdFileHeader(replay_stream);
                if (first_player_name == input_player_name)
                {
                    target_player.SetFirstPlayer();
                }else if (second_player_name == input_player_name)
                {
                    target_player.SetSecondPlayer();
                }
                else
                {
                    return String.Empty;
                }
                ReadAndSkipPrdContents(replay_stream, record_game);
            }
            return puyofu_text;
        }
    }
}
