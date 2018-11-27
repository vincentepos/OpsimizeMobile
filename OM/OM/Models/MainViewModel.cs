using System;
using System.ComponentModel;

namespace OM.Models
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _conn;

        public string Conn 
        {
            get => _conn;
            set 
            {
                _conn = value;
                OnPropertyChanged();
            }
        }
    }
}
