using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NewMacGuffinsRUs
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
                Dictionary<string, List<Category>> categories = GetCategories(categoriesPath);

                // Get list of jpg photos and put them into the correct spot.
                // Parallel foreach is used to speed up the process, and it should be safe since moving any individual photo
                // does not interfere with moving other photos.
                IEnumerable<string> photos = Directory.EnumerateFiles(filephotoPath, "*.jpg");
                Parallel.ForEach(photos, (photo) => { MovePhoto(photo, categories); });
                Console.WriteLine("Finished moving photos.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Cannot find Photos or Categories directory, please make sure the exe is in the same root as these folders.");
                Console.ReadLine();
            }

        }

        // Move a photo into the correct category folder
        static void MovePhoto(string photoPath, Dictionary<string, List<Category>> categories)
        {
            string name = Path.GetFileNameWithoutExtension(photoPath);
            Category cat = FindCategory(name, categories);
            if (cat == null)
            {
                Console.WriteLine($"NOTICE: Unable to find a category for {name}, please file manually.");
                return;
            };
            Directory.Move(photoPath, cat.Path + "/" + Path.GetFileName(photoPath));

            // Output that the file has been moved.
            // It might be a good idea to take this out if moving thousands of files
            // since the writes may actually cause the program to run slower.
            // Console.WriteLine("Categorized " + name + " in " + cat.Name);
        }

        // Create a dictionary to hold all of the categories, keyed by the product name
        // This is done to improve processing speed filtering down to correct product categories before checking metadata
        static Dictionary<string, List<Category>> GetCategories(string path)
        {
            var comparer = StringComparer.OrdinalIgnoreCase; // make dictionary keys ignore case
            var categories = new Dictionary<string, List<Category>>(comparer);
            foreach (var dir in Directory.GetDirectories(path))
            {
                Category cat = new Category(dir);
                if (categories.ContainsKey(cat.ProductCode))
                {
                    categories[cat.ProductCode].Add(cat);
                }
                else
                {
                    categories.Add(cat.ProductCode, new List<Category> { cat });
                }
            }
            return categories;
        }

        // Return the best category for a given image
        static Category FindCategory(string imageName, Dictionary<string, List<Category>> categories)
        {
            var comparer = StringComparer.OrdinalIgnoreCase; // used to ignore case in comparisons
            string[] splitName = StringFuncs.SplitName(imageName);
            string productName = splitName[0];
            string metadata = splitName.ElementAtOrDefault(1); // returns null if there is no metadata

            // Return null if there is no matching category
            if (!categories.ContainsKey(productName))
            {
                Console.WriteLine("No category for {0}", imageName);
                return null;
            }

            // Narrow down the categories to just ones with a matching product name
            var matchingCategories = categories[productName];

            // If the image only has a name, return category with no other metadata
            if (metadata is null)
            {
                Category baseCategory = matchingCategories.First(cat => cat.Metadata is null);
                return baseCategory;
            }

            List<string> metaList = StringFuncs.SplitMetadata(metadata).ToList();

            // Sort by number of matching metadata items between categories and images.
            // This is done through ordering the count of the intersection between the two.
            // Doing it this way handles arbitrary ordering and number of metadata tags.
            IEnumerable<Category> match = matchingCategories
                                               .OrderByDescending(cat => metaList.Intersect(cat.MetaList, comparer).Count());

            // Remove all cases where the category has more metadata than the image.
            // Keep categories without metadata (metaList[0] = "").
            match = match.Where(cat => cat.MetaList[0] == "" || !cat.MetaList.Except(metaList, comparer).Any());

            // Return the best match
            return match.Any() ? match.First() : null;
        }
    }
}
