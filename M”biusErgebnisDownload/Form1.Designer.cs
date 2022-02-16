﻿namespace MöbiusErgebnisDownload
{
    partial class MobiusDownladExams
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Window UI Block

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tB_Teilnehmer = new System.Windows.Forms.TextBox();
            this.b_Users = new System.Windows.Forms.Button();
            this.oFD_Teilnehmer = new System.Windows.Forms.FolderBrowserDialog();
            this.b_StartWebBrowser = new System.Windows.Forms.Button();
            this.b_Download = new System.Windows.Forms.Button();
            this.tB_Status = new System.Windows.Forms.TextBox();
            this.Merge = new System.Windows.Forms.Button();
            this.fBD_Downloads = new System.Windows.Forms.FolderBrowserDialog();
            
            this.SuspendLayout();
            // 
            // tB_Teilnehmer
            // 
            this.tB_Teilnehmer.Location = new System.Drawing.Point(12, 87);
            this.tB_Teilnehmer.Name = "tB_Teilnehmer";
            this.tB_Teilnehmer.Size = new System.Drawing.Size(673, 20);
            this.tB_Teilnehmer.TabIndex = 0;
            // 
            // b_Users
            // 
            this.b_Users.Location = new System.Drawing.Point(691, 86);
            this.b_Users.Name = "b_Users";
            this.b_Users.Size = new System.Drawing.Size(75, 23);
            this.b_Users.TabIndex = 1;
            this.b_Users.Text = "Browse Folder";
            this.b_Users.UseVisualStyleBackColor = true;
            this.b_Users.Click += new System.EventHandler(this.B_Users_Click);
            // 
            // oFD_Teilnehmer
            // 
            // this.oFD_Teilnehmer.FileName = ".csv";
            // this.oFD_Teilnehmer.RestoreDirectory = true;
            // 
            // b_StartWebBrowser
            // 
            this.b_StartWebBrowser.Location = new System.Drawing.Point(691, 132);
            this.b_StartWebBrowser.Name = "b_StartWebBrowser";
            this.b_StartWebBrowser.Size = new System.Drawing.Size(75, 23);
            this.b_StartWebBrowser.TabIndex = 2;
            this.b_StartWebBrowser.Text = "Webbrowser";
            this.b_StartWebBrowser.UseVisualStyleBackColor = true;
            this.b_StartWebBrowser.Click += new System.EventHandler(this.B_StartWebBrowser_Click);
            // 
            // b_Download
            // 
            this.b_Download.Location = new System.Drawing.Point(691, 177);
            this.b_Download.Name = "b_Download";
            this.b_Download.Size = new System.Drawing.Size(75, 23);
            this.b_Download.TabIndex = 3;
            this.b_Download.Text = "Download";
            this.b_Download.UseVisualStyleBackColor = true;
            this.b_Download.Click += new System.EventHandler(this.Download_Click);
            // 
            // tB_Status
            // 
            this.tB_Status.Enabled = false;
            this.tB_Status.Location = new System.Drawing.Point(12, 236);
            this.tB_Status.Name = "tB_Status";
            this.tB_Status.Size = new System.Drawing.Size(673, 20);
            this.tB_Status.TabIndex = 4;
            // 
            // label1
            // 
            this.Merge.AutoSize = true;
            this.Merge.Location = new System.Drawing.Point(692, 240);
            this.Merge.Name = "label1";
            this.Merge.Size = new System.Drawing.Size(75, 23);
            this.Merge.TabIndex = 5;
            this.Merge.Text = "Merge CSV";
            this.Merge.Click += new System.EventHandler(this.ValidateFileType);

            // 
            // MobiusDownladExams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Merge);
            this.Controls.Add(this.tB_Status);
            this.Controls.Add(this.b_Download);
            this.Controls.Add(this.b_StartWebBrowser);
            this.Controls.Add(this.b_Users);
            this.Controls.Add(this.tB_Teilnehmer);
            this.Name = "MobiusDownladExams";
            this.Text = "Möbius Prüfungen herunterladen.";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MobiusDownladExams_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tB_Teilnehmer;
        private System.Windows.Forms.Button b_Users;
        private System.Windows.Forms.FolderBrowserDialog oFD_Teilnehmer;
        private System.Windows.Forms.Button b_StartWebBrowser;
        private System.Windows.Forms.Button b_Download;
        private System.Windows.Forms.TextBox tB_Status;
        private System.Windows.Forms.Button Merge;
        private System.Windows.Forms.FolderBrowserDialog fBD_Downloads;
    }
}

