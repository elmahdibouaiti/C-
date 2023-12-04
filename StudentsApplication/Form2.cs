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
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            da = new SqlDataAdapter("select * from Students", cn);
            cb = new SqlCommandBuilder(da);
            da.Fill(ds, "Students");
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
    }
}
