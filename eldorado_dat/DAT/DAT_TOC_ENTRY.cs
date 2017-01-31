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


namespace eldorado_dat
{
    public partial class DAT
    {

        [DebuggerDisplay("toc_entryPtr = {toc_entryPtr} ")]
        public class TOC_ENTRY
        {
            public uint32 toc_entryPtr;

            public TOC_ENTRY()
            {
            }

            public TOC_ENTRY(uint32 in_entryPtr)
            {
                this.toc_entryPtr = in_entryPtr;
            }

            public void readIn(BinaryReader br, IO.ByteOrder endian)
            {
                toc_entryPtr = IO.ReadUInt32(br, endian);
            }
        }
    }
}
