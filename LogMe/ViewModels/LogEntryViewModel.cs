using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LogMe.Models;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;
using LogMe.Views;

namespace LogMe.ViewModels
{
    public class LogEntryViewModel : INotifyPropertyChanged
    {
        public static INavigation Navigation { get; set; }
        public LogEntry Entry { get; set; }

        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan EndTime { get; set; }

        public Task<ObservableCollection<FlareTrigger>> FlareTriggersTask { get; private set; }
        public Task<ObservableCollection<AffectedArea>> AffectedAreasTask { get; private set; }
        public ObservableCollection<FlareTrigger> FlareTriggers { get; set; }
        public ObservableCollection<AffectedArea> AffectedAreas { get; set; }
        public List<object> SelectedFlareTriggers { get; set; }
        public List<object> SelectedAffectedAreas { get; set; } = new List<object>();
        //public List<FlareTrigger> SelectedFlareTriggers { get; set; }
        //public List<AffectedArea> SelectedAffectedAreas { get; set; }

        public ICommand SelectTriggersCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        //public ICommand AffectedAreasCommand => new Command(() => { });
        public ICommand FlareTriggersCommand { get; private set; }



        public event PropertyChangedEventHandler PropertyChanged;

        public LogEntryViewModel(INavigation nav, LogEntry entry = null)
        {
            Navigation = nav;

            SelectTriggersCommand = new Command(() => { UpdateSelectedTriggers(); });
            SaveCommand = new Command(async () => { await SaveEntry(); });
            DeleteCommand = new Command(async () => { await DeleteEntry(); });
            FlareTriggersCommand = new Command<Type>(
            async (pageType) =>
            {
                Page page = (Page)Activator.CreateInstance(pageType, this);
                await Navigation.PushAsync(page);
            });


            Entry = entry ?? new LogEntry();
            StartDate = Entry.StartDate.Date;
            StartTime = Entry.StartDate.TimeOfDay;
            EndDate = Entry.EndDate.Date;
            EndTime = Entry.EndDate.TimeOfDay;
            SelectedAffectedAreas.AddRange(Entry.AffectedAreas);
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedAffectedAreas"));
            //SelectedFlareTriggers.AddRange(Entry.FlareTriggers);
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedFlareTriggers"));
            //SelectedAffectedAreas = Entry.AffectedAreas;
            //SelectedFlareTriggers = Entry.FlareTriggers;

            FlareTriggersTask = Task.Run(async () => new ObservableCollection<FlareTrigger>(await App.Database.GetEntryAsync<FlareTrigger>()));
            AffectedAreasTask = Task.Run(async () => new ObservableCollection<AffectedArea>(await App.Database.GetEntryAsync<AffectedArea>()));

        }

        public async Task InitTags()
        {
            FlareTriggers ??= await FlareTriggersTask;
            SelectedFlareTriggers ??= FlareTriggers.Where((x) => Entry.FlareTriggers.Select((y) => y.ID).Contains(x.ID)).Cast<object>().ToList();
        }

        public async Task SaveEntry()
        {
            Entry.StartDate = StartDate.Add(StartTime);
            Entry.EndDate = EndDate.Add(EndTime);
            Entry.FlareTriggers = SelectedFlareTriggers.Cast<FlareTrigger>().ToList();
            await App.Database.SaveEntryAsync(Entry);
            await Navigation.PopAsync();
        }

        public async Task DeleteEntry()
        {
            await App.Database.DeleteEntryAsync(Entry);
            await Navigation.PopAsync();

        }

        public void UpdateSelectedTriggers()
        {

        }

        public async Task AddTrigger(string name)
        {
            if (name != "")
            {
                var trigger = new FlareTrigger
                {
                    Name = name
                };
                Debug.Print(name);
                FlareTriggers.Add(trigger);
                await App.Database.SaveEntryAsync(trigger);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlareTriggers"));
            }
        }
    }
}
