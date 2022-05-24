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
    public partial class Main : Form
    {
        private string _filePath = Path.Combine(Environment.CurrentDirectory, "student.txt");

        public Main()
        {
            InitializeComponent();
            //var students = new List<Student>();
            //students.Add(new Student { FirstName = "Maja"});
            //students.Add(new Student { FirstName = "Marek"});
            //students.Add(new Student { FirstName = "Małgosia"});

            //SerializeToFile(students);
            var students = DeserializeFromFile();
            foreach(var student in students)
            {
                MessageBox.Show(student.FirstName);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }
    }
}
