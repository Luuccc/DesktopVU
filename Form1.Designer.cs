namespace Desktop_VU
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.addMonitor = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.refDevices = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// addMonitor
			// 
			this.addMonitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.addMonitor.Location = new System.Drawing.Point(23, 79);
			this.addMonitor.Name = "addMonitor";
			this.addMonitor.Size = new System.Drawing.Size(75, 28);
			this.addMonitor.TabIndex = 0;
			this.addMonitor.Text = "+";
			this.addMonitor.UseVisualStyleBackColor = true;
			this.addMonitor.Click += new System.EventHandler(this.addMonitor_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 10;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// refDevices
			// 
			this.refDevices.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.refDevices.Location = new System.Drawing.Point(139, 79);
			this.refDevices.Name = "refDevices";
			this.refDevices.Size = new System.Drawing.Size(75, 28);
			this.refDevices.TabIndex = 1;
			this.refDevices.Text = "↺";
			this.refDevices.UseCompatibleTextRendering = true;
			this.refDevices.UseVisualStyleBackColor = true;
			this.refDevices.Click += new System.EventHandler(this.refDevices_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(240, 119);
			this.Controls.Add(this.refDevices);
			this.Controls.Add(this.addMonitor);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "VU Meter";
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addMonitor;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button refDevices;
    }
}

