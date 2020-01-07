using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectAsyncUITimer{

    class Printer{

        public Printer(MainWindow main_window) {

            int x = 0;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Int32.TryParse(main_window.textblock2.Text, out x);

                main_window.textblock2.Text = (x + 1).ToString();
            });
        }
    }
}
