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

namespace TestTask
{
    public partial class INSERT : Form
    {
        private SqlConnection sqlconnection=null;
        public INSERT(SqlConnection connection)
        {
            InitializeComponent();

            sqlconnection = connection;
        }

        private void INSERT_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "db_a883d5_workersDataSet1.titles". При необходимости она может быть перемещена или удалена.
            this.titlesTableAdapter.Fill(this.db_a883d5_workersDataSet1.titles);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "db_a883d5_workersDataSet2.departments". При необходимости она может быть перемещена или удалена.
            this.departmentsTableAdapter.Fill(this.db_a883d5_workersDataSet2.departments);

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlCommand insertHODCommand = new SqlCommand("INSERT INTO [employees] (birth_date, first_name, last_name, patronymic, gender)VALUES(@birth_date, @first_name, @last_name, @patronymic, @gender)", sqlconnection);

            insertHODCommand.Parameters.AddWithValue("birth_date", Convert.ToDateTime(textBox5.Text));
            insertHODCommand.Parameters.AddWithValue("first_name", textBox1.Text);
            insertHODCommand.Parameters.AddWithValue("last_name", textBox2.Text);
            insertHODCommand.Parameters.AddWithValue("patronymic", textBox3.Text);
            insertHODCommand.Parameters.AddWithValue("gender", textBox4.Text);

            try
            {
                await insertHODCommand.ExecuteNonQueryAsync();

                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally {
                Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }


    }
}
