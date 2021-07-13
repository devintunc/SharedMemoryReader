using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;

namespace SharedMemoryReader
{
    public class SharedMemory_Reader
    {
        private MemoryMappedFile mappedFile;
        private string MappedFileName;
        private byte MessageCounter;
        private int CharecterLimit;
        public string Message;

        Thread ReadPollThread;
         
        public void Initialize(string FileName)
        {
            MappedFileName = FileName;
            mappedFile = MemoryMappedFile.OpenExisting(FileName);
            MessageCounter = 0;
            CharecterLimit = 10;            
        }
        public void Start()
        {
            ReadPollThread = new Thread(ReadPoll);
            ReadPollThread.Start();
        }
        public void Stop()
        {
            if (ReadPollThread.IsAlive)
                ReadPollThread.Abort();            
        }
        public void ReadPoll()
        {
            for(; ; )
            {
                ReadMessage();
            }
        }
        private void ReadMessage()
        {
            using (MemoryMappedViewStream view = mappedFile.CreateViewStream())
            {
                byte[] ReceivedBytes = new byte[CharecterLimit+1];
                view.Read(ReceivedBytes, 0, ReceivedBytes.Length);
                byte[] MessageBytes = TransferArray(ReceivedBytes, CharecterLimit);
                string message = Encoding.Default.GetString(MessageBytes);

                if (isNewMessage(ReceivedBytes[10]))
                {
                    Console.WriteLine(message);
                    Message = message;
                    MessageCounter = ReceivedBytes[10];
                } 
            }
        }
        private bool isNewMessage(byte Count)
        {
            if (MessageCounter == Count)
                return false;
            else
                return true;
        }
        private byte[] TransferArray(byte[] Array,int count)
        {
            byte[] Temporary = new byte[count];
            for (int i = 0; i < count; i++)
            {
                Temporary[i] = Array[i];
            }
            return Temporary;
        }
    }
}
