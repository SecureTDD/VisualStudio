using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Secure_TDD.Model.Entities
{
    class LoadException : Exception
    {
        public LoadException() { }

        public LoadException(string message) : base(message) 
        {
            MessageBox.Show(message);
        }

        public LoadException(string message, Exception inner) : base(message, inner) 
        {
            MessageBox.Show(message);
        }
    }
}
