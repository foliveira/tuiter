namespace Tuiter.Source.Twitter
{
    using System;
    using System.Text;

    internal sealed class Account
    {
        internal string Username { get; set; }
        internal string Password { get; set; }

        public override string ToString()
        {
            return Convert.ToBase64String(new ASCIIEncoding().GetBytes(Username + ":" + Password));
        }
    }
}
