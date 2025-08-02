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
            label4 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            panel2 = new System.Windows.Forms.Panel();
            btn_switchToLogin = new System.Windows.Forms.Button();
            btn_switchToSignUp = new System.Windows.Forms.Button();
            lb_welcome = new System.Windows.Forms.Label();
            btn_enter = new System.Windows.Forms.Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // btn_login
            // 
            btn_login.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            btn_login.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_login.Location = new System.Drawing.Point(33, 77);
            btn_login.Name = "btn_login";
            btn_login.Size = new System.Drawing.Size(129, 39);
            btn_login.TabIndex = 0;
            btn_login.Text = "Login";
            btn_login.UseVisualStyleBackColor = false;
            btn_login.Click += btn_login_Click;
            // 
            // btn_signup
            // 
            btn_signup.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            btn_signup.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_signup.Location = new System.Drawing.Point(33, 134);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new System.Drawing.Size(129, 39);
            btn_signup.TabIndex = 1;
            btn_signup.Text = "Signup";
            btn_signup.UseVisualStyleBackColor = false;
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
            panel1.Controls.Add(label4);
            panel1.Controls.Add(btn_login);
            panel1.Controls.Add(btn_signup);
            panel1.Location = new System.Drawing.Point(89, 69);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(200, 208);
            panel1.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label4.Location = new System.Drawing.Point(33, 27);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(129, 21);
            label4.TabIndex = 2;
            label4.Text = "Tic Tac Toe chat";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(14, 48);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(64, 15);
            label1.TabIndex = 6;
            label1.Text = "Username";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(14, 77);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(59, 15);
            label2.TabIndex = 7;
            label2.Text = "Password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(11, 109);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(107, 15);
            label3.TabIndex = 8;
            label3.Text = "Confirm Password";
            // 
            // panel2
            // 
            panel2.Controls.Add(btn_switchToLogin);
            panel2.Controls.Add(btn_switchToSignUp);
            panel2.Controls.Add(lb_welcome);
            panel2.Controls.Add(btn_enter);
            panel2.Controls.Add(textBox3);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(textBox1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(textBox2);
            panel2.Controls.Add(label1);
            panel2.Location = new System.Drawing.Point(51, 50);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(279, 253);
            panel2.TabIndex = 9;
            // 
            // btn_switchToLogin
            // 
            btn_switchToLogin.BackColor = System.Drawing.SystemColors.InactiveCaption;
            btn_switchToLogin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btn_switchToLogin.Location = new System.Drawing.Point(72, 214);
            btn_switchToLogin.Name = "btn_switchToLogin";
            btn_switchToLogin.Size = new System.Drawing.Size(203, 23);
            btn_switchToLogin.TabIndex = 12;
            btn_switchToLogin.Text = "Already have an account? Login";
            btn_switchToLogin.UseVisualStyleBackColor = false;
            btn_switchToLogin.Click += btn_switchToLogin_Click;
            // 
            // btn_switchToSignUp
            // 
            btn_switchToSignUp.BackColor = System.Drawing.SystemColors.InactiveCaption;
            btn_switchToSignUp.Location = new System.Drawing.Point(72, 214);
            btn_switchToSignUp.Name = "btn_switchToSignUp";
            btn_switchToSignUp.Size = new System.Drawing.Size(203, 23);
            btn_switchToSignUp.TabIndex = 11;
            btn_switchToSignUp.Text = "Don’t have an account? Sign up";
            btn_switchToSignUp.UseVisualStyleBackColor = false;
            btn_switchToSignUp.Click += btn_switchToSignUp_Click;
            // 
            // lb_welcome
            // 
            lb_welcome.AutoSize = true;
            lb_welcome.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_welcome.Location = new System.Drawing.Point(14, 15);
            lb_welcome.Name = "lb_welcome";
            lb_welcome.Size = new System.Drawing.Size(82, 21);
            lb_welcome.TabIndex = 10;
            lb_welcome.Text = "Welcome";
            // 
            // btn_enter
            // 
            btn_enter.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            btn_enter.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btn_enter.Location = new System.Drawing.Point(72, 151);
            btn_enter.Name = "btn_enter";
            btn_enter.Size = new System.Drawing.Size(122, 35);
            btn_enter.TabIndex = 9;
            btn_enter.Text = "Login/Sign Up";
            btn_enter.UseVisualStyleBackColor = false;
            btn_enter.Click += btn_enter_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.MenuHighlight;
            ClientSize = new System.Drawing.Size(384, 351);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "LoginForm";
            Text = "LoginForm";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private System.Windows.Forms.Button btn_switchToLogin;
        private System.Windows.Forms.Button btn_switchToSignUp;
        private System.Windows.Forms.Label label4;
    }
}