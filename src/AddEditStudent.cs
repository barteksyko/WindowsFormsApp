using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public partial class AddEditStudent : Form
    {
        private string _filePath = Path.Combine(Environment.CurrentDirectory, "student.txt");

        public AddEditStudent(int IdConstr = 0)
        {
            InitializeComponent();
            if(IdConstr != 0)
            {
                var students = DeserializeFromFile();
                var student = students.FirstOrDefault(x => x.Id == IdConstr);

                if(student == null)
                {
                    throw new Exception("student Id is null");
                }

                tbId.Text = student.Id.ToString();
                tbName.Text = student.FirstName;
                tbSurname.Text = student.SecondName;
                tbMathematic.Text = student.Math;
                tbTechnology.Text = student.Technology;
                tbPhysics.Text = student.Physics;
            }
            
        }

        public void SerializeToFile(List<Student> students)
        {
            var serializer = new XmlSerializer(typeof(List<Student>));
            
            using (var streamWriter = new StreamWriter(_filePath))
            {
                serializer.Serialize(streamWriter, students);
                streamWriter.Close();
            }
        }

        public List<Student> DeserializeFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Student>();
            }

            var serializer = new XmlSerializer(typeof(List<Student>));

            using (var streamReader = new StreamReader(_filePath))
            {
                var students = (List<Student>)serializer.Deserialize(streamReader);
                streamReader.Close();
                return students;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = DeserializeFromFile();

            var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();

            var studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;

            var student = new Student()
            {
                Id = studentId,
                FirstName = tbName.Text,
                SecondName = tbSurname.Text,
                Math = tbMathematic.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbForeignLang.Text,
                Comments = rtbComments.Text
            };

            students.Add(student);

            SerializeToFile(students);
            Close();
        }
    }
}
