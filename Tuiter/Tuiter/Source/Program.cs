namespace Tuiter.Source
{
    using System;
    using System.Windows.Forms;
    using UI.Forms;

    internal static class Program
    {
        [MTAThread]
        private static void Main()
        {
            Application.Run(new TuiterForm());
        }
    }
}