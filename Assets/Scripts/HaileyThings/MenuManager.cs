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
  List<string> cheats;

  void Start() {
    money = Inventory.Instance.GetFunds();
    menu.SetActive(false);
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
    //do cheats things
  }

  public void ButtonOk() {
    menu.SetActive(false);
  }

  public void ButtonTitle() {
    SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
  }
}
