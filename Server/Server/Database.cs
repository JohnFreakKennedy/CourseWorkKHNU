using System;
using Microsoft.Data.SqlClient;
using System.Data;
using MyJsonObjects;
using System.Collections.Generic;
namespace Server
{
    class Database
    {
        static string connection_string = @"Data Source=DankovPC;Initial Catalog=DrugStore;Integrated Security=True";
        private SqlConnection connection = new SqlConnection(connection_string);
        public Database()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }
        // ~Database()
        //{
        //    if (connection.State == ConnectionState.Open)
        //        connection.Close();
        //}
        public List<Drug> GetDrugsList()
        {
            List<Drug> list = new List<Drug>();
            SqlCommand command = new SqlCommand("SELECT name,date_of_manufacture,date_of_expiration,price FROM Drug", connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string name = Convert.ToString(reader.GetValue(0));
                    var date_of_manufacture = Convert.ToDateTime(reader.GetValue(1));
                    var date_of_expiration = Convert.ToDateTime(reader.GetValue(2));
                    var price = Convert.ToDecimal(reader.GetValue(3));
                    list.Add(new Drug(name, date_of_manufacture,date_of_expiration,price));
                }
            }
            reader.Close();
            connection.Close();
            return list;
        }
        public List<Employee> GetEmployeeList()
        {
            List<Employee> list = new List<Employee>();
            SqlCommand command = new SqlCommand("SELECT Person.name AS Name , surname AS Surname, date_of_employment AS EmploymentDate, " +
                "Positions.name AS Position, Pharmacy.name AS Company FROM Person " +
                " INNER JOIN Employee ON(Employee.person_id = Person.id) " +
                " INNER JOIN Pharmacy ON(Pharmacy.id = Employee.pharmacy_id) " +
                " INNER JOIN Positions ON(Employee.position_id = Positions.id); ", connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string name = Convert.ToString(reader.GetValue(0));
                    var surname = Convert.ToString(reader.GetValue(1));
                    var employmentDate = Convert.ToDateTime(reader.GetValue(2));
                    var position = Convert.ToString(reader.GetValue(3));
                    var company = Convert.ToString(reader.GetValue(4));
                    list.Add(new Employee(name,surname,employmentDate,position,company));
                }
            }
            reader.Close();
            connection.Close();
            return list;
        }
        public int GetGroupId(string groupName)
        {
            SqlCommand command = new SqlCommand("SELECT id FROM GroupUsers WHERE [name] = @groupName", connection);
            command.Parameters.Add("@groupName", SqlDbType.NVarChar).Value = groupName;
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int user_id = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            return user_id;
        }
        public string GetGroupName(int groupId)
        {
            SqlCommand command = new SqlCommand("SELECT [name] FROM GroupUsers WHERE id = @id", connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = groupId;
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string group_name = Convert.ToString(reader.GetValue(0));
            reader.Close();
            return group_name;
        }
        public int AddUser(string email, string login, string passwordHash)
        {
            SqlCommand command = new SqlCommand("INSERT INTO Users (email,[login],passwordHash,groupId) VALUES (@email,@login,@passwordHash,@groupId)", connection);
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
            command.Parameters.Add("@login", SqlDbType.NVarChar).Value = login;
            command.Parameters.Add("@passwordHash", SqlDbType.NVarChar).Value = passwordHash;
            command.Parameters.Add("@groupId", SqlDbType.Int).Value = GetGroupId("User");
            return command.ExecuteNonQuery();
        }
        public int ChangePassword(string email, string password_hash)
        {
            SqlCommand command = new SqlCommand("UPDATE Users SET password_hash = @passwordHash WHERE email = @email", connection);
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
            command.Parameters.Add("@passwordHash", SqlDbType.NVarChar).Value = password_hash;
            return command.ExecuteNonQuery();
        }
        public bool CheckUserLogin(string login)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT id FROM Users WHERE [login] = @login", connection);
            command.Parameters.Add("@login", SqlDbType.NVarChar).Value = login;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            return Convert.ToBoolean(table.Rows.Count);
        }
        public bool CheckUserEmail(string email)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT id FROM Users WHERE email = @email", connection);
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            return Convert.ToBoolean(table.Rows.Count);
        }
        public bool CheckUser(string login, string passwordHash)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT id FROM Users WHERE [login] = @login AND passwordHash = @passwordHash", connection);
            command.Parameters.Add("@login", SqlDbType.NVarChar).Value = login;
            command.Parameters.Add("@passwordHash", SqlDbType.NVarChar).Value = passwordHash;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            return Convert.ToBoolean(table.Rows.Count);
        }
        public int GetUserId(string login)
        {
            SqlCommand command = new SqlCommand("SELECT id FROM Users WHERE [login] = @login", connection);
            command.Parameters.Add("@login", SqlDbType.NVarChar).Value = login;
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int user_id = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            return user_id;
        }
        public string GetUserEmail(string login)
        {
            SqlCommand command = new SqlCommand("SELECT email FROM Users WHERE [login] = @login", connection);
            command.Parameters.Add("@login", SqlDbType.NVarChar).Value = login;
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string email = Convert.ToString(reader.GetValue(0));
            reader.Close();
            return email;
        }
        public string GetUserGroup(string login)
        {
            SqlCommand command = new SqlCommand("SELECT groupId FROM Users WHERE [login] = @login", connection);
            command.Parameters.Add("@login", SqlDbType.NVarChar).Value = login;
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int group_id = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            return GetGroupName(group_id);
        }
        public int AddIron(string ironHash, string login)
        {
            SqlCommand command = new SqlCommand("INSERT INTO Iron (ironHash,userId) VALUES (@ironHash,@userId)", connection);
            command.Parameters.Add("@ironHash", SqlDbType.NVarChar).Value = ironHash;
            command.Parameters.Add("@userId", SqlDbType.Int).Value = GetUserId( login);
            return command.ExecuteNonQuery();
        }
        public bool CheckIron(string ironHash, string login)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand("SELECT id FROM Iron WHERE userId = @userId AND ironHash = @ironHash", connection);
            command.Parameters.Add("@ironHash", SqlDbType.NVarChar).Value = ironHash;
            command.Parameters.Add("@userId", SqlDbType.Int).Value = GetUserId(login);
            adapter.SelectCommand = command;
            adapter.Fill(table);
            return Convert.ToBoolean(table.Rows.Count);
        }
        public int AddPerson(string name, string surname, string fatherName, DateTime birthday, int userId)
        {
            SqlCommand command = new SqlCommand("INSERT INTO Person ([name],surname,fatherName,birthday,userId) VALUES (@name,@surname,@fatherName,@birthday,@userId)", connection);
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            command.Parameters.Add("@surname", SqlDbType.NVarChar).Value = surname;
            command.Parameters.Add("@fatherName", SqlDbType.NVarChar).Value = fatherName;
            command.Parameters.Add("@birthday", SqlDbType.Date).Value = birthday;
            command.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
            return command.ExecuteNonQuery();
        }
    }
}
