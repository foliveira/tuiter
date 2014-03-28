namespace Tuiter.Source.UI.Forms
{
    using System;
    using System.Windows.Forms;
    using Microsoft.WindowsMobile.Samples.Location;

    internal partial class Send : Form
    {
        internal event Action<string, string> Message;
        private static readonly Gps Gps = new Gps();

        internal Send() : this(null, null) { }

        internal Send(string name) : this(name, null) { }

        internal Send(string name, string msg)
        {
            InitializeComponent();

            _tweeterName.Text = name;
            _tweetMessage.Text = msg;

            if (!Gps.Opened)
                Gps.Open();
        }

        private void SendMessage(string name, string msg)
        {
            var message = Message;

            if (message != null) 
                message(name, msg);
        }

        private void TweetClick(object sender, EventArgs e)
        {
            var msg = _tweetMessage.Text;

            if (_gpsCheck.Checked)
            {
                var position = Gps.GetPosition();
                if (position.LatitudeValid && position.LongitudeValid)
                    msg = string.Format("{0} [{1},{2}]", msg, position.Latitude, position.Longitude);
            }

            if (msg.Length > 0)
                SendMessage(_tweeterName.Text, msg);
        }

        private void ShowInputPanel(object sender, EventArgs e)
        {
            Menu = _mainMenu;
            _inputPanel.Enabled = true;
        }

        private void HideInputPanel(object sender, EventArgs e)
        {
            Menu = null;
            _inputPanel.Enabled = false;
        }

        private void DontTweet(object sender, EventArgs e)
        {
            Close();
        }
    }
}