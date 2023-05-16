using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//TODO check for doubles

public class GachaManager : MonoBehaviour {
  [SerializeField] GameObject yesNo;
  [SerializeField] GameObject x300;
  [SerializeField] GameObject x3000;
  [SerializeField] GameObject single;
  [SerializeField] GameObject multiple;
  [SerializeField] TMP_Text moneyTXT;

  [SerializeField] GameObject funds;

  [SerializeField] Button pull1;
  [SerializeField] Button pull10;

  public static GachaManager instance;

  public string active = "Diablo";
  public bool onePull = false;
  public List<string> rarity;
  public List<GameObject> charactersPulled;

  GameObject[] gachaCharacters = GameManager.Instance.GetGatchaCards_GameObjects();

  /*  List<int> activePool;
    List<int> standardPool;
    List<int> ruinartPool;
    List<int> diabloPool;*/

  int money;

  void Start() {
    instance = this;
    yesNo.SetActive(false);
    x3000.SetActive(false);
    x300.SetActive(false);
    single.SetActive(false);
    multiple.SetActive(false);
    funds.SetActive(false);

    money = Inventory.Instance.GetFunds();
    charactersPulled = new List<GameObject>();
  }

  void Update() {
    moneyTXT.text = money.ToString();

    /*    switch (active) {
          case "Diablo":
            activePool = diabloPool;
            break;
          case "Ruinart":
            activePool = ruinartPool;
            break;
          case "Standard":
            activePool = standardPool;
            break;
        }*/
  }

  private void SetSingle() {
    x300.SetActive(true);
    single.SetActive(true);

    x3000.SetActive(false);
    multiple.SetActive(false);

    yesNo.SetActive(true);
  }

  private void SetMultiple() {
    x300.SetActive(false);
    single.SetActive(false);

    x3000.SetActive(true);
    multiple.SetActive(true);

    yesNo.SetActive(true);
  }

  private void ClearSettings() {
    yesNo.SetActive(false);
    x3000.SetActive(false);
    x300.SetActive(false);
    single.SetActive(false);
    multiple.SetActive(false);
  }

  public void PullSingle() {
    SetSingle();

    onePull = true;
  }

  public void PullTen() {
    SetMultiple();
  }

  public void Confirm() {
    if (onePull) {
      if (money >= 300) {
        money -= 300;
        Inventory.Instance.AddFunds(-300);
        Gacha();
        onePull = true;
        Inventory.Instance.AddPity(1);
        SceneManager.LoadScene("PullScene", LoadSceneMode.Single);
      } else {
        funds.SetActive(true);

        ClearSettings();
      }
    } else {
      if (money >= 3000) {
        money -= 3000;
        Inventory.Instance.AddFunds(-3000);
        int gachas = 10;

        while (gachas > 0) {
          Gacha();
          gachas--;
        }

        Inventory.Instance.AddPity(10);

        SceneManager.LoadScene("PullScene", LoadSceneMode.Single);
      } else {
        funds.SetActive(true);

        ClearSettings();
      }
    }
  }

  public void Deny() {
    ClearSettings();

    onePull = false;
  }

  public void Ok() {
    funds.SetActive(false);
  }

  public void ChangeBanner(string banner) {
    active = banner;
  }

  //populate the pools
  public void OrganizeGacha() {

  }

  private void Gacha() {

    //Redo to have less chance for higher things
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

    //double check this
    if (Inventory.Instance.GetPity() == 50) {
      award = 5;
    }

    switch (award) {
      case 5:
        rarity.Add("Legendary");
        GetCharacter("Legendary");
        break;
      case 20:
        rarity.Add("Rare");
        GetCharacter("Rare");
        break;
      case 25:
        rarity.Add("Common");
        GetCharacter("Common");
        break;
      case 40:
        rarity.Add("Uncommon");
        GetCharacter("Uncommon");
        break;
    }
  }

  private void GetCharacter(string rarity) {
    int ran = 0;

    switch (rarity) {
      case "Uncommon":
        ran = RandomNum(0, 12);
        break;
      case "Common":
        ran = RandomNum(13, 20);
        break;
      case "Rare":
        ran = RandomNum(21, 27);
        break;
      case "Legendary":
        ran = RandomNum(28, 33);
        break;
    }
    var AlreadyOwned = Inventory.Instance.GetInventory();

    foreach (var item in AlreadyOwned) {
      if (gachaCharacters[ran] == item) {
        money += 50;
      }
    }

    Debug.Log("Pulled Character: " + ran);
    charactersPulled.Add(gachaCharacters[ran]);

    Debug.Log("Name: " + gachaCharacters[ran].name);
  }

  private int RandomNum(int min, int max) {
    return Random.Range(min, max);
  }
}
