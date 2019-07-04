using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _09052019.Any
{
    public abstract class OnPropertyChangedClass : INotifyPropertyChanged
    {
        /// <summary>Событие для извещения об изменения свойства</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>Метод для вызова события извещения об изменении свойства</summary>
        /// <param name="propertyName">Изменившееся свойство</param>
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
        }
    }
}
