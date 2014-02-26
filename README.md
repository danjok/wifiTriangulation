**WhereAmI**
========================
A WiFi Localizer for Windows OS

*Authors:   Abbà Maurizio - Annuzzi Daniele*

## Description ##
WhereAmI is a context-aware system based on WiFi localization. It lets Windows Users to save places, configure actions to be automatically executed and check time statistics for places where they have been.

The localization is only based on Wireless Access Points retrieved by the Wifi (802.11) network adapters installed in the Windows machine.

Actions can be:
* commands to be executed in the Command Prompt (such as the automatic firewall configuration, network settings, ...)
* application executions
* message notification in the task bar
* wallpaper setting

### Technical features ###
* WPF Application (Microsoft .NET Framework 4.5)
* Dependencies managed with NuGet
* Use of Entity Framework (EF 6 - Code First approach)
* Use of [Managed Wifi API] (http://managedwifi.codeplex.com/) 

### Core ###
The localization algorithm is based on the following scenario:

we memorized a set of locations, each location has a certain number of networks associated, with its relative power.
It's like a snapshot of the wifi interface list captured in that location.

When we take a snapshot, we want to compare it to the ones memorized in order to understand which is the most similar and therefore the location where we are.

The algorithm compute a "similarity" score for each snapshot memorized, comparing it with the actual one.
The score is based on the following pseudocode, given L the location memorized and C the actual one:

1. for all networks in C and in L:
    2. compute t = sum( (100-abs(pow_C - pow_L))^2*pow_L/100)
    3. score_L = N_networks_inC && N_networks_inL / N_networks_inC * t

The winning location is going to be the one with the highest score

