using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Projekt_PK4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DatabasePage : Page
    {
        private Database database;

        private DatabaseSearch databaseSearch;

        private bool TestingMode = false;

        public DatabasePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            database = e.Parameter as Database;

            databaseSearch = new DatabaseSearch(database.medBase);
        }




        private async void AppBarButtonReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog("Czy chcesz zapisać bazę przed powrotem do menu głównego?", "Uwaga");
            messageDialog.Commands.Add(new UICommand("Tak", command => { database.Save(); Frame.Navigate(typeof(MainPage)); }));
            messageDialog.Commands.Add(new UICommand("Nie", command => { Frame.Navigate(typeof(MainPage)); }));
            messageDialog.Commands.Add(new UICommand("Anuluj"));

            await messageDialog.ShowAsync();
        }

        private void AppBarButtonSaveBase_Click(object sender, RoutedEventArgs e)
        {
            //database.Save();
            //database.SaveLog();

            database.Save();
        }


        private void AppBarButtonAddMed_Click(object sender, RoutedEventArgs e)
        {
            NavigationParameters navigationParameters = new NavigationParameters(database, -1);
            Frame.Navigate(typeof(CreateMedicinePage), navigationParameters);
        }

        private void AppBarButtonSearchMed_Click(object sender, RoutedEventArgs e)
        {
            if (GridSearchCriteria.Visibility == Visibility.Visible) 
            {
                GridSearchCriteria.Visibility = Visibility.Collapsed;
                databaseSearch.ClearCriteria(database.medBase);
            }
            else
            {
                GridSearchCriteria.Visibility = Visibility.Visible;
            }
        }

        private async void AppBarButtonDeleteMed_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewDatabase.SelectedItem is Medicine med)
            {
                MessageDialog messageDialog = new MessageDialog("Czy na pewno chcesz usunąc wybrany lek?", med.Name);
                messageDialog.Commands.Add(new UICommand("Usuń", command => { database.DeleteMedicine(med); databaseSearch.displayedMedBase.RemoveAt(ListViewDatabase.SelectedIndex); }));
                messageDialog.Commands.Add(new UICommand("Anuluj"));

                await messageDialog.ShowAsync();
            }
        }

        private void AppBarButtonDisplayMed_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewDatabase.SelectedItem is Medicine med)
            {
                int index = database.medBase.IndexOf(med);

                NavigationParameters navigationParameters = new NavigationParameters(database, index);
                Frame.Navigate(typeof(MedicinePage), navigationParameters);
            }
        }

        private void AppBarButtonEditMed_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewDatabase.SelectedItem is Medicine med)
            {
                int index = database.medBase.IndexOf(med);

                NavigationParameters navigationParameters = new NavigationParameters(database, index);
                Frame.Navigate(typeof(CreateMedicinePage), navigationParameters);
            }
        }

        private void AppBarButtonDisplayLog_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DatabaseLogPage), database);
        }



        private void ControlBoxSearchMed_TextChanged(object sender, TextChangedEventArgs e)
        {
            databaseSearch.Search(database.medBase);
        }

        private void CheckBoxSearchMedReimbursed_Click(object sender, RoutedEventArgs e)
        {
            databaseSearch.Search(database.medBase);
        }

        private void ButtonSearchMedClear_Click(object sender, RoutedEventArgs e)
        {
            databaseSearch.ClearCriteria(database.medBase);
        }





















        private void AppBarButtonTESTING_R_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++) 
            {
                RandomDatabase.GenerateRandomReplacementsInDatabase(database);
            }
        }

        private void AppBarButtonTESTING_M_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++) 
            {
                RandomDatabase.AddRandomMedicineToDatabase(database);
            }

            databaseSearch.Search(database.medBase);
        }

    }
}
