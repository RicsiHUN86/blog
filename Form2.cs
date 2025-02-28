using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace blog
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private const string ConnectionString = "Server=localhost;Database=blog;Uid=root;Password=;SslMode=None";
        private string AddNexBlogger(string username, string email, string password)
        {
            try
            {
                string sql = "INSERT INTO usertable(UserName, Email, Password) VALUES (@username,@email,@password)";

                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                    this.Close();
                    return "Sikeres regisztráció.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(AddNexBlogger(textBox5.Text, textBox6.Text, textBox7.Text));
        }
    }
}