using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TeacherApp.Services.Interfaces;

namespace TeacherApp.Services
{
    public class FileWriterService : IFileWriterService
    {
        public void WriteToFile(string filepath, string data)
        {
            File.WriteAllText(filepath, data);
        }
    }
}
