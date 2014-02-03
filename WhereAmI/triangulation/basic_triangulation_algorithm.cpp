#include <iostream>

#include <stdlib.h>     /* srand, rand */
#include <time.h>       /* time */


#include "basic_triangulation_algorithm.h"

using namespace std;

class SingleNetwork {
    /*a SingleNetwork is identified by an ssid and a signal quality*/
    private:
        char ucSSID[DOT11_SSID_MAX_LENGTH];
        unsigned long wlanSignalQuality;
    public:
        SingleNetwork() {
            int a = rand()%10;
            sprintf(ucSSID,"%s%d", "testNetwork", a);
            wlanSignalQuality = rand()%100+1;
        }
        ~SingleNetwork() {
        }
        void printSingleNetwork() {
            cout <<"\t Name: "<< ucSSID << " Power: " << wlanSignalQuality << endl;
            return ;
        }
        char * getucSSID() {
            return ucSSID;
        }
        int getPower() {
            return wlanSignalQuality;
        }
};
class Snapshot {
    /*a snapshot is identified by a name and a list of SingleNetwork objects associated*/
    private:
        char name[SNAPSHOT_NAME_MAX_LENGTH];
        unsigned long nameLength;
//        list <SingleNetwork> listOfNetworks; //it's a list because we always iterate on all elements, we add to it always in head
        tr1::unordered_map <string,SingleNetwork> listOfNetworks; //let's try with a hash table: in this way we lose when we insert elements, but then we are a lot better during comparison Score (as we have to check the whole list n times)
        unsigned long similarityScore; //this will be the score that is computed for that snapshot compared to the reference one
    public:
        Snapshot(int i) {
        /*Here we will do the request to DB etc, right now is just a dummy creation*/
            sprintf(name, "%s%d", "Snapshot", i);
            nameLength=strlen(name);
            int cntNetworks=0;
            set<string> nameNetworkList;
            for (cntNetworks=0; cntNetworks<i; cntNetworks+=1) {
                string item;
                SingleNetwork * temp= NULL;
                while ( item.empty() || nameNetworkList.find(item) != nameNetworkList.end()) {
                    if (temp != NULL)
                        free(temp);
                    temp = new SingleNetwork();
                    item = temp->getucSSID();
                }
                listOfNetworks.insert(make_pair( item, *temp ));
                nameNetworkList.insert(item);
            }
        }
        void printSnapshot() {
            cout << "printing single snapshot" << endl;
            cout << name << endl;
            cout << "printing list of associated networks" << endl;
            tr1::unordered_map<string,SingleNetwork>::iterator it;
            it = listOfNetworks.begin();
            while(it != listOfNetworks.end()) {
                it->second.printSingleNetwork();
                it++;
            }

            return ;
        }

        ~Snapshot(){
        }
        double computeScore(Snapshot * check) {
            //the idea is to compute a score based on netfound/ncheck * sum((powcheck-pownet)^2)
            //NO! the fact is that we watn to look at similar network, our score system is max high, so we want to reward the one with the smallest distance between the 2 networks -> 100- diff
            //Working
            //Experimental: introduction of a coefficent for weighing networks: the power of t should be multiplied by a weight based on the power of the network present in list
            //a snapshot with a network which is heard 95 should be far more important then a newtork heard 20
            tr1::unordered_map<string,SingleNetwork>::iterator itList;
            tr1::unordered_map<string,SingleNetwork>::iterator itCheck;
            itList = listOfNetworks.begin();
            unsigned long totalDiff=0;
            float nNetFound=0;
            //iterate on all networks present in check
            itCheck = check->listOfNetworks.begin();
            while(itCheck != check->listOfNetworks.end()) {
                string nameNetCheck = itCheck->second.getucSSID();
                itList = listOfNetworks.find(nameNetCheck);
                if (itList != listOfNetworks.end()) {//found
                    double t = 100 - abs((itCheck->second.getPower()-itList->second.getPower()));
                    double weigth = computeWeigthForPower(itList->second.getPower());
                    totalDiff+=pow(t, 2) * weigth;
                    //totalDiff+=pow(t, 2);
                    nNetFound+=1;
                    cout << "Match for " << nameNetCheck << "computed t=" << t << endl;
                }
                itCheck++;
            }
            cout << "TotalDiff is " << totalDiff << " found " << nNetFound << " over " << check->listOfNetworks.size() << endl; 
            return (nNetFound/check->listOfNetworks.size()) * totalDiff;
        }

};

double computeWeigthForPower(unsigned int power) {
    /*first test: weight is simple power/100, so a network heard at 20 will have a weight of 0.2 one at 95 will have a 0.95*/
    return power/100.0;
}

list<Snapshot> getSnapshotList() {
/*Create a dummy list of snapshots*/
    std::list<Snapshot> tempList;
    int i=1;
    for (i=1;i<10;i+=1) {
        tempList.push_back(Snapshot(i));
    }
    return tempList;
}

int main() {
    std::list<Snapshot> snapshotList;
    snapshotList = getSnapshotList();
    list<Snapshot>::iterator p = snapshotList.begin();
/*    while(p != snapshotList.end()) {
        p->printSnapshot();
        p++;
    }
*/
    Snapshot * check = new Snapshot(4);
    check->printSnapshot();
    p=snapshotList.begin();
    double maxScore=-1;
    Snapshot * maxSnapshot;
    double tempScore=0;
    while(p != snapshotList.end()) {
        p->printSnapshot();
        tempScore = p->computeScore(check);
        cout << "tempscore is " << tempScore << endl;
        if (tempScore > maxScore) {
            maxScore=tempScore;
            maxSnapshot = &(*p);
        }
        p++;
    }
    if (maxScore==-1) {
        p=snapshotList.begin();
        maxSnapshot = &(*p);
    }
    cout << "max resemblance with"<< endl;
    maxSnapshot->printSnapshot();
    cout << "score " << maxScore << endl;
    return 0;
}
