using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityStandardAssets.ImageEffects;

public class ModalMenu : MonoBehaviour {

    public KeyCode toggleKey = KeyCode.Escape;
    public KeyCode restartShortcut = KeyCode.Space;
    public CanvasGroup menuPanel, helpPanel;
    public CanvasGroup buttonsPanel, settingsPanel;
    public CanvasGroup ioAddressEntry;
    public Transform inputEntriesParent, outputEntriesParent;
    public float menuDuration = 0.1f, helpDuration = 0.5f;
    //public BlurOptimized blurEffect;
    public PlcConnection plc;
    public RectTransform plcToggleButton;

    Vector3 menuInitialScale, helpInitialScale, settingsInitialScale;
    Coroutine menuCoroutine, helpCoroutine;

    bool _menuState = true;
    bool MenuState
    {
        get
        {
            return _menuState;
        }
        set
        {
            if (_menuState != value)
            {
                _menuState = value;
                if (menuCoroutine != null)
                    StopCoroutine(menuCoroutine);
                menuCoroutine = StartCoroutine(Fade(menuPanel, menuInitialScale, value, menuDuration, true));
            }
        }

    }

    bool _helpState = true;
    bool HelpState
    {
        get
        {
            return _helpState;
        }
        set
        {
            if (_helpState != value)
            {
                _helpState = value;
                if (helpCoroutine != null)
                    StopCoroutine(helpCoroutine);
                helpCoroutine = StartCoroutine(Fade(helpPanel, helpInitialScale, value, helpDuration, false));
            }
        }

    }

    bool _settingsState = true;
    bool SettingsState
    {
        get
        {
            return _settingsState;
        }
        set
        {
            if (_settingsState != value)
            {
                _settingsState = value;
                //if (helpCoroutine != null)
                //StopCoroutine(helpCoroutine);
                //helpCoroutine = StartCoroutine(Fade(helpPanel, helpInitialScale, value, helpDuration, false));
                settingsPanel.alpha = value ? 1 : 0;
                settingsPanel.transform.localScale = value ? settingsInitialScale : Vector3.zero;

                // Data handling
                if (value)
                {
                    // Load labels and populate UI
                    float yPos = 0;
                    foreach(PlcIO input in plc.inputs)
                    {
                        CanvasGroup entry = Instantiate(ioAddressEntry, inputEntriesParent, false);
                        entry.GetComponentInChildren<Text>().text = input.label;
                        entry.transform.localPosition = new Vector3(entry.transform.localPosition.x, yPos, entry.transform.localPosition.z);
                        //yPos -= 40;
                    }
                }else
                {

                }
            }
        }
    }


    // Use this for initialization
    void Start () {

        menuInitialScale = menuPanel.transform.localScale;
        helpInitialScale = helpPanel.transform.localScale;
        settingsInitialScale = settingsPanel.transform.localScale;

        //_menuState = menuPanel.alpha != 0.0f;
        //_helpState = helpPanel.alpha != 0.0f;
        SetMenuVisibility(false);
        SetHelpVisibility(false);
        SetSettingsVisibility(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(toggleKey))
            SetMenuVisibility(!MenuState);

        if (Input.GetKeyDown(restartShortcut))
            RestartScene();

    }

    IEnumerator Fade(CanvasGroup panel, Vector3 initialScale, bool state, float duration, bool changeBlur)
    {
        float passedTime = 0;
        float startAlpha = panel.alpha;
        float targetAlpha = state ? 1 : 0;

        float currentDistance = Mathf.Abs(targetAlpha - startAlpha);
        float currentDuration = duration * currentDistance;

        if (!state)
        {
            //if (changeBlur) blurEffect.enabled = state;
        }
        else
        {
            panel.transform.localScale = initialScale;
        }


        while (passedTime <= currentDuration)
        {
            passedTime += Time.deltaTime;

            panel.alpha = Mathf.Lerp(startAlpha, targetAlpha, passedTime / currentDuration);

            yield return new WaitForEndOfFrame();
        }
        if (state)
        {
            //if (changeBlur) blurEffect.enabled = state;
        }
        else { 
            panel.transform.localScale = Vector3.zero;
        }
            panel.alpha = targetAlpha;
    }


    // Onclick methods
    public void ExitApp()
    {
        Application.Quit();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetMenuVisibility(bool visible)
    {
        MenuState = visible;
        if (visible) 
            SetHelpVisibility(false);
    }

    public void SetHelpVisibility(bool visible)
    {
        HelpState = visible;
        if (visible)
            SetMenuVisibility(false);
    }

    public void SetMenuButtonsVisibility(bool visible)
    {
        buttonsPanel.alpha = visible ? 1 : 0;
    }

    public void SetSettingsVisibility(bool visible)
    {
        SettingsState = visible;
        if (visible)
        {
            SetMenuButtonsVisibility(false);
        }
    }

}
