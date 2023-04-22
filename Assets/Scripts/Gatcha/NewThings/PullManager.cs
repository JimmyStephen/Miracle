using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullManager : MonoBehaviour
{
  [SerializeField] Animator light;

  private void OnTriggerEnter(Collider other) {
    if(other.tag == "AnimStart") {
      light.SetTrigger("StartAnim");
    }
    else if(other.tag == "ExitScene") {
      //load PullShow
    }
  }
}
