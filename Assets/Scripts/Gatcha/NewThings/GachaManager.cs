using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour {

  string active = "Diablo";

  void Start() {

  }

  void Update() {

  }

  public void PullSingle() {
    if(Inventory.Instance.GetFunds() >= 300) {
      Inventory.Instance.UpdateFunds(-300);

      Gacha();
    }
    else {
      Debug.Log("You too Poor ):");
    }
  }

  public void PullTen() {
    if (Inventory.Instance.GetFunds() >= 3000) {
      Inventory.Instance.UpdateFunds(-3000);

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
        //GetCharacter(legendary);
        Debug.Log("Legendary");
        break;
      case 20:
        //GetCharacter(rare);
        Debug.Log("Rare");
        break;
      case 25:
        //GetCharacter(common);
        Debug.Log("Common");
        break;
      case 40:
        //GetCharacter(Uncommon);
        Debug.Log("Uncommon");
        break;
    }
  }
}
