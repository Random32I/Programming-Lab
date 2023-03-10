using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool paused;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] bool controls;
    [SerializeField] bool recording = false;
    [SerializeField] static KeyCode[] controlsKeys = new KeyCode[2] {KeyCode.Space, KeyCode.E};
    [SerializeField] TMP_Text[] controlText = new TMP_Text[2];
    [SerializeField] int controlIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (paused && !controls)
        {
            pauseMenu.SetActive(true);
            controlsMenu.SetActive(false);

        }
        else if (paused && controls)
        {
            pauseMenu.SetActive(false);
            controlsMenu.SetActive(true);
        }
        else if (!paused)
        {
            pauseMenu.SetActive(false);
            controlsMenu.SetActive(false);
        }

        if (recording && paused)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) //use of Enumeration
            {
                if (Input.GetKeyDown(key) && !Input.GetKeyDown(KeyCode.Escape))
                {
                    controlsKeys[controlIndex] = key;
                    controlText[controlIndex].text = key.ToString();
                    recording = false;
                    break;
                }
            }
        }
    }

    public void SetPaused(bool state)
    {
        paused = state;
    }

    public bool GetPaused()
    {
        return paused;
    }

    public void SetControls(bool state)
    {
        controls = state;
    }

    public void SetRecording(bool state)
    {
        recording = state;
    }

    public void SetControlIndex(int index)
    {
        controlIndex = index;
    }

    public KeyCode GetControls(int index)
    {
        return controlsKeys[index];
    }
}
