using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FirstList
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Hero> heroes = new ObservableCollection<Hero>();
        private bool editMode;
        private int selectedIndex;

        public MainPage()
        {
            InitializeComponent();
            HeroesView.ItemsSource = heroes;
            heroes.Add(new Hero { DisplayName = "Batman", Description = "I'm the night" });
            heroes.Add(new Hero { DisplayName = "Superman", Description = "God among humans" });
            heroes.Add(new Hero { DisplayName = "Spiderman", Description = "closeby hero" });

            var request = HttpWebRequest.Create(string.Format(@"https://opentdb.com/api.php?amount=10", "198440")); request.ContentType = "application/json"; request.Method = "GET"; using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK) Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode); using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd(); if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.Out.WriteLine("Response Body: \r\n {0}", content); dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(content); Console.Out.WriteLine("response code: " + data.response_code);
                    }
                }
            }
        }

        void addHero(object sender, EventArgs arg)
        {
            if (editMode)
            {
                heroes.RemoveAt(selectedIndex);
                heroes.Insert(selectedIndex, new Hero { DisplayName = entry_hero.Text, Description = entry_description.Text });
                editMode = false;
                entry_hero.Text = "";
                entry_description.Text = "";
                button_add.Text = "Add Hero";
            }
            else
            {
                bool repeated = false;
                foreach (Hero hero in heroes)
                {
                    if (hero.DisplayName.Equals(entry_hero.Text))
                    {
                        repeated = true;
                    }
                }
                if (repeated == false) heroes.Add(new Hero { DisplayName = entry_hero.Text, Description = entry_description.Text });
            }
                
        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; 
            }
            editMode = true;
            selectedIndex = (HeroesView.ItemsSource as ObservableCollection<Hero>).IndexOf(e.SelectedItem as Hero);
            entry_hero.Text = ((Hero)e.SelectedItem).DisplayName;
            entry_description.Text = ((Hero)e.SelectedItem).Description;
            button_add.Text = "Edit Hero";

        }

    }

    public class Hero
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
