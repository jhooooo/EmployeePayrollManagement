using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Sprout.Exam.Common.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException() : base()
        {
        }
        public DuplicateException(string message) : base(message)
        {
        }
        public DuplicateException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
