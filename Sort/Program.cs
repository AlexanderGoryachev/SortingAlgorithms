#define DisplayArray // comment out this line to see the actual time of the array sorting

using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Sort
{
    class Program
    {
        private static Random random = new Random();

        private static bool HasNumbersOnly(string value)
        {
            return !(Regex.IsMatch(value, @"[^0-9]"));
        }

        private static bool visualizeProcess = true;

        static void Main(string[] args)
        {
            if (visualizeProcess)
                Console.SetWindowSize(100, 70);
            

            Console.WriteLine("Enter initial parameters\n");
            Console.WriteLine("Size of array: ");
            var line = Console.ReadLine();

            var size = 30;
            if (!String.IsNullOrEmpty(line) & HasNumbersOnly(line))
                size = int.Parse(line);
            else
                WriteColoredLine("30\n", ConsoleColor.Green);
            
            Console.WriteLine("Minimum random element in the array: ");
            line = Console.ReadLine();

            var minValue = 1;
            if (!String.IsNullOrEmpty(line) & HasNumbersOnly(line))
                minValue = int.Parse(line);
            else
                WriteColoredLine("1\n", ConsoleColor.Green);

            Console.WriteLine("Maximum random element in the array: ");
            line = Console.ReadLine();

            var maxValue = 60;
            if (!String.IsNullOrEmpty(line) & HasNumbersOnly(line))
                maxValue = int.Parse(line);
            else
                WriteColoredLine("60\n", ConsoleColor.Green);

            Console.WriteLine("Enter the number of the type of a sorting algorithm:");
            Console.WriteLine("1. Insertion sort");
            Console.WriteLine("2. Gnome sort");
            Console.WriteLine("3. Bubble sort");
            Console.WriteLine("4. Shaker sort");
            Console.WriteLine("5. Heap sort");
            Console.WriteLine("6. Quick sort");
            line = Console.ReadLine();

            var sortType = 1;
            if (!String.IsNullOrEmpty(line) & HasNumbersOnly(line))
                sortType = int.Parse(line);
            else
                WriteColoredLine("1\n", ConsoleColor.Green);

            var beginSorting = false;
            while (beginSorting == false)
            {
                Console.WriteLine("Visualize the process of sorting? (Y/N) ");
                line = Console.ReadLine();

                if (line == "Y" || line == "y")
                {
                    visualizeProcess = true;
                    beginSorting = true;
                }
                else if (line == "N" || line == "n")
                {
                    visualizeProcess = false;
                    beginSorting = true;
                }
                else
                {
                    WriteColoredLine("Entered value is not valid!", ConsoleColor.Red);
                    Console.WriteLine("Enter \"Y\" or \"N\" for confirm your choise\n");
                }
            }

            Console.Clear();

            var a = GetArrayOfRandomNumbers(size, minValue, maxValue);
            int[] oldArray = new int[a.Count()];
            a.CopyTo(oldArray, 0);

            if (visualizeProcess)
            {
                WriteArray(a);
                Console.WriteLine("\nPress \"Enter\" to start sorting");
                Console.ReadLine();
            }
            else
                Console.WriteLine("Sort begun...");
            

            var sw = new Stopwatch();
            sw.Start();

            switch (sortType)
            {
                case 1:
                    InsertionSort(a);                   // ~ 5.45 Sec - an array of 50 000 random numbers
                    break;
                case 2:
                    GnomeSort(a);                       // ~14 Sec - an array of 50 000 random numbers
                    break;
                case 3:
                    BubbleSort(a);                      // ~16 Sec - an array of 50 000 random numbers
                    break;
                case 4:
                    ShakerSort(a);                      // ~23 Sec - an array of 50 000 random numbers
                    break;
                case 5:
                   HeapSort(a);                         // ~0.04 Sec - an array of 50 000 random numbers
                                                        // ~0.67 Sec - an array of 1 000 000 random numbers
                                                        // ~190 Sec - an array of 100 000 000 random numbers
                    break;
                case 6:
                    QuickSort(a, 0, a.Length - 1);      // ~0.025 Sec - an array of 50 000 random numbers
                                                        // ~0,35 Sec - an array of 1 000 000 random numbers
                                                        // ~44 Sec - an array of 100 000 000 random numbers
                    break;
                default:
                    InsertionSort(a);                   // ~ 5.45 Sec - an array of 50 000 random numbers
                    break;
            }            

            sw.Stop();
            Console.WriteLine("\nSorting complete");

            if (!visualizeProcess)
                Console.WriteLine("\n" + "Elapsed Milliseconds: " + sw.ElapsedMilliseconds);


            //Console.WriteLine("n# Old New \n");
            //for (int i = 0; i < oldArray.Count(); i++)
            //    Console.WriteLine(i + 1 + ". " + oldArray[i].ToString() + "   " + a[i].ToString());

            Console.ReadLine();
        }

        private static int[] GetArrayOfRandomNumbers(int arraySize, int minValue, int maxValue)         
        {
            int[] a = new int[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                a[i] = random.Next(minValue, maxValue);
            }
            return a;
        }

        private static void InsertionSort(int[] a)
        {
            int temp = 0;
            int i = 0, j = 0;

            for (i = 1; i <= (a.Length) - 1; i++)
            {
                temp = a[i];
                j = i - 1;
                while (j >= 0 && a[j] > temp)
                {
                    a[j + 1] = a[j];
                    j = j - 1;
                }
                a[j + 1] = temp;
                           
                if (visualizeProcess)            
                    DisplayArrayWithSleep(a, true, 100);                
            }
        }

        private static void GnomeSort(int[] a)
        {
            int i = 0;
            while (i < a.Length)
            {
                if (i == 0 || a[i - 1] <= a[i]) i++;
                else
                {
                    int tmp = a[i];
                    a[i] = a[i - 1];
                    a[--i] = tmp;
                }
                
                if (visualizeProcess)            
                    DisplayArrayWithSleep(a, true, 100);                
            }
        }

        private static void BubbleSort(int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                for (int j = 0; j < a.Length - 1 - i; j++)
                {
                    if (a[j] > a[j + 1]) 
                    {
                        Swap(a, j, j + 1);
                    }    
                }                
            }
        }

        static int[] ShakerSort(int[] a)
        {
            int beg, end;
            int count = 0;

            for (int i = 0; i < a.Length / 2; i++) //можно переберать за кол-во итераций, в 2 раза меньше
            {                                        //целочисленное деление округляет в меньшую сторону
                beg = 0;
                end = a.Length - 1;

                do
                {
                    count += 2;
                    /* идем спереди */
                    if (a[beg] > a[beg + 1])
                        Swap(a, beg, beg + 1);
                    beg++;//сдвигаем позицию вперед


                    /* идем сзади */
                    if (a[end - 1] > a[end])
                        Swap(a, end - 1, end);
                    end--;//сдвигаем позицию назад
                }
                while (beg <= end);// условия усреднения

            }
            Console.Write("\nКоличество сравнений = {0}\n", count);
            return a;
        }

        /* Поменять элементы местами */
        private static void Swap(int[] a, int i, int j)
        {
            int glass;
            glass = a[i];
            a[i] = a[j];
            a[j] = glass;            
    
            if (visualizeProcess)
                DisplayArrayWithSleep(a, true, 100);            
        }

        private static void HeapSort(int[] a)
        {
            int i;
            int temp;

            for (i = (a.Length / 2) - 1; i >= 0; i--)
            {
                siftDown(a, i, a.Length);
            }

            for (i = a.Length - 1; i >= 1; i--)
            {
                temp = a[0];
                a[0] = a[i];
                a[i] = temp;
                siftDown(a, 0, i - 1);
            }
            
                if (visualizeProcess)            
                    DisplayArrayWithSleep(a, true, 100);
        }

        private static void siftDown(int[] a, int i, int j)
        {
            bool done = false;
            int maxChild;
            int temp;

            while ((i * 2 < j) && (!done))
            {
                if (i * 2 == j)
                    maxChild = i * 2; 
                else if (a[i * 2] > a[i * 2 + 1])
                    maxChild = i * 2;
                else
                    maxChild = i * 2 + 1;

                if (a[i] < a[maxChild])
                {
                    temp = a[i];
                    a[i] = a[maxChild];
                    a[maxChild] = temp;
                    i = maxChild;
                }
                else
                {
                    done = true;
                }
                
                if (visualizeProcess)            
                    DisplayArrayWithSleep(a, true, 100);                
            }
        }

        private static void QuickSort(int[] a, int i, int j)
        {
            if (i < j)
            {
                int q = Partition(a, i, j);
                QuickSort(a, i, q);
                QuickSort(a, q + 1, j);
            }
            
                if (visualizeProcess)            
                    DisplayArrayWithSleep(a, true, 100);                
        }

        private static int Partition(int[] a, int p, int q)
        {
            // int r = a[p];
            int r = a[random.Next(0, q - p) + p]; //
            int i = p - 1;
            int j = q + 1;
            while (true)
            {
                do
                {
                    j--;
                }
                while (a[j] > r);
                do
                {
                    i++;
                }
                while (a[i] < r);
                if (i < j)
                {
                    int tmp = a[i];
                    a[i] = a[j];
                    a[j] = tmp;
                }
                else
                {
                    return j;
                }
                
                if (visualizeProcess)            
                    DisplayArrayWithSleep(a, true, 100);                
            }
        }

        private static void DisplayArrayWithSleep(int[] a, bool clearScreen, int sleepTime)
        {
            if (clearScreen)
                Console.Clear();
            WriteArray(a);
            Thread.Sleep(sleepTime);
        }

        private static void WriteArray(int[] a)
        {
            char pad = '-';
            foreach (int c in a)
            {
                var str = string.Empty;
                Console.WriteLine(str.PadRight(c, pad));
            }
        }

        private static void WriteColoredLine(string s, ConsoleColor newColor)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            Console.WriteLine(s);
            Console.ForegroundColor = oldColor;
        }
    }
}
