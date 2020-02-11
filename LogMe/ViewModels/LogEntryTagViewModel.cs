using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LogMe.Models;
using Xamarin.Forms;

namespace LogMe.ViewModels
{
    public interface ILogEntryTagsViewModel<out T> : INotifyPropertyChanged where T : ITagEntry
    {

        static INavigation Navigation { get; set; }

        //public ICommand SelectTagsCommand { get; private set; }
        ICommand DoneCommand { get; }

        ObservableCollection<object> ETags { get; }
        List<object> SelectedTags { get; set; }

        Task InitTags();
        Task AddTag(string name);
        IEnumerable<T> GetSelectedTags();
    }


    public class LogEntryTagsViewModel<T> : ILogEntryTagsViewModel<T> where T : class, ITagEntry, new()
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static INavigation Navigation { get; set; }

        //public ICommand SelectTagsCommand { get; private set; }
        public ICommand DoneCommand { get; private set; }

        public Task<ObservableCollection<T>> GetTagsTask { get; private set; }
        public ObservableCollection<object> ETags { get; set; }
        public ObservableCollection<T> Tags { get; set; }
        public List<T> EntryTags { get; set; }
        public List<object> SelectedTags { get; set; }

        public LogEntryTagsViewModel(List<T> entryTags, INavigation nav)
        {
            GetTagsTask = Task.Run(async () => new ObservableCollection<T>(await App.Database.GetEntryAsync<T>()));
            Navigation = nav;
            EntryTags = entryTags;
            DoneCommand = new Command(async () => { await Navigation.PopModalAsync(); });
        }

        public async Task InitTags()
        {
            Tags ??= await GetTagsTask;
            ETags ??= new ObservableCollection<object>(Tags.Cast<object>());
            SelectedTags ??= Tags.Where((tag) => EntryTags.Select((selTag) => selTag.ID).Contains(tag.ID)).Cast<object>().ToList();
        }

        public async Task AddTag(string name)
        {
            if (name != null && !name.Equals(""))
            {
                var tag = new T
                {
                    Name = name
                };
                if (await App.Database.SaveEntryAsync(tag))
                {
                    Tags.Add(tag);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tags"));
                }
            }
        }

        public IEnumerable<T> GetSelectedTags()
        {
            return SelectedTags.Cast<T>();
        }
    }
}
