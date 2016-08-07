using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace MDMtest
{
    public partial class FormMain : Form
    {
        MDMtestDBEntities entities;
        int selectedCustomerId;         //Id of selected customer in Customer grid
        int selectedOrderId;            //Id of selected order Order grid 
        string selectedCustomerName;    //Name of selected customer in Customer grid

        public FormMain()
        {
            InitializeComponent();
            entities = new MDMtestDBEntities();
            selectedCustomerId = 1;
            selectedOrderId = 1;
            selectedCustomerName = null;
            InitializeGridCustomer();
        }

#region Grids Initialization

        public void InitializeGridCustomer()
        {
            dataGridViewCustomer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCustomer.ReadOnly = true;
            dataGridViewCustomer.MultiSelect = false;
            dataGridViewCustomer.DataSource = entities.Customer.ToList();
        }

        void InitializeGridOrder(int selectedCustomerId)
        {
            dataGridViewOrder.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewOrder.ReadOnly = true;
            dataGridViewOrder.MultiSelect = false;

            var query = from a in entities.Customer
                        join b in entities.Order on a.CustomerId equals b.CustomerId
                        where b.CustomerId == selectedCustomerId
                        select b;
            dataGridViewOrder.DataSource = query.ToList();
        }

        private void OrderId()
        {
            if (dataGridViewOrder.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridViewOrder.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridViewOrder.Rows[selectedRowIndex];
                selectedOrderId = int.Parse(selectedRow.Cells["OrderId"].Value.ToString());
            }
        }

#endregion

#region Buttons actions

        private void buttonAddCustomer_Click(object sender, EventArgs e)
        {
            entities.Customer.Add(new Customer());
            entities.SaveChanges();
            InitializeGridCustomer();
        }

        private void buttonDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrder.Rows.Count > 0)
            {
                string message = "You're trying to remove client - " + selectedCustomerName + " with Id = " + selectedCustomerId.ToString() + ". He/She has " + dataGridViewOrder.RowCount.ToString() + " orders.";
                string caption = "Are you sure?";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    var rowToDelete = entities.Customer.SingleOrDefault(item => item.CustomerId == selectedCustomerId);
                    entities.Customer.Remove(rowToDelete);
                    entities.SaveChanges();
                    InitializeGridCustomer();
                }
            }
            else
            {
                var rowToDelete = entities.Customer.SingleOrDefault(item => item.CustomerId == selectedCustomerId);
                entities.Customer.Remove(rowToDelete);
                entities.SaveChanges();
                InitializeGridCustomer();
            }
        }

        private void buttonAddOrder_Click(object sender, EventArgs e)
        {
            entities.Order.Add(new Order() { CustomerId = selectedCustomerId });
            entities.SaveChanges();
            InitializeGridOrder(selectedCustomerId);
        }

        private void buttonDeleteOrder_Click(object sender, EventArgs e)
        {
            OrderId();
            var rowToDelete = entities.Order.SingleOrDefault(item => item.OrderId == selectedOrderId);
            entities.Order.Remove(rowToDelete);
            entities.SaveChanges();
            InitializeGridOrder(selectedCustomerId);
        }

#endregion

#region Other events: Selection, DoubleClick

        private void dataGridViewCustomer_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCustomer.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridViewCustomer.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridViewCustomer.Rows[selectedRowIndex];
                selectedCustomerId = int.Parse(selectedRow.Cells["CustomerId"].Value.ToString());
                if (selectedRow.Cells["Name"].Value is string) selectedCustomerName = selectedRow.Cells["Name"].Value.ToString();
            }

            InitializeGridOrder(selectedCustomerId);
        }

        private void dataGridViewCustomer_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FormEditing formForEdit = new FormEditing();
            formForEdit.check = "Customer";
            formForEdit.selectedId = selectedCustomerId;
            formForEdit.InitializeGrid();
            formForEdit.Show();
        }

        private void dataGridViewOrder_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OrderId();
            FormEditing formForEdit = new FormEditing();
            formForEdit.check = "Order";
            formForEdit.selectedId = selectedOrderId;
            formForEdit.InitializeGrid();
            formForEdit.Show();
        }
        
#endregion
    }
}
