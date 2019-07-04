using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using _09052019.Any;
using _09052019.Model;
using System.Windows.Data;
using System.Windows;
using System.Windows.Threading;

namespace _09052019.ViewModel
{
    public class MainViewModel : OnPropertyChangedClass
    {
        readonly MODEL_OTRS model;
        DispatcherTimer timer = new DispatcherTimer();

        public MainViewModel()
        {   
            model = new MODEL_OTRS();
            model.PropertyChanged += Model_PropertyChanged;
            Start();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 8);
            timer.Tick += new EventHandler(this.InvalidateSampleData);
            timer.Start();
        }

        async void Start()
        {
            string d = DateTime.Now.Date.ToString("dd");
            string m = DateTime.Now.Date.ToString("MM");
            string y = DateTime.Now.Date.ToString("yyyy");
            string дата = y + "-" + m + "-" + d;
            await Task.WhenAll(model.Load2(дата));
            await model.Load();
            Сортировка_при_старте();
        }

        private void InvalidateSampleData(object state, EventArgs e)
        {
            Start();
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "ИзмененаСтатистика")
            {
                ДанныеИзОТРС = model.GetДанные_из_отрс();
                очередь_олл = model.очередь_олл;
            }

            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "ИзмененыДанныеИзТелефонии")
            {
                поступило_за_день = model.количество_звонков_поступивших;
                принято_за_день = model.количество_звонков_принятых;
            }
        }

        private int _поступило_за_день;
        public int поступило_за_день
        {
            get { return _поступило_за_день; }
            private set { _поступило_за_день = value; OnPropertyChanged(); }
        }


        private int _принято_за_день;
        public int принято_за_день
        {
            get { return _принято_за_день; }
            private set { _принято_за_день = value; OnPropertyChanged(); }
        }

        private int _очередь_олл;
        public int очередь_олл
        {
            get { return _очередь_олл; }
            set { _очередь_олл = value; OnPropertyChanged(); }
        }



        public ObservableCollection<MODEL_OTRS> ДанныеИзОТРС 
        { 
            get { return _данные_из_отрс; } 
            private set { _данные_из_отрс = value; OnPropertyChanged(); ViewДанныеИзОТРС.Source = ДанныеИзОТРС; } 
        }
        public CollectionViewSource ViewДанныеИзОТРС { get { return _viewДанныеИзОТРС; } private set { _viewДанныеИзОТРС = value; OnPropertyChanged(); } }
        private ObservableCollection<MODEL_OTRS> _данные_из_отрс;
        private CollectionViewSource _viewДанныеИзОТРС = new CollectionViewSource() { };

       


        private ListSortDirection _sortDirection; 
        private RelayCommand _sortCommand;
        public RelayCommand Сортировка { get { return _sortCommand ?? (_sortCommand = new RelayCommand(Метод_сортировки_при_клике_на_столбец)); } }

        private void Сортировка_при_старте()
        {
            _sortDirection = ListSortDirection.Descending;
            ViewДанныеИзОТРС.SortDescriptions.Clear();
            ViewДанныеИзОТРС.SortDescriptions.Add(new SortDescription("Количество_баллов", _sortDirection));
        }
        private void Метод_сортировки_при_клике_на_столбец(object parameter)
        {
            _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                          ListSortDirection.Descending :
                                          ListSortDirection.Ascending; 
            ViewДанныеИзОТРС.SortDescriptions.Clear();
            ViewДанныеИзОТРС.SortDescriptions.Add(new SortDescription(parameter.ToString(), _sortDirection));
        }

    }
}
