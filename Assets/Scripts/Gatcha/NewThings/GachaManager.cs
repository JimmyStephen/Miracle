using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GachaManager : MonoBehaviour {

  public static GachaManager instance;

  int money = 5000;

  string active = "Diablo";
  bool onePull = false;
  public string rarity;

  void Start() {
    instance = this;
  }

  void Update() {

  }

  public void PullSingle() {
    if(money >= 300) {
      money -= 300;

      Gacha();
      onePull = true;
      //go to pull screen
      SceneManager.LoadScene("PullScene", LoadSceneMode.Single);
    }
    else {
      Debug.Log("You too Poor ):");
    }
  }

  public void PullTen() {
    if (money >= 3000) {
      money -= 3000;

      int gachas = 10;

      while(gachas > 0) {
        Gacha();
        gachas--;
      }
    } else {
      Debug.Log("You too Poor ):");
    }
  }

  private void Gacha() {

    //redo for 4 rates
    int[] rates = {
      40,
      25,
      20,
      5
    };

    int total = 0;

    foreach (var item in rates) {
      total += item;
    }

    int ranNum = Random.Range(0, total);
    int award = 0;

    foreach (var weight in rates) {
      if (ranNum <= weight) {
        award = weight;
        break;
      } else {
        ranNum -= weight;
      }
    }

    switch (award) {
      case 5:
        rarity = "Legendary";
        break;
      case 20:
        rarity = "Rare";
        break;
      case 25:
        rarity = "Common";
        break;
      case 40:
        rarity = "Uncommon";
        break;
    }
  }
}
