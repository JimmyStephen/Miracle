using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour {
  void Start() {

  }

  void Update() {

  }

  public void Gacha() {

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
        //get legendary character
        break;
      case 20:
        //get rare character
        break;
      case 25:
        //get common character
        break;
      case 40:
        //get uncommon character
        break;
    }
  }

  public void GetCharacter() {

  }
}
