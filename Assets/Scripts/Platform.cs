using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static Material currentMaterial;

    [SerializeField] internal Transform sp1 = null;
    [SerializeField] internal Transform sp2 = null;

    public bool _random { get; private set; }
    public bool isNormalColor { get; private set; } = true; // Normal = purple

    private void Awake()
    {
        GetComponent<MeshRenderer>().material = currentMaterial;

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

    public void DestroyPlatform()
    {
        Destroy(gameObject);
    }
}
