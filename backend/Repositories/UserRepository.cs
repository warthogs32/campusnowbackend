using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using backend.Models;

namespace backend.Repositories
{
    public class UserRepository
    {
        public bool PostNewUser(UserRecord newUser)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
                {
                    conn.Open();
                    string getEventQuery = @"insert into cn.Users (UserName, Password, FirstName, LastName, JoinDate) values
                        (@UserName, @Password, @FirstName, @LastName, @JoinDate, @IsAdmin);";
                    using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", newUser.UserName);
                        cmd.Parameters.AddWithValue("@Password", newUser.Password);
                        cmd.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", newUser.LastName);
                        cmd.Parameters.AddWithValue("@JoinDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IsAdmin", Convert.ToInt32(false));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }
    }
}