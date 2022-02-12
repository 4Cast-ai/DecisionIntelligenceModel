using System;
using System.Diagnostics;
using System.Threading;

namespace Infrastructure.Helpers
{
    public partial class Util
    {
        public static void Measure(Action body)
        {
            DateTime startTime = DateTime.Now;
            body();
            Debug.WriteLine($"{DateTime.Now - startTime} : {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
    
