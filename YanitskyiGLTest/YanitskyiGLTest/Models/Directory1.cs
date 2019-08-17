using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Directory
    {
        public string Name { get; set; }
        public string DataOfCreating { get; set; }
        public List<File> Files { get; set; }
        public List<Directory> Children { get; set; }
        public override string ToString()
        {
            return $"Name: {Name}; Data of creating: {DataOfCreating}; File and Children count: ";//{(Files.Count + Children.Count).ToString()}"
        }
    }
}