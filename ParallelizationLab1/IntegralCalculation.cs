using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ParallelizationLab1
{
    internal class IntegralCalculation
    {
        private static double alpha;
        private static double beta;

        /// <summary>
        /// Количество отрезков
        /// </summary>
        private static double M;
        private static double N;
        private static double K;

        private static double x1;
        private static double x2;
        private static double x3;
        private static double x4;

        /// <summary>
        /// Количество потоков
        /// </summary>
        private static int fluxes;

        /// <summary>
        /// Количество расчетов на один поток
        /// </summary>
        private static double optionPerFlux;

        /// <summary>
        /// Длина отрезка. Больше отрезков - меньше длина
        /// </summary>
        private static double lengthStrip;

        /// <summary>
        /// Итоговое значение площади
        /// </summary>
        private static double areaResult = 0.0;

        private static List<Thread> threadsList = new List<Thread>();

        public static object block = new object();


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

            optionPerFlux = M / fluxes;

            lengthStrip = Convert.ToDouble((beta - alpha) / M);
            //Console.WriteLine($"{lengthStrip:0.0000000000}");
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
            ---------------------
            2;7,5
            225000000
            4
            1
            3;5;6;7
            10
        */


        public static double[] IntegCalculate(List<string> myData)
        {

            CalcIntConstr(myData); //вызываем каждый раз для новых данных

            Stopwatch clock = new Stopwatch();

            double timeResult = 0.0; //время подсчета площади
            areaResult = 0;

            clock.Start(); //запускаем подсчет времени

            ParalStart();

            ThreadExtension.WaitAll(threadsList); //бред... работает не так, как я хочу, а так, как я не понимаю

            clock.Stop(); //останавливаем подсчет времени

            double arRes = areaResult;
            areaResult = 0;
            timeResult = clock.ElapsedMilliseconds / 1000.0;

            return new double[] { timeResult, arRes };
        }

        private static void ParalStart()
        {
            //запаковываем
            Borders borders = new Borders();
            borders.leftBorder = alpha;
            borders.rightBorder = alpha + (lengthStrip * optionPerFlux);

            //List<Task> tasks = new List<Task>();
            //tasks.Add(Task.Run(Method_Monte_Karlo));
            //await Task.WhenAll(tasks);

            for (int i = 0; i < fluxes; i++)
            {
                StartNewFlux(borders);

                Thread.Sleep(100);
                borders.leftBorder = borders.rightBorder;
                borders.rightBorder += lengthStrip * optionPerFlux;
            }

            borders.leftBorder = 0;
            borders.rightBorder = 0;
        }

        private static void StartNewFlux(object bor)
        {
            ParameterizedThreadStart paramParCalThread = new ParameterizedThreadStart(ParalCalculate);
            Thread thread = new Thread(paramParCalThread);
            threadsList.Add(thread);
            thread.Start(bor);
        }

        private static void ParalCalculate(object pocket)
        {
            //распаковываем
            Borders borders = pocket as Borders;
            double localAlpha = borders.leftBorder;
            double localBeta = borders.rightBorder;
            //Console.WriteLine($"localAlpha - {localAlpha}  localBeta - {localBeta}");

            for (double i = localAlpha; i <= localBeta; i += lengthStrip) // alpha и beta МЕНЯЮТСЯ
            {
                double ii = i + lengthStrip; // lengthStrip не мняется

                double yn0 = yFromX(i, x1, x2, x3, x4); // i в этом методе появляется
                double yn1 = yFromX(ii, x1, x2, x3, x4);

                double sn = HeightStrip(yn0, yn1) * lengthStrip;

                areaResult += sn;

                //Console.WriteLine($"{i:0.0000000000} {localBeta:0.0000000000} {Thread.CurrentThread.GetHashCode()}");
            }
            //if (localBeta == beta)
            //{
            //    threadsList[threadsList.Count - 1].Join(); //блокирует так, что ответ неверный
            //}

            //Console.WriteLine($"\nParalCalculate закончилось - {localBeta} - {Thread.CurrentThread.GetHashCode()}");
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
