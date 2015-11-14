using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Simplicity.Core;

namespace SortOfRandomFileChooser
{
    public static class Program
    {
        private static List<string> listOfFilesFound;

        public static void Main(string[] args)
        {
            var applicationInfo = new ApplicationInfo();
            Console.WriteLine("***************************************************************");
            Console.WriteLine(string.Format("-- {0} {1}", applicationInfo.AssemblyTitle, applicationInfo.AssemblyVersion));
            Console.WriteLine(string.Format("-- {0}", applicationInfo.AssemblyCopyright));
            Console.WriteLine("***************************************************************");
            
            GetRandomFiles();
        }

        private static void GetRandomFiles()
        {
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
                var filesInDirectoryList = new List<string>(filesInDirectory);

                var random = new Random((int) DateTime.Now.Ticks << 0xFFFFFF);

                if (fileCountRequired != null)
                {
                    listOfFilesFound = new List<string>();

                    var fileCount = int.Parse(fileCountRequired);

                    for (var i = 0; i < fileCount; i++)
                    {
                        var fileName = filesInDirectoryList[random.Next(0, filesInDirectoryList.Count)];

                        if (!listOfFilesFound.Exists(e => e == fileName))
                        {
                            listOfFilesFound.Add(fileName);
                        }
                        else
                        {
                            fileCount++;
                        }
                    }

                    Console.WriteLine("The following files were selected: ");
                    foreach (var fileFound in listOfFilesFound)
                    {
                        Console.WriteLine(fileFound);
                    }

                    Console.WriteLine("Copy to Local Directory? Y/N");
                    Console.Write("Response: ");
                    var writeToFileResponse = Console.ReadLine();

                    if (writeToFileResponse == "Y" || writeToFileResponse == "y")
                    {
                        CopyFilesToFolder();
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

        private static void CopyFilesToFolder()
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
            }

            Console.WriteLine("File Paths Exported to: {0}", FileManagement.RelativeToApplication("RetrievedFiles.txt"));
            Console.WriteLine("Files Copied to: {0}", FileManagement.RelativeToApplication("Random Files"));
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }
    }
}
