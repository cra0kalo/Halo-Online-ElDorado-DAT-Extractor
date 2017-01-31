using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Cra0Framework.Core
{
    public static class Utilz
    {

        static public void CreatePath(string pathName)
        {
            if (!(Directory.Exists(pathName)))
            {
                Directory.CreateDirectory(pathName);
            }
        }

        static public int _sizeof(byte[] data)
        {
            return data.Length;
        }

        static public string cToHex(uint decval)
        {
            return decval.ToString("X");
        }

        static public string cToHex(int decval)
        {
            return decval.ToString("X");
        }

        static public int cToDEC(string hexval)
        {
            return Convert.ToInt32(hexval, 16);
        }


        public static byte[] StructureToByteArray<T>(T strc) where T : struct
        {
            int objsize = Marshal.SizeOf(typeof(T));
            byte[] pBuffer = new byte[objsize];
            IntPtr buff_ptr = Marshal.AllocHGlobal(objsize);
            Marshal.StructureToPtr(strc, buff_ptr, true);
            Marshal.Copy(buff_ptr, pBuffer, 0, objsize);
            Marshal.FreeHGlobal(buff_ptr);
            return pBuffer;
        }


        /// <summary>
        /// Marshals a byte array into the appropriate structure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public static T ByteArrayToStructure<T>(byte[] byteData) where T : struct
        {
            byte[] buffer = byteData;
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T data = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),typeof(T));
            handle.Free();
            return data;
        }


        /// <summary>
        /// Marshals a byte array into the appropriate structure (doesn't use GC)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public unsafe static T ByteArrayToStructureFAST<T>(byte[] byteData) where T : struct
        {
            fixed (byte* dataptr = byteData)
            {
                T data = (T)Marshal.PtrToStructure(((IntPtr) dataptr), typeof(T));
                return data;
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            // Insert null checking here for production
            byte[] buffer = new byte[8192];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }



        static public byte[] GetBytes_ASCII(string inString)
        {
            return System.Text.Encoding.ASCII.GetBytes(inString);
        }

        static public byte[] GetBytes_UTF8(string inString)
        {
            return System.Text.Encoding.UTF8.GetBytes(inString);
        }

        static public string GetString_ASCII(byte[] byteArray)
        {
            return System.Text.Encoding.ASCII.GetString(byteArray);
        }

        static public string GetString_UTF8(byte[] byteArray)
        {
            return System.Text.Encoding.UTF8.GetString(byteArray);
        }
        static public bool FourCCCheck(char[] magicA, char[] magicB)
        {
            if (magicA.Length != magicB.Length)
                return false;
            bool result = magicA.SequenceEqual(magicB);
            return result;
        }

        static public bool matchMagic4(byte[] magicA, byte[] magicB)
        {
            if (magicA.Length != magicB.Length)
                return false;
            bool result = magicA.SequenceEqual(magicB);
            return result;
        }





    }
}
