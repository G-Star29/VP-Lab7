using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Lab7.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Lab7.Views
{
    public partial class WindowView : UserControl
    {
        public WindowView()
        {
            InitializeComponent();
            this.FindControl<MenuItem>("Save").Click += async delegate
            {
                var taskPath = new SaveFileDialog().ShowAsync((Window)this.Parent);

                string path = await taskPath;
                var context = this.Parent.DataContext as MainWindowViewModel;
                if (path is not null)
                {
                    context.WriteFile(path);
                }
                context.OpenWindowView();

            };
            this.FindControl<MenuItem>("Load").Click += async delegate
            {
                var taskPath = new OpenFileDialog().ShowAsync((Window)this.Parent);
                string[]? path = await taskPath;
                var context = this.Parent.DataContext as MainWindowViewModel;
                if (path is not null)
                {
                    context.ReadFile(string.Join("/", path));
                }
                context.OpenWindowView();
            };

        }
        private void ShowAboutWindow(object sender, RoutedEventArgs e)
        {
            var dialogWindow = new AboutView();
            dialogWindow.ShowDialog((Window)this.Parent);
        }
        private void Close_App(object sender, RoutedEventArgs e)
        {
            var context = this.Parent as MainWindow;
            context.Close();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
