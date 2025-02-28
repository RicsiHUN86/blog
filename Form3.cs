using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using static blog.Form1;

namespace blog
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            ListComments();
        }
        private const string ConnectionString = "Server=localhost;Database=blog;Uid=root;Password=;SslMode=None";
        private void btnSend_Click(object sender, EventArgs e)
        {
            string comment = textBox1.Text.Trim();
            if (!string.IsNullOrWhiteSpace(comment))
            {
                string sql = "INSERT INTO blogtable (Title, Comment, UserId) VALUES (@title, @comment, @userid)";

                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@title", "N/A");
                        command.Parameters.AddWithValue("@comment", comment);
                        command.Parameters.AddWithValue("@userid", UserId.Id);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    ListComments();
            }
                listBox1.Items.Add($"You: {comment}");
                textBox1.Clear();
            }
        }

        private bool ListComments()
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT blogtable.Comment, usertable.UserName FROM usertable RIGHT JOIN blogtable ON usertable.Id = blogtable.UserId;";

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        var comment = new
                        {
                            Comment = dr.GetString(0),
                            User = dr.GetString(1),
                            UserId.Id
                        };
                    }
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba történt: " + ex.Message);
                return false;
            }
        }
    }
}
