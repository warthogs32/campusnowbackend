using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace backend.Exceptions
{
    public class RepoException : Exception
    {
        private string _message;
        public override string Message
        {
            get => _message;
        }
        public RepoException(string message) : base()
        {
            _message = message;
        }
        
    }
}