using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestProjectAsyncUITimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        TestTask test_task = null;

        public MainWindow()
        {
            InitializeComponent();
            test_task = new TestTask(this);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //this.textblock1.Text = "geiaaaa";
           
            //older version
            //Task.Factory.StartNew(() => test_task.startCalculation(this));

            //Task tt = test_task.starter();
            //tt.Wait();

            Task.Run(() => test_task.starter());
        }
    }
}
