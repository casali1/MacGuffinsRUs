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
            var program = new Program();

            var index = Environment.CurrentDirectory.IndexOf("Code");
            var currentDirectory = Environment.CurrentDirectory.Substring(0, index);
            var photoPath = Path.Combine(currentDirectory, @"Base\\Photos\\");

            var files = Directory.GetFiles(photoPath);

            var categoriesPath = Path.Combine(currentDirectory, @"Base\\Categories\\");

            var dirs = Directory.GetDirectories(categoriesPath);

            foreach (var file in files)
            {
                var hasFileMoved = false;

                //Check for ProductNumber-Size-Color files
                hasFileMoved = program.CheckProductNumberSizeColor(dirs, file);

                //Check for ProductNumber-Size files and Product-Color files
                if (!hasFileMoved) hasFileMoved = program.CheckProductNumberSizeOrProductColor(dirs, file);

                //Check for ProductNumbers
                if (!hasFileMoved) program.CheckProductNumber(dirs, file);             
            }

            //if (Directory.Exists(photoPath) && Directory.Exists(categoryPath))
            //{


            //}
            //else
            //{
            //    Console.WriteLine("Cannot find Photos or Categories directory, please make sure the exe is in the same root as these folders.");
            //    Console.ReadLine();
            //}

        }



        #region Helper functions
     
        public bool CheckProductNumberSizeColor(string[] dirs, string file)
        {
            var indexOfFile = file.IndexOf("Photos\\\\");
            var fileNameJPG = file.Remove(0, indexOfFile);
            fileNameJPG = fileNameJPG.Replace("Photos\\\\", "");
            var fileName = fileNameJPG.Replace(".jpg", "");

            var hasFileMoved = false;
            foreach (var dir in dirs)
            {
                var indexOfCat = dir.IndexOf("Categories\\\\");
                var category = dir.Remove(0, indexOfCat);
                category = category.Replace("Categories\\\\", "");

                if (category.ToLower() == fileName.ToLower())
                {
                    var destFile = Path.Combine(dir, fileNameJPG);
                    File.Move(file, destFile);
                    hasFileMoved = true;
                    break;
                }
            }

            return hasFileMoved;
        }

        public bool CheckProductNumberSizeOrProductColor(string[] dirs, string file)
        {
            var indexOfFile = file.IndexOf("Photos\\\\");
            var fileNameJPG = file.Remove(0, indexOfFile);
            fileNameJPG = fileNameJPG.Replace("Photos\\\\", "");
            var fileName = fileNameJPG.Replace(".jpg", "");

            var hasFileMoved = false;
            foreach (var dir in dirs)
            {
                var category = GetCategory(dir);

                var fileElements = fileName.Split('-');

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
                else if (fileElements.Length == 2)
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

            return hasFileMoved;
        }

        public void CheckProductNumber(string[] dirs, string file)
        {
            var indexOfFile = file.IndexOf("Photos\\\\");
            var fileNameJPG = file.Remove(0, indexOfFile);
            fileNameJPG = fileNameJPG.Replace("Photos\\\\", "");
            var fileName = fileNameJPG.Replace(".jpg", "");

            foreach (var dir in dirs)
            {
                var fileElements = fileName.Split('-');

                if (GetCategory(dir).ToLower() == fileElements[0].ToLower())
                {
                    var destFile = Path.Combine(dir, fileNameJPG);
                    File.Move(file, destFile);
                    break;
                }
            }
        }

        public string GetCategory(string dir)
        {
            var indexOfCat = dir.IndexOf("Categories\\\\");
            var category = dir.Remove(0, indexOfCat);
            return category.Replace("Categories\\\\", "");
        }

        #endregion
    }
}
