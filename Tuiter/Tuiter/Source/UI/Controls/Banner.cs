namespace Tuiter.Source.UI.Controls
{
    using System;
    using System.Windows.Forms;

    internal partial class Banner : UserControl
    {
        private readonly Timer _timer;

        internal Banner()
        {
            InitializeComponent();

            _timer = new Timer { Interval = 2000 };
            _timer.Tick += Close;
            Visible = false;
        }

        internal void Wait()
        {
            if (InvokeRequired)
                BeginInvoke(new Action(Wait));
            else
                Cursor.Current = Cursors.WaitCursor;
        }

        internal void Ready()
        {
            Ready(null);
        }

        internal void Ready(string msg)
        {
            if (InvokeRequired)
                BeginInvoke(new Action<string>(Ready), msg);
            else
            {
                Cursor.Current = Cursors.Default;

                if (msg != null)
                {
                    _status.Text = msg;
                    _timer.Enabled = true;

                    Visible = true; 
                }
            }
        }

        private void Close(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new EventHandler(Close), sender, e);
            else
            {
                Visible = false;
                _timer.Enabled = false;
                _status.Text = string.Empty;
            }
        }
    }
}