using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Views
{
    internal class Program
    {
        // объявление классов AuthorizationUserVM и TaskItemVM
        static AuthorizationUserVM AuthUsers = new AuthorizationUserVM();
        static TaskItemVM Tasks = new TaskItemVM();

        static void Main()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ ПРОЕКТОМ");
            while (true)
            {
                Console.WriteLine("\n1. Войти в аккаунт;");
                Console.WriteLine("0. Выйти из системы.");
                Console.WriteLine();
                Console.Write("Выберите действие: ");
                string selectedAction = Console.ReadLine();
                switch (selectedAction)
                {
                    case "0":
                        return;
                    case "1":
                        LoginUser();
                        break;
                    default:
                        Console.WriteLine("Введённого варианта действия нет в списке!");
                        break;
                }
            }
        }

        // реализация авторизации пользователя в системе
        static void LoginUser()
        {
            Console.Write("\nВведите логин: ");
            string login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            if (AuthUsers.DataVerification(login, password)) // проверка логина и пароля
            {
                User currentUser = AuthUsers.GetUser(login);
                Console.WriteLine($"Добро пожаловать, {currentUser.FullName}!");
                if (currentUser.Role == "Manager")
                {
                    MenuManager(); // вывод меню управляющего
                }
                else
                {
                    MenuEmployee(currentUser); // вывод меню сотрудника
                }
            }
            else
            {
                Console.WriteLine("Введён неверный логин или пароль!");
            }
        }

        // метод для управления функциями управляющего
        static void MenuManager()
        {
            while (true)
            {
                Console.WriteLine("\nМЕНЮ УПРАВЛЯЮЩЕГО:");
                Console.WriteLine("1. Создать задачу для сотрудника;");
                Console.WriteLine("2. Зарегистрировать нового сотрудника;");
                Console.WriteLine("3. Выйти из аккаунта.");
                Console.Write("\nВыберите действие: ");
                string selectedAction = Console.ReadLine();

                switch (selectedAction)
                {
                    case "1":
                        CreateNewTask();
                        break;
                    case "2":
                        UserRegistration();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Введённого варианта действия нет в списке!");
                        break;
                }
            }
        }

        // метод для управления функциями сотрудника
        static void MenuEmployee(User employee)
        {
            while (true)
            {
                Console.WriteLine("\nМЕНЮ СОТРУДНИКА:");
                Console.WriteLine("1. Мои задачи;");
                Console.WriteLine("2. Изменить статус задачи;");
                Console.WriteLine("3. Выйти из аккаунта.");
                Console.Write("\nВыберите действие: ");
                string selectedAction = Console.ReadLine();

                switch (selectedAction)
                {
                    case "1":
                        ViewTasksEmployee(employee);
                        break;
                    case "2":
                        UpdateTaskStatus(employee);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Введённого варианта действия нет в списке!");
                        break;
                }
            }
        }

        // метод для вывода списка задач конкретного сотрудника
        static void ViewTasksEmployee(User employee)
        {
            List<TaskItem> tasksEmployee = Tasks.GetTasksForUser(employee.Login); // заполнение списка задач сотрудника
            Console.WriteLine("\nЗАДАЧИ:\n");
            foreach(TaskItem task in tasksEmployee) // отображение задач
            {
                Console.WriteLine($"ID проекта: {task.IdProject},\n Название: {task.Title},\n Описание: {task.Description},\n Статус: {task.Status};\n");
            }
        }

        // метод для изменения статуса задачи
        static void UpdateTaskStatus(User employee)
        {
            int numberTask;
            string newStatus;
            List<TaskItem> tasksEmployee = Tasks.GetTasksForUser(employee.Login); 
            Console.WriteLine("\nЗАДАЧИ:\n");
            foreach (TaskItem task in tasksEmployee)
            {
                Console.WriteLine($"ID проекта: {task.IdProject}, Название: {task.Title}, Статус: {task.Status};");
            }
            while (true)
            {
                Console.Write("Введите ID проекта задачи, статус которой хотите поменять: ");
                string inputNumberTask = Console.ReadLine();
                if (inputNumberTask.All(char.IsDigit)) // проверка, что введённое значение является числом
                {
                    numberTask = int.Parse(inputNumberTask); // перевод из строки в число
                    if (numberTask > 0 && numberTask <= tasksEmployee.Count) // проверка, что введённое число есть в списке
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Такого варианта нет в списке!\nПовторите попытку.\n");
                    }
                }
                else
                {
                    Console.WriteLine("Вы ввели некорректные значения!\nПовторите попытку.\n");
                }
            }
            while(true)
            {
                Console.Write("Выберите статус ( 1 - To do,  2 - In Progress,  3 - Done ): ");
                string choice = Console.ReadLine();

                // механизм для корректной записи статуса
                if (choice == "1")
                {
                    newStatus = "To do";
                    break;
                }
                else if (choice == "2")
                {
                    newStatus = "In Progress";
                    break;
                }
                else if (choice == "3")
                {
                    newStatus = "Done";
                    break;
                }
                else
                {
                    Console.WriteLine("Такого варианта нет в списке!\nПовторите попытку.\n");
                }
            }

            Tasks.UpdateTaskStatus(numberTask, newStatus); // вызов метода для изменения статуса задачи
            Console.WriteLine("Статус задачи успешно изменён!\n");
        }

        // метод для регистрации нового сотрудника
        static void UserRegistration()
        {
            string login, password, fullName; // объявление переменных
            while (true)
            {
                Console.Write("\nВведите логин: ");
                login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                password = Console.ReadLine();

                string pattern = @"^\S*$"; // регулярное выражение на проверку, что строке нет пробелов

                if (Regex.IsMatch(login, pattern) && Regex.IsMatch(password, pattern))
                {
                    User source = AuthUsers.GetUser(login); // проверка, что пользователя с таким же логином нет в файле
                    if (source == null)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Пользователь с таким логином уже существует!\nПовторите попытку.");
                    }
                }
                else
                {
                    Console.WriteLine("Поля логина и пароля не могут содержать в себе пробелы!\nПовторите попытку.");
                }
            }

            while (true)
            {
                Console.Write("Введите полное ФИО сотрудника: ");
                fullName = Console.ReadLine();

                string pattern = @"^[А-Яа-яЁё\s]+$"; // регулярное выражение, на наличие в строке только букв и пробелов

                if (Regex.IsMatch(fullName, pattern))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неккоректный ввод ФИО сотрудника, возможно вы использовали латинские буквы или иные символы!\nПовторите попытку.");
                }
            }

            User newUser = new User { Login = login, Password = password, Role = "Employee", FullName = fullName }; // заполнение объекта
            AuthUsers.SaveUser(newUser); // вызов метода для добавления нового пользователя в файл
            Console.WriteLine($"\nСотрудник {fullName} с логином {login} успешно зарегистрирован!");
        }

        // метод для назначения задачи сотруднику
        static void CreateNewTask()
        {
            int numberEmployee, idProject; // объявление переменных
            List<User> employees = AuthUsers.Users.Where(x => x.Role == "Employee").ToList(); // заполнение списка только сотрудниками
            if (employees.Count == 0) // если в файле нет сотрудников, можно перейти и создать
            {
                Console.Write("Нет зарегистированных сотрудников!\nХотите зарегистрировать нового сотрудника? ( 1 - да / любой символ - нет ): ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        UserRegistration();
                        return;
                    default:
                        return;
                }
            }
            Console.WriteLine("Список сотрудников:");
            for (int i = 0; i < employees.Count; i++) // вывод списка сотрудников
            {
                Console.WriteLine($"{i+1}. {employees[i].FullName} - {employees[i].Login}");
            }

            while (true)
            {
                Console.Write("Введите номер сотрудника, которму хотите назначить задачу: ");
                string inputNumberEmployee = Console.ReadLine();
                
                if (inputNumberEmployee.All(char.IsDigit)) // проверка, что введённое значение является числом
                {
                    numberEmployee = int.Parse(inputNumberEmployee) - 1; // перевод из строки в число, при этом вычитаем единицу, т.к. список начинается с нуля
                    if (numberEmployee >= 0 && numberEmployee < employees.Count) // проверяем, что сотрудник под данным число есть
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Такого варианта нет в списке!\nПовторите попытку.\n");
                    }
                }
                else
                {
                    Console.WriteLine("Вы ввели некорректные значения!\nПовторите попытку.\n");
                }
            }

            while (true) 
            {
                Console.Write("Введите идентификационный номер проекта: ");
                string strIdProject = Console.ReadLine();
                if (strIdProject.All(char.IsDigit)) // проверка, что введённое значение является числом
                {
                    idProject = int.Parse(strIdProject); //перевод из строки в число
                    if (!Tasks.GetTask(idProject)) // проверка, что задачи с таким номером проекта нет
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Задача с таким идентификационным номером проекта уже существует!\n");
                    }
                }
                else
                {
                    Console.WriteLine("Номер проекта может состоять только из цифр!\n");
                }
            }

            Console.Write("Введите название задачи: ");
            string title = Console.ReadLine();
            Console.Write("Введите описание задачи: ");
            string description = Console.ReadLine();
            Tasks.CreateTask(idProject, title, description, employees[numberEmployee].Login); // вызов метода добавления задачи для конкретного сотрудника
            Console.WriteLine("Задача успешно отправлена сотруднику!");
        }
    }
}
