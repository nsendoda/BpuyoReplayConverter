using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BpuyoReplayConverter
{
    class ReplayFolderProcesser
    {
        static public void StartProcess(String input_folder_path, RichTextBox result_textbox, String input_player_name)
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
            int file_i = 0;
            foreach (System.IO.FileInfo f in files)
            {
                file_i++;
                // ぷよ譜を取得
                // 該当プレイヤーが対戦にいない場合空文字列が返ってくる
                String Output = replay_data_sacanner.ReadAndTraceData(f.FullName, input_player_name);
                if (Output == String.Empty)
                {
                    file_count--;
                    continue;
                }

                String output_file_path
                    = System.IO.Path.Combine(output_folder_path, f.Name.Split('.').First()) + ".txt";

                // ファイルに書き込み
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(output_file_path, false,
                    System.Text.Encoding.Unicode))
                {
                    sw.Write(Output);
                }
                result_textbox.Text = "処理中..(" + file_i.ToString() + "/" + file_max_count.ToString() + ")";
            }
            result_textbox.Text = "処理が終了しました。(" + file_count.ToString() + "/" + file_max_count.ToString() + "prdファイルが有効)";
        }
    }
}
