using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereAmI.models;

namespace WhereAmI
{
    //create your custom DB initializer by inheriting one of the intializer
    public class AppContextInitializer : CreateDatabaseIfNotExists<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            IList<Place> defaultPlaces = new List<Place>();

            List<Wifi> snapshot1 = new List<Wifi>();
            snapshot1.Add(new Wifi() { MAC="A",SSID = "Home", PowerPerc = 100 });
            snapshot1.Add(new Wifi() { MAC="B",SSID = "HomeDani", PowerPerc = 1 });
            snapshot1.Add(new Wifi() { MAC="C",SSID = "dlink", PowerPerc = 10 });

            List<Wifi> snapshot2 = new List<Wifi>();
            snapshot2.Add(new Wifi() { MAC="D",SSID = "Polito", PowerPerc = 10 });
            snapshot2.Add(new Wifi() { MAC="E",SSID = "Lab1", PowerPerc = 30 });
            snapshot2.Add(new Wifi() { MAC="F",SSID = "Lab2", PowerPerc = 50 });

            List<Wifi> snapshot3 = new List<Wifi>();
            snapshot3.Add(new Wifi() { MAC="1",SSID = "CompanyHall", PowerPerc = 50 });
            snapshot3.Add(new Wifi() { MAC="2",SSID = "CompanyRoom1", PowerPerc = 20 });
            snapshot3.Add(new Wifi() { MAC="3",SSID = "CompanyLab", PowerPerc = 80 });

            //Actions
            models.Action a1 = new models.Action() { ActionId = 1, Type="cmd", Name = "Use DHCP", Command = "netsh interface ip set address name='Connessione rete wireless' source=dhcp" };
            models.Action a2 = new models.Action() { ActionId = 2, Type="cmd", Name = "Turn on firewall", Command = "netsh advfirewall set currentprofile state on" };
            models.Action a3 = new models.Action() { ActionId = 3, Type="cmd", Name = "Turn off firewall", Command = "netsh advfirewall set currentprofile state off" };
            models.Action a4 = new models.Action() { ActionId = 1, Type="cmd", Name = "Use Static Address", Command = "netsh interface ip set address name='Connessione rete wireless' 192.168.1.1 255.255.255.0 192.168.0.1 1" };
            
            context.Actions.Add(a1);
            context.Actions.Add(a2);
            context.Actions.Add(a3);
            context.Actions.Add(a4);

            ObservableCollection<models.Action> actions1 = new ObservableCollection<models.Action>();
            actions1.Add(a1);
            actions1.Add(a2);

            ObservableCollection<models.Action> actions2 = new ObservableCollection<models.Action>();
            actions2.Add(a3);
            actions2.Add(a2);

            defaultPlaces.Add(new Place() { Name = "Home", Cnt = 0, Snapshot = Place.serializationSnapshot(snapshot1), InActions = actions1 });
            defaultPlaces.Add(new Place() { Name = "Polito", Cnt = 0, Snapshot = Place.serializationSnapshot(snapshot2), InActions = null });
            defaultPlaces.Add(new Place() { Name = "Work", Cnt = 0, Snapshot = Place.serializationSnapshot(snapshot3), InActions = actions2 });

            foreach (Place p in defaultPlaces)
                context.Places.Add(p);

            //All standards will
            base.Seed(context);
        }
    }

    public class AppContext : DbContext
    {
        //public AppContext(): base()
        public AppContext()
            : base("WhereAmI_DBConnectionString")
        {
            //Database.SetInitializer<AppContext>(new CreateDatabaseIfNotExists<AppContext>());
            //Database.SetInitializer<AppContext>(new DropCreateDatabaseAlways<AppContext>());
            //Database.SetInitializer<AppContext>(new DropCreateDatabaseIfModelChanges<AppContext>());
            //Database.SetInitializer<AppContext>(new AppContextInitializer());
        }

        public DbSet<Place> Places { get; set; }
        public DbSet<WhereAmI.models.Action> Actions { get; set; }
    }

}
