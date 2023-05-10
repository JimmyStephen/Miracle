using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;



public class GatchaCard : Card
{
    [SerializeField] string ConnectionName = "Unknown";

    public string GetConnectionName() { return ConnectionName; }

    private void Start()
    {
        Init();
    }
}
