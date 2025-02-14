using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace blog
{
    public partial class Form2 : Form
    {
        private string connectionString = "Server=localhost;Database=Blog;Uid=root;Pwd=;"; // Állítsd be a saját MySQL adataidat!
        private int loggedInUserId; // Az aktuálisan bejelentkezett felhasználó ID-je

        public Form2(int userId)
        {
            InitializeComponent();
            loggedInUserId = userId;
            LoadComments();
        }

        private void LoadComments()
        {
            listBox1.Items.Clear();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT BlogTable.Comment, UserTable.UserName FROM BlogTable " +
                               "JOIN UserTable ON BlogTable.UserId = UserTable.Id " +
                               "ORDER BY BlogTable.Id DESC";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string comment = $"{reader["UserName"]}: {reader["Comment"]}";
                        listBox1.Items.Add(comment);
                    }
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)) // <-- textBox1 használata
            {
                MessageBox.Show("A hozzászólás nem lehet üres!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO BlogTable (Title, Comment, UserId) VALUES (@title, @comment, @userId)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", "N/A"); // Ha nincs szükség címre, alapértelmezett érték
                        cmd.Parameters.AddWithValue("@comment", textBox1.Text);
                        cmd.Parameters.AddWithValue("@userId", loggedInUserId); // Ellenőrizd, hogy az userId helyesen van beállítva!

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Komment hozzáadva!", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox1.Clear();
                            LoadComments(); // Frissíti a listát
                        }
                        else
                        {
                            MessageBox.Show("Nem sikerült a komment hozzáadása!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
