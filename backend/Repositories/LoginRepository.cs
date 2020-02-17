using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using backend.Models;

namespace backend.Repositories
{
    public class LoginRepository
    {
        public static UserRecord CurrentUser = new UserRecord();
        public bool IsUserLoginValid(string userName, string password)
        {
            bool exists;
            using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
            {
                conn.Open();
                string getEventQuery = @"select * from cn.Users
	                where UserName = @UserName and Password = @Password;";
                using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Password", password);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            exists = true;
                            if (reader.Read())
                            {
                                CurrentUser = new UserRecord()
                                {
                                    UserId = Int32.Parse(reader["UserId"].ToString()),
                                    UserName = reader["UserName"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    JoinDate = DateTime.Parse(reader["JoinDate"].ToString())
                                };
                            }
                        }
                        else
                        {
                            exists = false;
                        }
                    }
                }
            }
            return exists;
        }
    }
}