using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab1
{
    internal class ReadingFile
    {
        private static string pathInputData;
        private static int rowData = 0;
        private static int countLinesData = 0;

        private static List<string> lines = new List<string>();

        static ReadingFile()
        {
            //Console.WriteLine("начало конструктора");
            pathInputData = Path.Combine(Environment.CurrentDirectory, "InputData.txt");

            using (StreamReader reader = new StreamReader(pathInputData, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            string[] wordsFirstRow = lines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            rowData = Convert.ToInt32(wordsFirstRow[0]);

            for (int i = 1; !char.IsNumber(lines[i][0]) && (lines[i][0] != '-'); i++)
            {
                countLinesData++;
            }
            //Console.WriteLine("конец конструктора\n");
        }

        /*
            8 - c какой строки данные (эта строка нулевая)
            leftBorder(a),rightBorder(b)
            numberOfSegments(M)
            numberOfRoots(N)
            coNormion(K)
            root1,root2,root...,rootN
            Fluxes
            ---------------------
            1,10
            1000000
            4
            1
            2,3,6,7
            1
        */

        public static List<string> InputDataRead()
        {
            //Console.WriteLine("начало метода");
            //Console.WriteLine($"\n{countLinesData} - количество строк данных");
            //Console.WriteLine($"{rowData} - c какой строки идут данные\n");

            List<string> myData = new List<string>();

            for (int i = rowData; i < countLinesData + rowData; i++)
            {
                if (i < lines.Count)
                    myData.Add(lines[i]);
            }

            rowData += (countLinesData + 1);
            //Console.WriteLine("конец метода\n");
            return myData;
        }

    }
}
