using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
  [SerializeField] GameObject menu;
  [SerializeField] TMP_Text moneyTXT;

  [Header("Cheat Menu")]
  [SerializeField] GameObject cheatBox;
  [SerializeField] TMP_Text invalid;
  //double check this is the right one in PS
  [SerializeField] TMP_InputField inputField;

  [Header("Options Menu")]
  [SerializeField] GameObject optionsBox;

  int money;
  List<string> cheats = new List<string>() { "DiabloTheCheater", "ILU"};
  string given;

  void Start() {
    money = Inventory.Instance.GetFunds();
    menu.SetActive(false);
    optionsBox.SetActive(false);
  }

  void Update() {
    moneyTXT.text = money.ToString();
  }

  public void ButtonMenu() {
    menu.SetActive(true);
  }

  public void ButtonOptions() {
    cheatBox.SetActive(false);
    optionsBox.SetActive(true);
  }

  public void ButtonSubmit() {
    ReadInput();
    CheatCheck();
  }

  public void ButtonOk() {
    menu.SetActive(false);
  }

  public void ButtonTitle() {
    SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
  }

  public void ReadInput() {
    if (!string.IsNullOrEmpty(inputField.text)) {
      given = inputField.text;
      inputField.text = "";
    } else {
      invalid.text = "Please input a code";
    }
  }

  public void CheatCheck() {
    List<bool> cheatUse = Inventory.Instance.GetCheatUse();

    if(given == cheats[0] && !cheatUse[0]) {
      money += 50000;
      Inventory.Instance.SetCheatUse(true, 0);

      invalid.text = "Gained 50000";
    }
    else if(given == cheats[1] && !cheatUse[1]) {
      money += 3000;
      Inventory.Instance.SetCheatUse(true, 1);

      invalid.text = "Gained 3000";
    }
    else if(given != cheats[0] && given != cheats[1]) {
      invalid.text = "Incorrect Value";
    }
    else if((given == cheats[0] && cheatUse[0]) || (given == cheats[1] && !cheatUse[1])) {
      invalid.text = "Code has already been used";
    }

    Inventory.Instance.UpdateFunds(money);
  }
}
