# Introduction
If you have TIA Portal but don't have any machine to program, then this project is for you. It is a 3D simulation of *Packing Machine* which is controlled by *Siemens S7-PLCSIM* (virtual PLC). So basically you can program PLCSIM using TIA Portal, and test the program visually in the 3D graphical simulation.

# Prequisites
  - **Windows** operating system
  - Installed [**TIA Portal**](http://www.industry.siemens.com/topics/global/en/tia-portal/Pages/default.aspx) and [**Simatic S7-PLCIM**](http://w3.siemens.com/mcms/simatic-controller-software/en/step7/simatic-s7-plcsim/pages/default.aspx) (both tested with V13)
  - Download and unzip [**NetToPLCSim**](https://sourceforge.net/projects/nettoplcsim/files/latest/download)
  - Clone/download this repository

# Get Started
It is important to do the following steps in order they are written in.

## NetToPLCSim
1. Restart your computer (optional, read the **Note** below).
2. Start **NetToPLCsim.exe** as Administrator.
3. If dialog window about Port 102 pops up, click on Yes to make the port available.
4. In NetToPLCSim open the **defualt.ini** file contained in this repo.
5. Click on *Start Server* button.
>**Note:** The computer restart is not needed if you haven't started TIA Portal or PLCSIM yet since the computer power-up. 

![Screen](https://cloud.githubusercontent.com/assets/26493472/24073674/7a75d8ba-0bfb-11e7-92a4-8159509bccb8.png)

## PLCSIM
1. Start **PLCSIM**.
2. Create new *S7-1200* project or open existing one.

## TIA Portal
1. Start **TIA Portal**.
2. Open the **PackingMachine_Clean** project contained in this repo.
3. Download the project to running PLCSIM instance.

## Packing Machine App
1. Start the **PackingMachine.exe** app contained in this repo.
2. Choose screen resolution and hit Play button.
3. Click on the *Connect* button in lower left corner.
4. If everything goes ok, it should change its text to *Disconnect* (which means it is successfully connected to PLCSIM). 

![image](https://cloud.githubusercontent.com/assets/26493472/24073735/659e09c0-0bfc-11e7-9e04-e1699d42539d.png)

# How to Program
Now you can *Save as* the opened PackingMachine_Clean TIA Portal project under different name and start programming. The list of Inputs and Outputs can be found in the same-named PLC Tag tables. Even in the simulation app after pressing Esc or Space and clicking on the Help icon, a list of keyboard shortcuts is present which can be used to manually trigger individual outputs (only when Disconnected from PLCSIM).

![image](https://cloud.githubusercontent.com/assets/26493472/24757240/e58317f2-1ade-11e7-9439-65ba47b2e0b9.jpg)

After pressing **Esc** a pause menu is triggered. You can also use **Space** shortcut to restart the scene.

![image](https://cloud.githubusercontent.com/assets/26493472/24073817/0aae2f3e-0bfe-11e7-9476-10efcc76502c.png)

# Support
If you have any questions, feel free to open New Issue.

# Special Thanks to
* **Davide Nardella** for creating Sharp7
* **Thomas Wiens** for creating NetToPLCSim
* **Michele Cattafesta** (www.mesta-automation.com) for [great tutorial](https://www.mesta-automation.com/how-to-write-a-siemens-s7-plc-driver-with-c-and-sharp7/#comment-950) on Sharp7

