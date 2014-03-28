namespace Tuiter.Source.UI.Controls
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    using Utils;
    using Twitter;

    internal partial class VisualTweet : UserControl, IComparable
    {
        private Tweet _tweet;

        internal VisualTweet()
        {
            InitializeComponent();
        }

        internal Tweet Tweet
        {
            get
            {
                return _tweet;
            }
            set
            {
                if (_tweet != null)
                    return;

                _tweet = value;

                InitializeItem();

                _tweet.ValueChanged += OnValueChanged;
            }
        }

        public int CompareTo(object obj)
        {
            var item = obj as VisualTweet;

            if (item == null) 
                throw new ArgumentException("I do need another VisualTweet to compare to, you know?");

            return ((DateTime) _tweet["created_at"]).CompareTo(item._tweet["created_at"]);
        }

        private void InitializeItem()
        {
            foreach (var key in _tweet.GetProperties())
            {
                Populate(key, _tweet[key]);
            }
        }

        private void Populate(string key, object value)
        {
            if (InvokeRequired)
                BeginInvoke(new Action<string, object>(Populate), key, value);
            else
            {
                switch (key)
                {
                    case "screen_name":
                        _name.Text = value as string;
                        break;
                    case "image":
                        var b = value as byte[];
                        if (b != null)
                        {
                            var ms = new MemoryStream(b);
                            _picture.Image = new Bitmap(ms);
                        }
                        break;
                    case "created_at":
                        _date.Text = Date.TimeElapsed((DateTime) value);
                        break;
                    case "text":
                        _message.Text = value as string;
                        break;
                }
            }
        }

        private void OnValueChanged(string key, object value)
        {
            Populate(key, value);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _date.Text = Date.TimeElapsed((DateTime) _tweet["created_at"]);

            base.OnPaint(e);
        }
    }
}