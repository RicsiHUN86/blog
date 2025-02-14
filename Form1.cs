using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace blog
{
    public partial class Form1 : Form
    {
        private MySqlConnection connection;
        private string loggedInUser = "";

        public Form1()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            string connectionString = "SERVER=localhost;DATABASE=Blog;UID=root;PASSWORD=;SslMode=None";
            connection = new MySqlConnection(connectionString);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox3.Text.Trim();
            string password = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Kérlek töltsd ki mindkét mezőt!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                connection.Open();
                string query = "SELECT * FROM UserTable WHERE UserName = @username AND Password = @password";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    loggedInUser = username;
                    MessageBox.Show("Sikeres bejelentkezés!", "Üdvözöljük", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    reader.Close();
                    this.Hide();
                    Form2 form2 = new Form2(loggedInUser); 
                    form2.ShowDialog();
                    this.Close(); 
                }
                else
                {
                    MessageBox.Show("Hibás felhasználónév vagy jelszó!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string username = textBox5.Text.Trim();
            string email = textBox6.Text.Trim();
            string password = textBox7.Text.Trim();

            try
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM UserTable WHERE UserName = @username";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                checkCmd.Parameters.AddWithValue("@username", username);
                int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (userExists > 0)
                {
                    MessageBox.Show("Ez a felhasználónév már foglalt!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string insertQuery = "INSERT INTO UserTable (UserName, Email, Password) VALUES (@username, @email, @password)";
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                insertCmd.Parameters.AddWithValue("@username", username);
                insertCmd.Parameters.AddWithValue("@email", email);
                insertCmd.Parameters.AddWithValue("@password", password);
                insertCmd.ExecuteNonQuery();

                MessageBox.Show("Sikeres regisztráció! Most bejelentkezhetsz.", "Üdvözöljük", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
