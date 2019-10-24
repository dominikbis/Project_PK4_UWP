using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Projekt_PK4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Database database;

        public MainPage()
        {
            this.InitializeComponent();
        }



        private void ButtonMenuNewBase_Click(object sender, RoutedEventArgs e)
        {
            PivotMenuOption.SelectedItem = PivotItemNewBase;
        }

        private void ButtonMenuLoadBase_Click(object sender, RoutedEventArgs e)
        {
            PivotMenuOption.SelectedItem = PivotItemLoadBase;
        }


        private async void AppBarButtonCreateBase_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNewBaseName.Text != "")//
            {
                database = new Database(TextBoxNewBaseName.Text, TextBoxNewBaseAccessPath.Text);

                Frame.Navigate(typeof(DatabasePage), database);
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Nazwa bazy nie może pozostać pusta!", "Uwaga!");
                await messageDialog.ShowAsync();
            }
        }

        private async void AppBarButtonLoadBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                database = new Database(TextBoxLoadBaseAccessPath.Text);
            }
            catch (System.AggregateException aex)
            {
                MessageDialog messageDialog;
                foreach (var ex in aex.Flatten().InnerExceptions)
                {
                    if (ex is FileNotFoundException)
                    {
                        messageDialog = new MessageDialog("Nie można odnaleźć określonego pliku!", "Wczytywanie nie powiodło się");
                        await messageDialog.ShowAsync();
                        return;
                    }
                }
                messageDialog = new MessageDialog("Nie można prawidłowo odczytać danych z pliku!", "Wczytywanie nie powiodło się");
                await messageDialog.ShowAsync();
                return;
            }

            Frame.Navigate(typeof(DatabasePage), database);
        }

        private void ButtonMenuExit_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }
    }
}
