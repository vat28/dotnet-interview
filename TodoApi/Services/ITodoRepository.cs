namespace TodoApi.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TodoApi.Models;

    public interface ITodoRepository
    {
        Task<int> CreateTodoAsync(Todo todo);
        Task<List<Todo>> GetAllTodosAsync();
        Task<Todo?> GetTodoByIdAsync(int id);
        Task<int> UpdateTodoAsync(int id, Todo todo);
        Task<bool> DeleteTodoAsync(int id);
    }
}