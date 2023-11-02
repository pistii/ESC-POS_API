using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
// See https://aka.ms/new-console-template for more information

namespace testPrinter // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start printing...");
            PrinterSetup setup = new PrinterSetup();

            setup.SetupPrinter();
        }
    }
}