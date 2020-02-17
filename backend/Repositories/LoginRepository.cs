using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace backend.Repositories
{
    public class LoginRepository
    {
        public bool IsUserLoginValid(string userName, string password)
        {
            bool exists;
            using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
            {
                conn.Open();
                string getEventQuery = @"select UserId from cn.Users where 
                    EXISTS (select * from cn.Users 
	                where UserName = @UserName and Password = @Password);";
                using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Password", password);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            exists = true;
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