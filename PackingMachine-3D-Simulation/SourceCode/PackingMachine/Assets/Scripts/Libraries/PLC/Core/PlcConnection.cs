using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sharp7;
using System.Linq;

public class PlcConnection : MonoBehaviour {

    public bool onlineAtStart = true;
    public KeyCode toggleOnlineOnKey = KeyCode.None;

    public string ipAddress = "127.0.0.1";

    public PlcInput[] inputs;
    public PlcOutput[] outputs;

    protected PlcInput[] sortedInputs;
    protected PlcOutput[] sortedOutputs;

    public string[] inputAddresses;
    public string[] outputAddresses;

    protected string[] sortedInputAddresses;
    protected string[] sortedOutputAddresses;

    public bool debugByClick = false;
    public bool debugByKeys = false;
    public bool debugBySound = false;
    public string[] outputDebugKeys;
    public AudioClip[] inputSounds;

    S7Client plc;
    bool isConnected = false;
    int resultCode;
    byte[] zeroByte = new byte[]{0};

    // Constants defined in TIA Portal
    const int INPUTS_DB_NUMBER = 10;
    const int OUTPUTS_DB_NUMBER = 100;

    bool[] lastInputs;
    int[][] intInputAddresses;
    int[][] intOutputAddresses;

    // Lists of pairs : first number in pair defines start index, the second defines the amount
    List<int[]> inputBlocks = new List<int[]>(); 
    List<int[]> outputBlocks = new List<int[]>();

    // Inputs events - due to multithreading
    Queue<PlcIO.Change> inputEvents = new Queue<PlcIO.Change>();

    private bool _online;
    public bool Online
    {
        get
        {
            return _online;
        }
        set
        {
            if(_online != value)
            {
                _online = value;
                if (value && !isConnected)
                    ConnectWithPLC();
            }
        }
    }

    public void EnqueueInputEvent(PlcIO.Change inputEvent)
    {
        inputEvents.Enqueue(inputEvent);
    }

    public bool IsOnline()
    {
        return isConnected && Online;
    }

    public void GoOnline()
    {
        Online = true;
    }

    public void GoOffline()
    {
        Online = false;
    }


    void Awake()
    {
        // Load saved data - make backup for case the file doesn't exist or is misformated
        string[] defaultInputAddresses = (string[])inputAddresses.Clone();
        string[] defaultOutputAddresses = (string[])outputAddresses.Clone();
        if(!PlcIO.ReadIOAddresses(ref inputAddresses, ref outputAddresses))
        {
            inputAddresses = defaultInputAddresses;
            outputAddresses = defaultOutputAddresses;
            PlcIO.SaveIOAddresses(inputAddresses, outputAddresses, inputs, outputs); // TODO
        }

        // Duplicate before sorting        
        sortedInputAddresses = (string[])inputAddresses.Clone();
        sortedOutputAddresses = (string[])outputAddresses.Clone();
        sortedInputs = (PlcInput[])inputs.Clone();
        sortedOutputs = (PlcOutput[])outputs.Clone();

        // Sort IO by addresses
        Array.Sort(sortedInputAddresses, sortedInputs);
        Array.Sort(sortedOutputAddresses, sortedOutputs);

        // Bind inputs and outputs to this plc
        BindIO(sortedInputs);
        BindIO(sortedOutputs);

        // Convert string addresses to numerical
        intInputAddresses = ParseAddresses(sortedInputAddresses);
        intOutputAddresses = ParseAddresses(sortedOutputAddresses);

        // Initialize the outputBlocks
        outputBlocks = GetBlocks(intOutputAddresses);
        inputBlocks = GetBlocks(intInputAddresses);
    }


    void Start () {

        // Set Debug By click of outputs
        foreach (PlcOutput o in sortedOutputs)
            o.debugByClick = debugByClick;

        // Connect to PLC
        if (onlineAtStart) {
            GoOnline();
        }
    }

    // Update is called once per frame
    void Update () {    

        if(toggleOnlineOnKey != KeyCode.None && Input.GetKeyDown(toggleOnlineOnKey))
            Online = !Online;
        
            
        if (debugByKeys)
        {
            for(int i=0; i<outputDebugKeys.Length && i<outputs.Length; i++)
            {
                if (Input.GetKeyDown(outputDebugKeys[i]))
                    outputs[i].Value = !outputs[i].Value;
            }
        }
    }

