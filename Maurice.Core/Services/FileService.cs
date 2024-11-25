using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maurice.Core.Services
{
    public class FileService
    {
        public string GetFileName(string filePath)
        {
            // Extract and return the file name from the file path
            return Path.GetFileName(filePath);
        }
    }
}