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
            //ReadingFile readingFile = new ReadingFile(); 
            //var myData = ReadingFile.InputDataRead();

            var amOfData = ReadingFile.AmountOfData();
            //IntegralCalculation integralCalculation = new IntegralCalculation();

            Console.WriteLine(amOfData);
            const int nol = 0;
            for (int j = nol; j < amOfData; j++)
            {
                var myData = ReadingFile.InputDataRead();


                //var res = IntegralCalculation.IntegCalculate(myData);
                var res = IntegralCalcWithTasks.IntegCalculate(myData);


                Console.WriteLine($"{res[0]} - время");
                Console.WriteLine($"{res[1]} - площадь");

                //myData = ReadingFile.InputDataRead();
                Console.WriteLine();
            }

            //Console.ReadKey();
        }
    }
}
