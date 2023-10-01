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

        private static int amountOfData = 0;

        private static List<string> lines = new List<string>();

        static ReadingFile()
        {
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

            amountOfData = lines.Count / rowData;
        }

        /*
            8 - c какой строки данные (эта строка нулевая)
            leftBorder(a);rightBorder(b)	- 0
            numberOfSegments(M)
            numberOfRoots(N)
            coNormion(K)
            root1;root2;root...;rootN	- 5
            Fluxes
            ---------------------
            2;7,5
            225000000
            4
            1
            3;5;6;7
            1
        */

        public static int AmountOfData()
        {
            return amountOfData;
        }

        public static List<string> InputDataRead()
        {
            List<string> myData = new List<string>();

            for (int i = rowData; i < countLinesData + rowData; i++)
            {
                if (i < lines.Count)
                    myData.Add(lines[i]);
                //else
                //    return null;
            }

            rowData += (countLinesData + 1);

            return myData;
        }

    }
}
