using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string firstLevel;
    public GameObject screenOptions;
    public GameObject killMutants;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void OpenOptions()
    {
        killMutants.SetActive(false);
        screenOptions.SetActive(true);
        
    }

    public void CloseOptions()
    {
        screenOptions.SetActive(false);
        killMutants.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
