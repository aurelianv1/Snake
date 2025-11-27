using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneName;
    // Funcția care va porni jocul
    public void PlayGame()
    {
        // Încarcă scena de joc
        SceneManager.LoadScene(sceneName); 
    }
}
