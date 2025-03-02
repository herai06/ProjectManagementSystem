using ProjectManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManagementSystem.ViewModels
{
    public class AuthorizationUserVM
    {
        public List<User> Users = new List<User>();

        // конструктор для заполнения списка пользователей данными из файла
        public AuthorizationUserVM()
        {
            if (File.Exists("users.json"))
            {
                string usersJson = File.ReadAllText("users.json");
                Users = JsonSerializer.Deserialize<List<User>>(usersJson);
            }
        }

        // метод для проверки, что пользователь с введёнными данными существует
        public bool DataVerification(string login, string password)
        {
            return Users.Exists(x => x.Login == login && x.Password == password);
        }

        // метод для получения пользователя по его логину
        public User GetUser(string login)
        {
            return Users.Find(x => x.Login == login);
        }

        // метод для сохранения данных пользователя в файл
        public void SaveUser(User newUser)
        {
            Users.Add(newUser);
            string userJson = JsonSerializer.Serialize(Users);
            File.WriteAllText("users.json", userJson);
        }
    }
}
