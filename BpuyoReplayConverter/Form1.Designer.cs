namespace BpuyoReplayConverter
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.InputPlayerNameText = new System.Windows.Forms.RichTextBox();
            this.FolderPathText = new System.Windows.Forms.RichTextBox();
            this.FolderSetButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(136, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(269, 129);
            this.button1.TabIndex = 0;
            this.button1.Text = "出力開始";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(117, 327);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(382, 30);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "ここに出力経過が表示されます";
            // 
            // InputPlayerNameText
            // 
            this.InputPlayerNameText.Location = new System.Drawing.Point(273, 103);
            this.InputPlayerNameText.Name = "InputPlayerNameText";
            this.InputPlayerNameText.Size = new System.Drawing.Size(247, 28);
            this.InputPlayerNameText.TabIndex = 2;
            this.InputPlayerNameText.Text = "NONAME";
            // 
            // FolderPathText
            // 
            this.FolderPathText.Location = new System.Drawing.Point(117, 65);
            this.FolderPathText.Name = "FolderPathText";
            this.FolderPathText.Size = new System.Drawing.Size(403, 32);
            this.FolderPathText.TabIndex = 3;
            this.FolderPathText.Text = "";
            // 
            // FolderSetButton
            // 
            this.FolderSetButton.Location = new System.Drawing.Point(12, 65);
            this.FolderSetButton.Name = "FolderSetButton";
            this.FolderSetButton.Size = new System.Drawing.Size(88, 23);
            this.FolderSetButton.TabIndex = 4;
            this.FolderSetButton.Text = "フォルダを指定";
            this.FolderSetButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "抽出したいプレイヤー名を入力してください。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 337);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "出力結果";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(104, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(317, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Bぷよのリプレイファイル(prd)をぷよ譜へ変換機";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 369);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FolderSetButton);
            this.Controls.Add(this.FolderPathText);
            this.Controls.Add(this.InputPlayerNameText);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox InputPlayerNameText;
        private System.Windows.Forms.RichTextBox FolderPathText;
        private System.Windows.Forms.Button FolderSetButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

