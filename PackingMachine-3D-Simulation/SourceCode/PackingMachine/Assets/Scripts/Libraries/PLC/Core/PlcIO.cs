using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlcIO : MonoBehaviour
{

    public string label = "Untitled Sensor";

    protected PlcConnection plc;
    protected int indexAtPlc;

    public void BindToPlc(PlcConnection plc, int index)
    {
        this.plc = plc;
        this.indexAtPlc = index;
    }

    protected void PublishChange(bool newValue)
    {
        if (plc != null)
            plc.EnqueueInputEvent(new Change(indexAtPlc, newValue));
    }

    public class Change
    {
        public int Index { get; }
        public bool NewValue { get; }

        public Change(int index, bool newValue)
        {
            this.Index = index;
            this.NewValue = newValue;
        }
        
    }

    private const string USER_IO_ADDRESSES_PATH = "user_io_addresses.v3.txt";

    /** Saves input/output addresses to PlayerPrefs. */
    public static void SaveIOAddresses(string[] inputAddresses, string[] outputAddresses, PlcInput[] plcInputs, PlcOutput[] plcOutputs)
    {
        string outputContent = inputAddresses.Length + " inputs\n" + outputAddresses.Length + " outputs\n";
        outputContent += "----\n";
        for(int i=0; i<inputAddresses.Length; i++)
            outputContent += inputAddresses[i] + " " + plcInputs[i].label + "\n";
        outputContent += "----\n";
        for (int i = 0; i < outputAddresses.Length; i++)
            outputContent += outputAddresses[i] + " " + plcOutputs[i].label + "\n";

        System.IO.File.WriteAllText(USER_IO_ADDRESSES_PATH, outputContent);
    }

    /** Populates the passed arrays with read data, if data dosn't exist or an error occurs false is returned.*/
    public static bool ReadIOAddresses(ref string[] inputAddresses, ref string[] outputAddresses)
    {
        if (System.IO.File.Exists(USER_IO_ADDRESSES_PATH))
        {
            try
            {
                System.IO.StreamReader fin = new System.IO.StreamReader(USER_IO_ADDRESSES_PATH);

                int iLength = Convert.ToInt32(fin.ReadLine().Split(' ')[0]);
                inputAddresses = new string[iLength];

                int oLength = Convert.ToInt32(fin.ReadLine().Split(' ')[0]);
                outputAddresses = new string[oLength];

                fin.ReadLine();

                string line;
                for (int i = 0; i < iLength; i++)
                {
                    if ((line = fin.ReadLine()) != null)
                        inputAddresses[i] = line.Split(' ')[0];
                    else
                        return false;
                }
                fin.ReadLine();
                for (int i = 0; i < oLength; i++)
                {
                    if ((line = fin.ReadLine()) != null)
                        outputAddresses[i] = line.Split(' ')[0];
                    else
                        return false;
                }
            }catch(Exception e)
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }
}

public class PlcInput : PlcIO
{
    protected bool _value = false;

    public virtual bool Value
    {
        get
        {
            return _value;
        }
        protected set
        {
            if (_value != value)
                PublishChange(value);
            _value = value;
        }
    }
}

public class PlcOutput : PlcIO
{
    public bool debugByClick = true;

    protected bool _value = false;

    public virtual bool Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
        }
    }

    void OnMouseDown()
    {
        if ((plc == null && debugByClick) || (plc != null && plc.debugByClick))
            Value = !Value;
    }
}

