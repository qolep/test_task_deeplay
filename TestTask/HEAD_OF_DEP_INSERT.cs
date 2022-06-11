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
    public partial class HEAD_OF_DEP_INSERT : Form
        
    {
        private SqlConnection sqlConnection = null;

        private int emp_id;
        public HEAD_OF_DEP_INSERT(SqlConnection connection, int emp_id)
        {
            InitializeComponent();
            
            sqlConnection = connection;

            this.emp_id = emp_id;
        }

        private void HEAD_OF_DEP_INSERT_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "db_a883d5_workersDataSet2.departments". При необходимости она может быть перемещена или удалена.
            this.departmentsTableAdapter.Fill(this.db_a883d5_workersDataSet2.departments);

        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToString(comboBox1.SelectedValue));
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            SqlCommand DEL_THRASH = new SqlCommand("DELETE FROM [dir] WHERE emp_id=@emp_id", sqlConnection);  //очистка таблиц от записей о прошлых должностях
            DEL_THRASH.Parameters.AddWithValue("emp_id", emp_id);
            SqlCommand DEL_THRASH_1 = new SqlCommand("DELETE FROM [emp] WHERE emp_id=@emp_id", sqlConnection);
            DEL_THRASH_1.Parameters.AddWithValue("emp_id", emp_id);
            SqlCommand DEL_THRASH_2 = new SqlCommand("DELETE FROM [heads_of_department] WHERE emp_id=@emp_id", sqlConnection);
            DEL_THRASH_2.Parameters.AddWithValue("emp_id", emp_id);
            SqlCommand DEL_THRASH_3 = new SqlCommand("DELETE FROM [inspectors] WHERE emp_id=@emp_id", sqlConnection);
            DEL_THRASH_3.Parameters.AddWithValue("emp_id", emp_id);
            SqlCommand HOD_INSERT_employees = new SqlCommand("UPDATE [employees] SET title_ID=@title_ID, dept_ID=@dept_id WHERE emp_id=@emp_ID ", sqlConnection);
            // SqlCommand DIRECTOR_INSERT_employees = new SqlCommand("INSERT INTO [employees](title_ID, dept_ID)VALUES(@title_ID, @dept_ID  ) SELECT", sqlConnection);
            HOD_INSERT_employees.Parameters.AddWithValue("emp_ID", emp_id);
            HOD_INSERT_employees.Parameters.AddWithValue("title_ID", 7);
            HOD_INSERT_employees.Parameters.AddWithValue("dept_ID", comboBox1.SelectedValue);

            SqlCommand HOD_INSERT_hod = new SqlCommand("INSERT INTO [heads_of_department](emp_id, con_date)VALUES(@emp_id, @con_date)", sqlConnection);
            HOD_INSERT_hod.Parameters.AddWithValue("emp_id", emp_id);
            HOD_INSERT_hod.Parameters.AddWithValue("con_date", Convert.ToDateTime(textBox1.Text));


            try
            {
                await DEL_THRASH.ExecuteNonQueryAsync();
                await DEL_THRASH_1.ExecuteNonQueryAsync();
                await DEL_THRASH_2.ExecuteNonQueryAsync();
                await DEL_THRASH_3.ExecuteNonQueryAsync();
                await HOD_INSERT_employees.ExecuteNonQueryAsync();
                await HOD_INSERT_hod.ExecuteNonQueryAsync();

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
    }

