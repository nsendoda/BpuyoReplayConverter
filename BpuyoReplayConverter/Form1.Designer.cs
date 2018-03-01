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
            this.PlayerNameText = new System.Windows.Forms.RichTextBox();
            this.FolderPathText = new System.Windows.Forms.RichTextBox();
            this.FolderSetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(40, 173);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "出力開始";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(139, 173);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(226, 59);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // PlayerNameText
            // 
            this.PlayerNameText.Location = new System.Drawing.Point(456, 22);
            this.PlayerNameText.Name = "PlayerNameText";
            this.PlayerNameText.Size = new System.Drawing.Size(100, 28);
            this.PlayerNameText.TabIndex = 2;
            this.PlayerNameText.Text = "";
            // 
            // FolderPathText
            // 
            this.FolderPathText.Location = new System.Drawing.Point(2, 110);
            this.FolderPathText.Name = "FolderPathText";
            this.FolderPathText.Size = new System.Drawing.Size(585, 57);
            this.FolderPathText.TabIndex = 3;
            this.FolderPathText.Text = "";
            // 
            // FolderSetButton
            // 
            this.FolderSetButton.Location = new System.Drawing.Point(49, 65);
            this.FolderSetButton.Name = "FolderSetButton";
            this.FolderSetButton.Size = new System.Drawing.Size(88, 23);
            this.FolderSetButton.TabIndex = 4;
            this.FolderSetButton.Text = "フォルダを指定";
            this.FolderSetButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 234);
            this.Controls.Add(this.FolderSetButton);
            this.Controls.Add(this.FolderPathText);
            this.Controls.Add(this.PlayerNameText);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox PlayerNameText;
        private System.Windows.Forms.RichTextBox FolderPathText;
        private System.Windows.Forms.Button FolderSetButton;
    }
}

