namespace Tuiter.Source.UI.Forms
{
    partial class Send
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._gpsCheck = new System.Windows.Forms.CheckBox();
            this._tweetMessage = new System.Windows.Forms.TextBox();
            this._tweeterName = new System.Windows.Forms.TextBox();
            this._sendTweet = new System.Windows.Forms.Button();
            this._inputPanel = new Microsoft.WindowsCE.Forms.InputPanel(this.components);
            this._mainMenu = new System.Windows.Forms.MainMenu();
            this._exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _gpsCheck
            // 
            this._gpsCheck.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._gpsCheck.ForeColor = System.Drawing.SystemColors.ControlDark;
            this._gpsCheck.Location = new System.Drawing.Point(3, 271);
            this._gpsCheck.Name = "_gpsCheck";
            this._gpsCheck.Size = new System.Drawing.Size(156, 20);
            this._gpsCheck.TabIndex = 0;
            this._gpsCheck.Text = "Add GPS Location";
            // 
            // _tweetMessage
            // 
            this._tweetMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tweetMessage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._tweetMessage.ForeColor = System.Drawing.Color.DimGray;
            this._tweetMessage.Location = new System.Drawing.Point(4, 31);
            this._tweetMessage.MaxLength = 140;
            this._tweetMessage.Multiline = true;
            this._tweetMessage.Name = "_tweetMessage";
            this._tweetMessage.Size = new System.Drawing.Size(233, 166);
            this._tweetMessage.TabIndex = 1;
            this._tweetMessage.GotFocus += new System.EventHandler(this.ShowInputPanel);
            this._tweetMessage.LostFocus += new System.EventHandler(this.HideInputPanel);
            // 
            // _tweeterName
            // 
            this._tweeterName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tweeterName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this._tweeterName.ForeColor = System.Drawing.Color.DimGray;
            this._tweeterName.Location = new System.Drawing.Point(4, 4);
            this._tweeterName.Name = "_tweeterName";
            this._tweeterName.ReadOnly = true;
            this._tweeterName.Size = new System.Drawing.Size(233, 24);
            this._tweeterName.TabIndex = 2;
            // 
            // _sendTweet
            // 
            this._sendTweet.BackColor = System.Drawing.SystemColors.ControlDark;
            this._sendTweet.ForeColor = System.Drawing.SystemColors.ControlLight;
            this._sendTweet.Location = new System.Drawing.Point(4, 203);
            this._sendTweet.Name = "_sendTweet";
            this._sendTweet.Size = new System.Drawing.Size(233, 62);
            this._sendTweet.TabIndex = 3;
            this._sendTweet.Text = "Tweet \'R\' Us!";
            this._sendTweet.Click += new System.EventHandler(this.TweetClick);
            // 
            // _exit
            // 
            this._exit.BackColor = System.Drawing.SystemColors.ControlDark;
            this._exit.ForeColor = System.Drawing.SystemColors.ControlLight;
            this._exit.Location = new System.Drawing.Point(165, 271);
            this._exit.Name = "_exit";
            this._exit.Size = new System.Drawing.Size(72, 20);
            this._exit.TabIndex = 4;
            this._exit.Text = "Exit";
            this._exit.Click += new System.EventHandler(this.DontTweet);
            // 
            // Send
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this._exit);
            this.Controls.Add(this._sendTweet);
            this.Controls.Add(this._tweeterName);
            this.Controls.Add(this._tweetMessage);
            this.Controls.Add(this._gpsCheck);
            this.Location = new System.Drawing.Point(0, 0);
            this.Menu = this._mainMenu;
            this.MinimizeBox = false;
            this.Name = "Send";
            this.Text = "Send";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox _gpsCheck;
        private System.Windows.Forms.TextBox _tweetMessage;
        private System.Windows.Forms.TextBox _tweeterName;
        private System.Windows.Forms.Button _sendTweet;
        private Microsoft.WindowsCE.Forms.InputPanel _inputPanel;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.MainMenu _mainMenu;
        private System.Windows.Forms.Button _exit;
    }
}