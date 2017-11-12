using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace PrimeNumber
{
    public partial class Form1 : Form
    {
        private readonly Timer _timer = new Timer();
        private readonly List<DataControl> _futures = new List<DataControl>();

        private sealed class DataControl
        {
            public Task CurrentTask { get; }
            public Label Status { get; }
            public Label Result { get; }
            public ProgressBar Progress { get; }
            public Button Cancel { get; }
            public Tuple Data { get; } 

            public DataControl(Task task, Label status, Label result, ProgressBar progress, Button cancel, Tuple data)
            {
                CurrentTask = task;
                Status = status;
                Result = result;
                Progress = progress;
                Cancel = cancel;
                Data = data;
            }
        }

        private sealed class Tuple
        {
            public int CurrentProgress { get; set; }
            public CancellationTokenSource CancelTokenSource { get; }
            public CancellationToken Token { get; }
            public int Result { get; set; }

            public Tuple(int currentProgress, CancellationToken token, CancellationTokenSource cancelTokenSource)
            {
                CurrentProgress = currentProgress;
                Token = token;
                CancelTokenSource = cancelTokenSource;
            }
        }

        public Form1()
        {
            InitializeComponent();
            _timer.Interval = 10;
            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            foreach (var f in _futures)
            {
                if (f.Data.Token.IsCancellationRequested)
                {
                    f.Status.Text = @"Cancel";
                    f.Progress.Visible = f.Result.Visible = f.Cancel.Visible = false;
                }
                else
                {
                    switch (f.CurrentTask.Status)
                    {
                        case TaskStatus.Running:
                            f.Cancel.Visible = f.Progress.Visible = true;
                            f.Progress.Value = f.Data.CurrentProgress;
                            f.Status.Text = @"Running";
                            break;
                        case TaskStatus.WaitingToRun:
                            f.Cancel.Visible = true;
                            f.Progress.Visible = f.Result.Visible = false;
                            f.Status.Text = @"Waiting";
                            break;
                        case TaskStatus.RanToCompletion:
                            f.Result.Text = f.Data.Result.ToString();
                            f.Cancel.Visible = f.Progress.Visible = false;
                            f.Result.Visible = true;
                            f.Status.Text = @"Finish";
                            break;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var value = Convert.ToInt32(numericUpDown1.Text);
            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            var data = new Tuple(0, token, cancelTokenSource);
            var status = new Label() {Width = 50};
            var result = new Label() {Width = 50, Visible = false};
            var progress = new ProgressBar() {Minimum = 0, Maximum = value, Width = 130, Visible = false};
            var cancel = new Button() {Text = @"Cancel", Visible = true};
            cancel.Click += (ss, ee) => { cancelTokenSource.Cancel(); };
            var panel = new FlowLayoutPanel() {Height = 30, Width = 300};
            panel.Controls.Add(status);
            panel.Controls.Add(result);
            panel.Controls.Add(progress);
            panel.Controls.Add(cancel);
            Controls["flowLayoutPanel1"].Controls.Add(panel);
            var task = new Task(() =>
            {
                var count = 0;
                for (var i = 1; i <= value; i++)
                {
                    var isPrime = true;
                    for (var j = 2; j <= Math.Sqrt(i); ++j)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                        if (i % j != 0) continue;
                        isPrime = false;
                        break;
                    }
                    if (isPrime)
                    {
                        ++count;
                    }
                    ++data.CurrentProgress;
                }
                data.Result = count;
            });
            task.Start();
            _futures.Add(new DataControl(task, status, result, progress, cancel, data));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var f in _futures)
            {
                f.Data.CancelTokenSource.Cancel();
            }
        }
    }
}
