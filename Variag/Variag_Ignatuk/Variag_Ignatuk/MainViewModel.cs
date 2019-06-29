using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Variag_Ignatuk
{
    public class MainViewModel : OnPropertyChangedClass
    {
        ApplicationContext db;

        public MainViewModel()
        {
            db = new ApplicationContext();
            db.Users.Load();
            Users = new ObservableCollection<User>(db.Users.Local.ToBindingList());
            StartSorting();
        }

        public ObservableCollection<User> Users { get => _users; private set { _users = value; OnPropertyChanged(); ViewUsers.Source = Users; } }
        public CollectionViewSource ViewUsers { get => _viewUsers; private set { _viewUsers = value; OnPropertyChanged(); } }

        private ObservableCollection<User> _users;
        private CollectionViewSource _viewUsers = new CollectionViewSource() { };

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        public RelayCommand OpenForm { get { return _openform ?? (_openform = new RelayCommand(AddUser)); } }
        private RelayCommand _openform;

        void AddUser(object parameter)
        {
            UserAddWindow phoneWindow = new UserAddWindow(new User());
            if (phoneWindow.ShowDialog() == true)
            {
                User phone = phoneWindow.User;
                db.Users.Add(phone);
                db.SaveChanges();
            }
            Users = new ObservableCollection<User>(db.Users.Local.ToBindingList());
        }


        /// <summary>
        /// Фильтрация
        /// </summary>
        private string _filtrSearch;
        public string FiltrSearch
        {
            get { return _filtrSearch; }
            set
            {
                _filtrSearch = value;
                OnPropertyChanged("FiltrSearch");
                AddFiltering();
            }
        }

        private void ShowOnlyBargainsFilter(object sender, FilterEventArgs e)
        {
            User user = e.Item as User;
            if (user != null)
            {
                if (user.lastname.Contains(FiltrSearch))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        private void AddFiltering()
        {
            ViewUsers.Filter += new FilterEventHandler(ShowOnlyBargainsFilter);
        }


        /// <summary>
        /// Сортировка при клике на столбец
        /// </summary>
        private ListSortDirection _sortDirection;
        private RelayCommand _sortCommand;
        public RelayCommand SortCommand { get { return _sortCommand ?? (_sortCommand = new RelayCommand(SortColumn)); } }

        private void StartSorting()
        {
            _sortDirection = ListSortDirection.Descending;
            ViewUsers.SortDescriptions.Clear();
            ViewUsers.SortDescriptions.Add(new SortDescription("age", _sortDirection));
        }

        private void SortColumn(object parameter)
        {
            _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                          ListSortDirection.Descending :
                                          ListSortDirection.Ascending;
            ViewUsers.SortDescriptions.Clear();
            ViewUsers.SortDescriptions.Add(new SortDescription(parameter.ToString(), _sortDirection));
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// 
        private User _selectedItem;
        public User SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand DeleteUserCommand { get { return _deleteuser ?? (_deleteuser = new RelayCommand(DeleteUser)); } }
        private RelayCommand _deleteuser;

        void DeleteUser(object parameter)
        {
            if (SelectedItem == null) return;
            User user = SelectedItem as User;
            db.Users.Remove(user);
            db.SaveChanges();
            Users = new ObservableCollection<User>(db.Users.Local.ToBindingList());
        }


    }

        
}
