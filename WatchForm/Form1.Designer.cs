namespace WatchForm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.label1 = new System.Windows.Forms.Label();
            this.systolic = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.diastolic = new System.Windows.Forms.TextBox();
            this.status = new System.Windows.Forms.TextBox();
            this.start = new System.Windows.Forms.Button();
            this.end = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Systolic";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // systolic
            // 
            this.systolic.Location = new System.Drawing.Point(136, 30);
            this.systolic.Name = "systolic";
            this.systolic.Size = new System.Drawing.Size(143, 22);
            this.systolic.TabIndex = 1;
            this.systolic.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Diastolic";
            // 
            // diastolic
            // 
            this.diastolic.Location = new System.Drawing.Point(136, 79);
            this.diastolic.Name = "diastolic";
            this.diastolic.Size = new System.Drawing.Size(143, 22);
            this.diastolic.TabIndex = 3;
            // 
            // status
            // 
            this.status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status.Location = new System.Drawing.Point(136, 134);
            this.status.Multiline = true;
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(964, 376);
            this.status.TabIndex = 4;
            // 
            // start
            // 
            this.start.AutoSize = true;
            this.start.Location = new System.Drawing.Point(136, 546);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(110, 38);
            this.start.TabIndex = 5;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // end
            // 
            this.end.Location = new System.Drawing.Point(995, 546);
            this.end.Name = "end";
            this.end.Size = new System.Drawing.Size(105, 37);
            this.end.TabIndex = 6;
            this.end.Text = "End";
            this.end.UseVisualStyleBackColor = true;
            this.end.Click += new System.EventHandler(this.end_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 610);
            this.Controls.Add(this.end);
            this.Controls.Add(this.start);
            this.Controls.Add(this.status);
            this.Controls.Add(this.diastolic);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.systolic);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox systolic;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox diastolic;
        private System.Windows.Forms.TextBox status;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button end;
    }
}

