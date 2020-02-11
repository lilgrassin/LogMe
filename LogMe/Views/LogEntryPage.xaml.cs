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

        public LogEntryPage(LogEntry entry = null)
        {
            BindingContext = new LogEntryViewModel(Navigation, entry);
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is LogEntryViewModel logEntryVM)
            {
                await logEntryVM.InitTags();
            }
        }

        void OnSeverityChanged(object sender, ValueChangedEventArgs e)
        {
            severitySlider.Value = Math.Round(e.NewValue);
        }
    }
}
