using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavFuckLib
{
	// http://blog.yomak.info/2011/11/wavecnet.html
	class WavHeader
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
}
