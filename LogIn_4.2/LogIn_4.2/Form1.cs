using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace LogIn_4._2
{
    public partial class Form1 : Form
    {
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\derek.ho\Documents\UserDatabase.mdb");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'appData.Login' table. You can move, or remove it, as needed.
            this.loginTableAdapter.Fill(this.appData.Login);
            dataGridView.Rows.Add(9);           //Declare empty row for the table
            Display(true);                     //Disable all button
            for (int i = 1; i < 9; i++)         //Display the 1st row only
            {
                dataGridView.Rows[i].ReadOnly = false;
                dataGridView.Rows[i].Visible  = true;
            }
            txtApp_ID.Select();
        }

        private void Display(object sender)     //Function to enable and disable the button and textbox
        {
            
            if (sender.Equals(false))
            {
                txtApp_ID.Clear(); txtApp_Pwd.Clear();
                btnApprove.Enabled = false;
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
                txtApp_ID.Focus();
            }
            else if (sender.Equals(true))
            {
                btnApprove.Enabled = true;
                btnDelete.Enabled = true;
                btnEdit.Enabled = true;
            }
        }

        private void TxtApp_ID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                txtApp_Pwd.Focus();
        }

        private void TxtApp_Pwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if ((String.IsNullOrEmpty(txtApp_ID.Text)) || (String.IsNullOrEmpty(txtApp_Pwd.Text)))
                {
                    MessageBox.Show("Invalid Login", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Display(false);
                    return;
                }
                try
                {
                    AppDataTableAdapters.LoginTableAdapter login = new AppDataTableAdapters.LoginTableAdapter();
                    AppData.LoginDataTable dt = login.GetDataByIDPassword(txtApp_ID.Text, txtApp_Pwd.Text);
                    if(dt.Rows.Count == 1)
                    {
                        MessageBox.Show("Vaild login", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Display(true);
                        txtApp_ID.Enabled = false;
                        txtApp_Pwd.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Login", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Display(false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            con.Open();
            OleDbCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if ((Convert.ToBoolean(row.Cells[0].Value) == true))        //check for the tick box is selected
                {
                    cmd.CommandText = "insert into Login (EmployeeID,Name) values ('" + row.Cells[1] + "','" + row.Cells[2] + "')";
                    //,EmployeeID,Access,Permission
                    cmd.ExecuteNonQuery();
                }
            }
            con.Close();
            MessageBox.Show("User registered successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }



    }
}
/*
foreach (DataGridViewCell cell in row.Cells)            //check for each cell to make sure is not null
                    {
                        if (cell.Value != null)
                        {
                            Valid_count++;
                        }
                        else
                        {
                            MessageBox.Show("NULL!!");
                        }
                    }
*/