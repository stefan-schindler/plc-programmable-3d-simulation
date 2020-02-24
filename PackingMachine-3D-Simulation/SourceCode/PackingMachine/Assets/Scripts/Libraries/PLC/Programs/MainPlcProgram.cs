using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlcProgram : PlcProgram
{
    // Tags definition
    
    // Inputs
    public int packageAvailable = 0;
    public int packagePistonRetracted = 1;
    public int packagePistonExtended = 2;
    public int packageMotorDown = 3;
    public int packageMotorUp = 4;
    public int packageMotorLeft = 5;
    public int packageMotorRight = 6;
    // Outputs
    public int packagePiston = 0;
    public int packageMotorLift = 1;
    public int packageMotorRotate = 2;
    public int packageGrab = 3;



    protected override void ProgramCycle()
    {
        // Package piston
        if(!GetInput(packageAvailable) && GetInput(packagePistonRetracted)) {
            SetOutput(packagePiston, true);
        }
        if (GetInput(packagePistonExtended))
        {
            SetOutput(packagePiston, false);
        }

        // Package grab
        if(GetInput(packageAvailable) && !GetOutput(packageGrab))
        {
            SetOutput(packageMotorRotate, false);
            if (GetInput(packageMotorLeft))
            {
                SetOutput(packageMotorLift, false);
                if (GetInput(packageMotorDown))
                {
                    SetOutput(packageGrab, true);
                    SetOutput(packageMotorLift, true);
                }
            }
        }
    }
}
