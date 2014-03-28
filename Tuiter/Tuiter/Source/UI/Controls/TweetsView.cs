namespace Tuiter.Source.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Twitter;

    internal class TweetsView : UserControl
    {
        private readonly List<IComparable> _tweets;
        private Control _selected;

        internal TweetsView()
        {
            InitializeComponent();

            _tweets = new List<IComparable>();
        }

        internal Control Selected
        {
            get
            {
                return _selected;
            }

            private set
            {
                if (_selected != null)
                    SetBackColor(_selected, Color.White);

                if (_selected == value)
                    _selected = null;
                else
                {
                    _selected = value;
                    SetBackColor(_selected, Color.LightGray);
                }
            }
        }

        private void SetBackColor(Control control, Color color)
        {
            if (InvokeRequired)
                BeginInvoke(new Action<Control, Color>(SetBackColor), control, color);
            else
                control.BackColor = color;
        }

        private void TweetAdded(Control control)
        {
            var ctrl = control as IComparable;

            if (ctrl == null) return;

            var i = _tweets.TakeWhile(item => item.CompareTo(ctrl) >= 0).Count();

            _tweets.Insert(i, ctrl);

            RelocatePosition();
        }

        internal void TweetCreated(Tweet tweet)
        {
            if (InvokeRequired)
                BeginInvoke(new Action<Tweet>(TweetCreated), tweet);
            else
            {
                SuspendLayout();
                var item = new VisualTweet
                               {
                                   Tweet = tweet,
                                   Width = Width,
                                   Anchor = AnchorStyles.Left | AnchorStyles.Right
                               };
                item.Click += FingerClick;
                item.MouseDown += FingerDown;
                item.MouseMove += FingerUp;

                Controls.Add(item);
                TweetAdded(item);
                ResumeLayout(false);
            }
        }

        private void RelocatePosition()
        {
            var location = new Point(Location.X, Location.Y);

            foreach (VisualTweet item in _tweets)
            {
                item.Location = new Point(location.X, location.Y);
                location.Y += item.Height;
            }

            Size = new Size(Width, location.Y);
        }

        internal void Select(Point location)
        {
            location.X -= Location.X;
            location.Y -= Location.Y;

            foreach (VisualTweet item in _tweets)
            {
                if ((item.Location.Y < location.Y) && (item.Location.Y + item.Height > location.Y))
                {
                    Selected = item;
                    break;
                }
            }
        }
        
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // TweetsView
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = SystemColors.ControlLight;
            Name = "TweetsView";
            ResumeLayout(false);

        }

        private void FingerUp(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        private void FingerDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }

        private void FingerClick(object sender, EventArgs e)
        {
            OnClick(e);
        }
    }
}