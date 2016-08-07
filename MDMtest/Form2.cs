using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MDMtest
{
    public partial class FormEditing : Form
    {
        public string check;        //Checking with which table the from is working
        public int selectedId;      //Selected Id in a working table 

        MDMtestDBEntities entities;

        public FormEditing()
        {
            InitializeComponent();
            entities = new MDMtestDBEntities();
            check = "";
            selectedId = 1;
        }

        public void InitializeGrid()
        {
            if (check == "Customer")
            {
                var customerToEdit = from a in entities.Customer
                            where a.CustomerId == selectedId
                            select a;
                dataGridView.DataSource = customerToEdit.ToList();
            }
            else
            {
                var orderToEdit = from a in entities.Order
                            where a.OrderId == selectedId
                            select a;
                dataGridView.DataSource = orderToEdit.ToList();

                ///ComboBox - doesn't work
                /*
                DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                var dataForCombo =  (from a in entities.Customer
                                    select new { a.CustomerId}).ToList();
                comboCell.DataSource = dataForCombo;
                dataGridView.Rows[0].Cells[1] = comboCell;
                */
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            entities.SaveChanges();
        }

        private void buttonDiscard_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
