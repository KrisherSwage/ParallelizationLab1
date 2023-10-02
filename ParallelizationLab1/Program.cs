using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParallelizationLab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //DataFromFile();
            DataFromAlgorithm();


        }

        static void DataFromAlgorithm()
        {
            int numberOfExperiment = 10;

            var startData = ReadingFile.InputDataRead();
            var M = startData[1];
            var fluxes = startData[5];

            var myDataForFile = new List<string>();
            var bufferForAverageTime = new List<double>();

            myDataForFile.Add(M); //0
            myDataForFile.Add(fluxes); //1
            myDataForFile.Add(""); //2

            for (int i = 0; i < numberOfExperiment; i++)
            {
                var res = IntegralCalcWithTasks.IntegCalculate(startData);

                bufferForAverageTime.Add(Convert.ToDouble(res[0]));

                myDataForFile.Add(Convert.ToString(res[0]));

                double averageTime = bufferForAverageTime.Sum() / bufferForAverageTime.Count;
                myDataForFile[2] = Convert.ToString(averageTime);

                Console.WriteLine($"i = {i}; {res[0]} - время; {res[1]} - площадь");
            }

            WritingFile.CreateCSV(myDataForFile);
        }

        static void DataFromFile()
        {
            var amOfData = ReadingFile.AmountOfData();
            Console.WriteLine(amOfData);

            const int nol = 0;
            for (int j = nol; j < amOfData; j++)
            {
                var myData = ReadingFile.InputDataRead();

                var res = IntegralCalcWithTasks.IntegCalculate(myData);

                Console.WriteLine($"{res[0]} - время");
                Console.WriteLine($"{res[1]} - площадь");

                Console.WriteLine();
            }
        }
    }
}
