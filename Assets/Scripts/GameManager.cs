using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public void ChangeScene(int newScene)
    {
        SceneLoader.Instance.LoadScene(newScene);
    }
}
