using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace TestApp
{
    public partial class Form1 : Form
    {
        private DataTable clientsTable;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Width = (int)(this.Width*0.9);
            dataGridView1.Height = (int)(this.Height / 1.6);
            dataGridView1.Location = new Point(this.Width/2- dataGridView1.Width/2, (int)(this.Height*0.1));
            loadButton.Size = new Size(120,60);
            loadButton.Location = new Point(dataGridView1.Location.X+dataGridView1.Width/4-loadButton.Width/2, dataGridView1.Location.Y+(int)(dataGridView1.Height*1.1));
            saveButton.Size = new Size(120, 60);
            saveButton.Location = new Point(loadButton.Location.X+loadButton.Width*2,loadButton.Location.Y);
            toDbButton.Size = new Size(120, 60);
            toDbButton.Location = new Point(saveButton.Location.X + saveButton.Width * 2, saveButton.Location.Y);
            clientsTable = new DataTable();
            clientsTable.Columns.Add("CARDCODE", typeof(string));
            clientsTable.Columns.Add("STARTDATE", typeof(DateTime));
            clientsTable.Columns.Add("FINISHDATE", typeof(DateTime));
            clientsTable.Columns.Add("LASTNAME", typeof(string));
            clientsTable.Columns.Add("FIRSTNAME", typeof(string));
            clientsTable.Columns.Add("SURNAME", typeof(string));
            clientsTable.Columns.Add("FULLNAME", typeof(string));
            clientsTable.Columns.Add("GENDERID", typeof(int));
            clientsTable.Columns.Add("BIRTHDAY", typeof(DateTime));
            clientsTable.Columns.Add("PHONEHOME", typeof(string));
            clientsTable.Columns.Add("PHONEMOBIL", typeof(string));
            clientsTable.Columns.Add("EMAIL", typeof(string));
            clientsTable.Columns.Add("CITY", typeof(string));
            clientsTable.Columns.Add("STREET", typeof(string));
            clientsTable.Columns.Add("HOUSE", typeof(string));
            clientsTable.Columns.Add("APARTMENT", typeof(string));
            clientsTable.Columns.Add("ALTADDRESS", typeof(string));
            clientsTable.Columns.Add("CARDTYPE", typeof(string));
            clientsTable.Columns.Add("OWNERGUID", typeof(Guid));
            clientsTable.Columns.Add("CARDPER", typeof(int));
            clientsTable.Columns.Add("TURNOVER", typeof(decimal));
            dataGridView1.DataSource = clientsTable;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            DatabaseWorker dw = new DatabaseWorker("Server=localhost;Database=master;Trusted_Connection=True;");
            if (dataGridView1.Rows.Count > 1) dataGridView1.DataSource = null;
            clientsTable = dw.ReadTable(clientsTable,"Clients");
            dataGridView1.DataSource = clientsTable;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            //string[] fields = { "CARDCODE", "STARTDATE", "FINISHDATE", "LASTNAME", "FIRSTNAME", "SURNAME", 
            //    "FULLNAME", "GENDERID", "BIRTHDAY","PHONEHOME","PHONEMOBIL","EMAIL","CITY","STREET","HOUSE","APARTMENT",
            //"ALTADDRESS", "CARDTYPE", "OWNERGUID", "CARDPER","TURNOVER"};
            DatabaseWorker dw = new DatabaseWorker("Server=localhost;Database=master;Trusted_Connection=True;");
            dw.SaveData(clientsTable);
        }

        private void toDbButton_Click(object sender, EventArgs e)
        {
            DatabaseWorker dw = new DatabaseWorker("Server=localhost;Database=master;Trusted_Connection=True;");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.Title = "Select XML File";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                dw.LoadXML(openFileDialog.FileName, "Clients");
            }
        }
    }
}
