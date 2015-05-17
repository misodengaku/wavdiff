namespace fucktool
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
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
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
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.thresholdLabel = new System.Windows.Forms.Label();
			this.fileList = new System.Windows.Forms.ListBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.statusLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(114, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "処理対象ファイル（2つ）";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(12, 101);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(260, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "FUCK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(12, 130);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(224, 16);
			this.checkBox1.TabIndex = 3;
			this.checkBox1.Text = "逆からサーチ(曲の始まりが声のときに使用)";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// trackBar1
			// 
			this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackBar1.Location = new System.Drawing.Point(12, 164);
			this.trackBar1.Maximum = 32767;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(260, 45);
			this.trackBar1.TabIndex = 4;
			this.trackBar1.Value = 20000;
			this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
			// 
			// thresholdLabel
			// 
			this.thresholdLabel.AutoSize = true;
			this.thresholdLabel.Location = new System.Drawing.Point(12, 149);
			this.thresholdLabel.Name = "thresholdLabel";
			this.thresholdLabel.Size = new System.Drawing.Size(94, 12);
			this.thresholdLabel.TabIndex = 5;
			this.thresholdLabel.Text = "threshold (20000)";
			// 
			// fileList
			// 
			this.fileList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fileList.FormattingEnabled = true;
			this.fileList.ItemHeight = 12;
			this.fileList.Location = new System.Drawing.Point(12, 28);
			this.fileList.Name = "fileList";
			this.fileList.Size = new System.Drawing.Size(260, 28);
			this.fileList.TabIndex = 6;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(12, 74);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(207, 19);
			this.textBox1.TabIndex = 7;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(75, 12);
			this.label2.TabIndex = 8;
			this.label2.Text = "出力先ファイル";
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(225, 72);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(47, 23);
			this.button2.TabIndex = 9;
			this.button2.Text = "参照";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// statusLabel
			// 
			this.statusLabel.AutoSize = true;
			this.statusLabel.Location = new System.Drawing.Point(10, 212);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(20, 12);
			this.statusLabel.TabIndex = 10;
			this.statusLabel.Text = "OK";
			// 
			// Form1
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.statusLabel);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.fileList);
			this.Controls.Add(this.thresholdLabel);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "fucktool";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Label thresholdLabel;
		private System.Windows.Forms.ListBox fileList;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label statusLabel;
	}
}

