using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab1
{
    internal class WritingFile
    {
        public static void CreateCSV(List<string> myData)
        {
            string pathWriteData = Path.Combine(Environment.CurrentDirectory, "ParallionLab1.csv");

            using (StreamWriter sw = new StreamWriter(pathWriteData, true, System.Text.Encoding.UTF8))
            {
                for (int i = 0; i < myData.Count; i++)
                {
                    sw.WriteLine($"{myData[i]};");
                }
            }
        }

    }
}
