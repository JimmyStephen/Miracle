using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowManager : MonoBehaviour {
  [SerializeField] List<GameObject> posters;
  [SerializeField] List<GameObject> cards;

  List<GameObject> characters = GachaManager.instance.charactersPulled;

  void Start() {
    foreach (var item in posters) {
      item.SetActive(false);
    }
    foreach (var item in cards) {
      item.SetActive(false);
    }
  }

  void Update() {
    ShowCards();
  }

  private void ShowCards() {
    if (!GachaManager.instance.onePull) {
      for (int i = 0; i < characters.Count; i++) {
        cards[i].SetActive(true);
        posters[i].SetActive(true);
        cards[i].GetComponent<Image>().sprite = characters[i].GetComponent<GatchaCard>().GetSprite();
        Inventory.Instance.AddToInventory(characters[i]);
      }
    } else {
      posters[0].SetActive(true);
      cards[0].SetActive(true);
      cards[0].GetComponent<Image>().sprite = characters[0].GetComponent<GatchaCard>().GetSprite();
      Inventory.Instance.AddToInventory(characters[0]);
    }
  }

  public void Exit() {
    SceneManager.LoadScene("GachaShop", LoadSceneMode.Single);
  }
}