    public bool ConnectWithPLC()
    {
        // Connect
        plc = new S7Client();
        resultCode = plc.ConnectTo(ipAddress, 0, 1);
        isConnected = CheckResultCode(resultCode);

        //Debug.Log("Is connected = " + isConnected);

        if (!isConnected)
            return false;

        // Clear outputs and initialize inputs states
        InitializePLC();

        // Begin synchronization
        StartCoroutine(SyncWithPLC());

        return true;
    }

    void InitializePLC()
    {
        // Initialize inputs to default values
        lastInputs = new bool[sortedInputs.Length];
        /*for (int i = 0; i < inputs.Length; i++)
        {
            lastInputs[i] = inputs[i].Value;
            resultCode = plc.Write(S7Consts.S7AreaMK, intInputAddresses[i], inputs[i].Value);
            CheckResultCode(resultCode);
        }*/

        // Clear outputs
        foreach (int[] outputBlock in outputBlocks)
        {
            byte[] buffer = new byte[outputBlock[1]];
            resultCode = plc.WriteBlock(S7Consts.S7AreaPA, outputBlock[0], buffer);
            CheckResultCode(resultCode);
        }
    }

    IEnumerator SyncWithPLC(float startDelaySeconds = 0.5f)
    {
        // Delay to prevent hickups from connection
        yield return new WaitForSecondsRealtime(startDelaySeconds);

        while (isConnected)
        {
            if (Online)
            {
                // Update inputs - write events in queue
                foreach (int[] inputBlock in inputBlocks)
                {
                    int blockIndex = inputBlock[0];
                    int blockSize = inputBlock[1];

                    byte[] buffer = new byte[blockSize];

                    // Wait while PLC has read the data
                    /*byte[] dataRead = new byte[1];
                    int x = 0;
                    do
                    {
                        resultCode = plc.ReadBlock(S7Consts.S7AreaMK, blockIndex+blockSize-1, dataRead);
                        CheckResultCode(resultCode);

                        x++;
                        if (x > 1)
                            Debug.Log("read more " + x);
                        if (x > 100)
                        {
                            plc.Disconnect();
                            break;
                        }

                    } while (dataRead[0] >> 7 != 1); */

                    for (int i = 0; i < sortedInputs.Length; i++)
                    {
                        int wordIndex = intInputAddresses[i][0];
                        int bitIndex = (byte)intInputAddresses[i][1];

                        if (wordIndex >= blockIndex && wordIndex < blockIndex + blockSize)
                        {
                            int byteIndex = wordIndex - blockIndex;
                            bool value = sortedInputs[i].Value;

                            // Debug
                            if(debugBySound && value && !lastInputs[i] && inputSounds.Length > i && inputSounds[i] != null)
                                GetComponent<AudioSource>().PlayOneShot(inputSounds[i]);

                            if (value)
                                buffer[byteIndex] |= (byte)(1 << bitIndex);
                            else
                                buffer[byteIndex] &= (byte)~(1 << bitIndex);
                        }

                        lastInputs[i] = sortedInputs[i].Value;
                    }
                    //int shift = 7;
                    //buffer[buffer.Length - 1] &= (byte)~(1 << shift); // Data Read byte
                    resultCode = plc.WriteArea(S7Consts.S7AreaDB, INPUTS_DB_NUMBER, blockIndex, buffer.Length, S7Consts.S7WLByte, buffer);
                    CheckResultCode(resultCode);
                }

                // Update outputs - copy if changed at PLC
                foreach (int[] outputBlock in outputBlocks)
                {
                    byte[] buffer = new byte[outputBlock[1]];
                    //resultCode = plc.ReadBlock(S7Consts.S7AreaPA, outputBlock[0], buffer);
                    resultCode = plc.ReadArea(S7Consts.S7AreaDB, OUTPUTS_DB_NUMBER, outputBlock[0], outputBlock[1], S7Consts.S7WLByte, buffer);
                    CheckResultCode(resultCode);

                    for(int i=0; i<sortedOutputs.Length; i++)
                    {
                        if(intOutputAddresses[i][0] >= outputBlock[0] && intOutputAddresses[i][0] < outputBlock[0] + outputBlock[1])
                        {
                            int wordIndex = intOutputAddresses[i][0] - outputBlock[0];
                            int bitIndex = intOutputAddresses[i][1];
                            byte word = buffer[wordIndex];
                            bool value = ((word >> bitIndex) & 0x1) == 1;

                            if (value != sortedOutputs[i].Value)
                                sortedOutputs[i].Value = value;
                        }
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private bool CheckResultCode(int resultCode)
    {
        if (resultCode != 0)
        {
            Debug.LogError("Error communicating PLC: " + plc.ErrorText(resultCode) + "\nHave you enabled PUT/GET in TIA Portal and is NetToPlcSim started?");
            isConnected = false;
            Online = false;
            return false;
        }
        return true;
    }

    private void BindIO(PlcIO[] io)
    {
        for(int i=0; i<io.Length; i++)
        {
            io[i].BindToPlc(this, i);
        }
    }

    /** Input address like 0.4 and returns int[]{0,4}. */
    private int[] ParseAddress(string address)
    {
        string[] addressSplit = address.Trim().Split('.');
        int byteIndex = Convert.ToInt32(addressSplit[0]);
        int bitIndex = Convert.ToInt32(addressSplit[1]);
        return new int[] {byteIndex, bitIndex};
    }

    /** Takes string address array input {0.0, 0.1, etc..} and converts it to int array of pairs. */
    private int[][] ParseAddresses(string[] addresses)
    {
        int[][] intAddresses = new int[addresses.Length][];
        for (int i = 0; i < addresses.Length; i++)
            intAddresses[i] = ParseAddress(addresses[i]);
        return intAddresses;
    }

    /** Takes the numerical addresses and returns list of continuous blocks in memory which can be read. 
     * Example:
     *  input: {[0,1], [0,2], [1,2], [1,5], [3,5], [8,1], [9, 3]}
     *  returns: {[0,2], [3,1], [8,2]}
     */
    private List<int[]> GetBlocks(int[][] addresses)
    {
        HashSet<int> byteIndexes = new HashSet<int>();
        foreach(int[] address in addresses)
            byteIndexes.Add(address[0]);

        List<int> byteIndexesSorted = byteIndexes.ToList();
        byteIndexesSorted.Sort();

        List<int[]> blocks = new List<int[]>();
        int lastByteIndex = -2;
        for(int i=0; i<byteIndexesSorted.Count; i++)
        {
            int currentByteIndex = byteIndexesSorted[i];
            if (currentByteIndex - lastByteIndex > 1)
            {   
                blocks.Add(new int[] {byteIndexesSorted[i], 1});
            }
            else if(currentByteIndex != lastByteIndex)
            {
                blocks[blocks.Count - 1][1]++;
            }
            lastByteIndex = currentByteIndex;
        }
        return blocks;
    }

    public class InputChange {
        PlcInput input;
        
    }

}

public static class S7ClientExtenisons
{
    /** Reads buffer.Length bytes from area to the buffer, starting at startByteIndex. */
    public static int ReadBlock(this S7Client plc, int area, int startByteIndex, byte[] buffer)
    {
        return plc.ReadArea(area, 0, startByteIndex, buffer.Length, S7Consts.S7WLByte, buffer);
    }

    /** Writes buffer.Length bytes from area to the buffer, starting at startByteIndex. */
    public static int WriteBlock(this S7Client plc, int area, int startByteIndex, byte[] buffer)
    {
        return plc.WriteArea(area, 0, startByteIndex, buffer.Length, S7Consts.S7WLByte, buffer);
    }

    public static int Write(this S7Client plc, int area, int[] address, bool value)
    {
        int result;
        int wordIndex = address[0];
        int bitIndex = address[1];

        byte[] buffer = new byte[1];
        Sharp7.S7.SetBitAt(ref buffer, 0, bitIndex, value);

        result = plc.WriteArea(area, 0, wordIndex * 8 + bitIndex, buffer.Length, S7Consts.S7WLBit, buffer); // AreaMK - merker area (in germany) => memory bit
       
        return result;
    }

    public static int Read(this S7Client plc, int area, int[] address, AtomicBool value)
    {
        int result;

        int wordIndex = address[0];
        int bitIndex = address[1];

        byte[] buffer = new byte[1];

        switch (area)
        {
            case S7Consts.S7AreaMK:
                result = plc.MBRead(wordIndex, 1, buffer);
                
                break;

            case S7Consts.S7AreaPA:
                result = plc.ABRead(wordIndex, 1, buffer);
                break;

            default:
                result = S7Consts.errCliInvalidBlockType;
                break;
        }

        value.Value = Sharp7.S7.GetBitAt(buffer, 0, bitIndex);

        return result;
    }
}

public class AtomicBool
{
    public bool Value
    {
        get; set;
    }
}
