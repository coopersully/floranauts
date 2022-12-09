using System;
using UnityEngine;

public class PrefSaver : MonoBehaviour
{
    public static PrefSaver Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

}
