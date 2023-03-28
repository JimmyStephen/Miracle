using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    //Scenes
    // 0: Intro
    // 1: Main Menu
    // 2: Game
    // 3: Gatcha
    // 4: Inventory
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadScene(int sceneNum)
    {
        StartCoroutine(LoadLevel(sceneNum));
    }

    bool loading = false;
    IEnumerator LoadLevel(int levelIndex)
    {
        if (!loading)
        {
            loading = true;
            //play animation
            transition.SetTrigger("Start");
            //wait for animation to end
            yield return new WaitForSeconds(transitionTime);
            //load scene
            loading = false;
            SceneManager.LoadScene(levelIndex);
        }
    }
}
