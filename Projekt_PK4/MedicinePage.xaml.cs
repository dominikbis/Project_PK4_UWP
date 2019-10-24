using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Projekt_PK4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MedicinePage : Page
    {
        private Database database;
        private Medicine medicine;
        private int index;

        public MedicinePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationParameters navigationParameters = e.Parameter as NavigationParameters;
            database = navigationParameters.database;
            index = navigationParameters.index;

            medicine = database[index];
        }




        private void AppBarButtonReturnToDatabase_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DatabasePage), database);
        }

        private async void AppBarButtonDeleteMed_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog("Czy na pewno chcesz usunąc wybrany lek?", medicine.Name);
            messageDialog.Commands.Add(new UICommand("Usuń", command => { database.DeleteMedicine(index); Frame.Navigate(typeof(DatabasePage), database); }));
            messageDialog.Commands.Add(new UICommand("Anuluj"));

            await messageDialog.ShowAsync();
        }

        private void AppBarButtonEditMed_Click(object sender, RoutedEventArgs e)
        {
            NavigationParameters navigationParameters = new NavigationParameters(database, index);
            Frame.Navigate(typeof(CreateMedicinePage), navigationParameters);
        }

        private void ButtonDisplayReplacement_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewReplacements.SelectedItem is Medicine med) 
            {
                index = database.medBase.IndexOf(med);

                NavigationParameters navigationParameters = new NavigationParameters(database, index);
                Frame.Navigate(typeof(MedicinePage), navigationParameters);
            }
        }
    }
}
