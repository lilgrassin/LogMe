using System;
using System.Collections.Generic;
using System.Diagnostics;
using LogMe.Models;
using LogMe.ViewModels;

using Xamarin.Forms;

namespace LogMe.Views
{
    public partial class LogEntryPage : ContentPage
    {
        public LogEntryTriggersPage TriggersPage { get; set; }

        public LogEntryPage(LogEntry entry = null)
        {
            BindingContext = new LogEntryViewModel(Navigation, entry);
            TriggersPage = new LogEntryTriggersPage(BindingContext as LogEntryViewModel);

            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is LogEntryViewModel logEntryViewModel)
            {
                await logEntryViewModel.InitTags();
            }
        }

        void OnSeverityChanged(object sender, ValueChangedEventArgs e)
        {
            severitySlider.Value = Math.Round(e.NewValue);
        }
    }
}
