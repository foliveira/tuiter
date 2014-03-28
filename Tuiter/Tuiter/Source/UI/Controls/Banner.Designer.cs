namespace Tuiter.Source.UI.Controls
{
    partial class Banner
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
            this._status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _status
            // 
            this._status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._status.BackColor = System.Drawing.SystemColors.ControlDark;
            this._status.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._status.ForeColor = System.Drawing.Color.White;
            this._status.Location = new System.Drawing.Point(2, 2);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(80, 20);
            this._status.Text = "Message";
            this._status.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Banner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this._status);
            this.Name = "Banner";
            this.Size = new System.Drawing.Size(84, 24);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _status;

    }
}


