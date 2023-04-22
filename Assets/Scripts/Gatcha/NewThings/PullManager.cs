using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PullManager : MonoBehaviour
{
  [SerializeField] List<GameObject> objs = new List<GameObject>();

  Animator light;

  void Start() {
    foreach (var obj in objs) {
      obj.SetActive(false);
    }

    if(GachaManager.instance.rarity == "Uncommon") {
      objs[0].SetActive(true);
      light = objs[0].GetComponent<Animator>();
    }
    else if(GachaManager.instance.rarity == "Common") {
      objs[1].SetActive(true);
      light = objs[1].GetComponent<Animator>();
    }
    else if(GachaManager.instance.rarity == "Rare") {
      objs[2].SetActive(true);
      light = objs[2].GetComponent<Animator>();
    }
    else if(GachaManager.instance.rarity == "Legendary") {
      objs[3].SetActive(true);
      light = objs[3].GetComponent<Animator>();
    }
  }

  private void OnTriggerEnter(Collider other) {
    if(other.tag == "AnimStart") {
      light.SetTrigger("StartAnim");
    }
    else if(other.tag == "ExitScene") {
      //load PullShow
      SceneManager.LoadScene("TestShop", LoadSceneMode.Single);
    }
  }
}
