namespace Windows_Forms_Chat
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            MyPortTextBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            serverPortTextBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            ServerIPTextBox = new System.Windows.Forms.TextBox();
            ChatTextBox = new System.Windows.Forms.TextBox();
            TypeTextBox = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            HostButton = new System.Windows.Forms.Button();
            JoinButton = new System.Windows.Forms.Button();
            SendButton = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            button5 = new System.Windows.Forms.Button();
            button6 = new System.Windows.Forms.Button();
            button7 = new System.Windows.Forms.Button();
            button8 = new System.Windows.Forms.Button();
            button9 = new System.Windows.Forms.Button();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            button11 = new System.Windows.Forms.Button();
            button10 = new System.Windows.Forms.Button();
            list_users_btn_ser = new System.Windows.Forms.Button();
            tabPage2 = new System.Windows.Forms.TabPage();
            groupBox1 = new System.Windows.Forms.GroupBox();
            button13 = new System.Windows.Forms.Button();
            button12 = new System.Windows.Forms.Button();
            clear_btn = new System.Windows.Forms.Button();
            exit_btn = new System.Windows.Forms.Button();
            btn_help = new System.Windows.Forms.Button();
            list_users_btn = new System.Windows.Forms.Button();
            about_btn = new System.Windows.Forms.Button();
            whoAmIButton = new System.Windows.Forms.Button();
            commands_btn = new System.Windows.Forms.Button();
            change_username_btn = new System.Windows.Forms.Button();
            username_txt = new System.Windows.Forms.TextBox();
            lb_username = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 20);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(49, 15);
            label1.TabIndex = 0;
            label1.Text = "My Port";
            // 
            // MyPortTextBox
            // 
            MyPortTextBox.Location = new System.Drawing.Point(84, 17);
            MyPortTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MyPortTextBox.Name = "MyPortTextBox";
            MyPortTextBox.Size = new System.Drawing.Size(80, 23);
            MyPortTextBox.TabIndex = 1;
            MyPortTextBox.Text = "6666";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(19, 25);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(64, 15);
            label2.TabIndex = 2;
            label2.Text = "Server Port";
            // 
            // serverPortTextBox
            // 
            serverPortTextBox.Location = new System.Drawing.Point(19, 43);
            serverPortTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            serverPortTextBox.Name = "serverPortTextBox";
            serverPortTextBox.Size = new System.Drawing.Size(110, 23);
            serverPortTextBox.TabIndex = 3;
            serverPortTextBox.Text = "6666";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(152, 25);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(52, 15);
            label3.TabIndex = 4;
            label3.Text = "Server IP";
            // 
            // ServerIPTextBox
            // 
            ServerIPTextBox.Location = new System.Drawing.Point(152, 43);
            ServerIPTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ServerIPTextBox.Name = "ServerIPTextBox";
            ServerIPTextBox.Size = new System.Drawing.Size(140, 23);
            ServerIPTextBox.TabIndex = 5;
            ServerIPTextBox.Text = "127.0.0.1";
            // 
            // ChatTextBox
            // 
            ChatTextBox.Location = new System.Drawing.Point(21, 250);
            ChatTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ChatTextBox.Multiline = true;
            ChatTextBox.Name = "ChatTextBox";
            ChatTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            ChatTextBox.Size = new System.Drawing.Size(621, 229);
            ChatTextBox.TabIndex = 6;
            ChatTextBox.Text = "\r\n";
            // 
            // TypeTextBox
            // 
            TypeTextBox.Location = new System.Drawing.Point(21, 505);
            TypeTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            TypeTextBox.Name = "TypeTextBox";
            TypeTextBox.Size = new System.Drawing.Size(513, 23);
            TypeTextBox.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(22, 488);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(110, 15);
            label4.TabIndex = 8;
            label4.Text = "Type your message:";
            // 
            // HostButton
            // 
            HostButton.BackColor = System.Drawing.Color.LawnGreen;
            HostButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            HostButton.Location = new System.Drawing.Point(19, 86);
            HostButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HostButton.Name = "HostButton";
            HostButton.Size = new System.Drawing.Size(82, 22);
            HostButton.TabIndex = 9;
            HostButton.Text = "Conect";
            HostButton.UseVisualStyleBackColor = false;
            HostButton.Click += HostButton_Click;
            // 
            // JoinButton
            // 
            JoinButton.BackColor = System.Drawing.Color.LawnGreen;
            JoinButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            JoinButton.Location = new System.Drawing.Point(20, 64);
            JoinButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            JoinButton.Name = "JoinButton";
            JoinButton.Size = new System.Drawing.Size(82, 22);
            JoinButton.TabIndex = 10;
            JoinButton.Text = "Join Chat";
            JoinButton.UseVisualStyleBackColor = false;
            JoinButton.Click += JoinButton_Click;
            // 
            // SendButton
            // 
            SendButton.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            SendButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            SendButton.Location = new System.Drawing.Point(540, 503);
            SendButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            SendButton.Name = "SendButton";
            SendButton.Size = new System.Drawing.Size(100, 24);
            SendButton.TabIndex = 11;
            SendButton.Text = "Send";
            SendButton.UseVisualStyleBackColor = false;
            SendButton.Click += SendButton_Click;
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.FromArgb(220, 248, 198);
            button1.Font = new System.Drawing.Font("Segoe UI", 19F);
            button1.Location = new System.Drawing.Point(699, 107);
            button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(94, 79);
            button1.TabIndex = 13;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = System.Drawing.Color.FromArgb(220, 248, 198);
            button2.Font = new System.Drawing.Font("Segoe UI", 19F);
            button2.Location = new System.Drawing.Point(786, 107);
            button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(94, 79);
            button2.TabIndex = 13;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = System.Drawing.Color.FromArgb(220, 248, 198);
            button3.Font = new System.Drawing.Font("Segoe UI", 19F);
            button3.Location = new System.Drawing.Point(874, 107);
            button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(94, 79);
            button3.TabIndex = 13;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.BackColor = System.Drawing.Color.FromArgb(220, 248, 198);
            button4.Font = new System.Drawing.Font("Segoe UI", 19F);
            button4.Location = new System.Drawing.Point(699, 182);
            button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(94, 86);
            button4.TabIndex = 13;
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.BackColor = System.Drawing.Color.FromArgb(220, 248, 198);
            button5.Font = new System.Drawing.Font("Segoe UI", 19F);
            button5.Location = new System.Drawing.Point(786, 182);
            button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(94, 86);
            button5.TabIndex = 13;
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.BackColor = System.Drawing.Color.FromArgb(220, 248, 198);
            button6.Font = new System.Drawing.Font("Segoe UI", 19F);
            button6.Location = new System.Drawing.Point(874, 182);
            button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(94, 86);
            button6.TabIndex = 13;
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.BackColor = System.Drawing.Color.FromArgb(220, 248, 198);
            button7.Font = new System.Drawing.Font("Segoe UI", 19F);
            button7.Location = new System.Drawing.Point(699, 262);
            button7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(94, 97);
            button7.TabIndex = 13;
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.BackColor = System.Drawing.Color.FromArgb(220, 248, 198);
            button8.Font = new System.Drawing.Font("Segoe UI", 19F);
            button8.Location = new System.Drawing.Point(786, 262);
            button8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(94, 97);
            button8.TabIndex = 13;
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.BackColor = System.Drawing.Color.FromArgb(220, 248, 198);
            button9.Font = new System.Drawing.Font("Segoe UI", 19F);
            button9.Location = new System.Drawing.Point(874, 262);
            button9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(94, 97);
            button9.TabIndex = 13;
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new System.Drawing.Point(21, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(625, 197);
            tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = System.Drawing.Color.FromArgb(232, 232, 232);
            tabPage1.Controls.Add(button11);
            tabPage1.Controls.Add(button10);
            tabPage1.Controls.Add(list_users_btn_ser);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(serverPortTextBox);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(ServerIPTextBox);
            tabPage1.Controls.Add(HostButton);
            tabPage1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(617, 169);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Server";
            // 
            // button11
            // 
            button11.BackColor = System.Drawing.SystemColors.Highlight;
            button11.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            button11.Location = new System.Drawing.Point(253, 105);
            button11.Name = "button11";
            button11.Size = new System.Drawing.Size(116, 23);
            button11.TabIndex = 12;
            button11.Text = "Moderators List";
            button11.UseVisualStyleBackColor = false;
            button11.Click += button11_Click;
            // 
            // button10
            // 
            button10.BackColor = System.Drawing.SystemColors.Highlight;
            button10.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            button10.Location = new System.Drawing.Point(152, 105);
            button10.Name = "button10";
            button10.Size = new System.Drawing.Size(95, 23);
            button10.TabIndex = 11;
            button10.Text = "Commands";
            button10.UseVisualStyleBackColor = false;
            button10.Click += button10_Click;
            // 
            // list_users_btn_ser
            // 
            list_users_btn_ser.BackColor = System.Drawing.SystemColors.Highlight;
            list_users_btn_ser.ForeColor = System.Drawing.SystemColors.ButtonFace;
            list_users_btn_ser.Location = new System.Drawing.Point(19, 105);
            list_users_btn_ser.Name = "list_users_btn_ser";
            list_users_btn_ser.Size = new System.Drawing.Size(121, 23);
            list_users_btn_ser.TabIndex = 10;
            list_users_btn_ser.Text = "Connected Users";
            list_users_btn_ser.UseVisualStyleBackColor = false;
            list_users_btn_ser.Click += list_users_btn_ser_Click;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = System.Drawing.Color.FromArgb(232, 232, 232);
            tabPage2.Controls.Add(groupBox1);
            tabPage2.Controls.Add(change_username_btn);
            tabPage2.Controls.Add(username_txt);
            tabPage2.Controls.Add(lb_username);
            tabPage2.Controls.Add(MyPortTextBox);
            tabPage2.Controls.Add(label1);
            tabPage2.Controls.Add(JoinButton);
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(617, 169);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Client";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button13);
            groupBox1.Controls.Add(button12);
            groupBox1.Controls.Add(clear_btn);
            groupBox1.Controls.Add(exit_btn);
            groupBox1.Controls.Add(btn_help);
            groupBox1.Controls.Add(list_users_btn);
            groupBox1.Controls.Add(about_btn);
            groupBox1.Controls.Add(whoAmIButton);
            groupBox1.Controls.Add(commands_btn);
            groupBox1.Location = new System.Drawing.Point(50, 50);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(502, 100);
            groupBox1.TabIndex = 15;
            groupBox1.TabStop = false;
            groupBox1.Text = "Actions";
            // 
            // button13
            // 
            button13.BackColor = System.Drawing.SystemColors.Highlight;
            button13.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            button13.Location = new System.Drawing.Point(204, 60);
            button13.Name = "button13";
            button13.Size = new System.Drawing.Size(75, 23);
            button13.TabIndex = 22;
            button13.Text = "Time";
            button13.UseVisualStyleBackColor = false;
            button13.Click += button13_Click;
            // 
            // button12
            // 
            button12.BackColor = System.Drawing.SystemColors.Highlight;
            button12.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            button12.Location = new System.Drawing.Point(123, 60);
            button12.Name = "button12";
            button12.Size = new System.Drawing.Size(75, 23);
            button12.TabIndex = 21;
            button12.Text = "Joke";
            button12.UseVisualStyleBackColor = false;
            button12.Click += button12_Click;
            // 
            // clear_btn
            // 
            clear_btn.BackColor = System.Drawing.SystemColors.Highlight;
            clear_btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            clear_btn.Location = new System.Drawing.Point(285, 60);
            clear_btn.Name = "clear_btn";
            clear_btn.Size = new System.Drawing.Size(75, 23);
            clear_btn.TabIndex = 20;
            clear_btn.Text = "Clear";
            clear_btn.UseVisualStyleBackColor = false;
            clear_btn.Click += clear_btn_Click;
            // 
            // exit_btn
            // 
            exit_btn.BackColor = System.Drawing.Color.IndianRed;
            exit_btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            exit_btn.Location = new System.Drawing.Point(421, 22);
            exit_btn.Name = "exit_btn";
            exit_btn.Size = new System.Drawing.Size(75, 23);
            exit_btn.TabIndex = 19;
            exit_btn.Text = "Exit";
            exit_btn.UseVisualStyleBackColor = false;
            exit_btn.Click += exit_btn_Click;
            // 
            // btn_help
            // 
            btn_help.BackColor = System.Drawing.SystemColors.Highlight;
            btn_help.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            btn_help.Location = new System.Drawing.Point(260, 22);
            btn_help.Name = "btn_help";
            btn_help.Size = new System.Drawing.Size(75, 23);
            btn_help.TabIndex = 18;
            btn_help.Text = "Help";
            btn_help.UseVisualStyleBackColor = false;
            btn_help.Click += btn_help_Click;
            // 
            // list_users_btn
            // 
            list_users_btn.BackColor = System.Drawing.SystemColors.Highlight;
            list_users_btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            list_users_btn.Location = new System.Drawing.Point(6, 60);
            list_users_btn.Name = "list_users_btn";
            list_users_btn.Size = new System.Drawing.Size(111, 23);
            list_users_btn.TabIndex = 17;
            list_users_btn.Text = "Connected Users";
            list_users_btn.UseVisualStyleBackColor = false;
            list_users_btn.Click += list_users_btn_Click;
            // 
            // about_btn
            // 
            about_btn.BackColor = System.Drawing.SystemColors.Highlight;
            about_btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            about_btn.Location = new System.Drawing.Point(179, 22);
            about_btn.Name = "about_btn";
            about_btn.Size = new System.Drawing.Size(75, 23);
            about_btn.TabIndex = 16;
            about_btn.Text = "About";
            about_btn.UseVisualStyleBackColor = false;
            about_btn.Click += about_btn_Click;
            // 
            // whoAmIButton
            // 
            whoAmIButton.BackColor = System.Drawing.SystemColors.Highlight;
            whoAmIButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            whoAmIButton.Location = new System.Drawing.Point(98, 22);
            whoAmIButton.Name = "whoAmIButton";
            whoAmIButton.Size = new System.Drawing.Size(75, 23);
            whoAmIButton.TabIndex = 15;
            whoAmIButton.Text = "Whoami";
            whoAmIButton.UseVisualStyleBackColor = false;
            whoAmIButton.Click += whoAmIButton_Click;
            // 
            // commands_btn
            // 
            commands_btn.BackColor = System.Drawing.SystemColors.Highlight;
            commands_btn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            commands_btn.Location = new System.Drawing.Point(6, 22);
            commands_btn.Name = "commands_btn";
            commands_btn.Size = new System.Drawing.Size(86, 23);
            commands_btn.TabIndex = 14;
            commands_btn.Text = "Commands";
            commands_btn.UseVisualStyleBackColor = false;
            commands_btn.Click += commands_btn_Click;
            // 
            // change_username_btn
            // 
            change_username_btn.BackColor = System.Drawing.SystemColors.Highlight;
            change_username_btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            change_username_btn.Location = new System.Drawing.Point(409, 20);
            change_username_btn.Name = "change_username_btn";
            change_username_btn.Size = new System.Drawing.Size(76, 23);
            change_username_btn.TabIndex = 13;
            change_username_btn.Text = "Change";
            change_username_btn.UseVisualStyleBackColor = false;
            change_username_btn.Click += change_username_btn_Click;
            // 
            // username_txt
            // 
            username_txt.Location = new System.Drawing.Point(250, 20);
            username_txt.Name = "username_txt";
            username_txt.Size = new System.Drawing.Size(153, 23);
            username_txt.TabIndex = 12;
            // 
            // lb_username
            // 
            lb_username.AutoSize = true;
            lb_username.Location = new System.Drawing.Point(184, 20);
            lb_username.Name = "lb_username";
            lb_username.Size = new System.Drawing.Size(60, 15);
            lb_username.TabIndex = 11;
            lb_username.Text = "Username";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(22, 233);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(32, 15);
            label5.TabIndex = 15;
            label5.Text = "Chat";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(699, 85);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(56, 15);
            label6.TabIndex = 16;
            label6.Text = "Let's play";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            ClientSize = new System.Drawing.Size(1009, 551);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(tabControl1);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(SendButton);
            Controls.Add(label4);
            Controls.Add(TypeTextBox);
            Controls.Add(ChatTextBox);
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MyPortTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serverPortTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ServerIPTextBox;
        private System.Windows.Forms.TextBox ChatTextBox;
        private System.Windows.Forms.TextBox TypeTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button HostButton;
        private System.Windows.Forms.Button JoinButton;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lb_username;
        private System.Windows.Forms.TextBox username_txt;
        private System.Windows.Forms.Button change_username_btn;
        private System.Windows.Forms.Button commands_btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button whoAmIButton;
        private System.Windows.Forms.Button about_btn;
        private System.Windows.Forms.Button list_users_btn;
        private System.Windows.Forms.Button exit_btn;
        private System.Windows.Forms.Button btn_help;
        private System.Windows.Forms.Button clear_btn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button list_users_btn_ser;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
    }
}

