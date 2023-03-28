using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] toSpawn;
    [SerializeField] int minNum = 4;
    [SerializeField] int maxNum = 10;

    void Start()
    {
        int numSpawn = Random.Range(minNum, maxNum);
        while(numSpawn >= 0)
        {
            Instantiate(toSpawn[Random.Range(0, toSpawn.Length)], transform);
            numSpawn--;
        }
    }
}
