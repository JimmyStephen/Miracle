using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GachaManager : MonoBehaviour {

  public static GachaManager instance;

  public string active = "Diablo";
  public bool onePull = false;
  public List<string> rarity;

  int money = 5000;

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

      SceneManager.LoadScene("PullScene", LoadSceneMode.Single);
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
        //rarity = "Legendary";
        rarity.Add("Legendary");
        break;
      case 20:
        //rarity = "Rare";
        rarity.Add("Rare");
        break;
      case 25:
        //rarity = "Common";
        rarity.Add("Common");
        break;
      case 40:
        //rarity = "Uncommon";
        rarity.Add("Uncommon");
        break;
    }
  }
}
