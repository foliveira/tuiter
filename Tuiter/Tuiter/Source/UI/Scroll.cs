namespace Tuiter.Source.UI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class Scroll
    {
        private readonly Control _control;
        private int _oldY;

        internal Scroll(Control control)
        {
            _control = control;

            if (control.Parent == null)
                throw new ArgumentException("This Control is an orphan!");

            control.MouseDown += FingerDown;
            control.MouseMove += FingerUp;
        }

        private void FingerDown(object sender, MouseEventArgs e)
        {
            _oldY = e.Y;
        }

        private void FingerUp(object sender, MouseEventArgs e)
        {
            var y = e.Y - _oldY;

            if (y < 10 && y > -10 || _control.Height <= _control.Parent.Height) 
                return;

            ScrollControl(y);

            _oldY = e.Y;
        }

        private void ScrollControl(int y)
        {
            if (_control.InvokeRequired)
                _control.BeginInvoke(new Action<int>(ScrollControl), y);
            else
            {
                y += _control.Location.Y;
             
                if (y > 0) 
                    y = 0;
                else if (y < -1*(_control.Height - _control.Parent.Height))
                    y = -1*(_control.Height - _control.Parent.Height);

                _control.Location = new Point(_control.Location.X, y);
            }
        }
    }
}