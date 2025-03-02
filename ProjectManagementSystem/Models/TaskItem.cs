using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Models
{
    // класс для представления данных о назначенных задачах сотрудникам
    public partial class TaskItem
    {
        public int IdProject { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string UserLogin { get; set; }
    }
}
