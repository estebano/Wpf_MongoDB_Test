using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MongoDB.Bson;
using MongoDB.Driver;
using Wpf_MongoDB_Test.Model;

namespace Wpf_MongoDB_Test.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _db;
        protected static IMongoCollection<DefaultDocument> _collection;

        public ICommand AddNewCommand { get; set; }

        private string _newName;
        private string _newSurname;

        public string NewName
        {
            get
            {
                return _newName;
            }
            set
            {
                _newName = value;
                RaisePropertyChanged(() => NewName); }
        }

        public string NewSurname
        {
            get
            {
                return _newSurname;
            }
            set
            {
                _newSurname = value;
                RaisePropertyChanged(() => NewSurname);
            }
        }

        public ObservableCollection<DefaultDocument> Docs { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            InitCommands();

            _client = new MongoClient();
            _db = _client.GetDatabase("est");
            _collection = _db.GetCollection<DefaultDocument>("default");
            InitCollections();
            FetchData();

        }

        private void InitCommands()
        {
            AddNewCommand = new RelayCommand(TryAddNew);
        }

        private void TryAddNew()
        {
            try
            {
                AddNew();

            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void LogException(Exception ex)
        {
            //// TODO: Implement exceptions logging
        }

        private async void AddNew()
        {
            await _collection.InsertOneAsync(new DefaultDocument() {  Name = NewName, Surname = NewSurname});
            ClearInputs();
            FetchData();
        }

        private void ClearInputs()
        {
            NewName = string.Empty;
            NewSurname = string.Empty;
        }

        private void InitCollections()
        {
            Docs = new ObservableCollection<DefaultDocument>();
        }

        private async void FetchData()
        {
            Docs.Clear();

            var filter = Builders<DefaultDocument>.Filter.Empty;
            using (var cursor = await _collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var item in batch)
                    {
                        Docs.Add(item);
                    }
                }
            }
        }
    }
}