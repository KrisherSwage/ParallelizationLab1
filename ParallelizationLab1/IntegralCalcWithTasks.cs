using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelizationLab1
{
    internal class IntegralCalcWithTasks
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

        /// <summary>
        /// Список еще работающих потоков
        /// </summary>
        private static List<Thread> threadsList = new List<Thread>();

        public static object block = new object();

        /// <summary>
        /// Экземпляр класса Arearesul. Предназначен для безопасной работы с типом Double
        /// </summary>
        static AreaResult areaRes = new AreaResult();


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

        public static double[] IntegCalculate(List<string> myData)
        {

            CalcIntConstr(myData); //вызываем каждый раз для новых данных

            Stopwatch clock = new Stopwatch();

            double timeResult = 0.0; //время подсчета площади

            clock.Start(); //запускаем подсчет времени
            clock.Stop();

            clock.Start();

            double arRes = ParalStart();

            clock.Stop(); //останавливаем подсчет времени

            timeResult = clock.ElapsedMilliseconds / 1000.0;

            areaRes.Area = 0.0;
            clock.Reset();
            threadsList.Clear();

            return new double[] { timeResult, arRes };
        }

        private static double ParalStart()
        {
            Task<double>[] tasks = new Task<double>[fluxes];

            for (int i = 0; i < fluxes; i++)
            {
                double localAlpha = alpha + i * ((beta - alpha) / fluxes);
                double localBeta = localAlpha + ((beta - alpha) / fluxes);

                //Console.WriteLine($"{localAlpha} {localBeta}");
                tasks[i] = Task.Run(() => ParalCalculate(localAlpha, localBeta));
            }

            Task.WaitAll(tasks);

            double totalArea = 0.0;
            foreach (var task in tasks)
                totalArea += task.Result;

            return totalArea;
        }

        private static double ParalCalculate(double localAlpha, double localBeta)
        {
            double result = 0.0;

            for (double i = localAlpha; i <= localBeta; i += lengthStrip) // alpha и beta МЕНЯЮТСЯ
            {
                double ii = i + lengthStrip; // lengthStrip не мняется

                double yn0 = yFromX(i, x1, x2, x3, x4); // i в этом методе появляется
                double yn1 = yFromX(ii, x1, x2, x3, x4);

                double sn = HeightStrip(yn0, yn1) * lengthStrip;

                result += sn;
            }
            return result;
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
