using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Cra0Framework.Core;
using LZ4;

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

        private string In_FilePath;
        private string Out_FolderPath;

        private bool flag_debug;

        //Reader
        private BinaryReader br;
        private FileStream fs;

        private IO.ByteOrder endian = IO.ByteOrder.LittleEndian;

        //DAT DATA FILES
        private HEADER d_header = new HEADER();
        private List<TOC_ENTRY> d_tocEntryList = new List<TOC_ENTRY>();

        public DAT()
        {

        }

        public DAT(string InFilePath, string OutFolderPath,bool flag_debug)
        {
            this.In_FilePath = InFilePath;
            this.Out_FolderPath = OutFolderPath;
            this.flag_debug = flag_debug;
        }


        public bool ParseExport()
        {
            return _ParseDAT();
        }


        public void Dispose()
        {
            if (br != null)
                br.Close();
        }

        //internals
        private bool _ParseDAT()
        {
            fs = new FileStream(this.In_FilePath, FileMode.Open, FileAccess.Read);
            br = new BinaryReader(fs);

            //read header first
            d_header.readIn(br, endian);

            //Seek to TOC
            br.BaseStream.Seek(d_header.dTOC_ptr, SeekOrigin.Begin);

            //Read TOC entries
            for (int i = 0; i < d_header.dTOC_entryCount; i++)
            {
                TOC_ENTRY curEntry = new TOC_ENTRY();
                curEntry.readIn(br, endian);
                if (curEntry.toc_entryPtr == 0xFFFFFFFF)
                    continue;
                d_tocEntryList.Add(curEntry);
            }

            //Seek back to 0
            br.BaseStream.Seek(0, SeekOrigin.Begin);

            //try gather data for extract
            for (int j = 0; j < d_tocEntryList.Count; j++)
            {
                TOC_ENTRY curEntry = d_tocEntryList[j];
                TOC_ENTRY nxtEntry = null;
                if ((j + 1 < d_tocEntryList.Count))
                {
                    nxtEntry = d_tocEntryList[j + 1];
                }
                else
                {
                    nxtEntry = new TOC_ENTRY((uint32)d_header.dTOC_ptr);
                }


                br.BaseStream.Seek(curEntry.toc_entryPtr, SeekOrigin.Begin);
                CHUNK curChunk = new CHUNK();

                List<subCHUNK> subchunks = new List<subCHUNK>();
                while (br.BaseStream.Position < nxtEntry.toc_entryPtr - 16)
                {
                    subCHUNK cur = new subCHUNK();
                    cur.readIn(br, endian);
                    subchunks.Add(cur);

                    //br.BaseStream.Position += IO.PaddingAlign(br.BaseStream.Position, 16);


                }

                if (subchunks.Count == 0)
                    continue;

                curChunk._offset = subchunks[0]._offset;

                List<byte[]> decoded_chunks = new List<byte[]>();
                foreach(subCHUNK s in subchunks)
                {
                    if (s.cdata == null)
                        continue;
                    decoded_chunks.Add(s.cdata);
                    //add sizes
                    curChunk.zsize += s.zsize;
                    curChunk.size += s.size;
                }

                curChunk.cdata = decoded_chunks.SelectMany(x => x).ToArray();

                //write out file with .datchunk extension
                string out_filepath = Path.Combine(this.Out_FolderPath, "entry_" + j + ".datchunk");
                using (FileStream fw = new FileStream(out_filepath, FileMode.Create, FileAccess.Write))
                    using (BinaryWriter bw = new BinaryWriter(fw))
                    {
                        bw.Write(curChunk.cdata);
                        bw.Flush();
                    }

                decoded_chunks.Clear();
            }

            //Seek back to 0
            br.BaseStream.Seek(0, SeekOrigin.Begin);


            return true;
        }



        private void println(string textout)
        {
            Program.VText(textout);
        }

        private void println(string textout, ConsoleColor col)
        {
            Program.VText(textout, col);
        }

    }
}
