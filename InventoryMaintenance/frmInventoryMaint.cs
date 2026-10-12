namespace InventoryMaintenance
{
    public partial class frmInventoryMaint : Form
    {
        // Alex Eisenmann
        public frmInventoryMaint()
        {
            InitializeComponent();
        }

        private List<InventoryItem> items = null!;

        // Alex Eisenmann
        private void frmInventoryMaint_Load(object sender, EventArgs e)
        {
            items = InventoryDB.GetItems();
            LoadComboBox();
            FillItemListBox();
        }

        // Alex Eisenmann
        private void LoadComboBox()
        {
            cboFilterBy.DataSource = new string[] {
                "All", "Under $10", "$10 to $50", "Over $50"
            };
        }

        // Alex Eisenmann
        private void FillItemListBox()
        {
            lstItems.Items.Clear();
            string filter = cboFilterBy.SelectedValue?.ToString() ?? "";
            IEnumerable<InventoryItem> filteredItems = null!;
            if (filter == "Under $10")
            {
                filteredItems = from item in items
                                where item.Price < 10
                                orderby item.Description
                                select item;
            }
            else if (filter == "$10 to $50")
            {
                filteredItems = from item in items
                                where item.Price >= 10 && item.Price <= 50
                                orderby item.Description
                                select item;
            }
            else if (filter == "Over $50")
            {
                filteredItems = from item in items
                                where item.Price > 50
                                orderby item.Description
                                select item;
            }
            else
            {
                filteredItems = from item in items
                                orderby item.Description
                                select item;
            }            foreach (InventoryItem item in filteredItems)
            {
                lstItems.Items.Add(item.GetDisplayText());
            }
        }

        // Alex Eisenmann
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewItem newItemForm = new();
            InventoryItem item = newItemForm.GetNewItem();
            if (item != null)
            {
                items.Add(item);
                InventoryDB.SaveItems(items);
                FillItemListBox();
            }
        }

        // Alex Eisenmann
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = lstItems.SelectedIndex;
            if (i == -1)
            {
                MessageBox.Show("Please select an item to delete.", "No item selected");
            }
            else
            {
                string displayText = lstItems.Items[i].ToString() ?? "";
                InventoryItem item = items.First(
                    inv => inv.GetDisplayText() == displayText);
                string message = $"Are you sure you want to delete {item.Description}?";
                DialogResult result =
                    MessageBox.Show(message, "Confirm Delete",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    items.Remove(item);
                    InventoryDB.SaveItems(items);
                    FillItemListBox();
                }
            }
        }

        // Alex Eisenmann
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Alex Eisenmann
        private void cboFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillItemListBox();
        }
    }
}