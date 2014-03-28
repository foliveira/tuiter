namespace Tuiter.Source.UI.Controls
{
    using System.Windows.Forms;

    partial class VisualTweet
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualTweet));
            this._picture = new System.Windows.Forms.PictureBox();
            this._name = new System.Windows.Forms.Label();
            this._date = new System.Windows.Forms.Label();
            this._message = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _picture
            // 
            this._picture.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._picture.BackColor = System.Drawing.SystemColors.ControlLight;
            this._picture.Image = ((System.Drawing.Image)(resources.GetObject("_picture.Image")));
            this._picture.Location = new System.Drawing.Point(3, 26);
            this._picture.Name = "_picture";
            this._picture.Size = new System.Drawing.Size(48, 48);
            this._picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // _name
            // 
            this._name.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._name.ForeColor = System.Drawing.SystemColors.ControlDark;
            this._name.Location = new System.Drawing.Point(3, -1);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(142, 16);
            this._name.Text = "N/A";
            // 
            // _date
            // 
            this._date.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._date.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular);
            this._date.ForeColor = System.Drawing.SystemColors.ControlDark;
            this._date.Location = new System.Drawing.Point(0, 77);
            this._date.Name = "_date";
            this._date.Size = new System.Drawing.Size(140, 23);
            this._date.Text = "N/A";
            // 
            // _message
            // 
            this._message.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._message.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this._message.ForeColor = System.Drawing.SystemColors.ControlDark;
            this._message.Location = new System.Drawing.Point(57, 18);
            this._message.Name = "_message";
            this._message.Size = new System.Drawing.Size(83, 82);
            this._message.Text = "N/A";
            // 
            // VisualTweet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this._message);
            this.Controls.Add(this._date);
            this.Controls.Add(this._picture);
            this.Controls.Add(this._name);
            this.Name = "VisualTweet";
            this.Size = new System.Drawing.Size(140, 100);
            this.ResumeLayout(false);

        }

        #endregion

        private Label _date;
        private Label _name;
        private Label _message;
        private PictureBox _picture;
    }
}


