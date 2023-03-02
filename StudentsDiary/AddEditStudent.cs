using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {

        private int _studentId;
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private Student _student;

        public AddEditStudent(int id=0)
        {
            InitializeComponent();
            _studentId= id;

            GetStudentData();
            tbName.Select();
        }

        private void GetStudentData()
        {
            FillGroupIDComboBox();

            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";

                var students = _fileHelper.Deserialize();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak studenta o podanym Id.");

                FillTextBoxes();
            }
        }

        private void FillTextBoxes()
        {
            tbID.Text = _student.Id.ToString();
            tbName.Text = _student.Name;
            tbSurname.Text = _student.Surname;
            tbMath.Text = _student.Math;
            tbPhysics.Text = _student.Physics;
            tbTechnology.Text = _student.Technology;
            tbPolish.Text = _student.PolishLang;
            tbEnglish.Text = _student.ForeighLang;
            rtbRemarks.Text = _student.Remarks;
            ckBAditionalActivities.Checked = _student.AditionalActivities == "TAK" ? true : false;
            cboBGroupIDList.SelectedItem = _student.GroupID;
        }

        private void FillGroupIDComboBox()
        {
            string[] groupIDList = File.ReadAllLines(Program.GroupIDListPath);
            foreach (string groupID in groupIDList)
            {
                cboBGroupIDList.Items.Add(groupID);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.Deserialize();

            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
            else
                AssignIdToNewStudent(students);
            
            try
            {
                if (cboBGroupIDList.SelectedItem == null)
                {
                    MessageBox.Show("Nie wskazałeś do jakiej klasy należy uczeń. Została wskazana domyślna klasa (Ia).");
                    cboBGroupIDList.SelectedItem = "Ia";
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            AddNewStudentList(students);

            _fileHelper.SerializeToFile(students);

            Close();
        }

        private void AssignIdToNewStudent(List<Student>students)
        {
            var studentHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
            _studentId = studentHighestId == null ? 1 : studentHighestId.Id + 1;
        }

        private void AddNewStudentList(List<Student> students)
        {
            var student = new Student()
            {
                Id = _studentId,
                Name = tbName.Text,
                Surname = tbSurname.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                ForeighLang = tbEnglish.Text,
                PolishLang = tbPolish.Text,
                Technology = tbTechnology.Text,
                Remarks = rtbRemarks.Text,
                AditionalActivities = SetAditionalActivitiesCell(ckBAditionalActivities.Checked),
                GroupID=cboBGroupIDList.SelectedItem.ToString(),
        };

            students.Add(student);
        }

        private string SetAditionalActivitiesCell(bool activitiesChecked)
        {
            if (activitiesChecked)
                return "TAK";
            else
                return "NIE";
        }
    }
}
