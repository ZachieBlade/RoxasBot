using System;
using System.Collections.Generic;
using System.Text;

namespace RoxasBot
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var b = new Bot())
            {
                b.RunAsync().Wait();
            }
        }
    }
}