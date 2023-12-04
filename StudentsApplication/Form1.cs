using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StudentsApplication
{
    public partial class Form1 : Form
    {
        // Établissement de la connexion à la base de données SQL Server
        SqlConnection cn = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=cn;Integrated Security=True;");
        // Initialisation des objets SqlCommand, SqlDataReader et DataTable, ainsi que d'une variable d'itération
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        DataTable d = new DataTable();
        int i;


        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //ajouter un etudiant
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                int id = int.Parse(textBox4.Text); 
                string nom = textBox1.Text;
                string prenom = textBox2.Text;
                float note = float.Parse(textBox3.Text);

                string insertQuery = "INSERT INTO Students (id, nom, prenom, note) VALUES (@id, @nom, @prenom, @note)";

                cmd.Parameters.Clear();
                cmd.Connection = cn;
                cmd.CommandText = insertQuery;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@note", note);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Student added successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to add student.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }
        //modifier
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();

                int id = int.Parse(textBox4.Text);
                string nom = textBox1.Text;
                string prenom = textBox2.Text;
                float note = float.Parse(textBox3.Text);

                string updateQuery = "UPDATE Students SET nom = @nom, prenom = @prenom, note = @note WHERE id = @id";

                cmd.Parameters.Clear();
                cmd.Connection = cn;
                cmd.CommandText = updateQuery;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@note", note);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Student updated successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to update student.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        
        }
        //supprimer
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();

                int id = int.Parse(textBox4.Text);
                

                string deleteQuery = "DELETE FROM Students WHERE id = @id";

                cmd.Parameters.Clear();
                cmd.Connection = cn;
                cmd.CommandText = deleteQuery;
                cmd.Parameters.AddWithValue("@id", id);
                

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Student deleted successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to delete student.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }
        // Chargement initial du formulaire
        private void Form1_Load(object sender, EventArgs e)
        {
            cn.Open();
            string req = "select * from Students";
            cmd = new SqlCommand(req, cn);
            dr = cmd.ExecuteReader();
            d.Load(dr);
            dr.Close();
            cn.Close();
            i = 0;
            Afficher(i);
        }
        public void Afficher(int j)
        {
            textBox4.Text = d.Rows[j][0].ToString();
            textBox1.Text = d.Rows[j][1].ToString();
            textBox2.Text = d.Rows[j][2].ToString();
            textBox3.Text = d.Rows[j][3].ToString();

            MettreAjourClassement(j);
        }
        public void MettreAjourClassement(int positionActuelle)
        {
            int studentRank = positionActuelle + 1;
            label6.Text = $"{studentRank}/{d.Rows.Count}";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Fisrt 
            i = 0;
            Afficher(i);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Previous
            i--;
            Afficher(i);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Next
            i++;
            Afficher(i);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Last 
            i = d.Rows.Count - 1;
            Afficher(i);
        }
        //search
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = textBox5.Text;

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                DataRow[] foundRows = d.Select($"Nom LIKE '%{searchKeyword}%' OR Prenom LIKE '%{searchKeyword}%'");

                if (foundRows.Length > 0)
                {
                    // Si des étudiants sont trouvés, affiche le premier résultat
                    i = d.Rows.IndexOf(foundRows[0]);
                    Afficher(i);
                }
                else
                {
                    // Aucun étudiant trouvé, efface les champs
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                }
            }
            else
            {
                // Si la zone de recherche est vide, affiche le premier étudiant
                i = 0;
                Afficher(i);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
        }

        private void form2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
