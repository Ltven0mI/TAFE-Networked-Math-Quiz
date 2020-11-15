using System;
using System.Collections.Generic;
using System.Text;

namespace TeacherApp.Services.Interfaces
{
    public interface IFileWriterService
    {
        public void WriteToFile(string filepath, string data);
    }
}
