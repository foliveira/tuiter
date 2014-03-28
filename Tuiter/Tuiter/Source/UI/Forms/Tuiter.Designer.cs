namespace Tuiter.Source.UI.Forms
{
    using Data;
    using Controls;

    partial class TuiterForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._dataSet = new TuiterDS();
            this._banner = new Banner();

            ((System.ComponentModel.ISupportInitialize)(this._dataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // _dataSet
            // 
            this._dataSet.DataSetName = "TweetterliuDS";
            this._dataSet.Prefix = "";
            this._dataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // waitCursor
            // 
            this._banner.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this._banner.BackColor = System.Drawing.Color.DimGray;
            this._banner.Location = new System.Drawing.Point(3, 293);
            this._banner.Name = "waitCursor";
            this._banner.Size = new System.Drawing.Size(234, 24);
            this._banner.TabIndex = 1;
            this._banner.Visible = false;
            // 
            // TuiterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.Controls.Add(this._banner);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.Name = "TuiterForm";
            this.Text = "TuiterDS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this._dataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TuiterDS _dataSet;
        private Banner _banner;
    }
}