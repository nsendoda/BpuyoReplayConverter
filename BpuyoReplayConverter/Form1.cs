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
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Click += new EventHandler(this.OnClickStart);
            FolderSetButton.Click += new EventHandler(this.OnClickFolderSet);
        }

        private void OnClickStart(Object sender, EventArgs e)
        {
            //OKボタンがクリックされたとき、選択されたファイルを読み取り専用で開く
            ReplayFolderProcesser.StartProcess(FolderPathText.Text, richTextBox1, InputPlayerNameText.Text);
        }      

        private void OnClickFolderSet(Object sender, EventArgs e)
        {
            //OpenFileDialogクラスのインスタンスを作成
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //上部に表示する説明テキスト
            fbd.Description = "フォルダを指定してください";
            //ルートフォルダを実行ファイルのフォルダに設定
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            //最初に選択するフォルダを指定する
            //RootFolder以下にあるフォルダである必要がある
            fbd.SelectedPath = System.Environment.CurrentDirectory;
            //ダイアログを表示する
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名を表示する
                Console.WriteLine(fbd.SelectedPath);
                FolderPathText.Text = fbd.SelectedPath;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
