﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LogMe.Services;
using LogMe.Views;
using LogMe.Data;
using System.IO;

namespace LogMe
{
    public partial class App : Application
    {
        static LogEntryDatabase database;

        public static LogEntryDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new LogEntryDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
