using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace TaskTest {
    using System;
    using System.IO;
    using System.Threading.Tasks;

    //    start main 当前线程 1
    //    HandleFile enter 当前线程 5
    //    Please wait patiently while I do something important.
    //        call HandleFileAsync : 当前线程 5
    //        after task.Wait() :当前线程 1
    //        HandleFile exit
    //        Count: 1837
    //        after await HandleFileAsync 当前线程 5
    class Program {
        static void Main() {
            // Create task and start it.
            // ... Wait for it to complete.
            Console.WriteLine("start main 当前线程 {0}", Thread.CurrentThread.ManagedThreadId.ToString());
            Task task = new Task(ProcessDataAsync);
            task.Start();
            task.Wait();
            Console.WriteLine("after task.Wait() :当前线程 {0}", Thread.CurrentThread.ManagedThreadId.ToString());
            Console.ReadLine();
        }

        static async void ProcessDataAsync() {
            // Start the HandleFile method.
            Task<int> task = HandleFileAsync("Program.cs");

            // Control returns here before HandleFileAsync returns.
            // ... Prompt the user.
            Console.WriteLine("Please wait patiently " +
            "while I do something important.");

            Console.WriteLine("call HandleFileAsync : 当前线程 {0}", Thread.CurrentThread.ManagedThreadId.ToString());

            // Wait for the HandleFile task to complete.
            // ... Display its results.
            int x = await task;

            Console.WriteLine("Count: " + x);

            Console.WriteLine("after await HandleFileAsync 当前线程 {0}", Thread.CurrentThread.ManagedThreadId.ToString());

        }

        static async Task<int> HandleFileAsync(string file) {
            Console.WriteLine("HandleFile enter 当前线程 {0}", Thread.CurrentThread.ManagedThreadId.ToString());

            int count = 0;

            // Read in the specified file.
            // ... Use async StreamReader method.
            using (StreamReader reader = new StreamReader(file)) {
                string v = await reader.ReadToEndAsync();

                // ... Process the file data somehow.
                count += v.Length;

                // ... A slow-running computation.
                //     Dummy code.
                for (int i = 0; i < 1000000000000; i++) {
                    int x = v.GetHashCode();
                    if (x == 0) {
                        count--;
                    }
                }
            }
            Console.WriteLine("HandleFile exit");
            return count;
        }
    }
}
