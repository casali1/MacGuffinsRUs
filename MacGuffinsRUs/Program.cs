using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MacGuffinsRUs
{
    class Program
    {
        static void Main(string[] args)
        { 
            //const string photoPath = "Photos/";
            //const string categoryPath = "Categories/";


            //var currentDirectory = Environment.CurrentDirectory;

            var index = Environment.CurrentDirectory.IndexOf("Code");
            var currentDirectory = Environment.CurrentDirectory.Substring(0, index);
            var photoPath = Path.Combine(currentDirectory, @"Base\\Photos\\");

            var files = Directory.GetFiles(photoPath);

            var categoriesPath = Path.Combine(currentDirectory, @"Base\\Categories\\");

            var dirs = Directory.GetDirectories(categoriesPath);

            foreach(var file in files)
            {
                var indexOfFile = file.IndexOf("Photos\\");
                var fileNameJPG = file.Remove(0, indexOfFile);
                fileNameJPG = fileNameJPG.Replace("Photos\\\\", "");
                var fileName = fileNameJPG.Replace(".jpg", "");

                foreach(var dir in dirs)
                {
                    if(dir.ToLower().Contains(fileName.ToLower()))
                    {
                        var destFile = Path.Combine(dir, fileNameJPG);
                        System.IO.File.Copy(file, destFile, true);
                        break;
                    }
                }
            }

            //string[] directories = Directory.GetDirectories("\\\\MacGuffins\\Base", "Photos", SearchOption.AllDirectories);
            //var aaa = Directory.GetFiles("\\MacGuffins\\Base\\Photos");
            //var dirs = Directory.GetDirectories("Photos");
            //string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            //var dirs = Directory.GetDirectories("C:\\Users\\frank\\Desktop\\Experiments\\MacGuffins\\Base")

            //if (Directory.Exists(photoPath) && Directory.Exists(categoryPath))
            //{


            //}
            //else
            //{
            //    Console.WriteLine("Cannot find Photos or Categories directory, please make sure the exe is in the same root as these folders.");
            //    Console.ReadLine();
            //}

        }
    }
}
