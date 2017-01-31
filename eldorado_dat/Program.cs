using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cra0Framework.Core;

namespace eldorado_dat
{
    class Program
    {

        static DAT myDAT = null;
        static string Me_WorkingPath = null;
        static string In_FilePath = null;
        static string Out_FolderPath = null;

        static bool flag_debug = false;


        static void Main(string[] args)
        {
            Console.WriteLine(" ---Halo Online DAT Tool---");
            Console.WriteLine(" ---Version 1.0---");
            Console.WriteLine(" ---Authors: Cra0kalo---");
            Console.WriteLine("    ");
            Console.WriteLine("    For the halo online modding community!");
            Console.WriteLine("    ");
            Console.WriteLine(" ---http://dev.cra0kalo.com ---");
            Console.WriteLine(string.Empty);
            CMDCheck();
            Console.WriteLine(string.Empty);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

        }


        static public void CMDCheck()
        {


            // Get the values of the command line in an array
            // Index  Discription
            // 0      Full path of executing prograsm with program name
            // 1      First switch in command in your example
            string[] clArgs = Environment.GetCommandLineArgs();

            if (clArgs.Count() < 4 || clArgs.Count() > 5)
            {
                Console.WriteLine("Usage: eldorado_dat -d path/to/file.dat path/to/outputfolder");
                Console.WriteLine("Optional: --debug");
                Console.WriteLine("Example: eldorado_dat -d O:/RamBox/video.dat O:/RamBox/_extracted");
            }
            else
            {

                Console.WriteLine("Starting...");

                //set working path
                Me_WorkingPath = AppDomain.CurrentDomain.BaseDirectory;


                //check input -p followed by it's path
                int ac = 0;
                foreach (var arg in clArgs)
                {
                    if (arg == "-d")
                    {
                        In_FilePath = clArgs[ac + 1];
                        Out_FolderPath = clArgs[ac + 2];
                        break;
                    }

                    if (arg == "--debug")
                    {
                        flag_debug = true;
                    }



                    ac += 1;
                }

                if (In_FilePath == "" || Out_FolderPath == "")
                {
                    Console.WriteLine("Error check input arguments!");
                    return;
                }



                //filechecks I/O
                if (!(File.Exists(In_FilePath)))
                {

                    //maybe its in the working folder
                    if (File.Exists(Path.Combine(Me_WorkingPath, In_FilePath)))
                    {
                        In_FilePath = Path.Combine(Me_WorkingPath, In_FilePath);
                    }
                    else
                    {
                        In_FilePath = string.Empty;
                        Console.WriteLine("Error Input file doesn't seem to exist!");
                        return;
                    }
                }
                else
                {
                    //working folder
                    In_FilePath = Path.Combine(Me_WorkingPath, In_FilePath);
                }

                try
                {
                    Utilz.CreatePath(Out_FolderPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return;
                }


                if (System.IO.Path.IsPathRooted(Out_FolderPath) != true)
                {
                    Out_FolderPath = Path.Combine(Me_WorkingPath, Out_FolderPath);
                }



                Console.WriteLine("----------------------------------------------------------------------------");
                Console.WriteLine("InputFile: " + Path.GetFileName(In_FilePath));
                Console.WriteLine("InputFilePath: " + In_FilePath);
                Console.WriteLine("ExportPath: " + Out_FolderPath);


                //Pass to class object to export
                VText("");
                myDAT = new DAT(In_FilePath, Out_FolderPath, flag_debug);
                //Call export and finish
                bool result = myDAT.ParseExport();
                if (result != true)
                {
                    PError("An error has occurred somewhere during the export processs.");
                }
                else
                {
                    Console.WriteLine("Export completed()");
                }

            }
        }


        public static void VText(string text)
        {
            Console.WriteLine(text);
        }

        public static void VText(string text, ConsoleColor col)
        {
            ConsoleColor bc = Console.ForegroundColor;
            Console.ForegroundColor = col;
            Console.WriteLine(text);
            Console.ForegroundColor = bc;
        }

        public static void PError(string text)
        {
            // Console.Clear();
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine(text);
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("----------------------------------------------------------------------------");
        }


    } //end class
} //end namespace
