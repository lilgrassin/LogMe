using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using LogMe.ViewModels;
using LogMe.Models;

namespace LogMe.Views
{
    public partial class LogsPage : ContentPage
    {
        public LogsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await App.Database.GetEntryAsync<LogEntry>(true);
        }

        async void OnLogAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LogEntryPage());
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new LogEntryPage(e.SelectedItem as LogEntry));
            }
        }
    }
}
