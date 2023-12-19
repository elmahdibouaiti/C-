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
using System.Data.Common;
using System.Reflection.Emit;

namespace StudentsApplication
{
    public partial class Form2 : Form
    {
        SqlConnection cn = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=cn;Integrated Security=True;");
        SqlDataAdapter da;
        DataSet ds = new DataSet();
        SqlCommandBuilder cb;
        DataRow r;
        int i;
        private int currentPosition = 0;
        public Form2()
        {
            InitializeComponent();
            this.Load += Form2_Load;

            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                // Charger les données depuis la base de données dans le DataSet
                da = new SqlDataAdapter("select * from Students", cn);
                cb = new SqlCommandBuilder(da);
                da.Fill(ds, "Students");

                // Remplir le ComboBox avec les IDs des étudiants
                comboBox1.DataSource = ds.Tables["Students"];
                comboBox1.DisplayMember = "Id"; 
                comboBox1.ValueMember = "Id";

                // Charger les données depuis la base de données dans le DataSet pour la table 'option'
                SqlDataAdapter optionDa = new SqlDataAdapter("select * from [option]", cn);

                DataSet optionDs = new DataSet();
                optionDa.Fill(optionDs, "Option");

                comboBox2.DataSource = optionDs.Tables["Option"];
                comboBox2.DisplayMember = "nom";
                comboBox2.ValueMember = "Id";



            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
            }
        }

        private void ShowData(int position)
        {
            if (ds.Tables["Students"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Students"].Rows[position];
                textBox1.Text = row[0].ToString();
                textBox2.Text = row[1].ToString();
                textBox3.Text = row[2].ToString();
                textBox4.Text = row[3].ToString();
                textBox5.Text = row[4].ToString();
            }
            else
            {
                // Effacer les champs s'il n'y a pas de données
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            r = ds.Tables["Students"].NewRow();
            r[0] = int.Parse(textBox1.Text);
            r[1] = textBox2.Text;
            r[2] = textBox3.Text;
            r[3] = float.Parse(textBox4.Text);
            r[4] = int.Parse(textBox5.Text);
            ds.Tables["students"].Rows.Add(r);
            da.Update(ds, "Students");

            MessageBox.Show("student added by successeful");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                /*DataRow rowToUpdate = ds.Tables["Students"].Rows[i];

                
                rowToUpdate["nom"] = textBox2.Text;
                rowToUpdate["prenom"] = textBox3.Text;
                rowToUpdate["note"] = float.Parse(textBox4.Text);
                rowToUpdate["option"] = int.Parse(textBox5.Text);*/
                bool tr = false;
                foreach(DataRow r in ds.Tables["Students"].Rows)
                {
                    if(textBox1.Text == r[0].ToString())
                    {
                        tr = true;
                        r["nom"] = textBox2.Text;
                        r["prenom"] = textBox3.Text;
                        r["note"] = float.Parse(textBox4.Text);
                        r["option"] = int.Parse(textBox5.Text);
                        break;
                    }
                }

                if (tr)
                {
                    // Update the database with the changes
                    da.Update(ds, "Students");
                    MessageBox.Show("Student updated successfully!");
                }

                else
                {
                    MessageBox.Show("didn't find student");
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Assuming 'i' holds the index of the row you want to delete
                ds.Tables["Students"].Rows[i].Delete();

                // Update the database to reflect the deletion
                da.Update(ds, "Students");

                MessageBox.Show("Student deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                double totalNotes = 0;
                int numberOfStudents = ds.Tables["Students"].Rows.Count;

                // Parcourir chaque ligne et additionner les notes
                foreach (DataRow row in ds.Tables["Students"].Rows)
                {
                    totalNotes += Convert.ToDouble(row["note"]);
                }

                // Calculer la moyenne
                double average = totalNotes / numberOfStudents;

                // Afficher la moyenne dans une étiquette (label)
                label6.Text = "Moyenne des notes : " + average.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du calcul de la moyenne : " + ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //next
            if (currentPosition < ds.Tables["Students"].Rows.Count - 1)
            {
                currentPosition++;
                ShowData(currentPosition);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //previous
            if (currentPosition > 0)
            {
                currentPosition--;
                ShowData(currentPosition);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //first
            currentPosition = 0;
            ShowData(currentPosition);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //last 
            currentPosition = ds.Tables["Students"].Rows.Count - 1;
            ShowData(currentPosition);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                DataRowView selectedRow = comboBox1.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    int selectedID = Convert.ToInt32(selectedRow["Id"]); 
                    DataRow[] foundRows = ds.Tables["Students"].Select("Id = " + selectedID); 

                    if (foundRows.Length > 0)
                    {
                        int index = ds.Tables["Students"].Rows.IndexOf(foundRows[0]);
                        ShowData(index);
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue != null)
            {
                DataRowView selectedOption = comboBox2.SelectedItem as DataRowView;

                if (selectedOption != null)
                {
                    int selectedOptionID = Convert.ToInt32(selectedOption["Id"]); 
                    DataRow[] foundRows = ds.Tables["Students"].Select("option = " + selectedOptionID); 

                    if (foundRows.Length > 0)
                    {
                        int index = ds.Tables["Students"].Rows.IndexOf(foundRows[0]);
                        ShowData(index);
                    }
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox6.Text.Trim(); 

            if (!string.IsNullOrEmpty(searchTerm))
            {
                
                DataRow[] foundRows = ds.Tables["Students"].Select("nom LIKE '%" + searchTerm + "%'");

                if (foundRows.Length > 0)
                {
                    
                    DataSet searchResultDataSet = new DataSet();
                    DataTable searchResultTable = foundRows.CopyToDataTable();
                    searchResultDataSet.Tables.Add(searchResultTable);

                    
                    dataGridView1.DataSource = searchResultDataSet.Tables[0];
                }
                else
                {
                    
                    MessageBox.Show("Aucun étudiant trouvé avec ce nom.");
                }
            }
            else
            {
                
                MessageBox.Show("Veuillez saisir un nom pour effectuer la recherche.");
            }
        }
    }
}
