using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavFuckLib
{
    public class WavFuck
    {
	    private WavData[] _fileData = new WavData[2];

	    public int Threshold
	    {
		    get;
		    set
		    {
			    if (value > 32767 || value < 0)
			    {
				    throw new ArgumentOutOfRangeException();
			    }
		    }
	    }



		private static WavData OpenWavfile(string s)
		{

			var header = new WavHeader();
			var lDataList = new List<short>();
			var rDataList = new List<short>();
			using (var fs = new FileStream(s, FileMode.Open, FileAccess.Read))
			using (var br = new BinaryReader(fs))
			{
				try
				{
					header.riffID = br.ReadBytes(4);
					header.size = br.ReadUInt32();
					header.wavID = br.ReadBytes(4);
					header.fmtID = br.ReadBytes(4);
					header.fmtSize = br.ReadUInt32();
					header.format = br.ReadUInt16();
					header.channels = br.ReadUInt16();
					header.sampleRate = br.ReadUInt32();
					header.bytePerSec = br.ReadUInt32();
					header.blockSize = br.ReadUInt16();
					header.bit = br.ReadUInt16();
					header.dataID = br.ReadBytes(4);
					header.dataSize = br.ReadUInt32();

					for (var i = 0; i < header.dataSize / header.blockSize; i++)
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

			var wavData = new WavData();
			wavData.LeftChannel = lDataList;
			wavData.RightChannel = rDataList;
			wavData.Header = header;

			return wavData;
		}

	    public WavFuck(string infile1, string infile2)
	    {
		    Threshold = 20000;
			Parallel.Invoke(() => _fileData[0] = OpenWavfile(infile1), () => _fileData[1] = OpenWavfile(infile2));
	    }

	    public int SearchWavDiff()
	    {
		    if (_fileData[0] == null || _fileData[1] == null)
		    {
			    throw new Exception("File 1 or 2 is null.");
		    }

	    }
    }
}
