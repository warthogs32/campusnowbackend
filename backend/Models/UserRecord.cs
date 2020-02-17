using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace backend.Models
{
    public class UserRecord
    {
		private int _userId;
		public int UserId
		{
			get { return _userId; }
			set { _userId = value; }
		}

		private string _userName;
		public string UserName
		{
			get { return _userName; }
			set { _userName = value; }
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		private string _firstName;
		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		private string _lastName;
		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		private DateTime _joinDate;
		public DateTime JoinDate
		{
			get { return _joinDate; }
			set { _joinDate = value; }
		}






	}
}