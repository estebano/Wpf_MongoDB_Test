using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
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

            _client = new MongoClient();
            _db = _client.GetDatabase("est");
            InitCollections();
            FetchData();

        }

        private void InitCollections()
        {
            Docs = new ObservableCollection<DefaultDocument>();
        }

        private async void FetchData()
        {
            var collection = _db.GetCollection<DefaultDocument>("default");
            var filter = Builders<DefaultDocument>.Filter.Empty;
            using (var cursor = await collection.FindAsync(filter))
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