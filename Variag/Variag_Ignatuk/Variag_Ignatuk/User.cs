using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variag_Ignatuk
{
    public class User : OnPropertyChangedClass
    {
        private string _firstname;
        private string _lastname;
        private int _age;
        public int id { get; set; }

        public string firstname
        {
            get { return _firstname; }
            set
            {
                _firstname = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string lastname
        {
            get { return _lastname; }
            set
            {
                _lastname = value;
                OnPropertyChanged("LastName");
            }
        }

        public int age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }
    }
}
