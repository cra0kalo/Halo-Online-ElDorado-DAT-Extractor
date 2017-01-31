using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cra0Framework.Core;


//typedef c++ <3
using asciiz = System.String;

using int8 = System.Byte;
using int16 = System.Int16;
using int32 = System.Int32;
using int64 = System.Int64;

using uint8 = System.Byte;
using uint16 = System.UInt16;
using uint32 = System.UInt32;
using uint64 = System.UInt64;

using System.Diagnostics;
using LZ4;


namespace eldorado_dat
{
    public partial class DAT
    {
        [DebuggerDisplay("_offset = {_offset} size = {size}")]
        public class CHUNK
        {
            public uint32 _offset;
            public uint32 size;
            public uint32 zsize;
            public byte[] cdata;

        }


        public class subCHUNK : CHUNK
        {

            public void readIn(BinaryReader br, IO.ByteOrder endian)
            {
                _offset = (uint32)br.BaseStream.Position;
                size = IO.ReadUInt32(br, endian);
                zsize = IO.ReadUInt32(br, endian);
                if (size == 0)
                {
                    //align n return
                    br.BaseStream.Position += IO.PaddingAlign(br.BaseStream.Position, 2);
                    return;
                }

                byte[] zbuffer = br.ReadBytes((int)zsize);
                cdata = LZ4Codec.Decode(zbuffer, 0, zbuffer.Length, (int)size);
                if (cdata.Length != (int)size)
                    throw new Exception("Error");
            }
        }

    }
}
