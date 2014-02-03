using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereAmI.models;

namespace WhereAmI.triangulation
{
    public class Algorithm
    {
        public Algorithm()
        {

        }

        double computeWeigthForPower(int power) {
        /*first test: weight is simple power/100, so a network heard at 20 will have a weight of 0.2 one at 95 will have a 0.95*/
        return power/100.0;
        }

        private double computeScore(Place placeToCheck, ICollection<Wifi> currentWifis){
            //the idea is to compute a score based on netfound/ncheck * sum((powcheck-pownet)^2)
            //NO! the fact is that we watn to look at similar network, our score system is max high, so we want to reward the one with the smallest distance between the 2 networks -> 100- diff
            //Working
            //Experimental: introduction of a coefficent for weighing networks: the power of t should be multiplied by a weight based on the power of the network present in list
            //a snapshot with a network which is heard 95 should be far more important then a newtork heard 20
            //tr1::unordered_map<string,SingleNetwork>::iterator itList;
            //tr1::unordered_map<string,SingleNetwork>::iterator itCheck;
            //itList = listOfNetworks.begin();
            
            double totalDiff=0;
            float nNetFound=0;
            //iterate on all current networks
            foreach(Wifi currentWifi in currentWifis){
                 //look for a current network in networks of the place to check
                 Wifi storedWifi = placeToCheck.Wifis.Where(w => w.SSID == currentWifi.SSID).FirstOrDefault<Wifi>();
                 if(storedWifi != null) {
                     //found
                    double t = 100 - Math.Abs((currentWifi.PowerPerc-storedWifi.PowerPerc));
                    double weigth = computeWeigthForPower(storedWifi.PowerPerc);
                    totalDiff+= Math.Pow(t, 2.0) * weigth;
                    //totalDiff+=pow(t, 2);
                    nNetFound+=1;
                    //cout << "Match for " << nameNetCheck << "computed t=" << t << endl;
                 } 
            }
            //cout << "TotalDiff is " << totalDiff << " found " << nNetFound << " over " << check->listOfNetworks.size() << endl; 
            return (nNetFound/currentWifis.Count) * totalDiff;
        }

        private Place noPlace = new Place() {PlaceId=-1, Name = "NoPlace" };

        public Place computeCurrentPlace(ICollection<Wifi> currentWifis)
        {
            var ctx = DataManager.Instance.context;
            double maxScore = -1;
            Place mostSimilarPlace = noPlace;
            double tempScore = 0;
            foreach (Place place in ctx.Places){
                tempScore = computeScore(place, currentWifis);
                if (tempScore > maxScore)
                {
                    maxScore = tempScore;
                    mostSimilarPlace = place;
                }
            }
            return mostSimilarPlace;
        }
    }
}
