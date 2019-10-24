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

using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Projekt_PK4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateMedicinePage : Page
    {
        private Database database;
        private int index;

        private Medicine newMedicine;
        private ReplacementsCreator newReplacements = new ReplacementsCreator();
        private Medicine selectedReplacement;

        private List<string> levelOfFundingList = new List<string>(Enum.GetNames(typeof(LevelOfFunding)));//lista, bo stala liczba elementow
        private int MeidcineRM_LevelStart;


        public CreateMedicinePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);


            var lastPage = Frame.BackStack.LastOrDefault();                     //zapogieganie stackowaniu tej samej strony po odswiezeniu
            if (lastPage.SourcePageType.ToString().Contains("CreateMedicinePage") == true)
            {
                Frame.BackStack.Remove(lastPage);
            }

            NavigationParameters navigationParameters = e.Parameter as NavigationParameters;
            database = navigationParameters.database;
            index = navigationParameters.index;

            if (index >= 0)
            {
                newMedicine = database[index].Copy();
            }
            else
            {
                newMedicine = new Medicine();
            }

            MeidcineRM_LevelStart = (int)newMedicine.RM_Level;
        }


        private void AppBarButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            var lastPage = Frame.BackStack.LastOrDefault();
            if (lastPage.SourcePageType.ToString().Contains("DatabasePage") == true) 
            {
                Frame.Navigate(typeof(DatabasePage), database);
            }
            if(lastPage.SourcePageType.ToString().Contains("MedicinePage") == true)         //szuka czeci, a nie porownuje, moze powodowac bledy
            {
                NavigationParameters navigationParameters = new NavigationParameters(database, index);
                Frame.Navigate(typeof(MedicinePage), navigationParameters);
            }
        }

        private async void AppBarButtonDeleteMed_Click(object sender, RoutedEventArgs e)
        {
            if (index >= 0)
            {
                MessageDialog messageDialog = new MessageDialog("Czy na pewno chcesz usunąc wybrany lek?", database[index].Name);
                messageDialog.Commands.Add(new UICommand("Usuń", command => { database.DeleteMedicine(index); Frame.Navigate(typeof(DatabasePage), database); }));
                messageDialog.Commands.Add(new UICommand("Anuluj"));

                await messageDialog.ShowAsync();
            }
            else
            {
                Frame.Navigate(typeof(DatabasePage), database);
            }
        }

        private async void AppBarButtonSaveMed_Click(object sender, RoutedEventArgs e)//ustawione AllowFocusOnInteraction="True"
        {
            if (newMedicine.Name != "")
            {
                if (index >= 0)
                {
                    database.EditMedicine(newMedicine, index, newReplacements);
                }
                else
                {
                    database.AddMedicine(newMedicine, newReplacements);
                    index = database.medBase.Count() - 1;//getindex() oraz count w database
                }

                NavigationParameters navigationParameters = new NavigationParameters(database, index);
                Frame.Navigate(typeof(MedicinePage), navigationParameters);
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Nazwa leku nie może pozostać pusta!", "Uwaga");
                await messageDialog.ShowAsync();
            }
        }

        private void AppBarButtonRestoreMed_Click(object sender, RoutedEventArgs e)
        {
            NavigationParameters navigationParameters = new NavigationParameters(database, index);
            Frame.Navigate(GetType(), navigationParameters);
        }


        private void ComboBoxMedicineRM_LevelChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            newMedicine.RM_Level = (LevelOfFunding)ComboBoxMedicineRM_LevelChoice.SelectedIndex;
        }






        private void AutoSuggestBoxReplacements_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) 
            {
                AutoSuggestBoxReplacements.ItemsSource = database.medBase.Where(med => med.Name.IndexOf(sender.Text.ToString()) >= 0).ToList();
            }
        }

        private void AutoSuggestBoxReplacements_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var selectedItem = args.SelectedItem as Medicine;
            sender.Text = selectedItem.Name;

            selectedReplacement = selectedItem;
        }

        private void ButtonReplacementAdd_Click(object sender, RoutedEventArgs e)
        {
            if (selectedReplacement != null) 
            {
                if (newMedicine.replacements.IndexOf(selectedReplacement) < 0) 
                {
                    if (index < 0) 
                    {
                        newMedicine.replacements.Add(selectedReplacement);
                        newReplacements.medToAdd.Add(selectedReplacement);
                    }
                    else
                    {
                        if(selectedReplacement != database[index])
                        {
                            newMedicine.replacements.Add(selectedReplacement);
                            newReplacements.medToAdd.Add(selectedReplacement);
                        }
                    }
                }
            }
        }

        private void ButtonReplacementDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewReplacements.SelectedItem is Medicine med) 
            {
                newMedicine.replacements.Remove(med);

                if (newReplacements.medToAdd.IndexOf(med) < 0)
                {
                    newReplacements.medToDelete.Add(med);
                }
                else
                {
                    newReplacements.medToAdd.Remove(med);
                }
            }
        }

    }
}
