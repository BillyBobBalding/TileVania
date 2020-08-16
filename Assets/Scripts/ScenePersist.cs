using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    int previousSceneIndex = 0;

    private void Awake()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        ScenePersist[] persistsArr = FindObjectsOfType<ScenePersist>();

        foreach (var persist in persistsArr)
        {
            if (persist != this)  // if other persist
            {
                // if player died and is still on the same scene
                if (persist.previousSceneIndex == currentSceneIndex)
                {
                    // destroy this just created default persist
                    Destroy(gameObject);

                    return;
                }
                else
                {
                    // destroy previous persist
                    Destroy(persist.gameObject);
                }
            }
        }

        previousSceneIndex = currentSceneIndex;
        DontDestroyOnLoad(gameObject);

    }

    
}
