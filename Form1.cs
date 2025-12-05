using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ПМ._05
{
    public partial class Form1 : Form
    {
        private int failedAttempts = 0;
        private bool isBlocked = false;
        private Timer blockTimer;
        private int blockTimeRemaining = 0;

        
        private List<User> users = new List<User>();

        public Form1()
        {
            InitializeComponent();
            InitializeBlockTimer();
            InitializeUsers(); 
            UpdateUI();
        }

        public class User
        {
            public string Role { get; set; }
            public string FullName { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }

        private void InitializeUsers()
        {
            // Администраторы
            users.Add(new User { Role = "Администратор", FullName = "Пахомова Аиша Анатольевна", Login = "m4ic8j5qgstw@gmail.com", Password = "2L6KZG" });
            users.Add(new User { Role = "Администратор", FullName = "Жуков Роман Богданович", Login = "d43zfg9tlsyv@gmail.com", Password = "uzWC67" });
            users.Add(new User { Role = "Администратор", FullName = "Киселева Анастасия Максимовна", Login = "8ohgisf6k45w@outlook.com", Password = "8ntwUp" });

            // Менеджеры
            users.Add(new User { Role = "Менеджер", FullName = "Григорьева Арина Арсентьевна", Login = "hi1brwj46czx@mail.com", Password = "YOyhfR" });
            users.Add(new User { Role = "Менеджер", FullName = "Иванов Лев Михайлович", Login = "fvkbcamhlj52@gmail.com", Password = "RSbvHv" });
            users.Add(new User { Role = "Менеджер", FullName = "Григорьев Лев Давидович", Login = "9qxnce8jwruv@gmail.com", Password = "rwVDh9" });

            // Клиенты
            users.Add(new User { Role = "Клиент", FullName = "Поляков Степан Егорович", Login = "dotiex942p1r@gmail.com", Password = "LdNyos" });
            users.Add(new User { Role = "Клиент", FullName = "Леонова Алиса Кирилловна", Login = "n0bmi2h1xral@tutanota.com", Password = "gynQMT" });
            users.Add(new User { Role = "Клиент", FullName = "Яковлев Платон Константинович", Login = "sfm3t278kdvz@yahoo.com", Password = "AtnDjr" });
            users.Add(new User { Role = "Клиент", FullName = "Ковалева Ева Яковлевна", Login = "ilb8rdut0v7e@mail.com", Password = "JlFRCZ" });
        }

        private void InitializeBlockTimer()
        {
            blockTimer = new Timer();
            blockTimer.Interval = 1000;
            blockTimer.Tick += BlockTimer_Tick;
        }

        private void BlockTimer_Tick(object sender, EventArgs e)
        {
            blockTimeRemaining--;

            if (blockTimeRemaining <= 0)
            {
                blockTimer.Stop();
                isBlocked = false;
                UpdateUI();
            }
            else
            {
                UpdateBlockMessage();
            }
        }

        private void UpdateUI()
        {
            if (isBlocked)
            {
                button1.Enabled = false;
                button1.Text = $"Заблокировано ({blockTimeRemaining} сек)";
                button2.Enabled = false;
                label5.Text = $"Система заблокирована. Осталось: {blockTimeRemaining} сек";
                label5.Visible = true;
            }
            else
            {
                button1.Enabled = true;
                button1.Text = "Войти";
                button2.Enabled = true;

                if (failedAttempts > 0)
                {
                    label4.Visible = true;
                    textBox3.Visible = true;
                    pictureBox1.Visible = true;
                    label4.Text = "Введите текст с картинки";
                    label5.Visible = false;
                }
                else
                {
                    label4.Visible = false;
                    textBox3.Visible = false;
                    pictureBox1.Visible = false;
                    label5.Visible = false;
                }
            }
        }

        private void UpdateBlockMessage()
        {
            label5.Text = $"Система заблокирована. Осталось: {blockTimeRemaining} сек";
            button1.Text = $"Заблокировано ({blockTimeRemaining} сек)";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isBlocked)
            {
                MessageBox.Show($"Система заблокирована. Попробуйте через {blockTimeRemaining} секунд.",
                    "Блокировка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string login = textBox1.Text.Trim();
            string password = textBox2.Text;
            string captcha = textBox3.Text;

           
            if (failedAttempts > 0)
            {
                if (captcha != "uU2Dd9")
                {
                    failedAttempts++;
                    MessageBox.Show("Неверная CAPTCHA!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (failedAttempts >= 2)
                    {
                        BlockSystem();
                    }
                    else
                    {
                        UpdateUI();
                    }
                    return;
                }
            }

            
            if (CheckCredentials(login, password))
            {
                failedAttempts = 0;
                OpenUserForm(login);
            }
            else
            {
                failedAttempts++;
                MessageBox.Show("Неверный логин или пароль!", "Ошибка авторизации",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (failedAttempts >= 2)
                {
                    BlockSystem();
                }
                else
                {
                    UpdateUI();
                }
            }
        }

        private bool CheckCredentials(string login, string password)
        {
            var user = users.FirstOrDefault(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            return user != null;
        }

        private User GetUserByLogin(string login)
        {
            return users.FirstOrDefault(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
        }

        private void BlockSystem()
        {
            isBlocked = true;
            blockTimeRemaining = 10;
            blockTimer.Start();
            UpdateUI();
        }

        private void OpenUserForm(string login)
        {
            var user = GetUserByLogin(login);

            if (user == null)
            {
                MessageBox.Show("Пользователь не найден!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string userInfo = $"{user.Role}: {user.FullName}";

            if (user.Role == "Администратор")
            {
                AdminWindow adminForm = new AdminWindow();
                adminForm.CurrentUserName = userInfo; 
                adminForm.CurrentUserRole = user.Role; 
                adminForm.Show();
            }
            else if (user.Role == "Менеджер")
            {
               
                ClientForm clientForm = new ClientForm();
                clientForm.CurrentUserName = userInfo;
                clientForm.CurrentUserRole = user.Role;
                clientForm.Show();
            }
            else 
            {
                ClientForm clientForm = new ClientForm();
                clientForm.CurrentUserName = userInfo;
                clientForm.CurrentUserRole = user.Role;
                clientForm.Show();
            }

            this.Hide();
            ClearFields();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClientForm clientForm = new ClientForm();
            clientForm.CurrentUserName = "Гость";
            clientForm.CurrentUserRole = "Гость";
            clientForm.Show();
            this.Hide();
            ClearFields();
        }

        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            failedAttempts = 0;
            UpdateUI();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (blockTimer != null)
            {
                blockTimer.Stop();
                blockTimer.Dispose();
            }
        }

        private void label4_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
    }
}