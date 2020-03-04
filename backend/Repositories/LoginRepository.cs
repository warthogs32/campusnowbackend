using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Diagnostics.CodeAnalysis;
using backend.Models;

namespace backend.Repositories
{
    public class LoginRepository
    {
        private static string _sqlConnectionString;
        public LoginRepository(bool isTest = false)
        {
            if (isTest)
            {
                _sqlConnectionString = Properties.Resources.testsqlconnection;
            }
            else
            {
                _sqlConnectionString = Properties.Resources.sqlconnection;
            }
        }

        public static UserRecord CurrentUser = new UserRecord();
        public bool IsUserLoginValid(string userName, string password)
        {
            bool exists;
            using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
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

        [ExcludeFromCodeCoverage]
        public static bool Logout(string token)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string writeTokenQuery = @"insert into cn.logout_tokens (token) values (@token);";
                    using (SqlCommand cmd = new SqlCommand(writeTokenQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@token", token);
                        cmd.ExecuteNonQuery();
                    }
                }
                CurrentUser = null;
                return true;
            }
            catch (SqlException e)
            {
                return false;
            }
        }

        public static bool IsTokenBlacklisted(string token)
        {
            try 
            {
                bool exists = false;
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string ValidateTokenQuery = @"select * from cn.logout_tokens
	                where token = @token;";
                    using(SqlCommand cmd = new SqlCommand(ValidateTokenQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@token", token.Substring(7));
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.HasRows)
                            {
                                exists = true;
                            }
                        }
                    }
                }
                return exists;
            }
            catch(SqlException e)
            {
                return false;
            }
        }
    }
}