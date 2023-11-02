using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ESC_POS_API
{
    public class PrinterSetup
    {
        //Fő könyvtár: https://github.com/lukevp/ESC-POS-.NET/tree/master
        //Tesztelve:   https://github.com/roydejong/EscPosEmulator
        ImmediateNetworkPrinter printer;
        public PrinterSetup()
        {
            var hostnameOrIp = "192.168.88.178"; //"192.168.88.178"; //10.10.100.254
            var port = 9100;
            printer = new ImmediateNetworkPrinter(new ImmediateNetworkPrinterSettings()
            {
                ConnectionString = $"{hostnameOrIp}:{port}",
                PrinterName = "WIFI Printer",
                ConnectTimeoutMs = 60000,
                SendTimeoutMs = 30000,
                ReceiveTimeoutMs = 30000
            });
        }

        public async Task CreateNew(List<Items> items)
        {
            var e = new EPSON();
            var generatedCommands = new List<byte>();
            int total = 0;
            

            var header = ByteSplicer.Combine(
                e.CenterAlign(),
                e.PrintLine("-----------  Receipt  -----------"),
                e.FeedLines(2)
            );
            generatedCommands.AddRange(header);


            foreach (var item in items)
            {
                // Formázzuk az item nevét
                string leftAlignedName = item.ItemName.PadRight(30);
                string rightAlignedPrice = item.Price.ToString().PadLeft(10);

                total += item.Price * item.Quantity;
                // Kiírjuk az item nevét annyiszor, amennyi a mennyiség
                for (int i = 0; i < item.Quantity; i++)
                {
                    // Létrehozzuk az ESC/POS parancsokat az item nevének és árának kiírására
                    
                      var command = ByteSplicer.Combine(
                        e.LeftAlign(),
                        e.PrintLine(leftAlignedName),
                        e.RightAlign(),
                        e.PrintLine(rightAlignedPrice)
                    );
                    
                    generatedCommands.AddRange(command);
                }
            }

            //Hozzáadjuk a teljes összeget a végén
            var totalPrice = ByteSplicer.Combine(
                e.FeedLines(1),
                e.SetBarWidth(BarWidth.Thin),
                e.SetStyles(PrintStyle.Bold),
                e.LeftAlign(),
                e.SetBarWidth(BarWidth.Default),
                e.PrintLine("Total: "),
                e.RightAlign(),
                e.PrintLine(total.ToString()),
                e.SetStyles(PrintStyle.None),
                e.FeedLines(1)
                );

            var kelt = ByteSplicer.Combine(
                e.FeedLines(2),
                e.PrintLine("Kelt: "),
                e.RightAlign(),
                e.PrintLine(DateTime.Now.ToString()),
                e.FeedLines(2));

            generatedCommands.AddRange(totalPrice);
            generatedCommands.AddRange(kelt);
            generatedCommands.AddRange(e.FullCutAfterFeed(1));
            await SendToPrinter(generatedCommands);
        }

        public async Task SendToPrinter(List<byte> commands)
        {
            var gc = commands.ToArray();
            await printer.WriteAsync(
                ByteSplicer.Combine(gc));  //WriteAsync az async művelet a hálózati használat miatt
        }
    }
}
