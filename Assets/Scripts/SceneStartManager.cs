using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] bool PlayMusic = true;
    [SerializeField] bool StopMusic = false;
    [SerializeField, Tooltip("-1 for no audio")] int MusicIndex = -1;
    [SerializeField] AudioSource exitSceneAudio;
    [Header("Display")]
    [SerializeField] TMPro.TMP_Text fundsDisplay;

    private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.Instance;
        if (PlayMusic) AudioManager.Instance.PlayAudio(MusicIndex);
        if (StopMusic) AudioManager.Instance.StopAudio();
        if (fundsDisplay != null) fundsDisplay.text = inventory.GetFunds().ToString();
    }

    private void Update()
    {
        if (fundsDisplay != null) fundsDisplay.text = inventory.GetFunds().ToString();
    }

    public void ChangeScene(int index)
    {
        if (exitSceneAudio != null)
        {
            exitSceneAudio.Play();
        }
        GameManager.Instance.ChangeScene(index);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
