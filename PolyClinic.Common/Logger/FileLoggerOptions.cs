using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolyClinic.Common.Logger
{
    public class FileLoggerOptions
    {
        public virtual string FilePath { get; set; }

        public virtual string FolderPath { get; set; }
    }
}
