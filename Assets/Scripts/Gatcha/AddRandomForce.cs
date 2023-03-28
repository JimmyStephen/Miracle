using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AddRandomForce : MonoBehaviour
{
    [SerializeField] float speed = 100;
    void Start()
    {
        Vector2 force = new(Random.Range(0, 1.0f) * speed, Random.Range(0, 1.0f) * speed);
        GetComponent<Rigidbody2D>().AddForce(force);
    }
}
