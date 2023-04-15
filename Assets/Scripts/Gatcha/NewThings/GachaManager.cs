using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour {
  void Start() {

  }

  void Update() {

  }

  public void PullSingle() {
    //check if player has enough money
    //if enough money remove money
    //Gacha and show whats given
    //else
    //show that player needs money

  }

  public void PullTen() {
    //SinglePull x10
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

    int ranNum = UnityEngine.Random.Range(0, total);
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

  //Get a list of cards of the rarity
  private void GetCharacter(List<string> list) {
    //get a random number between 0 and the list.Count
    //get character associated within the count of that list

    int count = 0;

    //add character to inventory
  }
}
