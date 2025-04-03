// Assuming using C# with Windows Forms and MySql.Data NuGet package

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Make sure this is referenced

// Make sure this namespace matches your project
namespace LabExamMendozaMariela


{
    // !!! If your new form is NOT named "Student_Page", change the class name below !!!
    public partial class Student_Page : Form
    {
        // Connection String - Verify your username and password
        private string ConnectionString = "server=127.0.0.1;database=studentinfodb;uid=root;pwd=;";

        // Constructor - Calls InitializeComponent from the corresponding Designer.cs file
        public Student_Page()
        {
            InitializeComponent(); // This line is essential
        }

        // Form Load Event Handler - Attached in the Designer.cs file's InitializeComponent method
        private void Student_Page_Load(object? sender, EventArgs e) // Added '?' for nullable warning fix
        {
            // Add a check for DesignMode to prevent errors in the designer view
            if (!this.DesignMode)
            {
                loadStudentData();
            }
        }

        // Method to load data into the grid
        private void loadStudentData()
        {
            // IMPORTANT: This code assumes a DataGridView named 'StudentListDataGridView'
            // exists on the form (added via the Designer) and is declared/initialized
            // in the corresponding YourFormName.Designer.cs file.

            // Check if the control exists before using it
            if (this.StudentListDataGridView == null)
            {
                // This might happen if the name doesn't match or it wasn't added correctly.
                MessageBox.Show("DataGridView control 'StudentListDataGridView' not found or not initialized correctly. Check the form's designer and the control's Name property.", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Set up DataGridView columns programmatically if they aren't defined in the designer
            // This ensures the required columns exist.
            if (StudentListDataGridView.Columns.Count == 0)
            {
                StudentListDataGridView.Columns.Add("StudentIdColumn", "Student ID");
                StudentListDataGridView.Columns.Add("FullNameColumn", "Full Name");
                StudentListDataGridView.Columns["FullNameColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Make name column fill space

                DataGridViewButtonColumn viewButtonColumn = new DataGridViewButtonColumn();
                viewButtonColumn.Name = "ViewButtonColumn";
                viewButtonColumn.HeaderText = "Action";
                viewButtonColumn.Text = "VIEW";
                viewButtonColumn.UseColumnTextForButtonValue = true; // Show "VIEW" on all buttons in the column
                StudentListDataGridView.Columns.Add(viewButtonColumn);

                // Optional: Set some appearance/behavior properties if not set in the designer
                StudentListDataGridView.AllowUserToAddRows = false;
                StudentListDataGridView.AllowUserToDeleteRows = false;
                StudentListDataGridView.ReadOnly = true; // Make data cells read-only
                StudentListDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            // Clear previous data before loading new data
            StudentListDataGridView.Rows.Clear();


            DataTable StudentDataTable = new DataTable();

            // Use 'using' for automatic disposal of connection and command objects
            using (MySqlConnection DbConnection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    DbConnection.Open(); // Open connection
                    string SqlQuery = @"
                        SELECT
                            studentId,
                            CONCAT(firstName, ' ', lastName) AS FullName
                        FROM
                            studentrecordtb
                        ORDER BY
                            lastName, firstName;"; // Query to get needed data

                    using (MySqlCommand DbCommand = new MySqlCommand(SqlQuery, DbConnection))
                    {
                        using (MySqlDataAdapter DataAdapter = new MySqlDataAdapter(DbCommand))
                        {
                            DataAdapter.Fill(StudentDataTable); // Fill the DataTable with results
                        }
                    }
                }
                catch (MySqlException mex) // Catch specific DB errors
                {
                    MessageBox.Show($"Database Error: {mex.Message}\n\nPlease check connection string, MySQL server status, and user permissions.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Stop if there's a DB error
                }
                catch (Exception ex) // Catch other general errors
                {
                    MessageBox.Show($"Error loading student data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Stop if there's another error
                }
            } // Connection is automatically closed here


            // Populate DataGridView Rows Manually from the DataTable
            // This approach works well with programmatically added columns.
            foreach (DataRow row in StudentDataTable.Rows)
            {
                int rowIndex = StudentListDataGridView.Rows.Add(); // Add a new row
                DataGridViewRow dataGridViewRow = StudentListDataGridView.Rows[rowIndex]; // Get the new row

                // Populate cells using the column names defined earlier
                if (StudentListDataGridView.Columns.Contains("StudentIdColumn"))
                    dataGridViewRow.Cells["StudentIdColumn"].Value = row["studentId"];
                if (StudentListDataGridView.Columns.Contains("FullNameColumn"))
                    dataGridViewRow.Cells["FullNameColumn"].Value = row["FullName"];
                // The button text ("VIEW") is automatically handled by the column settings

                // Store the actual studentId (which should be an integer) in the row's Tag property
                try
                {
                    dataGridViewRow.Tag = Convert.ToInt32(row["studentId"]);
                }
                catch (Exception tagEx)
                {
                    // Handle case where studentId might not be a valid integer
                    Console.WriteLine($"Error converting studentId to int for Tag: {row["studentId"]} - {tagEx.Message}");
                    dataGridViewRow.Tag = -1; // Use -1 or null to indicate invalid ID
                }
            }
        }

        // Cell Click Event Handler for the VIEW button
        // Make sure this event is connected in the Designer.cs file if needed,
        // though often DataGridView events are added here. Let's add it programmatically if needed.
        // We might need to attach this handler if not done automatically. Check if needed.
        // Usually attached like: this.StudentListDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.studentListDataGridView_CellContentClick);
        private void studentListDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the click is on a valid row, a valid column, and the button column specifically
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && StudentListDataGridView.Columns[e.ColumnIndex].Name == "ViewButtonColumn")
            {
                // Use pattern matching to safely get the ID stored in the Tag
                if (StudentListDataGridView.Rows[e.RowIndex].Tag is int SelectedStudentId && SelectedStudentId > 0) // Ensure ID is a valid positive integer
                {
                    // Create and show the second form, passing the retrieved ID
                    StudentPage_Individual IndividualPage = new StudentPage_Individual(SelectedStudentId);
                    IndividualPage.Show();
                    // Optionally hide the current form: this.Hide();
                }
                else
                {
                    // Inform user if a valid ID wasn't found in the Tag
                    MessageBox.Show($"Could not retrieve a valid student ID for this row. Tag value: {StudentListDataGridView.Rows[e.RowIndex].Tag}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // The DataGridView declaration belongs in the Designer.cs file:
        // private System.Windows.Forms.DataGridView StudentListDataGridView;

    } // End of partial class Student_Page
} // End of namespace VelascoArjay_LabExam