using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantApplication
    {
        class Program
        {
            static void Main(string[] args)
            {
                // Create a list to store user accounts
                List<User> users = new List<User>();

                // Add some initial user accounts
                users.Add(new User { username = "user1", password = "pass1", isAdmin = false });
                users.Add(new User { username = "user2", password = "pass2", isAdmin = false });
                users.Add(new User { username = "admin", password = "admin", isAdmin = true });

                // Create a GUI for the application
                Application.Run(new MainForm(users));
            }
        }

        // User class to store user accounts
        public class User
        {
            public string username;
            public string password;
            public bool isAdmin;
        }

        // MainForm class for the GUI
        public class MainForm : Form
        {
            private List<User> users; // List of user accounts
            private User currentUser; // Current user logged in
            private TextBox usernameField; // Text field for username
            private TextBox passwordField; // Text field for password
            private Button loginButton; // Button to log in
            private Label errorLabel; // Label to display error messages

            public MainForm(List<User> users)
            {
                // Set the user account list
                this.users = users;

                // Set up the GUI
                this.Text = "Restaurant Application";
                this.Size = new Size(300, 200);
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;

                // Add a label for the username field
                Label usernameLabel = new Label();
                usernameLabel.Text = "Username:";
                usernameLabel.Location = new Point(10, 10);
                usernameLabel.Size = new Size(70, 13);
                this.Controls.Add(usernameLabel);

                // Add a text field for the username
                usernameField = new TextBox();
                usernameField.Location = new Point(90, 10);
                usernameField.Size = new Size(200, 20);
                this.Controls.Add(usernameField);

                // Add a label for the password field
                Label passwordLabel = new Label();
                passwordLabel.Text = "Password:";
                passwordLabel.Location = new Point(10, 35);
                passwordLabel.Size = new Size(70, 13);
                this.Controls.Add(passwordLabel);

                // Add a text field for the password
                passwordField = new TextBox();
                passwordField.Location = new Point(90, 35);
                passwordField.Size = new Size(200, 20);
                passwordField.UseSystemPasswordChar = true;
                this.Controls.Add(passwordField);

                // Add a button to log in
                loginButton = new Button();
                loginButton.Text = "Log In";
                loginButton.Location = new Point(10, 60);
                loginButton.Size = new Size(280, 23);
                loginButton.Click += new EventHandler(loginButton_Click);
                this.Controls.Add(loginButton);
                        // Add a label to display error messages
            errorLabel = new Label();
            errorLabel.Text = "";
            errorLabel.Location = new Point(10, 90);
            errorLabel.Size = new Size(280, 13);
            this.Controls.Add(errorLabel);
        }

        // Event handler for the login button
        private void loginButton_Click(object sender, EventArgs e)
        {
            // Get the username and password entered
            string username = usernameField.Text;
            string password = passwordField.Text;

            // Check if the entered credentials are correct
            User user = users.FirstOrDefault(u => u.username == username && u.password == password);
            if (user != null)
            {
                // Log in the user
                currentUser = user;

                // Show the appropriate front end for the user
                if (currentUser.isAdmin)
                {
                    // Show the administrative front end
                    this.Controls.Clear();
                    this.Controls.Add(new AdminFrontEnd(users, currentUser));
                }
                else
                {
                    // Show the restaurant floor/kitchen front end
                    this.Controls.Clear();
                    this.Controls.Add(new RestaurantFrontEnd(users, currentUser));
                }
            }
            else
            {
                // Display an error message
                errorLabel.Text = "Invalid username or password";
            }
        }
    }

    // AdminFrontEnd class for the administrative front end
    public class AdminFrontEnd : Control
    {
        private List<User> users; // List of user accounts
        private User currentUser; // Current user logged in
        private Button logoutButton; // Button to log out
        private Button addUserButton; // Button to add a new user
        private Button removeUserButton; // Button to remove a user
        private ListBox userListBox; // List box to display user accounts

        public AdminFrontEnd(List<User> users, User currentUser)
        {
            // Set the user account list and current user
            this.users = users;
            this.currentUser = currentUser;

            // Set up the GUI
            this.Dock = DockStyle.Fill;

            // Add a button to log out
            logoutButton = new Button();
            logoutButton.Text = "Log Out";
            logoutButton.Location = new Point(10, 10);
            logoutButton.Size = new Size(80, 23);
            logoutButton.Click += new EventHandler(logoutButton_Click);
            this.Controls.Add(logoutButton);

            // Add a button to add a new user
            addUserButton = new Button();
            addUserButton.Text = "Add User";
            addUserButton.Location = new Point(100, 10);
            addUserButton.Size = new Size(80, 23);
            addUserButton.Click += new EventHandler(addUserButton_Click);
            this.Controls.Add(addUserButton);

            // Add a button to remove a user
            removeUserButton = new Button();
            removeUserButton.Text = "Remove User";
            removeUserButton.Location = new Point(190, 10);
            removeUserButton.Size = new Size(80, 23);
            removeUserButton.Click += new EventHandler(removeUserButton_Click);
            this.Controls.Add(removeUserButton);
                    // Add a list box to display user accounts
            userListBox = new ListBox();
            userListBox.Location = new Point(10, 40);
            userListBox.Size = new Size(260, 150);
            userListBox.SelectionMode = SelectionMode.MultiExtended;
            this.Controls.Add(userListBox);

            // Populate the list box with the user accounts
            foreach (User user in users)
            {
                userListBox.Items.Add(user.username);
            }
        }

        // Event handler for the log out button
        private void logoutButton_Click(object sender, EventArgs e)
        {
            // Close the current form and open the login form
            this.FindForm().Close();
            Application.Run(new MainForm(users));
        }

        // Event handler for the add user button
        private void addUserButton_Click(object sender, EventArgs e)
        {
            // Show a dialog to enter the new user's credentials
            AddUserDialog addUserDialog = new AddUserDialog();
            DialogResult result = addUserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Add the new user to the list
                users.Add(new User { username = addUserDialog.Username, password = addUserDialog.Password, isAdmin = addUserDialog.IsAdmin });

                // Add the new user to the list box
                userListBox.Items.Add(addUserDialog.Username);
            }
        }

        // Event handler for the remove user button
        private void removeUserButton_Click(object sender, EventArgs e)
        {
            // Make sure at least one user is selected in the list box
            if (userListBox.SelectedIndices.Count > 0)
            {
                // Remove the selected users from the list
                for (int i = userListBox.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    int index = userListBox.SelectedIndices[i];
                    users.RemoveAt(index);
                    userListBox.Items.RemoveAt(index);
                }
            }
            else
            {
                MessageBox.Show("Please select one or more users to remove.", "Error");
            }
        }
    }

    // AddUserDialog class for the dialog to add a new user
    public class AddUserDialog : Form
    {
        private TextBox usernameField; // Text field for the username
        private TextBox passwordField; // Text field for the password
        private CheckBox adminCheckBox; // Check box to set the user as an administrator
        private Button okButton; // Button to add the user
        private Button cancelButton; // Button to cancel adding the user

        // Properties to get the entered username, password, and admin status
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool IsAdmin { get; private set; }

        public AddUserDialog()
        {
            // Set up the GUI for the dialog
            this.Text = "Add User";
            this.Size = new Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
                    // Add a label for the username field
            Label usernameLabel = new Label();
            usernameLabel.Text = "Username:";
            usernameLabel.Location = new Point(10, 10);
            usernameLabel.Size = new Size(70, 13);
            this.Controls.Add(usernameLabel);

            // Add a text field for the username
            usernameField = new TextBox();
            usernameField.Location = new Point(90, 10);
            usernameField.Size = new Size(200, 20);
            this.Controls.Add(usernameField);

            // Add a label for the password field
            Label passwordLabel = new Label();
            passwordLabel.Text = "Password:";
            passwordLabel.Location = new Point(10, 35);
            passwordLabel.Size = new Size(70, 13);
            this.Controls.Add(passwordLabel);

            // Add a text field for the password
            passwordField = new TextBox();
            passwordField.Location = new Point(90, 35);
            passwordField.Size = new Size(200, 20);
            passwordField.UseSystemPasswordChar = true;
            this.Controls.Add(passwordField);

            // Add a check box to set the user as an administrator
            adminCheckBox = new CheckBox();
            adminCheckBox.Text = "Set as administrator";
            adminCheckBox.Location = new Point(10, 60);
            adminCheckBox.Size = new Size(280, 17);
            this.Controls.Add(adminCheckBox);

            // Add an "OK" button to add the user
            okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(40, 100);
            okButton.Size = new Size(80, 23);
            okButton.Click += new EventHandler(okButton_Click);
            this.Controls.Add(okButton);

            // Add a "Cancel" button to cancel adding the user
            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(180, 100);
            cancelButton.Size = new Size(80, 23);
            cancelButton.Click += new EventHandler(cancelButton_Click);
            this.Controls.Add(cancelButton);
        }

        // Event handler for the "OK" button
        private void okButton_Click(object sender, EventArgs e)
        {
            // Get the entered username and password
            this.Username = usernameField.Text;
            this.Password = passwordField.Text;

            // Get the admin status
            this.IsAdmin = adminCheckBox.Checked;

            // Close the dialog
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Event handler for the "Cancel" button
        private void cancelButton_Click(object sender, EventArgs e)
        {
            // Close the dialog
            this.DialogResult = Dial
            ogResult.Cancel;
            this.Close();
        }
    }
    // RestaurantFrontEnd class for the restaurant floor/kitchen front end
    public class RestaurantFrontEnd : Control
    {
        private List<User> users; // List of user accounts
        private User currentUser; // Current user logged in
        private Button logoutButton; // Button to log out
        private Button updateStockButton; // Button to update stock levels
        private DataGridView stockDataGridView; // Data grid view to display stock levels
        private Button placeOrderButton; // Button to place an order
        private Button viewOrdersButton; // Button to view past orders
        private Button printReceiptButton; // Button to print a receipt for the current order

        public RestaurantFrontEnd(List<User> users, User currentUser)
        {
            // Set the user account list and current user
            this.users = users;
            this.currentUser = currentUser;

            // Set up the GUI
            this.Dock = DockStyle.Fill;

            // Add a button to log out
            logoutButton = new Button();
            logoutButton.Text = "Log Out";
            logoutButton.Location = new Point(10, 10);
            logoutButton.Size = new Size(80, 23);
            logoutButton.Click += new EventHandler(logoutButton_Click);
            this.Controls.Add(logoutButton);

            // Add a button to update stock levels
            updateStockButton = new Button();
            updateStockButton.Text = "Update Stock";
            updateStockButton.Location = new Point(100, 10);
            updateStockButton.Size = new Size(100, 23);
            updateStockButton.Click += new EventHandler(updateStockButton_Click);
            this.Controls.Add(updateStockButton);

            // Add a data grid view to display stock levels
            stockDataGridView = new DataGridView();
            stockDataGridView.Location = new Point(10, 40);
            stockDataGridView.Size = new Size(300, 200);
            stockDataGridView.Columns.Add("Item", "Item");
            stockDataGridView.Columns.Add("Quantity", "Quantity");
            this.Controls.Add(stockDataGridView);

            // Add a button to place an order
            placeOrderButton = new Button();
            placeOrderButton.Text = "Place Order";
            placeOrderButton.Location = new Point(10, 250);
            placeOrderButton.Size = new Size(80, 23);
            placeOrderButton.Click += new EventHandler(placeOrderButton_Click);
            this.Controls.Add(placeOrderButton);

            // Add a button to view past orders
            viewOrdersButton = new Button();
            viewOrdersButton.Text = "View Orders";
            viewOrdersButton.Location = new Point(100, 250);
            viewOrdersButton.Size = new Size(80, 23);
            viewOrdersButton.Click += new EventHandler(viewOrdersButton_Click);
            this.Controls.Add(viewOrdersButton);

            // Add a button to print a receipt for the current order
            printReceiptButton = new Button();
            printReceiptButton.Text = "Print Receipt";
            printReceiptButton.Location = new Point(190, 250);
            printReceiptButton.Size = new Size(80, 23);
            printReceiptButton.Click += new EventHandler(printReceiptButton_Click);
            this.Controls.Add(printReceiptButton);
        }
            // Event handler for the log out button
        private void logoutButton_Click(object sender, EventArgs e)
        {
            // Close the current form and open the login form
            this.FindForm().Close();
            Application.Run(new MainForm(users));
        }

        // Event handler for the update stock button
        private void updateStockButton_Click(object sender, EventArgs e)
        {
            // Show a dialog to update the stock levels
            UpdateStockDialog updateStockDialog = new UpdateStockDialog();
            DialogResult result = updateStockDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Update the stock levels in the data grid view
                foreach (KeyValuePair<string, int> entry in updateStockDialog.StockLevels)
                {
                    bool found = false;
                    foreach (DataGridViewRow row in stockDataGridView.Rows)
                    {
                        if (row.Cells[0].Value.ToString() == entry.Key)
                        {
                            row.Cells[1].Value = entry.Value;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        stockDataGridView.Rows.Add(entry.Key, entry.Value);
                    }
                }
            }
        }

        // Event handler for the place order button
        private void placeOrderButton_Click(object sender, EventArgs e)
        {
            // Show a dialog to place an order
            PlaceOrderDialog placeOrderDialog = new PlaceOrderDialog();
            DialogResult result = placeOrderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Calculate the total price of the order
                decimal totalPrice = 0;
                foreach (KeyValuePair<string, int> entry in placeOrderDialog.Order)
                {
                    decimal itemPrice = 0;
                    foreach (DataGridViewRow row in stockDataGridView.Rows)
                    {
                        if (row.Cells[0].Value.ToString() == entry.Key)
                        {
                            itemPrice = decimal.Parse(row.Cells[2].Value.ToString());
                            break;
                        }
                    }
                    totalPrice += itemPrice * entry.Value;
                }

                // Add the order to the database
                // Code to add the order to the database goes here

                // Display the total price of the order
                MessageBox.Show("Total price: " + totalPrice.ToString("C"), "Order Placed");
            }
        }

        // Event handler for the view orders button
        private void viewOrdersButton_Click(object sender, EventArgs e)
        {
            // Show a form to view past orders
            ViewOrdersForm viewOrdersForm = new ViewOrdersForm();
            viewOrdersForm
            .ShowDialog();
        }
            // Event handler for the print receipt button
        private void printReceiptButton_Click(object sender, EventArgs e)
        {
            // Print a receipt for the current order
            // Code to print the receipt goes here
        }
    }

    // UpdateStockDialog class for the dialog to update stock levels
    public class UpdateStockDialog : Form
    {
        private DataGridView stockDataGridView; // Data grid view to display and update stock levels
        private Button okButton; // Button to update the stock levels
        private Button cancelButton; // Button to cancel updating the stock levels

        // Property to get the updated stock levels
        public Dictionary<string, int> StockLevels { get; private set; }

        public UpdateStockDialog()
        {
            // Set up the GUI for the dialog
            this.Text = "Update Stock";
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Add a data grid view to display and update stock levels
            stockDataGridView = new DataGridView();
            stockDataGridView.Location = new Point(10, 10);
            stockDataGridView.Size = new Size(380, 200);
            stockDataGridView.Columns.Add("Item", "Item");
            stockDataGridView.Columns.Add("Quantity", "Quantity");
            stockDataGridView.Columns.Add("Price", "Price");
            this.Controls.Add(stockDataGridView);

            // Add an "OK" button to update the stock levels
            okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(40, 220);
            okButton.Size = new Size(80, 23);
            okButton.Click += new EventHandler(okButton_Click);
            this.Controls.Add(okButton);

            // Add a "Cancel" button to cancel updating the stock levels
            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(260, 220);
            cancelButton.Size = new Size(80, 23);
            cancelButton.Click += new EventHandler(cancelButton_Click);
            this.Controls.Add(cancelButton);
        }

        // Event handler for the "OK" button
        private void okButton_Click(object sender, EventArgs e)
        {
            // Get the updated stock levels
            this.StockLevels = new Dictionary<string, int>();
            foreach (DataGridViewRow row in stockDataGridView.Rows)
            {
                this.StockLevels.Add(row.Cells[0].Value.ToString(), int.Parse(row.Cells[1].Value.ToString()));
            }

            // Close the dialog
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Event handler for the "Cancel" button
        private void cancelButton_Click(object sender, EventArgs e)
        {
            // Close the dialog
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
    // PlaceOrderDialog class for the dialog to place an order
    public class PlaceOrderDialog : Form
    {
        private DataGridView menuDataGridView; // Data grid view to display the menu and select items for the order
        private Button addButton; // Button to add an item to the order
        private Button removeButton; // Button to remove an item from the order
        private DataGridView orderDataGridView; // Data grid view to display the current order
        private Button okButton; // Button to place the order
        private Button cancelButton; // Button to cancel placing the order

        // Property to get the items in the order
        public Dictionary<string, int> Order { get; private set; }

        public PlaceOrderDialog()
        {
            // Set up the GUI for the dialog
            this.Text = "Place Order";
            this.Size = new Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Add a data grid view to display the menu and select items for the order
            menuDataGridView = new DataGridView();
            menuDataGridView.Location = new Point(10, 10);
            menuDataGridView.Size = new Size(200, 300);
            menuDataGridView.Columns.Add("Item", "Item");
            menuDataGridView.Columns.Add("Price", "Price");
            this.Controls.Add(menuDataGridView);

            // Add a button to add an item to the order
            addButton = new Button();
            addButton.Text = "Add to Order >>";
            addButton.Location = new Point(220, 10);
            addButton.Size = new Size(120, 23);
            addButton.Click += new EventHandler(addButton_Click);
            this.Controls.Add(addButton);

            // Add a button to remove an item from the order
            removeButton = new Button();
            removeButton.Text = "<< Remove from Order";
            removeButton.Location = new Point(220, 40);
            removeButton.Size = new Size(120, 23);
            removeButton.Click += new EventHandler(removeButton_Click);
            this.Controls.Add(removeButton);

            // Add a data grid view to display the current order
            orderDataGridView = new DataGridView();
            orderDataGridView.Location = new Point(350, 10);
            orderDataGridView.Size = new Size(200, 300);
            orderDataGridView.Columns.Add("Item", "Item");
            orderDataGridView.Columns.Add("Quantity", "Quantity");
            orderDataGridView.Columns.Add("Price", "Price");
            this.Controls.Add(orderDataGridView);

            // Add an "OK" button to place the order
            okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(40, 320);
            okButton.Size = new Size(80, 23);
            okButton.Click += new EventHandler(okButton_Click);
            this.Controls.Add(okButton);
                    // Add a "Cancel" button to cancel placing the order
            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(260, 320);
            cancelButton.Size = new Size(80, 23);
            cancelButton.Click += new EventHandler(cancelButton_Click);
            this.Controls.Add(cancelButton);
        }

        // Event handler for the "Add to Order" button
        private void addButton_Click(object sender, EventArgs e)
        {
            // Add the selected item to the order
            if (menuDataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = menuDataGridView.SelectedRows[0];
                string item = selectedRow.Cells[0].Value.ToString();
                decimal price = decimal.Parse(selectedRow.Cells[1].Value.ToString());

                bool found = false;
                foreach (DataGridViewRow row in orderDataGridView.Rows)
                {
                    if (row.Cells[0].Value.ToString() == item)
                    {
                        int quantity = int.Parse(row.Cells[1].Value.ToString());
                        row.Cells[1].Value = quantity + 1;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    orderDataGridView.Rows.Add(item, 1, price);
                }
            }
        }

        // Event handler for the "Remove from Order" button
        private void removeButton_Click(object sender, EventArgs e)
        {
            // Remove the selected item from the order
            if (orderDataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = orderDataGridView.SelectedRows[0];
                int quantity = int.Parse(selectedRow.Cells[1].Value.ToString());
                if (quantity > 1)
                {
                    selectedRow.Cells[1].Value = quantity - 1;
                }
                else
                {
                    orderDataGridView.Rows.Remove(selectedRow);
                }
            }
        }

        // Event handler for the "OK" button
        private void okButton_Click(object sender, EventArgs e)
        {
            // Get the items in the order
            this.Order = new Dictionary<string, int>();
            foreach (DataGridViewRow row in orderDataGridView.Rows)
            {
                this.Order.Add(row.Cells[0].Value.ToString(), int.Parse(row.Cells[1].Value.ToString()));
            }

            // Close the dialog
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Event handler for the "Cancel" button
        private void cancelButton_Click(object sender, EventArgs e)
        {
            // Close the dialog
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    // ViewOrdersForm class for the form to view orders
    public classs ViewOrdersForm : Form
    {
        private DataGridView ordersDataGridView; // Data grid view to display the orders
            public ViewOrdersForm()
        {
            // Set up the GUI for the form
            this.Text = "View Orders";
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Add a data grid view to display past orders
            ordersDataGridView = new DataGridView();
            ordersDataGridView.Location = new Point(10, 10);
            ordersDataGridView.Size = new Size(380, 280);
            ordersDataGridView.Columns.Add("ID", "ID");
            ordersDataGridView.Columns.Add("Date", "Date");
            ordersDataGridView.Columns.Add("Total Price", "Total Price");
            this.Controls.Add(ordersDataGridView);

            // Retrieve past orders from the database and display them in the data grid view
            // Code to retrieve past orders from the database goes here
        }
    }
}








