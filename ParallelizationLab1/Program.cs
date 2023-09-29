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
            ReadingFile readingFile = new ReadingFile();
            var myData = ReadingFile.InputDataRead();

            //IntegralCalculation integralCalculation = new IntegralCalculation();

            const int nol = 0;
            for (int j = nol; j < 7; j++)
            {
                var res = IntegralCalculation.IntegCalculate(myData);

                Console.WriteLine($"{res[0]} - время");
                Console.WriteLine($"{res[1]} - площадь");

                myData = ReadingFile.InputDataRead();
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
