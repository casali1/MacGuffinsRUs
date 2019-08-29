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

            var index = Environment.CurrentDirectory.IndexOf("Code");
            var currentDirectory = Environment.CurrentDirectory.Substring(0, index);
            var photoPath = Path.Combine(currentDirectory, @"Base\\Photos\\");

            var files = Directory.GetFiles(photoPath);

            var categoriesPath = Path.Combine(currentDirectory, @"Base\\Categories\\");

            var dirs = Directory.GetDirectories(categoriesPath);

            foreach(var file in files)
            {
                var indexOfFile = file.IndexOf("Photos\\\\");
                var fileNameJPG = file.Remove(0, indexOfFile);
                fileNameJPG = fileNameJPG.Replace("Photos\\\\", "");
                var fileName = fileNameJPG.Replace(".jpg", "");

                var hasFileMoved = false;

                //Check for ProductNumber-Size-Color folders
                foreach (var dir in dirs)
                {
                    var indexOfCat = dir.IndexOf("Categories\\\\");
                    var category = dir.Remove(0, indexOfFile);
                    category = category.Replace("Categories\\\\", "");
                 
                    if (category.ToLower() == fileName.ToLower())
                    {
                        var destFile = Path.Combine(dir, fileNameJPG);
                        File.Move(file, destFile);
                        hasFileMoved = true;
                        break;
                    }
                }

                //Check for ProductNumber-Size and Product-Color folders
                if (!hasFileMoved)
                {
                    foreach (var dir in dirs)
                    {
                        var indexOfCat = dir.IndexOf("Categories\\\\");
                        var category = dir.Remove(0, indexOfFile);
                        category = category.Replace("Categories\\\\", "");

                        var fileElements = fileName.Split('-');

                        //Check for ProductNumber-Size folders
                        if (fileElements.Length > 2)
                        {
                            if (fileElements[1].ToLower() == "x") fileElements[1] = fileElements[1] + "-" + fileElements[2];  //Accounts for x.
                            if (fileElements.Length > 3) fileElements[2] = fileElements[3];                                   //Accounts for a 4th element.
                      
                            if (category.ToLower() == (fileElements[0] + "-" + fileElements[1]).ToLower() 
                                || category.ToLower() == (fileElements[0] + "-" + fileElements[2]).ToLower())
                            {
                                var destFile = Path.Combine(dir, fileNameJPG);
                                File.Move(file, destFile);
                                hasFileMoved = true;
                                break;
                            }
                        }
                        else if(fileElements.Length == 2)
                        {
                            if (category.ToLower() == (fileElements[0] + "-" + fileElements[1]).ToLower())
                            {
                                var destFile = Path.Combine(dir, fileNameJPG);
                                File.Move(file, destFile);
                                hasFileMoved = true;
                                break;
                            }
                        }
                    }
                }

                //Check for ProductNumber folders
                if (!hasFileMoved)
                {
                    foreach (var dir in dirs)
                    {
                        var indexOfCat = dir.IndexOf("Categories\\\\");
                        var category = dir.Remove(0, indexOfFile);
                        category = category.Replace("Categories\\\\", "");

                        var fileElements = fileName.Split('-');

                        if (category.ToLower() == fileElements[0].ToLower())
                        {
                            var destFile = Path.Combine(dir, fileNameJPG);
                            File.Move(file, destFile);
                            break;
                        }
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
