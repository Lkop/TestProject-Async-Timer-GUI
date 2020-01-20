using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectAsyncUITimer {

    public class FilePrinter {

        string filename;
        StreamWriter sw;

        public FilePrinter(string filename) {

            this.filename = filename;
        }

        public void create() {

            //using open and closes the file automatically
            using (File.Create(filename));
        }

        public void append(int num) {

            using (sw = File.AppendText(filename))
            {
                sw.WriteLine(num);
            }
        }
    }
}
