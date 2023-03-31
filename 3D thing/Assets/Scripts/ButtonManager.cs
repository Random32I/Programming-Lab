using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameManager game;
    [SerializeField] Player player;


    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        game.SetPaused(false);
        Cursor.lockState = CursorLockMode.Locked;
        game.SetControls(false);
        Time.timeScale = 1;
    }

    public void Settings()
    {
        game.SetControls(true);
    }

    public void Back()
    {
        game.SetControls(false);
    }

    public void Jump()
    {
        game.SetRecording(true);
        game.SetControlIndex(0);
    }

    public void Interact()
    {
        game.SetRecording(true);
        game.SetControlIndex(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}