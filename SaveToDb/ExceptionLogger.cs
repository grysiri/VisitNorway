using System;
using System.IO;

namespace SaveToDb
{
    public class ExceptionLogger
    {
        public void LogException(Exception e)
        {
            StreamWriter stream = File.AppendText("exceptions.txt");                       

            stream.Write("\r\nLogget exception: ");
            stream.WriteLine("{0}",DateTime.Now.ToUniversalTime());
            stream.WriteLine(e.ToString());
            stream.WriteLine("\n--------------------------------------\n");
            stream.Close();            
        }

    }
}
