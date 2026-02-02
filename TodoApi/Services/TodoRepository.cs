using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoRepository : ITodoRepository
    {
        private readonly string _connectionString;

        public TodoRepository()
        {
            var env = Environment.GetEnvironmentVariable("TODO_DB");
            _connectionString = string.IsNullOrWhiteSpace(env) ? "Data Source=todos.db" : env!;
        }

        public async Task<int> CreateTodoAsync(Todo todo)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = @"\n                INSERT INTO Todos (Title, Description, IsCompleted, CreatedAt)\n                VALUES (@title, @description, @isCompleted, @createdAt);\n                SELECT last_insert_rowid();\n            ";

            command.Parameters.AddWithValue("@title", todo.Title ?? string.Empty);
            command.Parameters.AddWithValue("@description", todo.Description ?? string.Empty);
            command.Parameters.AddWithValue("@isCompleted", todo.IsCompleted ? 1 : 0);
            command.Parameters.AddWithValue("@createdAt", DateTime.UtcNow.ToString("o"));

            var result = await command.ExecuteScalarAsync();
            var id = Convert.ToInt32(result);
            return id;
        }

        public async Task<List<Todo>> GetAllTodosAsync()
        {
            var todos = new List<Todo>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Title, Description, IsCompleted, CreatedAt FROM Todos";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                todos.Add(new Todo
                {
                    Id = reader.GetInt32(0),
                    Title = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4))
                });
            }

            return todos;
        }

        public async Task<Todo?> GetTodoByIdAsync(int id)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Title, Description, IsCompleted, CreatedAt FROM Todos WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Todo
                {
                    Id = reader.GetInt32(0),
                    Title = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4))
                };
            }

            return null;
        }

        public async Task<int> UpdateTodoAsync(int id, Todo todo)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = @"\n                UPDATE Todos\n                SET Title = @title, Description = @description, IsCompleted = @isCompleted\n                WHERE Id = @id\n            ";

            command.Parameters.AddWithValue("@title", todo.Title ?? string.Empty);
            command.Parameters.AddWithValue("@description", todo.Description ?? string.Empty);
            command.Parameters.AddWithValue("@isCompleted", todo.IsCompleted ? 1 : 0);
            command.Parameters.AddWithValue("@id", id);

            var rows = await command.ExecuteNonQueryAsync();
            return rows;
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Todos WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            var rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}