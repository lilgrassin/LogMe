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
    public class LogEntryViewModel
    {
        public static INavigation Navigation { get; set; }
        public LogEntry Entry { get; set; }

        public LogEntryTagsViewModel<FlareTrigger> FlareTriggerViewModel { get; set; }
        public LogEntryTagsViewModel<AffectedArea> AffectedAreaViewModel { get; set; }

        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan EndTime { get; set; }

        //public Task<ObservableCollection<FlareTrigger>> FlareTriggersTask { get; private set; }
        //public Task<ObservableCollection<AffectedArea>> AffectedAreasTask { get; private set; }
        //public ObservableCollection<FlareTrigger> FlareTriggers { get; set; }
        //public ObservableCollection<AffectedArea> AffectedAreas { get; set; }

        //public List<object> SelectedFlareTriggers { get; set; }
        //public List<object> SelectedAffectedAreas { get; set; }


        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        //public ICommand AffectedAreasCommand => new Command(() => { });
        public ICommand GetTagsCommand { get; private set; }



        //public event PropertyChangedEventHandler PropertyChanged;

        public LogEntryViewModel(INavigation nav, LogEntry entry = null)
        {
            Navigation = nav;
            Entry = entry ?? new LogEntry();
            FlareTriggerViewModel = new LogEntryTagsViewModel<FlareTrigger>(Entry.FlareTriggers, Navigation);
            AffectedAreaViewModel = new LogEntryTagsViewModel<AffectedArea>(Entry.AffectedAreas, Navigation);
            StartDate = Entry.StartDate.Date;
            StartTime = Entry.StartDate.TimeOfDay;
            EndDate = Entry.EndDate.Date;
            EndTime = Entry.EndDate.TimeOfDay;

            SaveCommand = new Command(async () => { await SaveEntry(); });
            DeleteCommand = new Command(async () => { await DeleteEntry(); });
            GetTagsCommand = new Command<string>(
            async (tagType) =>
            {
                switch (tagType)
                {
                    case "Trigger":
                        await Navigation.PushModalAsync(new NavigationPage(new LogEntryTagsPage(FlareTriggerViewModel, tagType)));
                        break;
                    case "Affected Area":
                        await Navigation.PushModalAsync(new NavigationPage(new LogEntryTagsPage(AffectedAreaViewModel, tagType)));
                        break;
                    default:
                        return;
                }
                //LogEntryTagsViewModel<ITagEntry> page = GetType().GetProperty(pageName).GetValue(this);
                //await Navigation.PushModalAsync(new NavigationPage(page));
            });
        }

        public async Task InitTags()
        {
            await FlareTriggerViewModel.InitTags();
            await AffectedAreaViewModel.InitTags();
        }

        public async Task SaveEntry()
        {
            Entry.StartDate = StartDate.Add(StartTime);
            Entry.EndDate = EndDate.Add(EndTime);
            Entry.FlareTriggers = FlareTriggerViewModel.GetSelectedTags().ToList();
            Entry.AffectedAreas = AffectedAreaViewModel.GetSelectedTags().ToList();
            if (!await App.Database.SaveEntryAsync(Entry))
            {
                Debug.Print("Failed to add to database");
            }
            await Navigation.PopAsync();
        }

        public async Task DeleteEntry()
        {
            await App.Database.DeleteEntryAsync(Entry);
            await Navigation.PopAsync();

        }

        //public void UpdateSelectedTriggers()
        //{

        //}

        //public async Task AddTrigger(string name)
        //{
        //    if (name != "")
        //    {
        //        var trigger = new FlareTrigger
        //        {
        //            Name = name
        //        };
        //        Debug.Print(name);
        //        FlareTriggers.Add(trigger);
        //        await App.Database.SaveEntryAsync(trigger);
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlareTriggers"));
        //    }
        //}
    }
}
