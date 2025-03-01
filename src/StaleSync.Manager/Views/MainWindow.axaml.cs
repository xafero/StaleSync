using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StaleSync.Manager.Core;
using StaleSync.Manager.ViewModels;
using Cfg = StaleSync.Proto.ConfigFile<StaleSync.Manager.Core.Config>;

namespace StaleSync.Manager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mvm)
            {
                Log.SetAction(txt =>
                {
                    var oldTxt = mvm.Greeting;
                    mvm.Greeting = oldTxt + Environment.NewLine + txt;
                });

                Task.Run(() =>
                {
                    Cfg.Load();
                    var cfg = Cfg.Config;
                    Server.Start(cfg.ServerPort);
                });
            }
        }
    }
}