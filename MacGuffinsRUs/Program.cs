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
            const string photoPath = "Photos/";
            const string categoryPath = "Categories/";

            

            if (Directory.Exists(photoPath) && Directory.Exists(categoryPath))
            {


            }
            else
            {
                Console.WriteLine("Cannot find Photos or Categories directory, please make sure the exe is in the same root as these folders.");
                Console.ReadLine();
            }

        }
    }
}
