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


namespace eldorado_dat
{
    public partial class DAT
    {

        public class HEADER
        {

            public uint32 blankMagic;
            public uint32 dTOC_ptr;
            public uint32 dTOC_entryCount;
            public uint32 unkVarA;
            public uint32 unkVarB;
            public uint32 unkVarC;
            public uint32 unkVarD;
            public uint32 unkVarE;

            public void readIn(BinaryReader br, IO.ByteOrder endian)
            {
                blankMagic = IO.ReadUInt32(br, endian);
                dTOC_ptr = IO.ReadUInt32(br, endian);
                dTOC_entryCount = IO.ReadUInt32(br, endian);
                unkVarA = IO.ReadUInt32(br, endian);
                unkVarB = IO.ReadUInt32(br, endian);
                unkVarC = IO.ReadUInt32(br, endian);
                unkVarD = IO.ReadUInt32(br, endian);
                unkVarE = IO.ReadUInt32(br, endian);
            }
        }
    }
}
