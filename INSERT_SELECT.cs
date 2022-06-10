using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTask
{
    public partial class INSERT_SELECT : Form
    {
        public INSERT_SELECT()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void INSERT_SELECT_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DIRECTOR_INSERT dir_select = new DIRECTOR_INSERT();

            dir_select.Show();

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HEAD_OF_DEP_INSERT hop_insert = new HEAD_OF_DEP_INSERT();

            hop_insert.Show();

            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            INSPECTOR_INSERT ins_insert = new INSPECTOR_INSERT();

            ins_insert.Show();

            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WORKER_INSERT worker_insert = new WORKER_INSERT();

            worker_insert.Show();

            Close();
        }
    }
}
