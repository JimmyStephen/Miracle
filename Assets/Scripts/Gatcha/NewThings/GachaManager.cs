using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour {
  [SerializeField] GameObject yesNo;
  [SerializeField] GameObject x300;
  [SerializeField] GameObject x3000;
  [SerializeField] GameObject single;
  [SerializeField] GameObject multiple;

  [SerializeField] GameObject funds;

  [SerializeField] Button pull1;
  [SerializeField] Button pull10;

  public static GachaManager instance;

  public string active = "Diablo";
  public bool onePull = false;
  public List<string> rarity;

  List<int> standardPool;
  List<int> diabloPool;
  List<int> ruinartPool;

  int money = 0;

  void Start() {
    instance = this;
    yesNo.SetActive(false);
    x3000.SetActive(false);
    x300.SetActive(false);
    single.SetActive(false);
    multiple.SetActive(false);
    funds.SetActive(false);
  }

  void Update() {

  }

  public void PullSingle() {
    x300.SetActive(true);
    single.SetActive(true);
    yesNo.SetActive(true);

    x3000.SetActive(false);
    multiple.SetActive(false);

    onePull = true;
  }

  public void PullTen() {
    x3000.SetActive(true);
    multiple.SetActive(true);
    yesNo.SetActive(true);

    x300.SetActive(false);
    single.SetActive(false);
  }

  public void Confirm() {
    if(onePull) {
      if (money >= 300) {
        money -= 300;

        Gacha();
        onePull = true;
        Inventory.Instance.AddPity(1);

        SceneManager.LoadScene("PullScene", LoadSceneMode.Single);
      } else {
        funds.SetActive(true);

        yesNo.SetActive(false);
        x300.SetActive(false);
        single.SetActive(false);
      }
    }
    else {
      if (money >= 3000) {
        money -= 3000;

        int gachas = 10;

        while (gachas > 0) {
          Gacha();
          gachas--;
        }

        Inventory.Instance.AddPity(10);

        SceneManager.LoadScene("PullScene", LoadSceneMode.Single);
      } else {
        funds.SetActive(true);

        yesNo.SetActive(false);
        x3000.SetActive(false);
        multiple.SetActive(false);
      }
    }
  }

  public void Deny() {
    yesNo.SetActive(false);
    x3000.SetActive(false);
    x300.SetActive(false);
    single.SetActive(false);
    multiple.SetActive(false);
    onePull = false;
  }

  public void Ok() {
    funds.SetActive(false);
  }

  public void Exit() {

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
        rarity.Add("Legendary");
        break;
      case 20:
        rarity.Add("Rare");
        break;
      case 25:
        rarity.Add("Common");
        break;
      case 40:
        rarity.Add("Uncommon");
        break;
    }
  }
}
