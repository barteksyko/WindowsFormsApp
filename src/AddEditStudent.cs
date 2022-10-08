using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public partial class AddEditStudent : Form
    {
        private int _studentId;
        private Student _student;
        private List<Group> _groups;

        public delegate void MySimpleDelegate();
        public event MySimpleDelegate StudentAdded;

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        public AddEditStudent(int IdConstr = 0)
        {
            InitializeComponent();
            _studentId = IdConstr;
            _groups = new List<Group>
            {
                new Group{Id = 0, Name = "brak"},
                new Group{Id = 1, Name = "1a"},
                new Group{Id = 2, Name = "2a"},
            };
            InitGroupCombobox();

            GetStudentDate();
            tbName.Select();
        }

        private void InitGroupCombobox()
        {
            cmbGroup.DataSource = _groups;
            cmbGroup.DisplayMember = "Name";
            cmbGroup.ValueMember = "Id";
        }

        private void OnStudentAdded()
        {
            StudentAdded?.Invoke();
        }

        private void GetStudentDate()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                {
                    throw new Exception("student Id is null");
                }

                FillTextBoxes();
            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbName.Text = _student.FirstName;
            tbSurname.Text = _student.SecondName;
            tbMathematic.Text = _student.Math;
            tbTechnology.Text = _student.Technology;
            tbPhysics.Text = _student.Physics;
            tbPolishLang.Text = _student.PolishLang;
            tbForeignLang.Text = _student.ForeignLang;
            rtbComments.Text = _student.Comments;
            cbAdditionalClasses.Checked = _student.AdditionalClasses;
            cmbGroup.SelectedItem = _groups.FirstOrDefault(x => x.Id == _student.GroupId);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
            else
            {
                AssignIdtoNewStudent(students);
            }

            AddNewUserToList(students);

            _fileHelper.SerializeToFile(students);

            OnStudentAdded();

            await LongProcessAsync();

            Close();
        }

        private async Task LongProcessAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(3000);
            });

        }

        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student()
            {
                Id = _studentId,
                FirstName = tbName.Text,
                SecondName = tbSurname.Text,
                Math = tbMathematic.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbForeignLang.Text,
                Comments = rtbComments.Text,
                AdditionalClasses = cbAdditionalClasses.Checked,
                GroupId = (cmbGroup.SelectedItem as Group).Id
            };

            students.Add(student);
        }

        private void AssignIdtoNewStudent(List<Student> students)
        {
            var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;
        }
    }
}
