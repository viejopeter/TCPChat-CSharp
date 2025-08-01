namespace Windows_Forms_Chat
{
    partial class LoginForm
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
            btn_login = new System.Windows.Forms.Button();
            btn_signup = new System.Windows.Forms.Button();
            textBox1 = new System.Windows.Forms.TextBox();
            textBox2 = new System.Windows.Forms.TextBox();
            textBox3 = new System.Windows.Forms.TextBox();
            panel1 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            panel2 = new System.Windows.Forms.Panel();
            lb_welcome = new System.Windows.Forms.Label();
            btn_enter = new System.Windows.Forms.Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // btn_login
            // 
            btn_login.Location = new System.Drawing.Point(33, 29);
            btn_login.Name = "btn_login";
            btn_login.Size = new System.Drawing.Size(129, 39);
            btn_login.TabIndex = 0;
            btn_login.Text = "Login";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += btn_login_Click;
            // 
            // btn_signup
            // 
            btn_signup.Location = new System.Drawing.Point(33, 88);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new System.Drawing.Size(129, 39);
            btn_signup.TabIndex = 1;
            btn_signup.Text = "Signup";
            btn_signup.UseVisualStyleBackColor = true;
            btn_signup.Click += btn_signup_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(123, 48);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(130, 23);
            textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(123, 77);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(130, 23);
            textBox2.TabIndex = 3;
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(123, 106);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(130, 23);
            textBox3.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.Controls.Add(btn_login);
            panel1.Controls.Add(btn_signup);
            panel1.Location = new System.Drawing.Point(30, 51);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(200, 161);
            panel1.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 48);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(60, 15);
            label1.TabIndex = 6;
            label1.Text = "Username";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(14, 77);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(57, 15);
            label2.TabIndex = 7;
            label2.Text = "Password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(11, 109);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(104, 15);
            label3.TabIndex = 8;
            label3.Text = "Confirm Password";
            // 
            // panel2
            // 
            panel2.Controls.Add(lb_welcome);
            panel2.Controls.Add(btn_enter);
            panel2.Controls.Add(textBox3);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(textBox1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(textBox2);
            panel2.Controls.Add(label1);
            panel2.Location = new System.Drawing.Point(291, 51);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(275, 186);
            panel2.TabIndex = 9;
            // 
            // lb_welcome
            // 
            lb_welcome.AutoSize = true;
            lb_welcome.Location = new System.Drawing.Point(98, 18);
            lb_welcome.Name = "lb_welcome";
            lb_welcome.Size = new System.Drawing.Size(57, 15);
            lb_welcome.TabIndex = 10;
            lb_welcome.Text = "Welcome";
            // 
            // btn_enter
            // 
            btn_enter.Location = new System.Drawing.Point(75, 138);
            btn_enter.Name = "btn_enter";
            btn_enter.Size = new System.Drawing.Size(104, 23);
            btn_enter.TabIndex = 9;
            btn_enter.Text = "Login/Sign Up";
            btn_enter.UseVisualStyleBackColor = true;
            btn_enter.Click += btn_enter_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(633, 387);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "LoginForm";
            Text = "LoginForm";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.Button btn_signup;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_enter;
        private System.Windows.Forms.Label lb_welcome;
    }
}