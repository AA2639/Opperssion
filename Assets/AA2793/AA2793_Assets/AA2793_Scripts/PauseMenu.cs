using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject optionsScreen, pauseScreen;


    bool escapePressed;
    private bool isPaused;

    bool cursorIsLocked = true;

    public string mainMenuScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        EnterMenu();
    }

    public void PauseUnpause()
    {
        
        if (!isPaused)
        {
            pauseScreen.SetActive(true);
            isPaused = true;

            Time.timeScale = 0f;

        }
        else
        {
            pauseScreen.SetActive(false);
            isPaused = false;
            escapePressed = false;
            cursorIsLocked = true;

            Time.timeScale = 1f;
        }
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }

    public void QuitToMain()
    {
        SceneManager.LoadScene(mainMenuScene);

        Time.timeScale = 1f;
    }

    //Escape button
    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            escapePressed = true;
        }
        else
        {
            escapePressed = false;
        }
    }

    //Changes here
    public void EnterMenu()
    {        
        if (escapePressed)
        {
            cursorIsLocked = false;
            PauseUnpause();
        }
        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
