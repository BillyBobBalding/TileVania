using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    //public ScenePersist scenePersist;
    //public Player player;  // hook up in inspector (Player Gameobject)

    void OnTriggerEnter2D(Collider2D other)
    {
        Time.timeScale = 0;  // > 1 would be slow motion, < 1 is speed up
        
        //player.StopPlayerMovement();
        //other.GetComponent<Player>().StopPlayerMovement();

        StartCoroutine(LoadNextLevel());
    }
    
    IEnumerator LoadNextLevel()
    {
        // realtime not affected by timeScale
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        //Destroy(FindObjectOfType<ScenePersist>());
        //scenePersist.DestroyOwn();

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);

        Time.timeScale = 1f;
    }
}
