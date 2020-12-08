using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Platform platform = null;
    [SerializeField] private GameObject platformPrefab = null;

    private List<Platform> platforms = new List<Platform>();

    public Platform GetFirstPlatform
    {
        get { return platforms[0]; }
    }

    private void Awake()
    {
        SpawnOnStart();
    }

    private void SpawnOnStart()
    {
        platforms.Add(platform);
        for (int i = 0; i < 15; i++)
        {
            platforms.Add(Instantiate(platformPrefab, platforms[i].SpawnPoint.position, Quaternion.identity).GetComponent<Platform>());
        }
    }

    internal void SpawnNewPlatform()
    {
        platforms.Add(Instantiate(platformPrefab, platforms[platforms.Count - 1].SpawnPoint.position, Quaternion.identity).GetComponent<Platform>());
    }
}
