using System;
using Newtonsoft.Json;
namespace MyJsonObjects
{
    public class AuthData
    {
        public AuthData(string login, string password, string processor)
        {
            this.login = login;
            this.password = password;
            this.processor = processor;
        }

        [JsonProperty("login")]
        public string login { get; set; }
        [JsonProperty("password")]
        public string password { get; set; }
        [JsonProperty("processor")]
        public string processor { get; set; }
    }
    public class ResetPassData
    {
        public ResetPassData(string code, string password, string confirm_password)
        {
            this.code = code;
            this.password = password;
            this.confirm_password = confirm_password;
        }

        [JsonProperty("code")]
        public string code { get; set; }
        [JsonProperty("password")]
        public string password { get; set; }
        [JsonProperty("confirm_password")]
        public string confirm_password { get; set; }
    }
    public class RegistrationData
    {
        public RegistrationData(string name, string surname, string father_name, DateTime birthday,
            string email, string login, string password, string confirm_password, string processor)
        {
            this.name = name;
            this.surname = surname;
            this.father_name = father_name;
            this.birthday = birthday;
            this.email = email;
            this.login = login;
            this.password = password;
            this.confirm_password = confirm_password;
            this.processor = processor;
        }

        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("surname")]
        public string surname { get; set; }
        [JsonProperty("father_name")]
        public string father_name { get; set; }
        [JsonProperty("birthday")]
        public DateTime birthday { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("login")]
        public string login { get; set; }
        [JsonProperty("password")]
        public string password { get; set; }
        [JsonProperty("confirm_password")]
        public string confirm_password { get; set; }
        [JsonProperty("processor")]
        public string processor { get; set; }
    }
    public class Drug
    {
        public Drug(string name, DateTime date_of_manufacture, DateTime date_of_expiration, decimal price)
        {
            this.name = name;
            this.date_of_manufacture = date_of_manufacture;
            this.date_of_expiration = date_of_expiration;
            this.price = price;
        }

        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("date_of_manufacture")]
        public DateTime date_of_manufacture { get; set; }
        [JsonProperty("date_of_expiration")]
        public DateTime date_of_expiration { get; set; }
        [JsonProperty("price")]
        public decimal price { get; set; }
    }
    public class Employee
    {
        public Employee(string name, string surname, DateTime employmentDate,string position,
            string company)
        {
            this.name = name;
            this.surname = surname;
            this.employmentDate = employmentDate;
            this.position = position;
            this.company = company;
        }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("surname")]
        public string surname { get; set; }
        [JsonProperty("employmentDate")]
        public DateTime employmentDate{ get; set; }
        [JsonProperty("position")]
        public string position { get; set; }
        [JsonProperty("company")]
        public string company { get; set; }
    }
}
