using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPLDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var checkList = new List<int>() { 1, 2, 3, 45, 5, 546, 46, 546, 46, 5465, 44654, 6546, 465, 46, 4, 654, 564, 654, 654, 64, 65465, 456, 465, 465, 4654, 4, 6465, 456, 46, 465, 4564, 65, 465, 4564, 65465, 465, 58 };
            Parallel.ForEach(checkList, (i) => Console.WriteLine($"i = {i}"));

            Parallel.For(0, 100, (i) => Console.WriteLine($"i = {i}"));

            var t1 = Task.Factory.StartNew(() => DoSomeImportantTask(1, 1500)).ContinueWith((prevTask) => DoSomeOtherImportantTask(1, 1500));
            var t2 = Task.Factory.StartNew(() => DoSomeImportantTask(2, 3000));
            var t3 = Task.Factory.StartNew(() => DoSomeImportantTask(3, 1000));

            Task[] myTasks = new Task[] { t1, t2, t3 };
            Task.WaitAll(myTasks);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Doing some other taks");
                Console.WriteLine($"i = {i}");
                Thread.Sleep(250);
            }

            try
            {
                var source = new CancellationTokenSource();
                var taskCanBeCancelled = Task.Factory.StartNew(() => DoSomeImportantTask(4, 1500, source.Token)).ContinueWith((prevTask) => DoSomeOtherImportantTask(4, 1500, source.Token));
                source.Cancel();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
            }

            Console.WriteLine("Press any key to Exit");
            Console.ReadKey();
        }

        private static void DoSomeImportantTask(int id, int sleepTime)
        {
            Console.WriteLine($"Task {id} is begining");
            Thread.Sleep(sleepTime);
            Console.WriteLine($"Task {id} has completed");
        }

        private static void DoSomeOtherImportantTask(int id, int sleepTime)
        {
            Console.WriteLine($"Task {id} is begining more work");
            Thread.Sleep(sleepTime);
            Console.WriteLine($"Task {id} has completed more work");
        }

        private static void DoSomeImportantTask(int id, int sleepTime, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine($"Task {id} is cancelled");
                token.ThrowIfCancellationRequested();
            }
            Console.WriteLine($"Task {id} is begining");
            Thread.Sleep(sleepTime);
            Console.WriteLine($"Task {id} has completed");
        }

        private static void DoSomeOtherImportantTask(int id, int sleepTime, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine($"Task {id} is cancelled");
                token.ThrowIfCancellationRequested();
            }
            Console.WriteLine($"Task {id} is begining more work");
            Thread.Sleep(sleepTime);
            Console.WriteLine($"Task {id} has completed more work");
        }


    }
}
