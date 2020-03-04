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
        private string _sqlConnectionString;
        public UserRepository(bool isTest = false)
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
        public bool PostNewUser(UserRecord newUser)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    string getEventQuery = @"insert into cn.Users (UserName, Password, FirstName, LastName, JoinDate, IsAdmin) values
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

        /*
         * You probably dont want to call this method on the actual db.
         * Therefore, it will only work with the test db
         */
        public bool ResetUsers()
        { 
            if (_sqlConnectionString == Properties.Resources.testsqlconnection)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        string getEventQuery = @"delete from cn.Users;";
                        using (SqlCommand cmd = new SqlCommand(getEventQuery, conn))
                        {
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
            else
            {
                Console.WriteLine("You cannot delete all users from the production database");
                return false;
            }
        }

        public bool ResetAutoIncrement()
        {
            if (_sqlConnectionString == Properties.Resources.testsqlconnection)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_sqlConnectionString))
                    {
                        conn.Open();
                        string resetIdentityQuery = @"DBCC CHECKIDENT('cn.Users', RESEED, 0)";
                        using (SqlCommand cmd = new SqlCommand(resetIdentityQuery, conn))
                        {
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
            else
            {
                Console.WriteLine("You cannot reset an identity in the production database");
                return false;
            }
        }
    }
}