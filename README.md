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
1. Restart your computer
2. Start **NetToPLCsim.exe** as Administrator.
3. If dialog window about Port 102 pops up, click on Yes to make the port available.
4. In NetToPLCSim open the **defualt.ini** file contained in this repo.
5. Click on *Start Server* button.

## PLCSIM
1. Start **PLCSIM**.
2. Create new *S7-1200* project or open existing.

## TIA Portal
1. Start **TIA Portal**.
2. Open the **PackingMachine_Clean** project contained in this repo.
3. Download the project to running PLCSIM instance.

## Packing Machine App
1. Start the **PackingMachine.exe** app contained in this repo.
2. Choose screen resolution and hit Play button.
3. Click on the *Connect* button in lower left corner.
4. If everything goes ok, it should change its text to *Disconnect* (which means it is successfully connected to PLCSIM). 
