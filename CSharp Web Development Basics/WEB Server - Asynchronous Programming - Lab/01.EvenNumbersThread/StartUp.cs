using System;
using System.Threading;
using System.Threading.Tasks;

namespace _01.EvenNumbersThread
{
    class StartUp
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string[] input = Console.ReadLine().Split(" ");

                int startNumber = int.Parse(input[0]);
                int endNumber = int.Parse(input[1]);

                var thread = new Thread(() => PrintEvenNumbers(startNumber, endNumber));
                thread.Start();
                thread.Join();

                Console.WriteLine("Thread finished work");
            }
        }

        private static void PrintEvenNumbers(int startNumber, int endNumber)
        {
            for (int i = startNumber; i < endNumber; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}
