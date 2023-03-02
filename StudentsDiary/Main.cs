using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class Main : Form
    {
        private FileHelper<List<Student>> _fileHelper=new FileHelper<List<Student>>(Program.FilePath);
        public bool IsMaximize 
        {
            get
            {
                return Settings.Default.isMaximize;
            }
            set
            {
                Settings.Default.isMaximize = value;
            }
        }

        public Main()
        {
            InitializeComponent();
            RefreshDiary();
            SetColumnsHeader();
            FillFilterClassComboBox();

            if (IsMaximize)
                WindowState = FormWindowState.Maximized;
        }

        private void RefreshDiary()
        {
            var studentList = _fileHelper.Deserialize().OrderBy(x => x.Id).ToList();
            
            for (int i = 0; i < studentList.Count; i++)
            {
                studentList[i].Id = i + 1;
            }

            _fileHelper.SerializeToFile(studentList);
            dgvDiary.DataSource = studentList;
            cboBGroupIDFilter.SelectedItem = "Wszystkie";
        }

        private void SetColumnsHeader()
        {
            dgvDiary.Columns[0].HeaderText = "L.p.";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Matematyka";
            dgvDiary.Columns[4].HeaderText = "Technologia";
            dgvDiary.Columns[5].HeaderText = "Fizyka";
            dgvDiary.Columns[6].HeaderText = "Język polski";
            dgvDiary.Columns[7].HeaderText = "Język angielski";
            dgvDiary.Columns[8].HeaderText = "Uwagi";
            dgvDiary.Columns[9].HeaderText = "Dodatkowe aktywności";
            dgvDiary.Columns[10].HeaderText = "Klasa";
        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent=new AddEditStudent();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
            addEditStudent.FormClosing -= AddEditStudent_FormClosing;
        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego dane chcesz edytować");
                return;
            }

            var addEditStudent = new AddEditStudent(Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
            addEditStudent.FormClosing -= AddEditStudent_FormClosing;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego chcesz usunąć");
                return;
            }
            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete = MessageBox.Show($"Czy na pewno chcesz usunąć ucznia {(selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString()).Trim()}", "Usuwanie ucznia", MessageBoxButtons.YesNo);

            if (confirmDelete == DialogResult.Yes)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }
        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.Deserialize();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                IsMaximize = true;
            else
                IsMaximize = false;

            Settings.Default.Save();
        }

        private void FillFilterClassComboBox()
        {
            string[] classesList = File.ReadAllLines(Program.GroupIDListPath);
            cboBGroupIDFilter.Items.Add("Wszystkie");
            foreach (string classes in classesList)
            {
                cboBGroupIDFilter.Items.Add(classes);
            }
        }

        private void cboBClassesFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            var studentList = _fileHelper.Deserialize().OrderBy(x => x.Id);
            var filteredStudentList = new List<Student>();

            if (cboBGroupIDFilter.SelectedItem.ToString() == "Wszystkie")
                filteredStudentList = studentList.ToList();
            else
                filteredStudentList = studentList.Where(x => x.GroupID == cboBGroupIDFilter.SelectedItem.ToString()).ToList();
            
            dgvDiary.DataSource = filteredStudentList;
        }
    }
}
