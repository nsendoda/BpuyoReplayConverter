using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace BpuyoReplayConverter
{

    public partial class Form1 : Form
    {
        public const int kNumberBlockFrame = 120;
        public const int kPlayerNumber = 2;
        public Form1()
        {
            InitializeComponent();
        }

        class ReplayBlockData
        {
            public byte[] data;
            public bool has_memory;
            public int size;

            public ReplayBlockData() { }

            public void Resize(int block_size)
            {
                size = block_size;
                data = new byte[size];
            }
        };

        // 一手ごとのフィールドと現在手とネクストとどこに置くかを示すクラス
        class RecordOnePut
        {
            const int kFieldSize = 78;
            const int kKumipuyoSize = 2;
            public int[] my_field = new int[kFieldSize];
            public int[] enemy_field = new int[kFieldSize];
            public int[] current_kumipuyo = new int[kKumipuyoSize];
            public int[] next_kumipuyo = new int[kKumipuyoSize];
            public int put_index; // 0 - 22
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Click += new EventHandler(this.OnClickStart);
            FolderSetButton.Click += new EventHandler(this.OnClickFolderSet);

        }

        private void OnClickStart(Object sender, EventArgs e)
        {
                //OKボタンがクリックされたとき、選択されたファイルを読み取り専用で開く
            System.IO.FileStream stream = new System.IO.FileStream(FolderPathText.Text, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            if (stream != null)
            {
                //内容を読み込み、表示する
                try
                {
                    GetData(FolderPathText.Text);
                }
                catch (Exception ex) {
                    richTextBox1.Text = "このファイルは読み取れません。";
                }
                stream.Close();
            }
        }      

        private void OnClickFolderSet(Object sender, EventArgs e)
        {
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            ofd.FileName = "default.prd";
            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = @"";
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "Bぷよリプレイファイル(*.prd)|*.prd|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに選択されるものを指定する
            ofd.FilterIndex = 1;
            //タイトルを設定する
            ofd.Title = "開くファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;
            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名を表示する
                Console.WriteLine(ofd.FileName);
                FolderPathText.Text = ofd.FileName;
            }
        }


        // ファイルのヘッダを読んで飛ばす
        void ReadAndSkipPrdFileHeader(System.IO.FileStream replay_stream)
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
            richTextBox1.Text += BitConverter.ToInt16(result_buffer, 0).ToString();
            replay_stream.Read(result_buffer, 0, kResultSize);
            richTextBox1.Text += BitConverter.ToInt16(result_buffer, 0).ToString();
            replay_stream.Read(skip_buffer, 0, kSecondSkipSize);
            replay_stream.Read(name_buffer, 0, kPlayerNameSize);
            richTextBox1.Text += System.Text.Encoding.UTF8.GetString(name_buffer);
            replay_stream.Read(name_buffer, 0, kPlayerNameSize);
            richTextBox1.Text += System.Text.Encoding.UTF8.GetString(name_buffer);

        }

        // 120フレームブロックのヘッダを読んで飛ばす
        void ReadAndSkipPrdBlockHeader(System.IO.FileStream replay_stream, ReplayBlockData replay_block)
        {
            byte[] has_memory = new byte[1];
            byte[] block_size = new byte[4];
            byte[] meta_data = new byte[2];// 78DA

            replay_stream.Read(has_memory, 0, 1);
            replay_stream.Read(block_size, 0, 4);
            replay_stream.Read(meta_data, 0, 2);

            replay_block.has_memory = BitConverter.ToBoolean(has_memory, 0);
            replay_block.Resize(BitConverter.ToInt32(block_size, 0));

            replay_stream.Read(replay_block.data, 0, replay_block.size);
        }

        // リプレイブロックを解凍したものを返す
        byte[] DecompressReplayBlock(ReplayBlockData replay_block)
        {
            const int kOneFrameBytePerPlayer = 1269;
            const int kNumberPlayer = 2;
            const int kUncompressedDataSize = kOneFrameBytePerPlayer * kNumberBlockFrame * kNumberPlayer;
            byte[] uncompressed_data = new byte[kUncompressedDataSize];
//  プレイヤー１人分のデータが1フレームにつき1269バイト、それが1P,2Pずつ交互に、120フレーム分並んで
            System.IO.MemoryStream memory_stream = new System.IO.MemoryStream(replay_block.data);
            System.IO.Compression.DeflateStream deflate_stream = new System.IO.Compression.DeflateStream(memory_stream, System.IO.Compression.CompressionMode.Decompress);
            deflate_stream.Read(uncompressed_data, 0, kUncompressedDataSize);
            memory_stream.Close();
            deflate_stream.Close();
            return uncompressed_data;
        }

        enum Mode : int { CHAIN = 4, WAIT };
        void AddRecord(byte[] unzip, RecordOnePut[,] match_record)
        {
            for (int flame_i = 0; flame_i < kNumberBlockFrame; flame_i++)
            {
                for (int player_i = 0; player_i < kPlayerNumber; player_i++)
                {
                    int base_index = flame_i * 2538 + player_i * 1269;
                    int match_index = base_index + 1269;
                    int hand_index = base_index + 1157;
                    int match_number = BitConverter.ToInt16(unzip, match_index);
                    int hand_number = unzip[hand_index];
                    Mode mode_player = (Mode)unzip[base_index];

                    if(mode_player == Mode.CHAIN){

                    }else if (mode_player == Mode.WAIT)
                    {

                    }
                }
            }
        }

        void ReadAndSkipPrdContents(System.IO.FileStream replay_stream, RecordOnePut[,] match_record)
        {
            ReplayBlockData replay_block = new ReplayBlockData();
            while(replay_stream.Length - replay_stream.Position > 0)
            {
                ReadAndSkipPrdBlockHeader(replay_stream, replay_block);
                if (replay_block.has_memory == false) continue;
                byte[] unzip = DecompressReplayBlock(replay_block);
                AddRecord(unzip, match_record);
            }
        }

        private void GetData(String replay_file_path)
        {
            // 最低限の試合数と手数
            // @suspicion vectorみたいに増やせるの？
            RecordOnePut[,] match_record = new RecordOnePut[20, 20];

            System.IO.FileStream replay_stream = new System.IO.FileStream(replay_file_path, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            ReadAndSkipPrdFileHeader(replay_stream);
            ReadAndSkipPrdContents(replay_stream, match_record);

            replay_stream.Close();
        }
    }
}
