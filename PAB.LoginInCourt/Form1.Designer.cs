namespace PAB.LoginInCourt
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
            this.LBalluser = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LBcheckusers = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LBautorizeduser = new System.Windows.Forms.Label();
            this.LBnotauthorizedusers = new System.Windows.Forms.Label();
            this.LBprogresstext = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LBwait = new System.Windows.Forms.Label();
            this.BtnSetting = new System.Windows.Forms.Button();
            this.BTNStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "სულ შესამოწმებელი იუზერი :";
            // 
            // LBalluser
            // 
            this.LBalluser.AutoSize = true;
            this.LBalluser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBalluser.Location = new System.Drawing.Point(276, 98);
            this.LBalluser.Name = "LBalluser";
            this.LBalluser.Size = new System.Drawing.Size(16, 17);
            this.LBalluser.TabIndex = 1;
            this.LBalluser.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(336, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "/";
            // 
            // LBcheckusers
            // 
            this.LBcheckusers.AutoSize = true;
            this.LBcheckusers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBcheckusers.Location = new System.Drawing.Point(385, 98);
            this.LBcheckusers.Name = "LBcheckusers";
            this.LBcheckusers.Size = new System.Drawing.Size(16, 17);
            this.LBcheckusers.TabIndex = 3;
            this.LBcheckusers.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(47, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "დალოგინდა :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(47, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "ვერ დალოგინდა :";
            // 
            // LBautorizeduser
            // 
            this.LBautorizeduser.AutoSize = true;
            this.LBautorizeduser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBautorizeduser.Location = new System.Drawing.Point(171, 144);
            this.LBautorizeduser.Name = "LBautorizeduser";
            this.LBautorizeduser.Size = new System.Drawing.Size(16, 17);
            this.LBautorizeduser.TabIndex = 6;
            this.LBautorizeduser.Text = "0";
            // 
            // LBnotauthorizedusers
            // 
            this.LBnotauthorizedusers.AutoSize = true;
            this.LBnotauthorizedusers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBnotauthorizedusers.Location = new System.Drawing.Point(171, 186);
            this.LBnotauthorizedusers.Name = "LBnotauthorizedusers";
            this.LBnotauthorizedusers.Size = new System.Drawing.Size(16, 17);
            this.LBnotauthorizedusers.TabIndex = 7;
            this.LBnotauthorizedusers.Text = "0";
            // 
            // LBprogresstext
            // 
            this.LBprogresstext.AutoSize = true;
            this.LBprogresstext.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBprogresstext.ForeColor = System.Drawing.Color.Red;
            this.LBprogresstext.Location = new System.Drawing.Point(230, 268);
            this.LBprogresstext.Name = "LBprogresstext";
            this.LBprogresstext.Size = new System.Drawing.Size(99, 17);
            this.LBprogresstext.TabIndex = 8;
            this.LBprogresstext.Text = "მუშავდება ...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(230, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "მიმდინარე ლოდინი :";
            // 
            // LBwait
            // 
            this.LBwait.AutoSize = true;
            this.LBwait.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBwait.Location = new System.Drawing.Point(377, 55);
            this.LBwait.Name = "LBwait";
            this.LBwait.Size = new System.Drawing.Size(50, 17);
            this.LBwait.TabIndex = 10;
            this.LBwait.Text = "0 წამი";
            // 
            // BtnSetting
            // 
            this.BtnSetting.Location = new System.Drawing.Point(466, 12);
            this.BtnSetting.Name = "BtnSetting";
            this.BtnSetting.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.BtnSetting.Size = new System.Drawing.Size(99, 24);
            this.BtnSetting.TabIndex = 11;
            this.BtnSetting.Text = "კონფიგურაცია";
            this.BtnSetting.UseVisualStyleBackColor = true;
            this.BtnSetting.Click += new System.EventHandler(this.BtnSetting_Click);
            // 
            // BTNStart
            // 
            this.BTNStart.Location = new System.Drawing.Point(230, 228);
            this.BTNStart.Name = "BTNStart";
            this.BTNStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.BTNStart.Size = new System.Drawing.Size(99, 24);
            this.BTNStart.TabIndex = 12;
            this.BTNStart.Text = "დაწყება";
            this.BTNStart.UseVisualStyleBackColor = true;
            this.BTNStart.Click += new System.EventHandler(this.BTNStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 305);
            this.Controls.Add(this.BTNStart);
            this.Controls.Add(this.BtnSetting);
            this.Controls.Add(this.LBwait);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LBprogresstext);
            this.Controls.Add(this.LBnotauthorizedusers);
            this.Controls.Add(this.LBautorizeduser);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.LBcheckusers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LBalluser);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Court Info";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LBalluser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LBcheckusers;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LBautorizeduser;
        private System.Windows.Forms.Label LBnotauthorizedusers;
        private System.Windows.Forms.Label LBprogresstext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LBwait;
        private System.Windows.Forms.Button BtnSetting;
        private System.Windows.Forms.Button BTNStart;
    }
}

