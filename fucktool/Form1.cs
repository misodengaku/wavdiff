using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fucktool
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			thresholdLabel.Text = "threshold (" + trackBar1.Value + ")";
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			// ドラッグ＆ドロップされたファイル
			var files = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList();
			files = files.Take(2).ToList();

			if (fileList.Items.Count == 0)
				fileList.Items.AddRange(files.Cast<object>().ToArray()); // リストボックスに表示
			else if (fileList.Items.Count > 0)
			{
				var fl = fileList.Items.Cast<string>().ToList();
				var mf = fl.Union(files).Reverse().Take(2).Reverse().Cast<object>().ToArray();
				fileList.Items.Clear();
				fileList.Items.AddRange(mf);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			var saveFileDialog = new SaveFileDialog();
			saveFileDialog.DefaultExt = "wav";
			saveFileDialog.Filter = "WAVE file|*.wav|すべてのファイル|*";
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				textBox1.Text = saveFileDialog.FileName;
			}

		}

		private async void button1_Click(object sender, EventArgs e)
		{
			var filename = @"Y:\EAC\その他\ラブライブ！\μ's - Music S.T.A.R.T!!\ok\そして最後のページにはfuck.wav";

			statusLabel.Text = "処理中...";
			var progress = new Progress<string>(s =>
			{
				statusLabel.Text = s;
			});
			await FuckClass.Fuck(fileList.Items.Cast<string>().ToList(), textBox1.Text, checkBox1.Checked, trackBar1.Value, progress);

			if (MessageBox.Show("処理が完了しました\nファイルを参照しますか？", "完了", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				var p = new System.Diagnostics.Process();
				p.StartInfo.FileName = "explorer.exe";
				p.StartInfo.Arguments = "/select,\"" + textBox1.Text;
				p.Start();
			}
			
			statusLabel.Text = "OK";
		}
	}
}
