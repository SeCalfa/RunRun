using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform sp1 = null;
    [SerializeField] private Transform sp2 = null;

    public bool _random { get; private set; }

    private void Awake()
    {
        _random = Random.Range(0, 2) > 0;
    }

    public Transform SpawnPoint
    {
        get
        {
            if (_random)
                return sp2;
            else
                return sp1;
        }
    }
}
