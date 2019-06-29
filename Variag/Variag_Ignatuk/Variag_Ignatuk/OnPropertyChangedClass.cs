using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Variag_Ignatuk
{
    /// <summary>Базовый класс с реализацией INPC </summary>
    public abstract class OnPropertyChangedClass : INotifyPropertyChanged
    {
        /// <summary>Событие для извещения об изменения свойства</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>Метод для вызова события извещения об изменении свойства</summary>
        /// <param name="propertyName">Изменившееся свойство</param>
        public void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
