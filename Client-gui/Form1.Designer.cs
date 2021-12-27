
namespace Client_gui
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
            this.Control_Bar = new System.Windows.Forms.Panel();
            this.MinimizePictureBox = new System.Windows.Forms.PictureBox();
            this.ExitPictureBox = new System.Windows.Forms.PictureBox();
            this.StartingPanel = new System.Windows.Forms.Panel();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.Output = new System.Windows.Forms.TextBox();
            this.ConsoleInput = new System.Windows.Forms.TextBox();
            this.SidePanel = new System.Windows.Forms.Panel();
            this.ChatCheck = new System.Windows.Forms.CheckBox();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.LoadingPicture = new System.Windows.Forms.PictureBox();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.IPlabel = new System.Windows.Forms.Label();
            this.TheIpInputTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.Connectbutton = new System.Windows.Forms.Button();
            this.ServerDisplay = new System.Windows.Forms.FlowLayoutPanel();
            this.ServerSearchWorker = new System.ComponentModel.BackgroundWorker();
            this.Control_Bar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinimizePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExitPictureBox)).BeginInit();
            this.StartingPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SidePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPicture)).BeginInit();
            this.ServerDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // Control_Bar
            // 
            this.Control_Bar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.Control_Bar.Controls.Add(this.MinimizePictureBox);
            this.Control_Bar.Controls.Add(this.ExitPictureBox);
            this.Control_Bar.Dock = System.Windows.Forms.DockStyle.Top;
            this.Control_Bar.Location = new System.Drawing.Point(0, 0);
            this.Control_Bar.Name = "Control_Bar";
            this.Control_Bar.Size = new System.Drawing.Size(769, 32);
            this.Control_Bar.TabIndex = 5;
            this.Control_Bar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ControlBar_Panel_MouseDown);
            // 
            // MinimizePictureBox
            // 
            this.MinimizePictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.MinimizePictureBox.Image = global::Client_gui.Properties.Resources._;
            this.MinimizePictureBox.Location = new System.Drawing.Point(705, 0);
            this.MinimizePictureBox.Name = "MinimizePictureBox";
            this.MinimizePictureBox.Size = new System.Drawing.Size(32, 32);
            this.MinimizePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MinimizePictureBox.TabIndex = 3;
            this.MinimizePictureBox.TabStop = false;
            this.MinimizePictureBox.Click += new System.EventHandler(this.MinimizePictureBox_Click);
            this.MinimizePictureBox.MouseLeave += new System.EventHandler(this.MinimizePictureBox_MouseLeave);
            this.MinimizePictureBox.MouseHover += new System.EventHandler(this.MinimizePictureBox_MouseHover);
            // 
            // ExitPictureBox
            // 
            this.ExitPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.ExitPictureBox.Image = global::Client_gui.Properties.Resources.x;
            this.ExitPictureBox.Location = new System.Drawing.Point(737, 0);
            this.ExitPictureBox.Name = "ExitPictureBox";
            this.ExitPictureBox.Size = new System.Drawing.Size(32, 32);
            this.ExitPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ExitPictureBox.TabIndex = 3;
            this.ExitPictureBox.TabStop = false;
            this.ExitPictureBox.Click += new System.EventHandler(this.ExitPictureBox_Click);
            this.ExitPictureBox.MouseLeave += new System.EventHandler(this.ExitPictureBox_MouseLeave);
            this.ExitPictureBox.MouseHover += new System.EventHandler(this.ExitPictureBox_MouseHover);
            // 
            // StartingPanel
            // 
            this.StartingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.StartingPanel.Controls.Add(this.MainPanel);
            this.StartingPanel.Controls.Add(this.LoadingPicture);
            this.StartingPanel.Controls.Add(this.ErrorLabel);
            this.StartingPanel.Controls.Add(this.IPlabel);
            this.StartingPanel.Controls.Add(this.TheIpInputTextBox);
            this.StartingPanel.Controls.Add(this.SearchButton);
            this.StartingPanel.Controls.Add(this.Connectbutton);
            this.StartingPanel.Controls.Add(this.ServerDisplay);
            this.StartingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StartingPanel.Location = new System.Drawing.Point(0, 32);
            this.StartingPanel.Name = "StartingPanel";
            this.StartingPanel.Size = new System.Drawing.Size(769, 410);
            this.StartingPanel.TabIndex = 7;
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.Output);
            this.MainPanel.Controls.Add(this.ConsoleInput);
            this.MainPanel.Controls.Add(this.SidePanel);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(769, 410);
            this.MainPanel.TabIndex = 8;
            // 
            // Output
            // 
            this.Output.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Output.Cursor = System.Windows.Forms.Cursors.Default;
            this.Output.Font = new System.Drawing.Font("BankGothic Lt BT", 11F);
            this.Output.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Output.Location = new System.Drawing.Point(155, 6);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.ReadOnly = true;
            this.Output.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.Output.Size = new System.Drawing.Size(602, 362);
            this.Output.TabIndex = 0;
            // 
            // ConsoleInput
            // 
            this.ConsoleInput.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ConsoleInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ConsoleInput.Font = new System.Drawing.Font("BankGothic Lt BT", 12F);
            this.ConsoleInput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ConsoleInput.Location = new System.Drawing.Point(155, 374);
            this.ConsoleInput.Name = "ConsoleInput";
            this.ConsoleInput.Size = new System.Drawing.Size(602, 24);
            this.ConsoleInput.TabIndex = 0;
            this.ConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConsoleInput_KeyDown);
            // 
            // SidePanel
            // 
            this.SidePanel.Controls.Add(this.ChatCheck);
            this.SidePanel.Controls.Add(this.DisconnectButton);
            this.SidePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.SidePanel.Location = new System.Drawing.Point(0, 0);
            this.SidePanel.Name = "SidePanel";
            this.SidePanel.Size = new System.Drawing.Size(149, 410);
            this.SidePanel.TabIndex = 1;
            // 
            // ChatCheck
            // 
            this.ChatCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ChatCheck.Font = new System.Drawing.Font("BankGothic Lt BT", 11F);
            this.ChatCheck.Location = new System.Drawing.Point(10, 348);
            this.ChatCheck.Name = "ChatCheck";
            this.ChatCheck.Size = new System.Drawing.Size(128, 18);
            this.ChatCheck.TabIndex = 1;
            this.ChatCheck.Text = "Chat";
            this.ChatCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ChatCheck.UseVisualStyleBackColor = true;
            this.ChatCheck.CheckedChanged += new System.EventHandler(this.ChatCheck_CheckedChanged);
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Font = new System.Drawing.Font("BankGothic Lt BT", 11F);
            this.DisconnectButton.Location = new System.Drawing.Point(10, 375);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(128, 23);
            this.DisconnectButton.TabIndex = 0;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // LoadingPicture
            // 
            this.LoadingPicture.Image = global::Client_gui.Properties.Resources.Loading;
            this.LoadingPicture.Location = new System.Drawing.Point(509, 185);
            this.LoadingPicture.Name = "LoadingPicture";
            this.LoadingPicture.Size = new System.Drawing.Size(24, 24);
            this.LoadingPicture.TabIndex = 7;
            this.LoadingPicture.TabStop = false;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.ErrorLabel.Location = new System.Drawing.Point(13, 293);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(91, 25);
            this.ErrorLabel.TabIndex = 3;
            this.ErrorLabel.Text = "Error";
            // 
            // IPlabel
            // 
            this.IPlabel.AutoSize = true;
            this.IPlabel.Font = new System.Drawing.Font("BankGothic Lt BT", 15F);
            this.IPlabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.IPlabel.Location = new System.Drawing.Point(14, 185);
            this.IPlabel.Name = "IPlabel";
            this.IPlabel.Size = new System.Drawing.Size(245, 21);
            this.IPlabel.TabIndex = 2;
            this.IPlabel.Text = "IP address with port:";
            // 
            // TheIpInputTextBox
            // 
            this.TheIpInputTextBox.Font = new System.Drawing.Font("BankGothic Lt BT", 12F);
            this.TheIpInputTextBox.Location = new System.Drawing.Point(265, 185);
            this.TheIpInputTextBox.Name = "TheIpInputTextBox";
            this.TheIpInputTextBox.Size = new System.Drawing.Size(238, 24);
            this.TheIpInputTextBox.TabIndex = 1;
            this.TheIpInputTextBox.TextChanged += new System.EventHandler(this.TheIpInputTextBox_TextChanged);
            this.TheIpInputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TheIpInputTextBox_KeyDown);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(180, 215);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(120, 33);
            this.SearchButton.TabIndex = 0;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // Connectbutton
            // 
            this.Connectbutton.Location = new System.Drawing.Point(306, 215);
            this.Connectbutton.Name = "Connectbutton";
            this.Connectbutton.Size = new System.Drawing.Size(156, 33);
            this.Connectbutton.TabIndex = 0;
            this.Connectbutton.Text = "Connect";
            this.Connectbutton.UseVisualStyleBackColor = true;
            this.Connectbutton.Click += new System.EventHandler(this.Connectbutton_Click);
            // 
            // ServerDisplay
            // 
            this.ServerDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ServerDisplay.Location = new System.Drawing.Point(579, 0);
            this.ServerDisplay.Name = "ServerDisplay";
            this.ServerDisplay.Size = new System.Drawing.Size(190, 410);
            this.ServerDisplay.TabIndex = 9;
            // 
            // ServerSearchWorker
            // 
            this.ServerSearchWorker.WorkerReportsProgress = true;
            this.ServerSearchWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ServerSearchWorker_DoWork);
            this.ServerSearchWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ServerSearchWorker_ProgressChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(769, 442);
            this.Controls.Add(this.StartingPanel);
            this.Controls.Add(this.Control_Bar);
            this.Font = new System.Drawing.Font("BankGothic Lt BT", 18F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Control_Bar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinimizePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExitPictureBox)).EndInit();
            this.StartingPanel.ResumeLayout(false);
            this.StartingPanel.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.SidePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPicture)).EndInit();
            this.ServerDisplay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Control_Bar;
        private System.Windows.Forms.PictureBox MinimizePictureBox;
        private System.Windows.Forms.PictureBox ExitPictureBox;
        private System.Windows.Forms.Panel StartingPanel;
        private System.Windows.Forms.TextBox TheIpInputTextBox;
        private System.Windows.Forms.Button Connectbutton;
        private System.Windows.Forms.Label IPlabel;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.PictureBox LoadingPicture;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.TextBox ConsoleInput;
        private System.Windows.Forms.Panel SidePanel;
        private System.Windows.Forms.TextBox Output;
        private System.Windows.Forms.CheckBox ChatCheck;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.Button SearchButton;
        private System.ComponentModel.BackgroundWorker ServerSearchWorker;
        private System.Windows.Forms.FlowLayoutPanel ServerDisplay;
    }
}

