using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace CreateReadUpdateDeleteProject
{
    public partial class Form1 : Form
    {
        SqlConnection connect
            = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\WINDOWS 10\Documents\crud.mdf;Integrated Security=True;Connect Timeout=30");
        public Form1()
        {
            InitializeComponent();

            // TO DISPLAY THE DATA TO YOUR DATA GRID VIEW
            displayData();
        }

        public void displayData()
        {
            UserListData uld = new UserListData();

            List<UserListData> listData = uld.getListData();
            dataGridView1.DataSource = listData;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if(full_name.Text == ""
                || gender.Text == ""
                || contact_number.Text == ""
                || email.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(connect.State != ConnectionState.Open)
                {
                    try
                    {
                        // TO GET THE DATE TODAY
                        DateTime today = DateTime.Today;
                        connect.Open();

                        string insertData = "INSERT INTO users " +
                            "(full_Name, gender, contact, email, bith_date, date_insert) " +
                            "VALUES(@fullName, @gender, @contact, @email, @birthDate, @dateInsert)";

                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@fullName", full_name.Text.Trim());
                            cmd.Parameters.AddWithValue("@gender", gender.Text.Trim());
                            cmd.Parameters.AddWithValue("@contact", contact_number.Text.Trim());
                            cmd.Parameters.AddWithValue("@email", email.Text.Trim());
                            cmd.Parameters.AddWithValue("@birthDate", birth_date.Value);
                            cmd.Parameters.AddWithValue("@dateInsert", today);

                            cmd.ExecuteNonQuery();

                            // TO DISPLAY THE DATA  
                            displayData();

                            MessageBox.Show("Added successfully!", "Information Message"
                                , MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // TO CLEAR ALL FIELDS
                            clearFields();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error: " + ex, "Error Message"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }

        public void clearFields()
        {
            full_name.Text = "";
            gender.SelectedIndex = -1;
            contact_number.Text = "";
            email.Text = "";
        }

        private int tempID = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                tempID = (int)row.Cells[0].Value;
                full_name.Text = row.Cells[1].Value.ToString();
                gender.Text = row.Cells[2].Value.ToString();
                contact_number.Text = row.Cells[3].Value.ToString();
                email.Text = row.Cells[4].Value.ToString();
                birth_date.Text = row.Cells[5].Value.ToString();
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (full_name.Text == ""
                || gender.Text == ""
                || contact_number.Text == ""
                || email.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Are you sure you want to Update ID: "
                    + tempID + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if(check == DialogResult.Yes)
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        try
                        {
                            // TO GET THE DATE TODAY
                            DateTime today = DateTime.Today;
                            connect.Open();

                            string updateData = "UPDATE users SET " +
                                "full_name = @fullName, gender = @gender, " +
                                "contact = @contact, email = @email, " +
                                "bith_date = @birthDate, date_update = @dateUpdate " +
                                "WHERE id = @id";

                            using(SqlCommand cmd = new SqlCommand(updateData, connect))
                            {
                                cmd.Parameters.AddWithValue("@fullName", full_name.Text.Trim());
                                cmd.Parameters.AddWithValue("@gender", gender.Text.Trim());
                                cmd.Parameters.AddWithValue("@contact", contact_number.Text.Trim());
                                cmd.Parameters.AddWithValue("@email", email.Text.Trim());
                                cmd.Parameters.AddWithValue("@birthDate", birth_date.Value);
                                cmd.Parameters.AddWithValue("@dateUpdate", today);
                                cmd.Parameters.AddWithValue("@id", tempID);

                                cmd.ExecuteNonQuery();

                                // TO DISPLAY THE DATA  
                                displayData();

                                MessageBox.Show("Updated successfully!", "Information Message"
                                    , MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // TO CLEAR ALL FIELDS
                                clearFields();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex, "Error Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connect.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Cancelled.", "Inforamtion Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (full_name.Text == ""
                || gender.Text == ""
                || contact_number.Text == ""
                || email.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Are you sure you want to Delete ID: "
                    + tempID + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        try
                        {
                            // TO GET THE DATE TODAY
                            DateTime today = DateTime.Today;
                            connect.Open();

                            string updateData = "DELETE FROM users WHERE id = @id";

                            using (SqlCommand cmd = new SqlCommand(updateData, connect))
                            {
                                cmd.Parameters.AddWithValue("@id", tempID);

                                cmd.ExecuteNonQuery();

                                // TO DISPLAY THE DATA  
                                displayData();

                                MessageBox.Show("Deleted successfully!", "Information Message"
                                    , MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // TO CLEAR ALL FIELDS
                                clearFields();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex, "Error Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connect.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Cancelled.", "Inforamtion Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }
    }
}


// THATS IT FOR THIS VIDEO, THANKS FOR WATCHING !! : )
// BTW, THANKS FOR YOUR SUPPORT, GUYS!
// WE ALMOST REACH 2K SUBSCRIBERS!! I'M SO HAPPY : ) 
// THANKS AGAIN!
// SEE YOU IN THE NEXT VIDEO TUTORIAL!! 