using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using signedup.Models;

namespace signedup.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        private string connectionString = "Data Source=DESKTOP-ENRUGTD\\SQLEXPRESS;Initial Catalog=suguna;Integrated Security=True;";

        // Insert a new user
        public void InsertUser(User user)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_InsertUser", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@State", user.State);
                    cmd.Parameters.AddWithValue("@City", user.City);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveUserFromNewEmployees(string userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Assuming you have an IsNewEmployee column to identify new employees
                string query = "DELETE FROM Users WHERE Id = @UserId AND IsNewEmployee = 1";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        // Update an existing user
        public void UpdateUser(User user)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_UpdateUser", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@State", user.State);
                    cmd.Parameters.AddWithValue("@City", user.City);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                   

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Delete a user
        public void DeleteUser(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_DeleteUser", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Get all users
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetAllUsers", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = new User
                        {
                            Id = (int)reader["Id"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DateOfBirth = (DateTime)reader["DateOfBirth"],
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Role = reader["Role"].ToString(),
                            Address = reader["Address"].ToString(),
                            State = reader["State"].ToString(),
                            City = reader["City"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString()
                        };
                        users.Add(user);
                    }
                }
            }
            return users;
        }

        // Get user by Id
        public User GetUserById(int id)
        {
            User user = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetUserById", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = (int)reader["Id"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DateOfBirth = (DateTime)reader["DateOfBirth"],
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Role = reader["Role"].ToString(),
                            Address = reader["Address"].ToString(),
                            State = reader["State"].ToString(),
                            City = reader["City"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString()
                        };
                    }
                }
            }
            return user;
        }
        public User ValidateUser(string username, string password)
        {
            User user = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ValidateUser", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); // In a real application, hash this

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = (int)reader["Id"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DateOfBirth = (DateTime)reader["DateOfBirth"],
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Role = reader["Role"].ToString(),
                            Address = reader["Address"].ToString(),
                            State = reader["State"].ToString(),
                            City = reader["City"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString() // Should not expose in real applications
                        };
                    }
                }
            }
            return user;
        }

        public void RemoveUser(string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", userId);
                command.ExecuteNonQuery();
            }
        }
        //public void AssignTaskToUser(int userId, int taskId, DateTime deadline)
        //{
        //    using (SqlConnection con = new SqlConnection(_connectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand("sp_AssignTaskToUser", con))
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@UserId", userId);
        //            cmd.Parameters.AddWithValue("@TaskId", taskId);
        //            cmd.Parameters.AddWithValue("@Deadline", deadline);

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}


        public void AssignTaskToUser(int taskId, int userId, DateTime deadline)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Begin a transaction
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Assign the task to the user
                        var assignTaskCommand = new SqlCommand("UPDATE Tasks SET AssignedUserId = @UserId WHERE TaskId = @TaskId", connection, transaction);
                        assignTaskCommand.Parameters.AddWithValue("@UserId", userId);
                        assignTaskCommand.Parameters.AddWithValue("@TaskId", taskId);
                        assignTaskCommand.Parameters.AddWithValue("@Deadline", deadline);
                        int rowsAffected = assignTaskCommand.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception("No rows were updated in the Tasks table. Check if the TaskId is correct.");
                        }

                        // 2. Remove the task from the unassigned tasks
                        var deleteUnassignedTaskCommand = new SqlCommand("DELETE FROM UserTasks WHERE TaskId = @TaskId", connection, transaction);
                        deleteUnassignedTaskCommand.Parameters.AddWithValue("@TaskId", taskId);
                        int unassignedRowsAffected = deleteUnassignedTaskCommand.ExecuteNonQuery();

                        // Optional: Check if any rows were deleted
                        if (unassignedRowsAffected == 0)
                        {
                            // Log this if necessary
                        }

                        // 3. Remove the user from new users list
                        var deleteUserCommand = new SqlCommand("DELETE FROM Users WHERE Id = @UserId AND IsNewEmployee = 1", connection, transaction);
                        deleteUserCommand.Parameters.AddWithValue("@UserId", userId);
                        int userRowsAffected = deleteUserCommand.ExecuteNonQuery();

                        // Optional: Check if any rows were deleted
                        if (userRowsAffected == 0)
                        {
                            // Log this if necessary
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if any error occurs
                        transaction.Rollback();
                        System.IO.File.AppendAllText("errorlog.txt", ex.ToString()); // Log to a file
                        throw; // Rethrow the exception to handle it in the controller
                    }
                }
            }
        }


    }
}
