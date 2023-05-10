using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PullManager : MonoBehaviour {
  [SerializeField] List<GameObject> objs = new List<GameObject>();
  [SerializeField] List<ParticleSystem> particles;
  [SerializeField] GameObject skip;

  Animator animatorLight;
  List<ParticleSystem> list = new List<ParticleSystem>();

  void Start() {
    skip.SetActive(false);

    foreach (var obj in objs) {
      obj.SetActive(false);
    }

    string rarity = Rarity();

    switch (rarity) {
      case "Uncommon":
        objs[0].SetActive(true);
        animatorLight = objs[0].GetComponent<Animator>();
        list.Add(particles[0]);
        list.Add(particles[3]);
        break;
      case "Common":
        objs[1].SetActive(true);
        animatorLight = objs[1].GetComponent<Animator>();
        list.Add(particles[1]);
        list.Add(particles[4]);
        break;
      case "Rare":
        objs[2].SetActive(true);
        animatorLight = objs[2].GetComponent<Animator>();
        list.Add(particles[0]);
        list.Add(particles[2]);
        list.Add(particles[5]);
        break;
      case "Legendary":
        objs[3].SetActive(true);
        animatorLight = objs[3].GetComponent<Animator>();
        list.Add(particles[0]);
        list.Add(particles[1]);
        list.Add(particles[2]);
        list.Add(particles[6]);
        break;
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.tag == "AnimStart") {
      animatorLight.SetTrigger("StartAnim");
      foreach(var item in list) {
        item.Play();
      }
      skip.SetActive(true);
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

  public void Skip() {
    SceneManager.LoadScene("PullShow", LoadSceneMode.Single);
  }
}
