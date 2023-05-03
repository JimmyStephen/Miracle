using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowManager : MonoBehaviour {
  [SerializeField] List<GameObject> posters;
  [SerializeField] List<GameObject> cards;

  void Start() {
    foreach (var item in posters) {
      item.SetActive(false);
    }
    foreach (var item in cards) {
      item.SetActive(false);
    }
  }

  void Update() {
    if (!GachaManager.instance.onePull) {
      foreach (var item in posters) {
        item.SetActive(true);
      }
      foreach(var item in cards) {
        item.SetActive(true);
      }
    } else {
      posters[0].SetActive(true);
      cards[0].SetActive(true);
    }
  }

  public void Exit() {
    SceneManager.LoadScene("TestShop", LoadSceneMode.Single);
  }
}
