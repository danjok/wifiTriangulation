using System;
using System.Collections.Generic;
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
            snapshot1.Add(new Wifi() { SSID = "Home", PowerPerc = 100 });
            snapshot1.Add(new Wifi() { SSID = "HomeDani", PowerPerc = 1 });
            snapshot1.Add(new Wifi() { SSID = "dlink", PowerPerc = 10 });

            List<Wifi> snapshot2 = new List<Wifi>();
            snapshot2.Add(new Wifi() { SSID = "Polito", PowerPerc = 10 });
            snapshot2.Add(new Wifi() { SSID = "Lab1", PowerPerc = 30 });
            snapshot2.Add(new Wifi() { SSID = "Lab2", PowerPerc = 50 });

            List<Wifi> snapshot3 = new List<Wifi>();
            snapshot3.Add(new Wifi() { SSID = "CompanyHall", PowerPerc = 50 });
            snapshot3.Add(new Wifi() { SSID = "CompanyRoom1", PowerPerc = 20 });
            snapshot3.Add(new Wifi() { SSID = "CompanyLab", PowerPerc = 80 });

            defaultPlaces.Add(new Place() { Name = "Home", Cnt = 0, Snapshot = Place.serializationSnapshot(snapshot1) });
            defaultPlaces.Add(new Place() { Name = "Polito", Cnt = 0, Snapshot = Place.serializationSnapshot(snapshot2) });
            defaultPlaces.Add(new Place() { Name = "Work", Cnt = 0, Snapshot = Place.serializationSnapshot(snapshot3) });

            foreach (Place p in defaultPlaces)
                context.Places.Add(p);
            
            //All standards will
            base.Seed(context);
        }
    }

    public class AppContext : DbContext
    {
        //public AppContext(): base()
        public AppContext(): base("WhereAmI_DBConnectionString")
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
