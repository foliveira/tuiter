namespace Tuiter.Source.UI.Forms
{
    using System;
    using System.Windows.Forms;
    using Twitter;

    internal partial class LoginForm : Form
    {
        public event Action<Account> Credentials;

        internal LoginForm()
        {
            InitializeComponent();
        }

        private void Login(object sender, EventArgs e)
        {
            CheckCredentials(new Account
                                 {
                                     Username = _usernameBox.Text,
                                     Password = _passwordBox.Text
                                 });
        
        }

        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CheckCredentials(Account credential)
        {
            var credentials = Credentials;

            if (credentials != null)
                credentials(credential);
        }

        private void ShowInputPanel(object sender, EventArgs e)
        {
            _inputPanel.Enabled = true;
            Menu = _mainMenu;
        }

        private void HideInputPanel(object sender, EventArgs e)
        {
            _inputPanel.Enabled = false;
            Menu = null;
        }
    }
}