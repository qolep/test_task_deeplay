using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace TestTask
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;

        private List<string[]> rows = null; //объектное представление базы данных
        private List<string[]> FilteredList = null; 

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "db_a883d5_workersDataSet1.titles". При необходимости она может быть перемещена или удалена.
            this.titlesTableAdapter.Fill(this.db_a883d5_workersDataSet1.titles);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "db_a883d5_workersDataSet.departments". При необходимости она может быть перемещена или удалена.
            //this.departmentsTableAdapter.Fill(this.db_a883d5_workersDataSet.departments);
            string connectionString = ConfigurationManager.ConnectionStrings["1"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync(); //open connection

            listView1.GridLines = true;

            listView1.FullRowSelect = true;

            listView1.View = View.Details;

            listView1.Columns.Add("ID");
            listView1.Columns.Add("Birthday");
            listView1.Columns.Add("Name");
            listView1.Columns.Add("Surname");
            listView1.Columns.Add("Patronymic");
            listView1.Columns.Add("Gender");
            listView1.Columns.Add("Job_Title");
            listView1.Columns.Add("Department");


            await LoadWorkersAsync();

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();

            }
        }

        private async Task LoadWorkersAsync() //SELECT
        {
            SqlDataReader sqlReader = null;

            rows = new List<string[]>();

            string[] row = null;

            SqlCommand getWorkersCommand = new SqlCommand("SELECT * FROM [employees]", sqlConnection);
           // SqlCommand getWorkersCommand_dep = new SqlCommand("SELECT dept_id FROM [departments]", sqlConnection);


            try
            {
                sqlReader = await getWorkersCommand.ExecuteReaderAsync();
                

                while (await sqlReader.ReadAsync()) 
                {
                    row = new string[] {
                        Convert.ToString(sqlReader["emp_id"]),
                        Convert.ToString(sqlReader["birth_date"]),
                        Convert.ToString(sqlReader["first_name"]),
                        Convert.ToString(sqlReader["last_name"]),
                        Convert.ToString(sqlReader["patronymic"]),
                        Convert.ToString(sqlReader["gender"]),
                        Convert.ToString(sqlReader["title_ID"]),
                        Convert.ToString(sqlReader["dept_id"]),

                    };

                    rows.Add(row);

                
                }

              /*  while (await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[]{
                    
                        Convert.ToString(sqlReader["emp_id"]),
                        Convert.ToString(sqlReader["birth_date"]),
                        Convert.ToString(sqlReader["first_name"]),
                        Convert.ToString(sqlReader["last_name"]),
                        Convert.ToString(sqlReader["patronymic"]),
                        Convert.ToString(sqlReader["gender"]),
                    });

                    listView1.Items.Add(item);
                }*/

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

            RefreshList(rows);

        }

        private async void toolStripButton5_Click(object sender, EventArgs e) //REFRESH BUTTON
        {
            listView1.Items.Clear();

            await LoadWorkersAsync();

        }

        private void toolStripButton1_Click(object sender, EventArgs e) //INSERT BUTTON
        {
            INSERT insert = new INSERT(sqlConnection);

            insert.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) // UPDATE BUTTON
        {
            if (listView1.SelectedItems.Count > 0) // проверка на то, была ли выделена строчка. Если нет - ошибка.
            {


                UPDATE update = new UPDATE(sqlConnection, Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text)); //обращение к выделенной строчке, получение emp_id

                update.Show();
            }
            else
            {
                MessageBox.Show("Ни одна строка не была выделена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void toolStripButton3_Click(object sender, EventArgs e) //DELETE BUTTON
        {
            if (listView1.SelectedItems.Count > 0) 
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление строки", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);




                switch (res)
                {
                    case DialogResult.OK:

                        SqlCommand deleteWorkerCommand = new SqlCommand("DELETE FROM [employees] WHERE [emp_id]=@emp_id", sqlConnection);


                        deleteWorkerCommand.Parameters.AddWithValue("emp_id", Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await deleteWorkerCommand.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            listView1.Items.Clear();

                        }

                        listView1.Items.Clear();

                        await LoadWorkersAsync();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Ни одна строка не была выделена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) //EXIT button
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) // ABOUT button
        {
            MessageBox.Show("test task.deeplay");
        }


        private void RefreshList(List<string[]> list) 
        {
            listView1.Items.Clear();

            foreach (string[] s in list) 
            {

                listView1.Items.Add(new ListViewItem(s));
            
            }
        
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)

        {
            switch (comboBox1.SelectedIndex) 
            {
                

                case 0:
                    FilteredList = rows.Where((x) =>
                    int.Parse(x[6]) == 1).ToList();

                    RefreshList(FilteredList);
                    break;

                case 1:
                    FilteredList = rows.Where((x) =>
                    int.Parse(x[6]) == 7).ToList();
                    
                    RefreshList(FilteredList);
                    break;

                case 2:
                    FilteredList = rows.Where((x) =>
                    int.Parse(x[6]) == 8).ToList();
                    
                    RefreshList(FilteredList);
                    break;

                case 3:
                    FilteredList = rows.Where((x) =>
                    int.Parse(x[6]) == 9).ToList();
                    
                    RefreshList(FilteredList);
                    break;

                case 4:

                    RefreshList(rows);
                    break;
            }


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {


                case 0:
                    FilteredList = rows.Where((x) =>
                    int.Parse(x[7]) == 1).ToList();

                    RefreshList(FilteredList);
                    break;

                case 1:
                    FilteredList = rows.Where((x) =>
                    int.Parse(x[7]) == 2).ToList();

                    RefreshList(FilteredList);
                    break;

                case 2:
                    FilteredList = rows.Where((x) =>
                    int.Parse(x[7]) == 3).ToList();

                    RefreshList(FilteredList);
                    break;

                case 3:
                    FilteredList = rows.Where((x) =>
                    int.Parse(x[7]) == 4).ToList();

                    RefreshList(FilteredList);
                    break;

                case 4:
                    FilteredList = rows.Where((x) =>
                    int.Parse(x[7]) == 5).ToList();

                    RefreshList(FilteredList);
                    break;

                case 5:
                    RefreshList(rows);
                    break;
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            INSERT_SELECT insert_select = new INSERT_SELECT();

            insert_select.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) // проверка на то, была ли выделена строчка. Если нет - ошибка.
            {


                DIRECTOR_INSERT dir_insert = new DIRECTOR_INSERT(sqlConnection, Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text)); //обращение к выделенной строчке, получение emp_id

                dir_insert.Show();
            }
            else
            {
                MessageBox.Show("Ни одна строка не была выделена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) // проверка на то, была ли выделена строчка. Если нет - ошибка.
            {


                HEAD_OF_DEP_INSERT hod_insert = new HEAD_OF_DEP_INSERT(sqlConnection, Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text)); //обращение к выделенной строчке, получение emp_id

                hod_insert.Show();
            }
            else
            {
                MessageBox.Show("Ни одна строка не была выделена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            

            
                


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) // проверка на то, была ли выделена строчка. Если нет - ошибка.
            {


                INSPECTOR_INSERT ins_insert = new INSPECTOR_INSERT(sqlConnection, Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text)); //обращение к выделенной строчке, получение emp_id

                ins_insert.Show();
            }
            else
            {
                MessageBox.Show("Ни одна строка не была выделена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) // проверка на то, была ли выделена строчка. Если нет - ошибка.
            {


                WORKER_INSERT emp_insert = new WORKER_INSERT(sqlConnection, Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text)); //обращение к выделенной строчке, получение emp_id

                emp_insert.Show();
            }
            else
            {
                MessageBox.Show("Ни одна строка не была выделена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void расшифровкаНекоторыхПолейТаблицыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            id_translate id_translate = new id_translate();
            id_translate.Show();
        }
    }
    }

