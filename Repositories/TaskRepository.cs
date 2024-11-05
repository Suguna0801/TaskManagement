using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using signedup.Models;

namespace signedup.Repositories
{
    public class TaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Users", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                Id = (int)reader["Id"],
                                Username = reader["Username"].ToString()
                                // Include other fields as necessary
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }
        public void AssignTask(int taskId, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("AssignTaskToUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TaskId", taskId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }



        //public void AssignUserTask(int userId, int taskId, DateTime deadline)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        using (var transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                //System.IO.File.AppendAllText("debuglog.txt", $"Connection String: {_connectionString}\n");
        //                var command = new SqlCommand("INSERT INTO UserTasks (UserId, TaskId, Deadline) VALUES (@UserId, @TaskId, @Deadline)", connection, transaction);
        //                command.Parameters.AddWithValue("@UserId", userId);
        //                command.Parameters.AddWithValue("@TaskId", taskId);
        //                command.Parameters.AddWithValue("@Deadline", deadline);
        //                System.IO.File.AppendAllText("debuglog.txt", $"Executing SQL: INSERT INTO UserTasks (UserId, TaskId, Deadline) VALUES ({userId}, {taskId}, '{deadline}')\n");
        //                command.ExecuteNonQuery();

        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                System.IO.File.AppendAllText("errorlog.txt", ex.ToString());
        //                throw; // Rethrow the exception to be handled by the controller
        //            }
        //        }
        //    }
        //}

        public void AssignUserTask(int userId, int taskId, DateTime deadline)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var command = new SqlCommand("AssignUserTask", connection, transaction);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@TaskId", taskId);
                    command.Parameters.AddWithValue("@Deadline", deadline);

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }

        public IEnumerable<User> GetDesigners()
        {
            List<User> designers = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Role = @Role", connection))
                {
                    command.Parameters.AddWithValue("@Role", "Designer");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                Id = (int)reader["Id"], 
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Username = reader["Username"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                            designers.Add(user);
                        }
                    }
                }
            }
            return designers;
        }



        public List<User> GetUsersByRole(string role)
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetUsersByRole", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@Role", role);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = (int)reader["Id"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            // Populate other fields as needed
                        });
                    }
                }
            }
            return users;
            //return dbContext.Users.Where(u => u.Role == role).ToList();
        }
        

        //public void AssignUserTask(int taskId, int userId, DateTime deadline)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        var command = new SqlCommand("AssignUserTask", connection) { CommandType = CommandType.StoredProcedure };
        //        command.Parameters.AddWithValue("@TaskId", taskId);
        //        command.Parameters.AddWithValue("@AssignedUserId", userId);
        //        command.Parameters.AddWithValue("@Deadline", deadline);

        //        connection.Open();
        //        command.ExecuteNonQuery();
        //    }
        //}

       

        public List<Task> GetUnassignedTasks()
        {
            List<Task> tasks = new List<Task>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT TaskId, Title, Description FROM Tasks WHERE AssignedUserId IS NULL"; // Example query
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            TaskId = reader["TaskId"] != DBNull.Value ? Convert.ToInt32(reader["TaskId"]) : 0,
                            Title = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : string.Empty,
                            Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                        });
                    }
                }
            }
            return tasks;
        }

        public List<Task> GetAssignedTasks()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Tasks WHERE AssignedUserId IS NOT NULL", connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    var tasks = new List<Task>();
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            //TaskId = (int)reader["TaskId"],
                            TaskId = reader["TaskId"] != DBNull.Value ? Convert.ToInt32(reader["TaskId"]) : 0,
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            Deadline = reader["Deadline"] != DBNull.Value ? Convert.ToDateTime(reader["Deadline"]) : (DateTime?)null,
                            //Role = reader["Role"].ToString(),
                            AssignedUserId = reader["AssignedUserId"] != DBNull.Value ? Convert.ToInt32(reader["AssignedUserId"]) : (int?)null
                        });
                   
                    }
                    return tasks;
                }
            }
        }


        // Get All Tasks
        public List<Task> GetAllTasks()
        {
            var tasks = new List<Task>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetAllTasks", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            TaskId = (int)reader["TaskId"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"]?.ToString(),
                            //AssignedUserId = reader["AssignedUserId"] as int?,
                            //Deadline = reader["Deadline"] as DateTime?,
                            //IsCompleted = (bool)reader["IsCompleted"]
                        });
                    }
                }
            }
            return tasks;
        }


        public void AddTask(Task task)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("AddTask", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@Description", task.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@AssignedUserId", task.AssignedUserId ?? (object)DBNull.Value);
                //command.Parameters.AddWithValue("@Deadline", task.Deadline ?? (object)DBNull.Value);
                //command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        //public List<Task> GetAllTasks()
        //{
        //    var tasks = new List<Task>();
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        var command = new SqlCommand("GetAllTasks", connection) { CommandType = CommandType.StoredProcedure };
        //        connection.Open();
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            tasks.Add(new Task
        //            {
        //                Id = (int)reader["Id"],
        //                Title = reader["Title"].ToString(),
        //                Description = reader["Description"]?.ToString(),
        //                AssignedUserId = reader["AssignedUserId"] as int?,
        //                Deadline = reader["Deadline"] as DateTime?,
        //                IsCompleted = (bool)reader["IsCompleted"]
        //            });
        //        }
        //    }
        //    return tasks;
        //}

        public void UpdateTask(Task task)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UpdateTask", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@TaskId", task.TaskId);
                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@Description", task.Description ?? (object)DBNull.Value);
                //command.Parameters.AddWithValue("@AssignedUserId", task.AssignedUserId ?? (object)DBNull.Value);
                //command.Parameters.AddWithValue("@Deadline", task.Deadline ?? (object)DBNull.Value);
                //command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteTask(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DeleteTask", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@TaskId", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        //public void AssignTaskToUser(int taskId, string userId, DateTime deadline)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        // Begin a transaction
        //        using (var transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                // 1. Assign the task to the user
        //                var assignTaskCommand = new SqlCommand("UPDATE Tasks SET AssignedUserId = @UserId WHERE TaskId = @TaskId", connection, transaction);
        //                assignTaskCommand.Parameters.AddWithValue("@UserId", userId);
        //                assignTaskCommand.Parameters.AddWithValue("@TaskId", taskId);
        //                assignTaskCommand.Parameters.AddWithValue("@Deadline", deadline);
        //                int rowsAffected = assignTaskCommand.ExecuteNonQuery();

        //                if (rowsAffected == 0)
        //                {
        //                    throw new Exception("No rows were updated in the Tasks table. Check if the TaskId is correct.");
        //                }

        //                // 2. Remove the task from the unassigned tasks
        //                var deleteUnassignedTaskCommand = new SqlCommand("DELETE FROM UserTasks WHERE TaskId = @TaskId", connection, transaction);
        //                deleteUnassignedTaskCommand.Parameters.AddWithValue("@TaskId", taskId);
        //                int unassignedRowsAffected = deleteUnassignedTaskCommand.ExecuteNonQuery();

        //                // Optional: Check if any rows were deleted
        //                if (unassignedRowsAffected == 0)
        //                {
        //                    // Log this if necessary
        //                }

        //                // 3. Remove the user from new users list
        //                var deleteUserCommand = new SqlCommand("DELETE FROM Users WHERE Id = @UserId AND IsNewEmployee = 1", connection, transaction);
        //                deleteUserCommand.Parameters.AddWithValue("@UserId", userId);
        //                int userRowsAffected = deleteUserCommand.ExecuteNonQuery();

        //                // Optional: Check if any rows were deleted
        //                if (userRowsAffected == 0)
        //                {
        //                    // Log this if necessary
        //                }

        //                // Commit the transaction
        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                // Rollback the transaction if any error occurs
        //                transaction.Rollback();
        //                System.IO.File.AppendAllText("errorlog.txt", ex.ToString()); // Log to a file
        //                throw; // Rethrow the exception to handle it in the controller
        //            }
        //        }
        //    }
        //}

        public void AssignTaskToUser(int taskId, string userId, DateTime deadline)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("AssignTaskToUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TaskId", taskId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Deadline", deadline);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //public void RemoveUserFromNewEmployees(string userId)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        string query = "DELETE FROM Users WHERE Id = @UserId AND IsNewEmployee = 1"; // Assuming you have a column to track new employees
        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.Parameters.AddWithValue("@UserId", userId);

        //        connection.Open();
        //        command.ExecuteNonQuery();
        //    }
        //}

        public Task GetTaskById(int id)
        {
            Task task = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Tasks WHERE TaskId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        task = new Task
                        {
                            TaskId = (int)reader["TaskId"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            //AssignedUserId = reader["AssignedUserId"] as int?,
                            //Deadline = reader["Deadline"] as DateTime?,
                            //IsCompleted = (bool)reader["IsCompleted"]
                        };
                    }
                }
            }
            return task;
        }

        
        public List<Task> GetAssignedTasksByUserId(int userId)
        {
            var tasks = new List<Task>();
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("sp_GetAssignedTasksByUserId", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            TaskId = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"]?.ToString(),
                            //Deadline = reader["Deadline"] as DateTime?,
                            //IsCompleted = (bool)reader["IsCompleted"]
                        });
                    }
                }
            }
            return tasks;
        }

        public void RemoveTask(int taskId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Tasks WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", taskId);
                command.ExecuteNonQuery();
            }
        }
        public List<Task> GetAssignedTasksForUser(string username)
        {
            var assignedTasks = new List<Task>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetAssignedTasksForUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Assuming the stored procedure takes a parameter for username
                command.Parameters.Add(new SqlParameter("@Username", username));

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        assignedTasks.Add(new Task
                        {
                            TaskId = (int)reader["TaskId"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            Deadline = reader["Deadline"] as DateTime?,
                            IsCompleted = (bool)reader["IsCompleted"]
                        });
                    }
                }
            }

            return assignedTasks;
        }

        public List<Task> GetCompletedTasksForUser(string username)
        {
            var completedTasks = new List<Task>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("GetCompletedTasksForUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Assuming the stored procedure takes a parameter for username
                command.Parameters.Add(new SqlParameter("@Username", username));

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        completedTasks.Add(new Task
                        {
                            TaskId = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            //CompletionDate = reader["CompletionDate"] as DateTime? // Assuming you have a CompletionDate column
                        });
                    }
                }
            }

            return completedTasks;
        }

        //public void AssignTaskToNewEmployee(int taskId, int userId, DateTime deadline)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        using (var transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                // Step 1: Assign the task to the user
        //                var assignTaskCommand = new SqlCommand("AssignUserTask", connection, transaction)
        //                {
        //                    CommandType = CommandType.StoredProcedure
        //                };
        //                assignTaskCommand.Parameters.AddWithValue("@UserId", userId);
        //                assignTaskCommand.Parameters.AddWithValue("@TaskId", taskId);
        //                assignTaskCommand.Parameters.AddWithValue("@Deadline", deadline);
        //                assignTaskCommand.ExecuteNonQuery();

        //                // Step 2: Remove the task from the unassigned tasks list
        //                var removeTaskCommand = new SqlCommand("RemoveTaskFromUnassigned", connection, transaction)
        //                {
        //                    CommandType = CommandType.StoredProcedure
        //                };
        //                removeTaskCommand.Parameters.AddWithValue("@TaskId", taskId);
        //                removeTaskCommand.ExecuteNonQuery();

        //                // Step 3: Remove the user from the new employees list
        //                var removeUserCommand = new SqlCommand("RemoveUserFromNewEmployees", connection, transaction)
        //                {
        //                    CommandType = CommandType.StoredProcedure
        //                };
        //                removeUserCommand.Parameters.AddWithValue("@UserId", userId);
        //                removeUserCommand.ExecuteNonQuery();

        //                // Commit the transaction
        //                transaction.Commit();
        //            }
        //            catch
        //            {
        //                // Rollback the transaction if any command fails
        //                transaction.Rollback();
        //                throw; // You can handle the exception as needed
        //            }
        //        }
        //    }
        //}

    }
}
