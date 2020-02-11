using System;
using System.Collections.Generic;
using LogMe.Models;
using LogMe.ViewModels;
using Xamarin.Forms;

namespace LogMe.Views
{
    public partial class LogEntryTagsPage : ContentPage
    {
        private string TagType { get; set; }

        public LogEntryTagsPage(LogEntryTagsViewModel<FlareTrigger> tagsView, string tagType) 
        {
            BindingContext = tagsView;
            TagType = tagType;
            Title = "Select " + tagType + "s";
            InitializeComponent();
        }

        public LogEntryTagsPage(LogEntryTagsViewModel<AffectedArea> tagsView, string tagType) 
        {
            BindingContext = tagsView;
            TagType = tagType;
            Title = "Select " + tagType + "s";
            InitializeComponent();
        }

        async void OnTagAddedClicked(object sender, EventArgs e)
        {
            if (BindingContext is ILogEntryTagsViewModel<ITagEntry> logEntryTriggerVM)
            {
                await logEntryTriggerVM.AddTag(await DisplayPromptAsync("New "+ TagType, ""));
            }
        }
    }
}
