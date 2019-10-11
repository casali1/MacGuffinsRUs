using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewMacGuffinsRUs
{
    public class Category
    {
        public string ProductCode { get; set; }
        public string Metadata { get; set; }
        public string Name { get; set; }
        public string[] MetaList { get; set; }
        public string Path { get; set; }

        public Category(string path)
        {
            this.Path = path;

            // GetFileName returns just the name from a path, even for directories
            this.Name = System.IO.Path.GetFileName(path);
            string[] splitName = StringFuncs.SplitName(Name);
            ProductCode = splitName[0];
            Metadata = splitName.ElementAtOrDefault(1); // sets to null if there is no other metadata
            if (Metadata is null)
            {
                MetaList = new string[] { "" }; // set to empty string to do intersection count
            }
            else
            {
                MetaList = StringFuncs.SplitMetadata(Metadata);
            }

        }
    }
}
