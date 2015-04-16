using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wavdiff
{
	class Program
	{
		// http://blog.yomak.info/2011/11/wavecnet.html
		struct WavHeader
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
			public List<byte> extendedArea;
		}

		static void Main(string[] args)
		{
			var options = new HashSet<string> { "-in1", "-in2", "-threshold", "-o", "-fuck", "-reverse" };
			string key = null;
			int threshold;
			var Header = new WavHeader();
			Tuple<List<short>, List<short>, WavHeader> file0 = null, file1 = null;

			var cmdargs = args
				.GroupBy(s => options.Contains(s) ? key = s : key)
				.ToDictionary(g => g.Key, g => g.Skip(1).FirstOrDefault());
			Parallel.Invoke(() => { file0 = OpenWavfile(cmdargs["-in1"]); }, () => { file1 = OpenWavfile(cmdargs["-in2"]); });

			var file0Data = file0.Item2;
			var file1Data = file1.Item2;
			var file0Head = 0;
			var file1Head = 0;
			var file0Count = 0;
			var file1Count = 0;
			var file0Flag = false;
			var file1Flag = false;
			int diff;
			List<Int16> lNewDataList = null, rNewDataList = null;



			if (!cmdargs.ContainsKey("-in1") && !cmdargs.ContainsKey("-in2") && !cmdargs.ContainsKey("-o"))
			{
				var path = Environment.GetCommandLineArgs()[0];
				Console.WriteLine("Usage: " + path + " -in1 filename -in2 filename -o outfilename [-threshold] [-fuck]");
				Console.WriteLine("\t-in1: input file 1 path");
				Console.WriteLine("\t-in2: input file 2 path");
				Console.WriteLine("\t-o: output file path");
				Console.WriteLine("\t-threshold: peak detect level (optional, default: 20000, 0~32767)");
				Console.WriteLine("\t-fuck: enable autofuck mode (optional)");
				Console.WriteLine("\t-reverse: enable reverse search mode (optional)");
				return;
			}
			if (!cmdargs.ContainsKey("-threshold"))
			{
				threshold = 20000;
			}
			else
			{
				threshold = int.Parse(cmdargs["-threshold"]);
			}


			if (cmdargs.ContainsKey("-fuck"))
			{
				Console.WriteLine("autofuck mode enabled..");
			}


			if (cmdargs.ContainsKey("-reverse"))
			{
				Console.WriteLine("Reverse search mode enable.");
				file0Data.Reverse();
				file1Data.Reverse();
			}
			Parallel.Invoke(() =>
			{
				for (var i = 0; i < file0Data.Count; i++)
				{
					if (!file0Flag && file0Data[i] != 0)
					{
						file0Head = i;
						file0Flag = true;
					}
					if (Math.Abs(file0Data[i]) > threshold)
					{
						file0Count = i;
						break;
					}
				}
			}, () =>
			{
				for (var i = 0; i < file1Data.Count; i++)
				{
					if (!file1Flag && file1Data[i] != 0)
					{
						file1Head = i;
						file1Flag = true;
					}
					if (Math.Abs(file1Data[i]) > threshold)
					{
						file1Count = i;
						break;
					}
				}
			});
			
			
			if (cmdargs.ContainsKey("-reverse"))
			{
				Parallel.Invoke(() => file0Data.Reverse(), () => file1Data.Reverse());
			}


			if (cmdargs.ContainsKey("-reverse"))
			{
				diff = (file0Data.Count - file0Count) - (file1Data.Count - file1Count);
			}
			else
			{
				diff = file0Count - file1Count;
			}

			if (diff < 0)
			{
				diff = diff*-1;
				Console.WriteLine("Diff: " + diff);
				Console.WriteLine("File: " + cmdargs["-in2"]);
				Parallel.Invoke(() =>
				{
					lNewDataList = file1.Item1.Skip(diff).Select(x =>
					{
						if (x != -32768)
							return (short) (x*-1);
						else
							return short.MaxValue;
					}).ToList<short>();
				}, () =>
				{
					rNewDataList = file1.Item2.Skip(diff).Select(x =>
					{
						if (x != -32768)
							return (short) (x*-1);
						else
							return short.MaxValue;
					}).ToList<short>();
				}, () =>
				{
					Header = file1.Item3;
				});
				
				


				if (cmdargs.ContainsKey("-fuck"))
				{
					Console.WriteLine("Autofucking...");
					// newdata + file0
					Parallel.Invoke(() => { lNewDataList = lNewDataList.Zip(file0.Item1, (s, s1) => (short)(s + s1)).ToList(); },
						() => { rNewDataList = rNewDataList.Zip(file0.Item2, (s, s1) => (short)(s + s1)).ToList(); });
					
					
				}
			}
			else
			{

				Console.WriteLine("Diff: " + diff);
				Console.WriteLine("File: " + cmdargs["-in1"]);

				Parallel.Invoke(() =>
				{
					lNewDataList = file0.Item1.Skip(diff).Select(x =>
					{
						if (x != -32768)
							return (short) (x*-1);
						else
							return short.MaxValue;
					}).ToList<short>();
				}, () =>
				{
					rNewDataList = file0.Item2.Skip(diff).Select(x =>
					{
						if (x != -32768)
							return (short) (x*-1);
						else
							return short.MaxValue;
					}).ToList<short>();
				}, () =>
				{
					Header = file0.Item3;
				});
				
				


				if (cmdargs.ContainsKey("-fuck"))
				{
					Console.WriteLine("Autofucking...");
					// newdata + file1
					// メモリが足りなくなる
					if (Header.sampleRate < 48001)
					{

						Parallel.Invoke(() => { lNewDataList = lNewDataList.Zip(file1.Item1, (s, s1) => (short) (s + s1)).ToList(); },
							() => { rNewDataList = rNewDataList.Zip(file1.Item2, (s, s1) => (short) (s + s1)).ToList(); });
					}
					else
					{
						lNewDataList = lNewDataList.Zip(file1.Item1, (s, s1) => (short)(s + s1)).ToList();
						rNewDataList = rNewDataList.Zip(file1.Item2, (s, s1) => (short)(s + s1)).ToList();
					}


				}
			}




			Header.dataSize = (uint)Math.Max(lNewDataList.Count, rNewDataList.Count) * 4;

			using (var fs = new FileStream(cmdargs["-o"], FileMode.Create, FileAccess.Write))
			using (var bw = new BinaryWriter(fs))
			{
				try
				{
					bw.Write(Header.riffID);
					bw.Write(Header.size);
					bw.Write(Header.wavID);
					bw.Write(Header.fmtID);
					bw.Write(Header.fmtSize);
					bw.Write(Header.format);
					bw.Write(Header.channels);
					bw.Write(Header.sampleRate);
					bw.Write(Header.bytePerSec);
					bw.Write(Header.blockSize);
					bw.Write(Header.bit);

					if (Header.format != 1)
					{
						bw.Write(Header.extendedArea.ToArray());
					}
					else
					{
						bw.Write(Header.dataID);
					}
					
					bw.Write(Header.dataSize);

					for (var i = 0; i < Header.dataSize / Header.blockSize; i++)
					{
						if (i < lNewDataList.Count)
						{
							bw.Write((ushort)lNewDataList[i]);
						}
						else
						{
							bw.Write(0);
						}

						if (i < rNewDataList.Count)
						{
							bw.Write((ushort)rNewDataList[i]);
						}
						else
						{
							bw.Write(0);
						}
					}
				}
				finally
				{
					bw.Close();
					fs.Close();
				}
			}


			Console.WriteLine("output: " + cmdargs["-o"]);
			return;
		}

		private static Tuple<List<short>, List<short>, WavHeader> OpenWavfile(string s)
		{

			var Header = new WavHeader();
			var lDataList = new List<short>();
			var rDataList = new List<short>();
			using (var fs = new FileStream(s, FileMode.Open, FileAccess.Read))
			using (var br = new BinaryReader(fs))
			{
				try
				{
					Header.riffID = br.ReadBytes(4);
					Header.size = br.ReadUInt32();
					Header.wavID = br.ReadBytes(4);
					Header.fmtID = br.ReadBytes(4);
					Header.fmtSize = br.ReadUInt32();
					Header.format = br.ReadUInt16();
					Header.channels = br.ReadUInt16();
					Header.sampleRate = br.ReadUInt32();
					Header.bytePerSec = br.ReadUInt32();
					Header.blockSize = br.ReadUInt16();
					Header.bit = br.ReadUInt16();
					if (Header.format != 1)
					{
						Header.extendedArea = new List<byte>();
						var extendSize = br.ReadInt16();
						Header.extendedArea.Add((byte)(extendSize & 0xFF00 >> 16));
						Header.extendedArea.Add((byte)(extendSize & 0xFF));
						Header.extendedArea.AddRange(br.ReadBytes(extendSize)); // extended area + "data"
						while (true)
						{
							var d = br.ReadByte();
							Header.extendedArea.Add(d);
							if (d == 'd')
							{
								d = br.ReadByte();
								Header.extendedArea.Add(d);
								if (d == 'a')
								{
									d = br.ReadByte();
									Header.extendedArea.Add(d);
									if (d == 't')
									{

										d = br.ReadByte();
										Header.extendedArea.Add(d);
										if (d == 'a')
										{
											break;
										}
									}
								}
							}
						}
					}
					else
					{
						Header.dataID = br.ReadBytes(4);
					}
					Header.dataSize = br.ReadUInt32();

					for (var i = 0; i < Header.dataSize / Header.blockSize; i++)
					{
						lDataList.Add((short)br.ReadUInt16());
						rDataList.Add((short)br.ReadUInt16());
					}
				}
				finally
				{
					br.Close();
					fs.Close();
				}
			}
			Console.WriteLine(s);


			return Tuple.Create(lDataList, rDataList, Header);
		}
	}
}
