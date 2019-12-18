using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TestProjectAsyncUITimer
{
    class TestTask
    {

        MainWindow main_window = null;

        public TestTask(Object obj) {

            this.main_window = (MainWindow)obj;
        }

        public void starter() {

            computation();
        }

        private void computation() {

            for (int i = 0; i < 1000000000; i++)
            {

                int str = i % 100000;

                if (str == 0)
                {
                    //If app closes before timer ends
                    //context become null and exception is thrown
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.main_window.textblock1.Text = i.ToString();
                    });

                }
                //Dispatcher.BeginInvoke(new Action(() =>
                //{
                //form1.textblock1.Text = str.ToString();
                //}), DispatcherPriority.Background);
            }
        }
    }
}
