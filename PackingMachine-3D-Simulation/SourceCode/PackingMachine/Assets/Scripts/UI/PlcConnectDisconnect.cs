using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlcConnectDisconnect : MonoBehaviour {

    public PlcConnection plc;
    public Text buttonText;
    public Image buttonIcon;

    bool _connectionState;
    bool ConnectionState
    {
        get
        {
            return _connectionState;
        }
        set
        {
            if (value != _connectionState)
            {
                //Debug.Log("new state = " + value);
                if (value)
                {
                    plc.GoOnline();
                }
                else {
                    plc.GoOffline();
                }
                _connectionState = plc.IsOnline();
                UpdateButtonStatus(buttonIcon, buttonText, _connectionState);
                
            }
        }
    }

	// Use this for initialization
	void Start () {
        _connectionState = !plc.IsOnline();
        ConnectionState = !ConnectionState;

    }
	
	// Update is called once per frame
	void Update () {
        ConnectionState = plc.IsOnline();
    }

    public void ToggleConnectDisconnect()
    {
        ConnectionState = !ConnectionState;
    }

    private void UpdateButtonStatus(Image icon, Text text, bool connected)
    {
        text.text = connected ? "Disconnect" : "Connect";
        icon.color = connected ? Color.green : Color.red;
    }
}
