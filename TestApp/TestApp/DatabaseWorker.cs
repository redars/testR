using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace TestApp
{
    internal class DatabaseWorker
    {
        private string connectionString;

        public DatabaseWorker (string connection) {
            this.connectionString = connection; //Server=localhost;Database=master;Trusted_Connection=True;
        }
        private DateTime ParseDate(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return DateTime.MinValue;
            }
            return DateTime.Parse(value);
        }

        private void CreateTable(string strConnection, string tableName)
        {
            string createTableQuery = @"
            CREATE TABLE " + tableName + @" (
                CARDCODE VARCHAR(50),
                STARTDATE DATE,
                FINISHDATE DATE,
                LASTNAME NVARCHAR(50),
                FIRSTNAME NVARCHAR(50),
                SURNAME NVARCHAR(50),
                FULLNAME NVARCHAR(150),
                GENDERID INT,
                BIRTHDAY DATE,
                PHONEHOME NVARCHAR(50),
                PHONEMOBIL NVARCHAR(50),
                EMAIL NVARCHAR(50),
                CITY NVARCHAR(50),
                STREET NVARCHAR(50),
                HOUSE NVARCHAR(10),
                APARTMENT NVARCHAR(10),
                ALTADDRESS NVARCHAR(100),
                CARDTYPE NVARCHAR(50),
                OWNERGUID UNIQUEIDENTIFIER,
                CARDPER INT,
                TURNOVER DECIMAL
            )";

            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public DataTable ReadTable(DataTable clientsTable,string tableName)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "SELECT * FROM Clients";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            connection.Close();
            return dataTable;
        }

        public void LoadXML (string path, string tableName){
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path); 
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            foreach (XmlNode cardNode in xmlDocument.SelectNodes("//Card"))
            {
                string cardCode = cardNode.Attributes["CARDCODE"].Value;
                DateTime startDate = ParseDate(cardNode.Attributes["STARTDATE"].Value);
                DateTime finishDate = ParseDate(cardNode.Attributes["FINISHDATE"].Value);
                string lastName = cardNode.Attributes["LASTNAME"].Value;
                string firstName = cardNode.Attributes["FIRSTNAME"].Value;
                string surname = cardNode.Attributes["SURNAME"].Value;
                string fullName = cardNode.Attributes["FULLNAME"].Value;
                int genderId = int.Parse(cardNode.Attributes["GENDERID"].Value);
                DateTime birthday = ParseDate(cardNode.Attributes["BIRTHDAY"].Value);
                string phoneHome = cardNode.Attributes["PHONEHOME"].Value;
                string phoneMobil = cardNode.Attributes["PHONEMOBIL"].Value;
                string email = cardNode.Attributes["EMAIL"].Value;
                string city = cardNode.Attributes["CITY"].Value;
                string street = cardNode.Attributes["STREET"].Value;
                string house = cardNode.Attributes["HOUSE"].Value;
                string apartment = cardNode.Attributes["APARTMENT"].Value;
                string altAddress = cardNode.Attributes["ALTADDRESS"].Value;
                string cardType = cardNode.Attributes["CARDTYPE"].Value;
                Guid ownerGuid = Guid.Parse(cardNode.Attributes["OWNERGUID"].Value);
                int cardPer = int.Parse(cardNode.Attributes["CARDPER"].Value);
                string turnover = cardNode.Attributes["TURNOVER"].Value;

                string query = $"INSERT INTO {tableName} (CARDCODE, STARTDATE, FINISHDATE, LASTNAME, FIRSTNAME, SURNAME, FULLNAME, GENDERID, BIRTHDAY, PHONEHOME, PHONEMOBIL, EMAIL, CITY, STREET, HOUSE, APARTMENT, ALTADDRESS, CARDTYPE, OWNERGUID, CARDPER, TURNOVER) VALUES" +
                    $"('{cardCode}', '{startDate}', '{finishDate}', '{lastName}', '{firstName}', '{surname}', '{fullName}', {genderId}, '{birthday}', '{phoneHome}', '{phoneMobil}', '{email}', '{city}', '{street}', '{house}', '{apartment}', '{altAddress}', '{cardType}', '{ownerGuid}', {cardPer}, {turnover})";

                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        public void SaveData(DataTable dataTable)
        {
            string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string tableName = "Clients";

                string deleteQuery = "DELETE FROM " + tableName;
                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                {
                    deleteCommand.ExecuteNonQuery();
                }
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
