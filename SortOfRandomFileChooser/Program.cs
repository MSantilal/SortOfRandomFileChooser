using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Simplicity.Core;

namespace SortOfRandomFileChooser
{
    public static class Program
    {
        private static List<string> listOfFilesFound;

        public static void Main(string[] args)
        {
            GetRandomFiles();
        }

        private static void GetRandomFiles()
        {
            var assemblyTitle = ((AssemblyTitleAttribute) Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false)).Title;
            var copyright = ((AssemblyCopyrightAttribute) Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false)).Copyright;
            var version = ((AssemblyFileVersionAttribute) Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyFileVersionAttribute), false)).Version;

            Console.WriteLine("***************************************************************");
            Console.WriteLine(string.Format("-- {0} {1}", assemblyTitle, version));
            Console.WriteLine(string.Format("-- {0}", copyright));
            Console.WriteLine("***************************************************************");

            Console.WriteLine(String.Empty);
            Console.Write("Enter Directory Path: ");
            var directoryPath = Console.ReadLine();
            Console.Write("Number of Files Required: ");
            var fileCountRequired = Console.ReadLine();
            Console.Write("File Extension Required: ");
            var fileExtension = Console.ReadLine();
            if (directoryPath != null)
            {
                if (directoryPath.Contains('"'))
                {
                    directoryPath = directoryPath.TrimStart('"');
                    directoryPath = directoryPath.TrimEnd('"');
                }

                var filesInDirectory = Directory.EnumerateFiles(directoryPath, string.Format("*{0}", fileExtension), SearchOption.AllDirectories);
                
                var files = new List<string>(filesInDirectory);
                var random = new Random((int) DateTime.Now.Ticks);
                if (fileCountRequired != null)
                {
                    listOfFilesFound = new List<string>();
                    for (int i = 0; i < int.Parse(fileCountRequired); i++)
                    {
                        var fileName = files[random.Next(0, files.Count)];
                        listOfFilesFound.Add(fileName);
                        Console.WriteLine(fileName);
                        Thread.Sleep(2500);
                    }
                    Console.WriteLine("Files Found. Copy to local directory? Y/N");
                    CopyFilesToFolder();
                }
            }
        }

        private static void CopyFilesToFolder()
        {
            Console.Write("Response: ");
            var writeToFileResponse = Console.ReadLine();
            var duplicatedFiles = new List<string>();
            if (writeToFileResponse == "Y" || writeToFileResponse == "y")
            {
                Console.WriteLine("Copying Files...");
                File.WriteAllLines(FileManagement.RelativeToApplication("CopiedFiles.txt"), listOfFilesFound);
                foreach (var filePaths in listOfFilesFound)
                {
                    var x = filePaths.Split('\\');
                    if (!Directory.Exists(FileManagement.RelativeToApplication("Random Files")))
                    {
                        Directory.CreateDirectory(FileManagement.RelativeToApplication("Random Files"));
                    }

                    if (!File.Exists(FileManagement.RelativeToApplication("Random Files", x[x.Length - 1])))
                    {
                        File.Copy(filePaths, FileManagement.RelativeToApplication("Random Files", x[x.Length - 1]));
                    }
                    else
                    {
                        duplicatedFiles.Add(filePaths);
                    }
                }

                Console.WriteLine("File Paths Exported to: {0}", FileManagement.RelativeToApplication("RetrievedFiles.txt"));
                Console.WriteLine("Files Copied to: {0}", FileManagement.RelativeToApplication("Random Files"));
                Console.WriteLine("The following files were not copied as they were duplicate: ");
                foreach (var dupFiles in duplicatedFiles)
                {
                    Console.WriteLine(dupFiles);
                }
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("You have selected No. Many Puppies are now crying at your betrayal.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
