The algorithm is based on the following scenario:

we memorized a set of locations, each location has a certain number of networks associated, with its relative power.
It's like a snapshot of the wifi interface list captured in that location.

When we take a snapshot, we want to compare it to the ones memorized in order to understand which is the most similar and therefore the location where we are.

The algorithm compute a "similarity" score for each snapshot memorized, comparing it with the actual one.
The score is based on the following pseudocode, given L the location memorized and C the actual one:

1. for all networks in C and in L:
    2. compute t = sum( (100-abs(pow_C - pow_L))^2*pow_L/100)
    3. score_L = N_networks_inC && N_networks_inL / N_networks_inC * t

The winning location is going to be the one with the highest score

