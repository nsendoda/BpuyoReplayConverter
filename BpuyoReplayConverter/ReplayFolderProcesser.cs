using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tommy;

namespace BpuyoReplayConverter
{
    class ReplayFolderProcesser
    {
        // フォルダに存在するprdファイルごとにデータを抜き出して出力を行う。
        // @args foder_path..フォルダパス
        // @args player_name..データを取り出すプレイヤー名
        // @args textbox..処理中表示用
        static public void StartProcess(String input_folder_path, RichTextBox result_textbox, String input_player_name, bool onlywin)
        {
            String output_folder_path = System.IO.Path.Combine(System.Environment.CurrentDirectory, input_player_name);
            // 実行フォルダに入力したプレイヤー名のフォルダが存在していないならば、作る
            if (!System.IO.File.Exists(output_folder_path))
            {
                System.IO.Directory.CreateDirectory(output_folder_path);
            }


            // 全てのリプレイファイルのパスを取得
            System.IO.DirectoryInfo input_directory_info = new System.IO.DirectoryInfo(input_folder_path);
            System.IO.FileInfo[] files = input_directory_info.GetFiles("*.prd", System.IO.SearchOption.AllDirectories);

            ReplayDataScanner replay_data_sacanner = new ReplayDataScanner();

            int file_max_count = files.Length;
            int file_count = files.Length;
            int file_i_for_display = 0;
            foreach (System.IO.FileInfo f in files)
            {
                file_i_for_display++;

                // 試合
                RecordGame record_game = new RecordGame();

                // ぷよ譜を取得
                bool ok = replay_data_sacanner.ReadAndTraceData(f.FullName, input_player_name, record_game);
                // 該当プレイヤーが対戦にいない場合
                if (! ok)
                {
                    file_count--;
                    continue;
                }

                // ファイル名生成
                String output_file_path
                    = System.IO.Path.Combine(output_folder_path, f.Name.Split('.').First()) + ".toml";

                TomlTable toml = RecordTomlHelper.Toml(record_game, onlywin);

                // ファイルに書き込み
                using (StreamWriter writer = new StreamWriter(File.OpenWrite(output_file_path)))
                    toml.ToTomlString(writer);
                // 処理中プロセステキスト表示
                result_textbox.Text = "処理中..(" + file_i_for_display.ToString() + "/" + file_max_count.ToString() + ")";
            }
            result_textbox.Text = "処理が終了しました。(" + file_count.ToString() + "/" + file_max_count.ToString() + "prdファイルが有効)";
        }
    }
}
