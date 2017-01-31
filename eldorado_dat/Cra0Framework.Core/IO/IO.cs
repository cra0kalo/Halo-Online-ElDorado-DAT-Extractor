using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cra0Framework.Core
{

    //Cra0 IO CLASS
    //Version 1.0

    public static class IO
    {

        public enum ByteOrder : int
        {
            LittleEndian,
            BigEndian
        }

        public static long PaddingAlign(long num, long alignTo)
        {
            if (num % alignTo == 0)
            {
                return 0;
            }
            else
            {
                return alignTo - (num % alignTo);
            }
        }

        public static void AlignStream(Stream fs, long alignTo)
        {
            fs.Seek(PaddingAlign(fs.Position, alignTo), SeekOrigin.Current);
        }

        public static void Write_PadStream(Stream fs, byte padKey, long alignTo)
        {

            long SeekBytes = PaddingAlign(fs.Position, alignTo);
            if (SeekBytes == 0)
                return;

            byte[] padBytes = new byte[SeekBytes];
            for (int i = 0; i < padBytes.Length; i++)
            {
                padBytes[i] = padKey;
            }

            fs.Write(padBytes, 0, padBytes.Length);

        }



        public static byte ReadByte(BinaryReader reader)
        {
            return reader.ReadByte();
        }

        public static byte[] ReadBytes(BinaryReader reader, int fieldSize, ByteOrder byteOrder)
        {
            byte[] bytes = new byte[fieldSize];
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return reader.ReadBytes(fieldSize);
            }
            else
            {
                for (int i = fieldSize - 1; i >= 0; i--)
                {
                    bytes[i] = reader.ReadByte();
                }
                return bytes;
            }
        }



        public static byte[] ReadByteArray_BIG(BinaryReader reader, ulong dataAsize)
        {
            return ReadBytes(reader, Convert.ToInt32(dataAsize), ByteOrder.BigEndian);
        }


        public static long ReadLong64(BinaryReader reader, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return reader.ReadInt64();
            }
            else // Big-Endian
            {
                return BitConverter.ToInt64(ReadBytes(reader, 8, ByteOrder.BigEndian), 0);
            }
        }

        public static ulong ReadULong64(BinaryReader reader, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return reader.ReadUInt64();
            }
            else // Big-Endian
            {
                return BitConverter.ToUInt64(ReadBytes(reader, 8, ByteOrder.BigEndian), 0);
            }
        }


        public static int ReadInt32(BinaryReader reader, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return reader.ReadInt32();
            }
            else // Big-Endian
            {
                return BitConverter.ToInt32(ReadBytes(reader, 4, ByteOrder.BigEndian), 0);
            }
        }

        public static uint ReadUInt32(BinaryReader reader, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return reader.ReadUInt32();
            }
            else // Big-Endian
            {
                return BitConverter.ToUInt32(ReadBytes(reader, 4, ByteOrder.BigEndian), 0);
            }
        }

        public static ulong ReadUInt64(BinaryReader reader, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return reader.ReadUInt64();
            }
            else // Big-Endian
            {
                return BitConverter.ToUInt64(ReadBytes(reader, 8, ByteOrder.BigEndian), 0);
            }
        }



        public static short ReadInt16(BinaryReader reader, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return reader.ReadInt16();
            }
            else // Big-Endian
            {
                return BitConverter.ToInt16(ReadBytes(reader, 2, ByteOrder.BigEndian), 0);
            }
        }

        public static ushort ReadUInt16(BinaryReader reader, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return reader.ReadUInt16();
            }
            else // Big-Endian
            {
                return BitConverter.ToUInt16(ReadBytes(reader, 2, ByteOrder.BigEndian), 0);
            }
        }

        public static byte ReadUInt8(BinaryReader reader, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return reader.ReadByte();
            }
            else // Big-Endian
            {
                return reader.ReadByte();
            }
        }


        //Float



        public static float ReadFloat(BinaryReader reader, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return reader.ReadSingle();
            }
            else // Big-Endian
            {
                return BitConverter.ToSingle(ReadBytes(reader, 4, ByteOrder.BigEndian), 0);
            }
        }


        public static bool ReadBool32(BinaryReader reader, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                return Convert.ToBoolean(reader.ReadUInt32());
            }
            else // Big-Endian
            {
                return Convert.ToBoolean(BitConverter.ToUInt32(ReadBytes(reader, 4, ByteOrder.BigEndian), 0));
            }
        }



        /// <summary>
        /// Reads a string till the Null terminator advances stream by how many characters it read
        /// </summary>
        /// <param name="ii_Reader"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ReadStringASCIIZ(BinaryReader reader)
        {
            //Read till Null terminator
            string str = string.Empty;
            byte b = 0;
            b = reader.ReadByte();
            while (b != 0)
            {
                str += ((char)b).ToString();
                b = reader.ReadByte();
            }
            return str;
        }

        /// <summary>
        /// Takes a binary stream reads set many bytes returns string without any trails
        /// </summary>
        /// <param name="ii_Reader"></param>
        /// <param name="bytes2read"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ReadStringABSLen(BinaryReader reader, Int32 bytes2read)
        {
            //Reads String from current Binary stream
            string str = string.Empty;
            byte[] StringBuffer = reader.ReadBytes(bytes2read);
            str = System.Text.Encoding.ASCII.GetString(StringBuffer).TrimEnd('\0');
            return str;
        }


        public static string ReadStringABSLen(BinaryReader reader, Int32 bytes2read, Int32 align)
        {
            //Reads String from current Binary stream
            string str = string.Empty;
            byte[] StringBuffer = reader.ReadBytes(bytes2read);
            str = System.Text.Encoding.ASCII.GetString(StringBuffer).TrimEnd('\0');
            //align
            reader.BaseStream.Position += PaddingAlign(reader.BaseStream.Position, align);
            return str;
        }


        public static void Write_bytes(BinaryWriter writer, ByteOrder byteOrder, byte[] value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian
            {
                byte[] reversedBytes = value;
                Array.Reverse(reversedBytes);
                writer.Write(reversedBytes);
            }
        }


        public static void Write_int8(BinaryWriter writer, ByteOrder byteOrder, sbyte value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian (unchanged)
            {
                writer.Write(value);
            }
        }


        public static void Write_int16(BinaryWriter writer, ByteOrder byteOrder, Int16 value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian
            {
                byte[] reversedBytes = BitConverter.GetBytes(value);
                Array.Reverse(reversedBytes);
                writer.Write(BitConverter.ToInt16(reversedBytes, 0)); // do we need to do this?
            }
        }


        public static void Write_int32(BinaryWriter writer, ByteOrder byteOrder, Int32 value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian
            {
                byte[] reversedBytes = BitConverter.GetBytes(value);
                Array.Reverse(reversedBytes);
                writer.Write(BitConverter.ToInt32(reversedBytes, 0)); // do we need to do this?
            }
        }

        public static void Write_int64(BinaryWriter writer, ByteOrder byteOrder, Int64 value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian
            {
                byte[] reversedBytes = BitConverter.GetBytes(value);
                Array.Reverse(reversedBytes);
                writer.Write(BitConverter.ToInt64(reversedBytes, 0)); // do we need to do this?
            }
        }


        public static void Write_uint8(BinaryWriter writer, ByteOrder byteOrder, byte value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian (unchanged)
            {
                writer.Write(value);
            }
        }


        public static void Write_uint16(BinaryWriter writer, ByteOrder byteOrder, UInt16 value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian
            {
                byte[] reversedBytes = BitConverter.GetBytes(value);
                Array.Reverse(reversedBytes);
                writer.Write(BitConverter.ToUInt16(reversedBytes, 0)); // do we need to do this?
            }
        }


        public static void Write_uint32(BinaryWriter writer, ByteOrder byteOrder, UInt32 value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian
            {
                byte[] reversedBytes = BitConverter.GetBytes(value);
                Array.Reverse(reversedBytes);
                writer.Write(BitConverter.ToUInt32(reversedBytes, 0)); // do we need to do this?
            }
        }

        public static void Write_uint64(BinaryWriter writer, ByteOrder byteOrder, UInt64 value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian
            {
                byte[] reversedBytes = BitConverter.GetBytes(value);
                Array.Reverse(reversedBytes);
                writer.Write(BitConverter.ToUInt64(reversedBytes, 0)); // do we need to do this?
            }
        }

        public static void Write_float(BinaryWriter writer, ByteOrder byteOrder, float value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian
            {
                byte[] reversedBytes = BitConverter.GetBytes(value);
                Array.Reverse(reversedBytes);
                writer.Write(BitConverter.ToSingle(reversedBytes, 0)); // do we need to do this?
            }
        }

        public static void Write_double(BinaryWriter writer, ByteOrder byteOrder, double value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(value);
            }
            else // Big-Endian
            {
                byte[] reversedBytes = BitConverter.GetBytes(value);
                Array.Reverse(reversedBytes);
                writer.Write(BitConverter.ToDouble(reversedBytes, 0)); // do we need to do this?
            }
        }

        public static void Write_bool(BinaryWriter writer, ByteOrder byteOrder, bool value)
        {
            if (byteOrder == ByteOrder.LittleEndian)
            {
                writer.Write(Convert.ToUInt32(value));
            }
            else // Big-Endian
            {
                byte[] reversedBytes = BitConverter.GetBytes(Convert.ToUInt32(value));
                Array.Reverse(reversedBytes);
                writer.Write(BitConverter.ToUInt32(reversedBytes, 0)); // do we need to do this?
            }
        }


        public static void WriteAscii(BinaryWriter writer, string asciiString)
        {
            //Writes an ascii string
            writer.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(asciiString));
        }

        public static void WriteAsciiz(BinaryWriter writer, string asciiString)
        {
            //Writes an ascii string with a null terminator hence "AsciiZ"
            writer.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(asciiString));
            writer.Write(new byte());
        }

        public static void WriteNullByte(BinaryWriter writer)
        {
            byte NullB = 0;
            writer.Write(NullB);
        }

        public static void WriteNullBytes(BinaryWriter writer, int count)
        {
            byte NullB = 0;
            for (int i = 0; i <= count - 1; i++)
            {
                writer.Write(NullB);
            }
        }

    }
}
