using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MacGuffinsRUs
{
    class Program
    {
        const string PhotoPath = "Base\\Photos\\";
        const string CategoryPath = "Base\\Categories\\";

        const string PhotosString = "Photos\\";
        const string CategoryString = "Categories\\";

        static void Main(string[] args)
        {
            var program = new Program();

            var index = Environment.CurrentDirectory.IndexOf("Code");
            var currentDirectory = Environment.CurrentDirectory.Substring(0, index);

            //Get file directories.
            var filephotoPath = Path.Combine(currentDirectory, @PhotoPath);
            var dirFiles = Directory.GetFiles(filephotoPath);

            //Get directories for the Catagories.
            var categoriesPath = Path.Combine(currentDirectory, @CategoryPath);
            var dirs = Directory.GetDirectories(categoriesPath);

            if (Directory.Exists(filephotoPath) && Directory.Exists(categoriesPath))
            {
                foreach (var file in dirFiles)
                {
                    var hasFileMoved = false;

                    //Check for Metadata 1 - Metadata 2 - Metadata 3 files
                    hasFileMoved = program.CheckSpecificMetadata(dirs, file);

                    //Check for Metadata 1 - Metadata 2 files and Metadata 1 - Metadata 3 files
                    if (!hasFileMoved) hasFileMoved = program.CheckMetadataOneAndMetadataTwoOrMetadataThree(dirs, file);

                    //Check for Metadata 1
                    if (!hasFileMoved) program.CheckMetadataOne(dirs, file);
                }
            }
            else
            {
                Console.WriteLine("Cannot find Photos or Categories directory, please make sure the exe is in the same root as these folders.");
                Console.ReadLine();
            }
        }

        #region Helper functions

        public bool CheckSpecificMetadata(string[] dirs, string file)
        {
            var indexOfFile = file.IndexOf(PhotosString);
            var fileNameJPG = file.Remove(0, indexOfFile);
            fileNameJPG = fileNameJPG.Replace(PhotosString, "");
            var fileName = fileNameJPG.Replace(".jpg", "");

            var hasFileMoved = false;
            foreach (var dir in dirs)
            {
                if (GetCategory(dir).ToLower() == fileName.ToLower())  //Checks for a specific filename.
                {
                    var destFile = Path.Combine(dir, fileNameJPG);
                    File.Move(file, destFile);
                    hasFileMoved = true;
                    break;
                }
            }

            return hasFileMoved;
        }

        public bool CheckMetadataOneAndMetadataTwoOrMetadataThree(string[] dirs, string file)
        {
            var indexOfFile = file.IndexOf(PhotosString);
            var fileNameJPG = file.Remove(0, indexOfFile);
            fileNameJPG = fileNameJPG.Replace(PhotosString, "");
            var fileName = fileNameJPG.Replace(".jpg", "");

            var hasFileMoved = false;
            foreach (var dir in dirs)
            {
                var category = GetCategory(dir);

                var fileElements = fileName.Split('-');

                if (fileElements.Length > 2) //Checks filenames with 3 or 4 elements.
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
                else if (fileElements.Length == 2) //Checks filenames with 2 elements.
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

        public void CheckMetadataOne(string[] dirs, string file)
        {
            var indexOfFile = file.IndexOf(PhotosString);
            var fileNameJPG = file.Remove(0, indexOfFile);
            fileNameJPG = fileNameJPG.Replace(PhotosString, "");
            var fileName = fileNameJPG.Replace(".jpg", "");

            foreach (var dir in dirs)
            {
                var fileElements = fileName.Split('-');

                if (GetCategory(dir).ToLower() == fileElements[0].ToLower())  //Checks filename's first element.
                {
                    var destFile = Path.Combine(dir, fileNameJPG);
                    File.Move(file, destFile);
                    break;
                }
            }
        }

        public string GetCategory(string dir)
        {
            var indexOfCat = dir.IndexOf(CategoryString);
            var category = dir.Remove(0, indexOfCat);
            return category.Replace(CategoryString, "");
        }

        #endregion
    }
}
