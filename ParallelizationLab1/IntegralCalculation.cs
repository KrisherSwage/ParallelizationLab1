using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ParallelizationLab1
{
    internal class IntegralCalculation
    {
        private static double alpha;
        private static double beta;

        private static double M;
        private static double N;
        private static double K;

        private static double x1;
        private static double x2;
        private static double x3;
        private static double x4;

        private static int fluxes;

        private static double lengthStrip;

        private static double squareresult = 0.0;


        private static void CalcIntConstr(List<string> myData)
        {
            string[] borders = myData[0].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            alpha = Convert.ToDouble(borders[0]);
            beta = Convert.ToDouble(borders[1]);

            M = Convert.ToDouble(myData[1]);
            N = Convert.ToDouble(myData[2]);
            K = Convert.ToDouble(myData[3]);

            string[] roots = myData[4].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            x1 = Convert.ToDouble(roots[0]);
            x2 = Convert.ToDouble(roots[1]);
            x3 = Convert.ToDouble(roots[2]);
            x4 = Convert.ToDouble(roots[3]);

            fluxes = Convert.ToInt32(myData[5]);

            lengthStrip = Convert.ToDouble((beta - alpha) / M);
            //Console.WriteLine($"{lengthStrip:0.0000000000}");
        }

        /*
            8 - c какой строки данные (эта строка нулевая)
            leftBorder(a),rightBorder(b) - 0
            numberOfSegments(M) - 1
            numberOfRoots(N) - 2
            coNormion(K) - 3
            root1,root2,root...,rootN - 4
            Fluxes - 5
            ---------------------
            1,10
            1000000
            4
            1
            2,3,6,7
            1
        */

        public static double[] IntegCalculate(List<string> myData)
        {
            CalcIntConstr(myData);

            Stopwatch clock = new Stopwatch();

            double timeResult = 0.0;
            squareresult = 0;

            clock.Start();

            squareresult = AsyncCalculate();

            clock.Stop();

            timeResult = clock.ElapsedMilliseconds / 1000.0;

            return new double[] { timeResult, squareresult };
        }

        private static double AsyncCalculate()
        {
            double localResult = 0.0;
            for (double i = alpha; i <= beta; i += lengthStrip) // alpha и beta не меняются
            {
                double ii = i + lengthStrip; // lengthStrip не мняется

                double yn0 = yFromX(i, x1, x2, x3, x4); // i в этом методе появляется
                double yn1 = yFromX(ii, x1, x2, x3, x4);

                double sn = HeightStrip(yn0, yn1) * lengthStrip;

                localResult += sn;
            }

            return localResult;
        }

        private static double yFromX(double x, double a, double b, double c, double d)
        {
            return (x - a) * (2 * x - b) * (x - c) * (x - d) + 1;
        }

        private static double HeightStrip(double yn0, double yn1)
        {
            return (yn0 + yn1) / 2;
        }

    }
}
