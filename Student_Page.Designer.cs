// NOTE: This namespace MUST match your project namespace
namespace LabExamMendozaMariela
{
    partial class Student_Page
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // Create the DataGridView object
            this.StudentListDataGridView = new System.Windows.Forms.DataGridView();
            // Required for designer interaction
            ((System.ComponentModel.ISupportInitialize)(this.StudentListDataGridView)).BeginInit();
            this.SuspendLayout();

            //
            // StudentListDataGridView Configuration (Based on your code/designer)
            //
            this.StudentListDataGridView.AllowUserToAddRows = false;
            this.StudentListDataGridView.AllowUserToDeleteRows = false;
            // AllowUserToResizeColumns/Rows defaults might be True, setting to False based on pasted code
            this.StudentListDataGridView.AllowUserToResizeColumns = false;
            this.StudentListDataGridView.AllowUserToResizeRows = false;
            this.StudentListDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StudentListDataGridView.Dock = System.Windows.Forms.DockStyle.Fill; // Make it fill the form
            this.StudentListDataGridView.Location = new System.Drawing.Point(0, 0);
            this.StudentListDataGridView.Name = "StudentListDataGridView"; // Crucial: Name must match
            this.StudentListDataGridView.ReadOnly = true;
            // Size might be overridden by Dock=Fill, but setting example
            this.StudentListDataGridView.Size = new System.Drawing.Size(800, 450);
            // RowTemplate setting might be needed depending on .NET version/defaults
            this.StudentListDataGridView.RowTemplate.Height = 29; // Example height
            this.StudentListDataGridView.TabIndex = 0;
            // *** Connect the CellContentClick event to the handler in Student_Page.cs ***
            this.StudentListDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.studentListDataGridView_CellContentClick);

            //
            // Student_Page Configuration (The Form itself)
            //
            // Using example size, adjust as needed if not using Dock=Fill for grid
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F); // Adjust font scaling if needed
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450); // Set Form size
            // *** Add the DataGridView to the Form's Controls ***
            this.Controls.Add(this.StudentListDataGridView);
            this.Name = "Student_Page"; // Form's Name property
            this.Text = "Student List"; // Form's Title text
            // *** CRITICAL: Connect the Form's Load event to the handler in Student_Page.cs ***
            this.Load += new System.EventHandler(this.Student_Page_Load); // This line was likely missing
            // Required for designer interaction
            ((System.ComponentModel.ISupportInitialize)(this.StudentListDataGridView)).EndInit();
            this.ResumeLayout(false);

            // *** REMOVED incorrect CellContentClick definition from here ***
        }

        #endregion

        // *** This is the SINGLE, CORRECT declaration for the control ***
        private System.Windows.Forms.DataGridView StudentListDataGridView;

        // *** REMOVED declarations for StudentPage_Individual controls from here ***
    }
}