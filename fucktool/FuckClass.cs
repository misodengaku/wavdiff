using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace fucktool
{
	public class FuckClass
	{

		// http://blog.yomak.info/2011/11/wavecnet.html
		public struct WavHeader
		{
			public byte[] riffID; // "riff"
			public uint size;  // ファイルサイズ-8
			public byte[] wavID;  // "WAVE"
			public byte[] fmtID;  // "fmt "
			public uint fmtSize; // fmtチャンクのバイト数
			public ushort format; // フォーマット
			public ushort channels; // チャンネル数
			public uint sampleRate; // サンプリングレート
			public uint bytePerSec; // データ速度
			public ushort blockSize; // ブロックサイズ
			public ushort bit;  // 量子化ビット数
			public byte[] dataID; // "data"
			public uint dataSize; // 波形データのバイト数
		}

		public class WavFile
		{
			private WavHeader _header;

			public WavHeader Header
			{
				get { return _header; }
				set { _header = value; }
			}

			public List<short> LeftChannel { get; set; }

			public List<short> RightChannel { get; set; }

			public int Count
			{
				get { return LeftChannel.Count; }
			}

			public int CueIndex { get; private set; }

			public WavFile()
			{
				CueIndex = -1;
				Header = new WavHeader();
				LeftChannel = new List<short>();
				RightChannel = new List<short>();
			}

			public void OpenWavfile(string s)
			{
				Header = new WavHeader();
				using (var fs = new FileStream(s, FileMode.Open, FileAccess.Read))
				using (var br = new BinaryReader(fs))
				{
					try
					{
						_header.riffID = br.ReadBytes(4);
						_header.size = br.ReadUInt32();
						_header.wavID = br.ReadBytes(4);
						_header.fmtID = br.ReadBytes(4);
						_header.fmtSize = br.ReadUInt32();
						_header.format = br.ReadUInt16();
						_header.channels = br.ReadUInt16();
						_header.sampleRate = br.ReadUInt32();
						_header.bytePerSec = br.ReadUInt32();
						_header.blockSize = br.ReadUInt16();
						_header.bit = br.ReadUInt16();
						_header.dataID = br.ReadBytes(4);
						_header.dataSize = br.ReadUInt32();

						for (var i = 0; i < Header.dataSize / Header.blockSize; i++)
						{
							LeftChannel.Add((short)br.ReadUInt16());
							RightChannel.Add((short)br.ReadUInt16());
						}
					}
					finally
					{
						br.Close();
						fs.Close();
					}
				}
			}

			public void SaveWavfile(string outputpath)
			{
				_header.dataSize = (uint)Math.Max(LeftChannel.Count, RightChannel.Count) * 4;

				//using (var fs = new FileStream(outputpath, FileMode.Create, FileAccess.Write))
				//using (var bw = new BinaryWriter(fs))
				/*
				
			public byte[] riffID; // "riff"
			public uint size;  // ファイルサイズ-8
			public byte[] wavID;  // "WAVE"
			public byte[] fmtID;  // "fmt "
			public uint fmtSize; // fmtチャンクのバイト数
			public ushort format; // フォーマット
			public ushort channels; // チャンネル数
			public uint sampleRate; // サンプリングレート
			public uint bytePerSec; // データ速度
			public ushort blockSize; // ブロックサイズ
			public ushort bit;  // 量子化ビット数
			public byte[] dataID; // "data"
			public uint dataSize; // 波形データのバイト数
				*/
				using (var bw = File.OpenWrite(outputpath))
				{
					try
					{
						bw.Write(Header.riffID, 0, 4);
						bw.Write(BitConverter.GetBytes(Header.size), 0, 4);
						bw.Write(Header.wavID, 0, 4);
						bw.Write(Header.fmtID, 0, 4);
						bw.Write(BitConverter.GetBytes(Header.fmtSize), 0, 4);
						bw.Write(BitConverter.GetBytes(Header.format), 0, 2);
						bw.Write(BitConverter.GetBytes(Header.channels), 0, 2);
						bw.Write(BitConverter.GetBytes(Header.sampleRate), 0, 4);
						bw.Write(BitConverter.GetBytes(Header.bytePerSec), 0, 4);
						bw.Write(BitConverter.GetBytes(Header.blockSize), 0, 2);
						bw.Write(BitConverter.GetBytes(Header.bit), 0, 2);
						bw.Write(Header.dataID, 0, 4);
						bw.Write(BitConverter.GetBytes(Header.dataSize), 0, 4);

						var zeroByte = new byte[] {0, 0};

						for (var i = 0; i < Header.dataSize / Header.blockSize; i++)
						{
							if (i < LeftChannel.Count)
							{
								bw.Write(BitConverter.GetBytes((ushort)LeftChannel[i]), 0, 2);
							}
							else
							{
								bw.Write(zeroByte, 0, 2);
							}

							if (i < RightChannel.Count)
							{
								bw.Write(BitConverter.GetBytes((ushort)RightChannel[i]), 0, 2);
							}
							else
							{
								bw.Write(zeroByte, 0, 2);
							}
						}
					}
					finally
					{
						bw.Close();
						//fs.Close();
					}
				}
			}

			public int Cue(int threshold, bool reverse = false)
			{
				if (reverse)
				{
					CueIndex = LeftChannel
						.Select((x, i) => new {Value = (int)x, Index = i })
						.Last(item => Math.Abs(item.Value) > threshold)
						.Index;
				}
				else
				{
					CueIndex = LeftChannel
						.Select((x, i) => new { Value = (int)x, Index = i })
						.First(item => Math.Abs(item.Value) > threshold)
						.Index;
				}
				return CueIndex;
			}

			public int GetDiff(WavFile file1)
			{
				return CueIndex - file1.CueIndex;
			}

			public void Merge(WavFile file1, int diff)
			{
				var newWavFile = new WavFile();
				WavFile mergeWavFile;

				if (diff < 0)
				{
					diff = diff * -1;

					var zeroList = new List<short>(new short[diff]);

					Parallel.Invoke(() =>
					{
						newWavFile.LeftChannel = file1.LeftChannel.Select(x =>
						{
							if (x != -32768)
								return (short)(x * -1);
							else
								return short.MaxValue;
						}).ToList<short>();
						newWavFile.LeftChannel.InsertRange(0, zeroList);
					}, () =>
					{
						newWavFile.RightChannel = newWavFile.RightChannel.Select(x =>
						{
							if (x != -32768)
								return (short)(x * -1);
							else
								return short.MaxValue;
						}).ToList<short>();
						newWavFile.RightChannel.InsertRange(0, zeroList);
					});

					mergeWavFile = this;
				}
				else
				{
					Parallel.Invoke(() =>
					{
						
						newWavFile.LeftChannel = LeftChannel.Skip(diff).Select(x =>
						{
							if (x != -32768)
								return (short)(x * -1);
							else
								return short.MaxValue;
						}).ToList<short>();
					}, () =>
					{
						
						newWavFile.RightChannel = RightChannel.Skip(diff).Select(x =>
						{
							if (x != -32768)
								return (short)(x * -1);
							else
								return short.MaxValue;
						}).ToList<short>();
					});

					mergeWavFile = file1;
				}
				
				Parallel.Invoke(() =>
				{
					LeftChannel = newWavFile.LeftChannel.Zip(mergeWavFile.LeftChannel, (s, s1) => (short)(s + s1)).ToList();
				},
				() =>
				{
					RightChannel = newWavFile.RightChannel.Zip(mergeWavFile.RightChannel, (s, s1) => (short)(s + s1)).ToList();
				});
			}
		}

		public static async Task<double> Fuck(List<string> files, string outputpath, bool reverse, int threshold, IProgress<string> progress)
		{
			if (files.Count != 2)
				throw new ArgumentOutOfRangeException("filesの長さは2である必要性があります");

			await Task.Run(() =>
			{

				string key = null;
				var Header = new WavHeader();
				WavFile file0 = new WavFile(), file1 = new WavFile();

				Parallel.Invoke(() =>
				{
					file0.OpenWavfile(files[0]);
					progress.Report("open: "+ files[0]);
					file0.Cue(threshold, reverse);
					progress.Report("先頭発見");
				}, () =>
				{
					file1.OpenWavfile(files[1]);
					progress.Report("open: " + files[1]);
					file1.Cue(threshold, reverse);
					progress.Report("先頭発見");
				});

				var diff = file0.GetDiff(file1);
				file0.Merge(file1, diff);
				progress.Report("保存中");
				file0.SaveWavfile(outputpath);
				progress.Report("saved: " + outputpath);

				/*
				if (file0.Count < file1.Count)
				{
					var diff = file1.GetDiff(file0, reverse);
					progress.Report("diff");
					file0.Merge(file1, diff);
					//file1.Merge(file0, diff);
					progress.Report("merged");
					file0.SaveWavfile(outputpath);
					progress.Report("saved: " + outputpath);
				}
				else
				{
					var diff = file0.GetDiff(file1, reverse);
					progress.Report("diff");
					file1.Merge(file0, diff);
					progress.Report("merged");
					file1.SaveWavfile(outputpath);
					progress.Report("saved: " + outputpath);
				}
				*/
			});
			return 0.0;
		}



		// TAF(Track Antiphase Fuckability)
		private static double GetTAF(List<short> l, List<short> r)
		{

			// TAF(Track Antiphase Fuckability)
			var lzc = l.Count(x => x == 0);
			var rzc = r.Count(x => x == 0);
			var lp = l.Select(x => Math.Abs((double)x)).Sum();
			var rp = r.Select(x => Math.Abs((double)x)).Sum();
			var p = (lp + rp) / 2 / l.Count;


			if (lzc + rzc == 0)
			{
				Console.WriteLine("TAF=0.00000");
				return 0.0;
			}
			else
			{
				Console.WriteLine("power = " + p);
				var taf = p * (double)(lzc + rzc) / 2 / l.Count;
				Console.WriteLine("TAF=" + taf.ToString("n5"));
				return taf;
			}

		}

	}
}
