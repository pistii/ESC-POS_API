using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using ESCPOS_NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testPrinter
{
    internal class PrinterSetup
    {

        static ImmediateNetworkPrinter printer;

        public async Task SetupPrinter()
        {
            await CreateNew();
        }

        public async Task CreateNew()
        {
            try
            {
                Console.WriteLine("Setup connection");
                var hostnameOrIp = "192.168.88.178"; //"192.168.88.178"; //10.10.100.254
                var port = 9100;
                var printer = new ImmediateNetworkPrinter(new ImmediateNetworkPrinterSettings() { 
                    ConnectionString = $"{hostnameOrIp}:{port}", 
                    PrinterName="WIFI Printer", 
                    ConnectTimeoutMs=60000, 
                    SendTimeoutMs=30000, 
                    ReceiveTimeoutMs=30000 });


                Console.WriteLine("create new printer");

                var e = new EPSON();
                
                var generatedCommands = new List<byte>();

                Console.WriteLine("add the new commands...");

                var commands = ByteSplicer.Combine(e.PrintLine("Teszt szöveg"));
                generatedCommands.AddRange(commands);

                var gc = generatedCommands.ToArray();
                Console.WriteLine("Printer online:" + printer.GetOnlineStatus(e).Result);
                 await printer.WriteAsync(commands); //WriteAsync az async művelet a hálózati használat miatt

                Console.WriteLine("End of write...");


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();

        }
    }
}
