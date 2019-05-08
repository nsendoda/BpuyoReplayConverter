using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpuyoReplayConverter
{
    // 渡されたリプレイファイルパスとプレイヤー名から
    // 各試合の各手数におけるデータを解析し、文字列として返す
    class ReplayDataScanner
    {

        private String puyofu_text;
        private String first_player_name;
        private String second_player_name;
        private PlayerState target_player; // 0なら1P, 1なら2P


        // 調査対象のプレイヤーのぷよ譜を返す
        // @note 調査対象のプレイヤーが対戦に存在しない場合は空文字列を返す
        public bool ReadAndTraceData(String replay_file_path, String input_player_name, RecordGame record_game)
        {
            Init(); // 返す文字列を初期化, 調査するプレイヤー名のset

            using (
                System.IO.FileStream replay_stream = new System.IO.FileStream(replay_file_path, System.IO.FileMode.Open,
                    System.IO.FileAccess.Read))
            {
                // 記録を行うプレイヤーが1Pか2Pかに存在するか調べる
                // 存在しなければ空文字列を返す
                ReadAndSkipPrdFileHeader(replay_stream);
                if (first_player_name == input_player_name)
                {
                    target_player.SetFirstPlayer();
                }
                else if (second_player_name == input_player_name)
                {
                    target_player.SetSecondPlayer();
                }
                else
                {
                    return false;
                }
                ReadAndSkipPrdContents(replay_stream, record_game);
            }
            return true;
        }

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
            puyofu_text += BitConverter.ToInt16(result_buffer, 0).ToString() + "\r\n";
            replay_stream.Read(result_buffer, 0, kResultSize);
            puyofu_text += BitConverter.ToInt16(result_buffer, 0).ToString() + "\r\n";
            replay_stream.Read(skip_buffer, 0, kSecondSkipSize);

            // 名前の読み取り
            replay_stream.Read(name_buffer, 0, kPlayerNameSize);
            first_player_name = System.Text.Encoding.UTF8.GetString(name_buffer);
            replay_stream.Read(name_buffer, 0, kPlayerNameSize);
            second_player_name = System.Text.Encoding.UTF8.GetString(name_buffer);

            //  制御文字を取り除く
            first_player_name = TrimControl(first_player_name);
            second_player_name = TrimControl(second_player_name);
        }

        private String TrimControl(String s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsControl(s[i]))
                {
                    return s.Substring(0, i);
                }
            }
            return s;
        }

        // ヘッダ以降のデータを読んでその内容をrecord_gameに書き込む
        private void ReadAndSkipPrdContents(System.IO.FileStream replay_stream, RecordGame record_game)
        {
            ReplayBlockData replay_block = new ReplayBlockData();
            while (replay_stream.Length - replay_stream.Position > 0)
            {
                ReadAndSkipPrd120BlockHeader(replay_stream, replay_block);

                if (replay_block.invalid_memory == true) continue;

                byte[] unzip = DecompressReplayBlock(replay_block);

                ConvertDecompressedBlockDataToRecord(unzip, record_game);
            }
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

        // decompressed_block_dataに入ってる各試合の一手ごとに記録を行う
        // @args record_game これに各試合ごとにおける各手数での情報を格納する
        private void ConvertDecompressedBlockDataToRecord(in byte[] decompressed_block_data, RecordGame record_game)
        {
            for (int frame_i = 0; frame_i < BpuyoParameter.BlockFrameSize; frame_i++)
            {
                int player_i = target_player.IsFirstPlayer() ? 0 : 1;

                FrameStream frame_stream = new FrameStream(decompressed_block_data, frame_i, player_i);

                // 取得した手数が不正ならcontinue
                if(frame_stream.match_count <= 0)
                {
                    continue;
                }

                // 新しい試合になったら更新
                if (record_game.matches.Count < frame_stream.match_count)
                {
                    record_game.matches.Add(new RecordMatch());
                }

                if (frame_stream.mode == Mode.MOVING)
                {
                    // 新しい手だったら更新
                    if (record_game.matches.Last().puts.Count < frame_stream.hand_count)
                    {
                        if (record_game.matches.Last().puts.Count > 0)
                        {
                            RecordOnePut pre_put = record_game.matches.Last().puts.Last();
                            record_game.matches.Last().puts.Add(new RecordOnePut());
                            record_game.matches.Last().puts.Last().SetPreMyField(pre_put);
                        }
                        else
                        {
                            record_game.matches.Last().puts.Add(new RecordOnePut());
                        }
                    }

                    // 1手目が置かれるまでcontinue
                    if (record_game.matches.Last().puts.Count == 0) continue;


                    if (frame_stream.Invalid()) continue;

                    record_game.matches.Last().puts.Last().SetModeBeforePut(frame_stream);
                    record_game.matches.Last().puts.Last().SetModeWait(frame_stream);

                }
                else if(frame_stream.mode == Mode.RESULT_WIN)
                {
                    record_game.matches.Last().SetWon();
                }
                else if(frame_stream.mode == Mode.RESULT_LOSE)
                {
                    record_game.matches.Last().SetLost();
                }
            }
        }


        // フィールドを前にずらす
        // これによって、与えられたフィールドに対してどこに置くかを示すデータセットに出来る
        private void FieldShiftForward(ref RecordGame record_game)
        {

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
    }
}
