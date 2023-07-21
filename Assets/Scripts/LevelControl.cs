using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{

    public void NextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel < SceneManager.sceneCountInBuildSettings - 1)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = 0;
            
        }
        SceneManager.LoadScene(currentLevel);
    }
}
