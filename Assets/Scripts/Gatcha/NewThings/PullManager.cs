using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PullManager : MonoBehaviour {
  [SerializeField] List<GameObject> objs = new List<GameObject>();
  [SerializeField] List<ParticleSystem> particles;

  Animator light;

  void Start() {
    foreach (var obj in objs) {
      obj.SetActive(false);
    }

    string rarity = Rarity();

    //do different animation for legendary
    if (rarity == "Uncommon") {
      objs[0].SetActive(true);
      light = objs[0].GetComponent<Animator>();
    } else if (rarity == "Common") {
      objs[1].SetActive(true);
      light = objs[1].GetComponent<Animator>();
    } else if (rarity == "Rare") {
      objs[2].SetActive(true);
      light = objs[2].GetComponent<Animator>();
    } else if (rarity == "Legendary") {
      objs[3].SetActive(true);
      light = objs[3].GetComponent<Animator>();
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.tag == "AnimStart") {
      light.SetTrigger("StartAnim");
      particles[0].Play();
      particles[1].Play();
    } else if (other.tag == "ExitScene") {
      SceneManager.LoadScene("PullShow", LoadSceneMode.Single);
    }
  }

  public string Rarity() {
    List<int> list = new List<int>();
    string rarity = "";

    foreach (string str in GachaManager.instance.rarity) {
      switch (str) {
        case "Uncommon":
          list.Add(1);
          break;
        case "Common":
          list.Add(2);
          break;
        case "Rare":
          list.Add(3);
          break;
        case "Legendary":
          list.Add(4);
          break;
      }
    }

    int max = list.Max();

    switch (max) {
      case 1:
        rarity = "Uncommon";
        break;
      case 2:
        rarity = "Common";
        break;
      case 3:
        rarity = "Rare";
        break;
      case 4:
        rarity = "Legendary";
        break;
    }

    return rarity;
  }
}
