using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Text;

namespace SharedMemoryReader
{


    class Program
    {
        
        static void Main(string[] args)
        {
            SharedMemory_Reader reader = new SharedMemory_Reader();
            reader.Initialize("Medium");

            reader.Start();
        }        
        


    }
}
