using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatiN;

namespace HtmlSnapshotter
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //for debug testing
            if (System.Diagnostics.Debugger.IsAttached)
            {
                args = new string[]{
                    "http://localhost:8000",
                    @"C:\web\weeksdevconsulting\snapshots\"
                };
            }
            var startingAddress = args[0];
            var outputFolder = args[1];
            HtmlSnapshot shotter = new HtmlSnapshot(startingAddress, outputFolder, 500);
            shotter.Shoot();
            shotter.Close();
        }
    }
}
