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
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace TestProjectAsyncUITimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker worker;
        public delegate void OnTaskCompleteDelegate(string text);
        Stopwatch stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
            //test_task = new TestTask(this);
            InitializeWorker();
        }

        private void InitializeWorker() {

            worker = new BackgroundWorker();

            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
 
            //worker.DoWork += worker_DoWork;
            //worker.ProgressChanged += worker_ProgressChanged;

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
        }

        private void enableButtons()
        {

                button1.IsEnabled = true;
                button2.IsEnabled = true;
        }

        private void disableButtons()
        {
            button1.IsEnabled = false;
            button2.IsEnabled = false;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
             OnTaskCompleteDelegate callback = new OnTaskCompleteDelegate(OnTaskComplete);

            disableButtons();

            stopwatch.Reset();
            stopwatch.Start();

            Task.Run(() =>
            {
                ThreadPoolProcess();

                stopwatch.Stop();

                int current_thread_id = Thread.CurrentThread.ManagedThreadId;

                //Accessing UI
                this.Dispatcher.Invoke(() =>
                {
                    callback("Task completed successfully\n Task thread ID: " + current_thread_id +"\n Watch ticks: " + stopwatch.ElapsedTicks);
                    
                });

                //Console.WriteLine("Task thread ID: {0}", Thread.CurrentThread.ManagedThreadId);
            });
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            OnTaskCompleteDelegate callback = new OnTaskCompleteDelegate(OnTaskComplete);

            disableButtons();

            stopwatch.Reset();
            stopwatch.Start();

            //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolProcess));
            ThreadPool.QueueUserWorkItem(o=>
            {
                ThreadPoolProcess();

                stopwatch.Stop();

                //Accessing UI
                this.Dispatcher.Invoke(() =>
                {
                    callback("Threadpooling completed successfully" + "\n Watch ticks: " + stopwatch.ElapsedTicks);
                });
            });  
        }

        public void OnTaskComplete(string text)
        {
            MessageBox.Show(text, "BLA BLA BLA", MessageBoxButton.OK, MessageBoxImage.Information);
            enableButtons();
        }

        private void ThreadPoolProcess()
        {
            for (int i = 0; i < Define.TIMES; i++)
            {
                new Printer(this);
            }
        }

        private void start_pb_Click(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy == false){

                disableButtons();

                start_pb.IsEnabled = false;
                cancel_pb.IsEnabled = true;

                stopwatch.Reset();
                stopwatch.Start();

                worker.RunWorkerAsync();
            }
        }

        private void cancel_pb_Click(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy){

                worker.CancelAsync();

                stopwatch.Reset();
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < Define.TIMES; i++)
            {
                (sender as BackgroundWorker).ReportProgress((int)((double)i/Define.TIMES*100));

                //Thread.Sleep(100);

                new Printer(this);

                if (worker.CancellationPending)
                {
                    // Set the Cancel flag so that the WorkerCompleted event knows that the process was cancelled.
                    e.Cancel = true;
                    worker.ReportProgress(0);

                    //Accessing UI Thread
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        start_pb.IsEnabled = true;
                        cancel_pb.IsEnabled = false;
                    });
                    return;
                }
            }
            worker.ReportProgress(100);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pb.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e){
            
            if (e.Cancelled){
                pb_text.Foreground = Brushes.Red;
                pb_text.Text = "Job Cancelled...";

                enableButtons();

            }else if (e.Error != null){
                pb_text.Foreground = Brushes.Red;
                pb_text.Text = "Error occured!";

            }else{
                pb_text.Foreground = Brushes.Green;
                pb_text.Text = "Job Completed!";

                OnTaskComplete("Threadpooling completed successfully" + "\n Watch ticks: " + stopwatch.ElapsedTicks);
            }

            start_pb.IsEnabled = true;
            cancel_pb.IsEnabled = false;
        }
    }
}
