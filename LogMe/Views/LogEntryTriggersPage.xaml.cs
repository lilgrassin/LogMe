using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using LogMe.Models;
using Xamarin.Forms;
using LogMe.ViewModels;
using System.Diagnostics;

namespace LogMe.Views
{
    public partial class LogEntryTriggersPage : ContentPage
    {
        public LogEntryTriggersPage()
        {
            InitializeComponent();

            //        BindingContext = entryView;
            //        InitializeComponent();
            //    }

            //    //protected override async void OnAppearing()
            //    //{
            //    //    base.OnAppearing();
            //    //    if (BindingContext is LogEntryTagsViewModel vm)
            //    //    {
            //    //        await vm.InitTags()
            //    //        //List<FlareTrigger> trig = triggersList.SelectedItems.Where(i => (BindingContext as LogEntryViewModel).FlareTriggers.Contains(i)).Cast<FlareTrigger>().ToList();
            //    //    }
            //    //}

            //    //async void OnTriggerAddedClicked(object sender, EventArgs e)
            //    //{
            //    //    if (BindingContext is LogEntryViewModel logEntryViewModel)
            //    //    {
            //    //        await logEntryViewModel.AddTrigger(await DisplayPromptAsync("New Trigger", ""));
            //    }
        }
    }
}
