using System;
using System.Data;
using System.Drawing; 
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Make sure to add MySql.Data reference/package

namespace LabExamMendozaMariela

{
    public partial class StudentPage_Individual : Form 
    {
        private int SelectedStudentId;  

      
        private string ConnectionString = "server=127.0.0.1;database=studentinfodb;uid=root;pwd=;";
        // ----- VERIFY YOUR CONNECTION DETAILS -----


        // Constructor
        public StudentPage_Individual(int StudentId)
        {
            InitializeComponent(); // Calls the code in StudentPage_Individual.Designer.cs
            this.SelectedStudentId = StudentId;
        }

        // Form Load Event Handler
        private void studentPageIndividual_Load(object sender, EventArgs e)
        {
            loadIndividualStudentData(); // Load data when the form opens
        }

        // Method to load data for the specific student
        private void loadIndividualStudentData()
        {
            // Use 'using' to ensure connection is closed even if errors occur
            using (MySqlConnection DbConnection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    DbConnection.Open(); // Open the database connection

                    // SQL Query to get student details and join with course/year tables
                    string SqlQuery = @"
                        SELECT
                            sr.studentId, sr.firstName, sr.lastName, sr.middleName,
                            sr.houseNo, sr.brgyName, sr.municipality, sr.province, sr.region, sr.country,
                            sr.birthdate, sr.age, sr.studContactNo, sr.emailAddress,
                            sr.guardianFirstName, sr.guardianLastName, sr.hobbies, sr.nickname,
                            c.courseName,
                            y.yearLvl
                        FROM
                            studentrecordtb sr
                        LEFT JOIN
                            coursetb c ON sr.courseId = c.courseId
                        LEFT JOIN
                            yeartb y ON sr.yearId = y.yearId
                        WHERE
                            sr.studentId = @StudentId;"; // Use parameter to prevent SQL injection

                    // Create command with query and connection
                    using (MySqlCommand DbCommand = new MySqlCommand(SqlQuery, DbConnection))
                    {
                        // Add the student ID parameter
                        DbCommand.Parameters.AddWithValue("@StudentId", this.SelectedStudentId);

                        // Execute the query and get the results
                        using (MySqlDataReader DataReader = DbCommand.ExecuteReader())
                        {
                            if (DataReader.Read()) // Check if a record was found
                            {
                                

                                StudentIdDisplayLabel.Text = GetString(DataReader, "studentId");

                                // Construct Full Name
                                string firstName = GetString(DataReader, "firstName");
                                string middleName = GetString(DataReader, "middleName");
                                string lastName = GetString(DataReader, "lastName");
                                FullNameDisplayLabel.Text = $"{firstName} {middleName} {lastName}".Replace("  ", " ").Trim();

                                // Construct Address
                                string houseNo = GetString(DataReader, "houseNo");
                                string brgy = GetString(DataReader, "brgyName");
                                string mun = GetString(DataReader, "municipality");
                                string prov = GetString(DataReader, "province");
                                string region = GetString(DataReader, "region");
                                string country = GetString(DataReader, "country");
                                AddressDisplayLabel.Text = $"{houseNo} {brgy}, {mun}, {prov}, {region}, {country}"
                                                           .Replace(" ,", ",") // Clean up spacing
                                                           .Replace("  ", " ")
                                                           .Trim(new char[] { ' ', ',' });

                                // Populate remaining labels based on warnings and query
                                BirthdateDisplayLabel.Text = GetString(DataReader, "birthdate");
                                AgeDisplayLabel.Text = GetString(DataReader, "age");
                                ContactDisplayLabel.Text = GetString(DataReader, "studContactNo");
                                EmailDisplayLabel.Text = GetString(DataReader, "emailAddress");

                                // Construct Guardian Name
                                string gFirstName = GetString(DataReader, "guardianFirstName");
                                string gLastName = GetString(DataReader, "guardianLastName");
                                GuardianNameDisplayLabel.Text = $"{gFirstName} {gLastName}".Trim();

                                HobbiesDisplayLabel.Text = GetString(DataReader, "hobbies");
                                NicknameDisplayLabel.Text = GetString(DataReader, "nickname");

                                // Display joined data
                                CourseDisplayLabel.Text = GetString(DataReader, "courseName");
                                YearLevelDisplayLabel.Text = GetString(DataReader, "yearLvl");
                                // ***** END OF ADDED/COMPLETED CODE *****
                            }
                            else
                            {
                                // Handle case where student ID is not found
                                MessageBox.Show($"Student with ID {this.SelectedStudentId} not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                this.Close(); // Close the form if student not found
                            }
                        } // DataReader is automatically closed and disposed here
                    } // DbCommand is automatically disposed here
                }
                catch (MySqlException mex)
                {
                    // Handle MySQL-specific errors
                    // Check mex.Message for details like "Access denied", "Unknown database" etc.
                    MessageBox.Show($"Database Error: {mex.Message}\n\nCheck connection string/credentials and ensure MySQL server is running.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close(); // Close form on error
                }
                catch (Exception ex)
                {
                    // Handle general errors during data loading
                    MessageBox.Show($"Error loading student details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close(); // Close form on error
                }
            } // DbConnection is automatically closed and disposed here
        }

        // Helper method to safely get string values from DataReader, handling DBNull
        private string GetString(MySqlDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName); // Get column index by name
                if (reader.IsDBNull(ordinal))
                {
                    return string.Empty; // Return empty string if DB value is NULL
                }
                else
                {
                    // Use GetValue and ToString() for flexibility
                    return reader.GetValue(ordinal).ToString();
                }
            }
            catch (IndexOutOfRangeException) // Handle cases where column name might be wrong in SQL Query
            {
                MessageBox.Show($"Error retrieving column: {columnName}. Check SQL Query.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "Error"; // Indicate an error occurred
            }
        }

        // Event handler for the close button - Ensure you have a Button named CloseBtn in Designer
        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close(); // Closes this specific form instance
        }

        // Add other methods if needed...

    } // End of partial class StudentPage_Individual
} 