using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceWriter
{
    public static class Log
    {
        public static void writeEventLog(String Message)
        {
            StreamWriter sw = null;
            try
            {
                string Date = System.DateTime.Now.ToString("dd-MM-yyyy");
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFolder\\WindowsService" + Date + ".txt", true);
                    sw.WriteLine(DateTime.Now.ToString() + " : " + Message);
                sw.Flush();
                sw.Close();

            }
            catch (Exception ex)
            {

            }
        }

    }
}
