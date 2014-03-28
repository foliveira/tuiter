namespace Tuiter.Source.UI.Forms
{
    partial class LoginForm
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this._label1 = new System.Windows.Forms.Label();
            this._label2 = new System.Windows.Forms.Label();
            this._passwordBox = new System.Windows.Forms.TextBox();
            this._logo = new System.Windows.Forms.PictureBox();
            this._exitButton = new System.Windows.Forms.Button();
            this._usernameBox = new System.Windows.Forms.TextBox();
            this._loginButton = new System.Windows.Forms.Button();
            this._inputPanel = new Microsoft.WindowsCE.Forms.InputPanel(this.components);
            this._mainMenu = new System.Windows.Forms.MainMenu();
            this.SuspendLayout();
            // 
            // _label1
            // 
            this._label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._label1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this._label1.Location = new System.Drawing.Point(46, 9);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(83, 20);
            this._label1.Text = "Name";
            // 
            // _label2
            // 
            this._label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._label2.ForeColor = System.Drawing.SystemColors.ControlDark;
            this._label2.Location = new System.Drawing.Point(46, 58);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(83, 20);
            this._label2.Text = "Password";
            // 
            // _passwordBox
            // 
            this._passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._passwordBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this._passwordBox.ForeColor = System.Drawing.Color.DimGray;
            this._passwordBox.Location = new System.Drawing.Point(46, 81);
            this._passwordBox.Name = "_passwordBox";
            this._passwordBox.PasswordChar = '*';
            this._passwordBox.Size = new System.Drawing.Size(165, 24);
            this._passwordBox.TabIndex = 1;
            this._passwordBox.Text = "dotRUL3s!";
            this._passwordBox.GotFocus += new System.EventHandler(this.ShowInputPanel);
            this._passwordBox.LostFocus += new System.EventHandler(this.HideInputPanel);
            // 
            // _logo
            // 
            this._logo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this._logo.BackColor = System.Drawing.Color.LightYellow;
            this._logo.Image = ((System.Drawing.Image)(resources.GetObject("_logo.Image")));
            this._logo.Location = new System.Drawing.Point(72, 118);
            this._logo.Name = "_logo";
            this._logo.Size = new System.Drawing.Size(113, 75);
            this._logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // _exitButton
            // 
            this._exitButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this._exitButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._exitButton.ForeColor = System.Drawing.SystemColors.Window;
            this._exitButton.Location = new System.Drawing.Point(84, 268);
            this._exitButton.Name = "_exitButton";
            this._exitButton.Size = new System.Drawing.Size(72, 20);
            this._exitButton.TabIndex = 10;
            this._exitButton.Text = "Exit";
            this._exitButton.Click += new System.EventHandler(this.Exit);
            // 
            // _usernameBox
            // 
            this._usernameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._usernameBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this._usernameBox.ForeColor = System.Drawing.Color.DimGray;
            this._usernameBox.Location = new System.Drawing.Point(46, 29);
            this._usernameBox.Name = "_usernameBox";
            this._usernameBox.Size = new System.Drawing.Size(165, 24);
            this._usernameBox.TabIndex = 0;
            this._usernameBox.Text = "fanoliveira";
            this._usernameBox.GotFocus += new System.EventHandler(this.ShowInputPanel);
            this._usernameBox.LostFocus += new System.EventHandler(this.HideInputPanel);
            // 
            // _loginButton
            // 
            this._loginButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this._loginButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._loginButton.ForeColor = System.Drawing.SystemColors.Menu;
            this._loginButton.Location = new System.Drawing.Point(46, 207);
            this._loginButton.Name = "_loginButton";
            this._loginButton.Size = new System.Drawing.Size(148, 45);
            this._loginButton.TabIndex = 2;
            this._loginButton.Text = "Login";
            this._loginButton.Click += new System.EventHandler(this.Login);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(257, 294);
            this.ControlBox = false;
            this.Controls.Add(this._loginButton);
            this.Controls.Add(this._usernameBox);
            this.Controls.Add(this._exitButton);
            this.Controls.Add(this._logo);
            this.Controls.Add(this._passwordBox);
            this.Controls.Add(this._label2);
            this.Controls.Add(this._label1);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this._mainMenu;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.Text = "Login";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _label1;
        private System.Windows.Forms.Label _label2;
        private System.Windows.Forms.TextBox _passwordBox;
        private System.Windows.Forms.PictureBox _logo;
        private System.Windows.Forms.Button _exitButton;
        private System.Windows.Forms.TextBox _usernameBox;
        private System.Windows.Forms.Button _loginButton;
        private Microsoft.WindowsCE.Forms.InputPanel _inputPanel;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.MainMenu _mainMenu;
    }
}