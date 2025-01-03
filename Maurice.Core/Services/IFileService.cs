using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maurice.Core.Services
{
    interface IFileService
    {
        public IDictionary<string, string> ParseXml(string filePath);
    }
}
