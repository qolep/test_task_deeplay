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
using System.Configuration;

namespace TestTask
{
    public partial class Form2 : Form
    {
        private SqlConnection sqlConnection = null;
        public Form2()
        {
            InitializeComponent();
        }
        private async void Form2_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["1"].ConnectionString; //получение доступа к строке подключения

            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync(); //open connection

            listView1.FullRowSelect=true;
            listView1.Columns.Add("id");
            listView1.Columns.Add("Направление деятельности"); //for directors

            listView2.FullRowSelect = true;
            listView2.Columns.Add("id");
            listView2.Columns.Add("Опыт работы"); //for workers

            listView3.FullRowSelect = true;
            listView3.Columns.Add("id");
            listView3.Columns.Add("Дата окончания контракта"); //for heads of department

            listView4.FullRowSelect = true;
            listView4.Columns.Add("id");
            listView4.Columns.Add("Область ответственности"); //for inspectors

            await LoadInfoAsync_dir();
            await LoadInfoAsync_worker();
            await LoadInfoAsync_head_of_dept();
            await LoadInfoAsync_inspector();

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

        private async Task LoadInfoAsync_dir() //select
        {
            SqlDataReader sqlReader = null;

            SqlCommand LoadInfo_dir = new SqlCommand("SELECT * FROM [dir]", sqlConnection);


            try
            {
                sqlReader = await LoadInfo_dir.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["emp_id"]),
                    Convert.ToString(sqlReader["dir_type"])
                    });

                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
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

        private async Task LoadInfoAsync_worker() //select
        {
            SqlDataReader sqlReader = null;

            SqlCommand LoadInfo_worker = new SqlCommand("SELECT * FROM [emp]", sqlConnection);


            try
            {
                sqlReader = await LoadInfo_worker.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["emp_id"]),
                    Convert.ToString(sqlReader["work_exp"])
                    });

                    listView2.Items.Add(item);
                }
            }
            catch (Exception ex)
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

        private async Task LoadInfoAsync_head_of_dept() //select
        {
            SqlDataReader sqlReader = null;

            SqlCommand LoadInfo_head_of_dept = new SqlCommand("SELECT * FROM [heads_of_department]", sqlConnection);


            try
            {
                sqlReader = await LoadInfo_head_of_dept.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["emp_id"]),
                    Convert.ToString(sqlReader["con_date"])
                    });

                    listView3.Items.Add(item);
                }
            }
            catch (Exception ex)
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

        private async Task LoadInfoAsync_inspector() //select
        {
            SqlDataReader sqlReader = null;

            SqlCommand LoadInfo_inspector = new SqlCommand("SELECT * FROM [inspectors]", sqlConnection);


            try
            {
                sqlReader = await LoadInfo_inspector.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[] {
                    Convert.ToString(sqlReader["emp_id"]),
                    Convert.ToString(sqlReader["ins_type"])
                    });

                    listView4.Items.Add(item);
                }
            }
            catch (Exception ex)
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

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            await LoadInfoAsync_dir();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();

            await LoadInfoAsync_worker();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();

            await LoadInfoAsync_head_of_dept();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            listView4.Items.Clear();

            await LoadInfoAsync_inspector();
        }
    }
}
