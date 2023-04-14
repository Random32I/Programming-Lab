using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using TMPro;
using System.Net;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

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
    [SerializeField] GameObject loadingBar;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] float loadingTime = 0;
    [SerializeField] Interactable[] interactables = new Interactable[5];

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GamePart2")
        {
            loadingTime = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (loadingTime != 2)
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                StartCoroutine(LoadingScreen());
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                loadingScreen.SetActive(false);
            }


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

    IEnumerator LoadingScreen()
    {
        loadingTime = Mathf.Lerp(loadingTime, 2, Time.deltaTime / Mathf.Abs(loadingTime - Random.Range(1,20)));
        loadingBar.transform.localScale = new Vector3(loadingTime, 1, 1);
        yield return new WaitForSeconds(0);
    }

    public List<T> FindAllObjectsOfType<T>() where T : Interactable
    {
        List<T> list = new List<T>();
        foreach(Interactable interactable in interactables)
        {
            if (interactable is T)
            {
                list.Add(interactable as T);
            }
        }

        return list;
    }
}
