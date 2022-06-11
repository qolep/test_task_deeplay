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
    public partial class UPDATE : Form
    {
        private SqlConnection sqlConnection = null;

        private int emp_id;
        public UPDATE(SqlConnection connection, int id)
        {
            InitializeComponent();

            sqlConnection = connection;

            this.emp_id = id;

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlCommand updateWorkerCommand = new SqlCommand("UPDATE [employees] SET [first_name]=@first_name, [last_name]=@last_name, [patronymic]=@patronymic, [birth_date]=@birth_date, [gender]=@gender WHERE [emp_id]=@emp_id", sqlConnection);

            updateWorkerCommand.Parameters.AddWithValue("emp_id", emp_id);
            updateWorkerCommand.Parameters.AddWithValue("first_name", textBox1.Text);
            updateWorkerCommand.Parameters.AddWithValue("last_name", textBox2.Text);
            updateWorkerCommand.Parameters.AddWithValue("patronymic", textBox3.Text);
            updateWorkerCommand.Parameters.AddWithValue("gender", textBox4.Text);
            updateWorkerCommand.Parameters.AddWithValue("birth_date", Convert.ToDateTime(textBox5.Text));

            try
            {
                await updateWorkerCommand.ExecuteNonQueryAsync();

                Close();

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void UPDATE_Load(object sender, EventArgs e)
        {
            SqlCommand getWorkerInfoCommand = new SqlCommand("SELECT [first_name], [last_name], [patronymic], [birth_date], [gender] FROM [employees] WHERE [emp_id]=@emp_id", sqlConnection);

            getWorkerInfoCommand.Parameters.AddWithValue("emp_id", emp_id);

            SqlDataReader sqlReader = null;

            try
            {
                sqlReader = await getWorkerInfoCommand.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    textBox1.Text = Convert.ToString(sqlReader["first_name"]);

                    textBox2.Text = Convert.ToString(sqlReader["last_name"]);

                    textBox3.Text = Convert.ToString(sqlReader["patronymic"]);

                    textBox4.Text = Convert.ToString(sqlReader["gender"]);

                    textBox5.Text = Convert.ToString(sqlReader["birth_date"]);

                }
            }

            catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                {
                    sqlReader.Close();
                        }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
