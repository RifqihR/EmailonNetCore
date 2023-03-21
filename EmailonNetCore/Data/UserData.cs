using EmailonNetCore.Model;
using Microsoft.Data.SqlClient;

namespace EmailonNetCore.Data
{
    public class UserData
    {
        private readonly string ConnectionString = "Data Source=DESKTOP-Q9ETLOU; Database= TugasDotnet; Integrated Security=True";
        public bool InputData(Users users) 
        {
            bool result = false;
            string query = "IINSERT INTO UserData (Name, Email, Task) VALUES (@name, @email, @task)";

            using (SqlConnection connection = new(ConnectionString)) 
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try 
                {
                    using SqlCommand command = new();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = query;
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@name", users.Name);
                    command.Parameters.AddWithValue("@email", users.Email);
                    command.Parameters.AddWithValue("@task", users.Task);

                    result = command.ExecuteNonQuery() > 0;
                    transaction.Commit();
                } 
                catch 
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
    }
}
