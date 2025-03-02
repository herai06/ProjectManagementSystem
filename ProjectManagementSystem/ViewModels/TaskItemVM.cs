using ProjectManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManagementSystem.ViewModels
{
    public class TaskItemVM
    {
        private List<TaskItem> TaskItems = new List<TaskItem>();

        // конструктор для заполнения списка со всеми задачами
        public TaskItemVM()
        {
            if (File.Exists("tasks.json"))
            {
                string tasksJson = File.ReadAllText("tasks.json");
                TaskItems = JsonSerializer.Deserialize<List<TaskItem>>(tasksJson);
            }
        }

        // метод для создания новой задачи
        public void CreateTask(int idProject, string title, string description, string userLogin)
        {
            TaskItem newTask = new TaskItem
            {
                IdProject = idProject,
                Title = title,
                Description = description,
                Status = "To do",
                UserLogin = userLogin
            };
            TaskItems.Add(newTask);
            SaveTasks();
        }

        // метод для получения задач, назначенных конкретному сотруднику
        public List<TaskItem> GetTasksForUser(string userLogin)
        {
            return TaskItems.FindAll(x => x.UserLogin == userLogin);
        }

        // метод для изменения статуса задачи по переданному идентификатору проекта
        public void UpdateTaskStatus(int idProject, string status)
        {
            TaskItem updateTask = TaskItems.Find(x => x.IdProject == idProject);
            if (updateTask != null)
            {
                updateTask.Status = status;
                SaveTasks();
            }
        }

        // метод для проверки наличия задачи с полученным идентификатором проета
        public bool GetTask(int idProject)
        {
            return TaskItems.Exists(x => x.IdProject == idProject);
        }

        // метод для сохранения данных в файле с задачами
        private void SaveTasks()
        {
            string tasksJson = JsonSerializer.Serialize(TaskItems);
            File.WriteAllText("tasks.json", tasksJson);
        }
    }
}
