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

            // Header information about developer (Monil)
            var assemblyTitle = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false)).Title;
            var copyright = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false)).Copyright;
            var version = ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyFileVersionAttribute), false)).Version;

            Console.WriteLine("***************************************************************");
            Console.WriteLine(string.Format("-- {0} {1}", assemblyTitle, version));
            Console.WriteLine(string.Format("-- {0}", copyright));
            Console.WriteLine("***************************************************************");

            // User inputs for dir location, number of files and type of files to be gathered
            Console.WriteLine(String.Empty);
            Console.Write("Enter Directory Path: ");
            var directoryPath = Console.ReadLine();
            Console.Write("Number of Files Required: ");
            var fileCountRequired = Console.ReadLine();
            Console.Write("File Extension Required: ");
            var fileExtension = Console.ReadLine();

            // Formatting dir entered for use
            if (directoryPath != null)
            {
                if (directoryPath.Contains('"'))
                {
                    directoryPath = directoryPath.TrimStart('"');
                    directoryPath = directoryPath.TrimEnd('"');
                }

                // Produces a list of all files in the specified dir, and with the specified extension as given by the user above
                // (if the user entered .java then all Java files in the dir etc.)
                var filesInDirectory = Directory.EnumerateFiles(directoryPath, string.Format("*{0}", fileExtension), SearchOption.AllDirectories);
                var files = new List<string>(filesInDirectory);

                // Random variable created based on the current date/time
                var random = new Random((int)DateTime.Now.Ticks);

                // Checks that the user entered a number of files to be randomly chosen
                if (fileCountRequired != null)
                {
                    // New list created to hold randomly selected file names, for loop cycles 
                    listOfFilesFound = new List<string>();

                    // Variable for stopping condition on for loop below
                    int j = int.Parse(fileCountRequired);

                    for (int i = 0; i < j; i++)
                    {
                        var fileName = files[random.Next(0, files.Count)];
                        bool fileExists;

                        if (fileExists = listOfFilesFound.Exists(element => element == fileName))
                        {
                            Console.WriteLine("*******************");
                            Console.WriteLine("*******************");
                            Console.WriteLine("Duplicate file found, skipping file and adding 1 to number of files required");
                            Console.WriteLine("*******************");
                            Console.WriteLine("*******************");
                            j += 1;

                        }
                        else
                        {
                            listOfFilesFound.Add(fileName);
                            Console.WriteLine(fileName);

                        }
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
