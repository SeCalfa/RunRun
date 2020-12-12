using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Platform platform = null;
    [SerializeField] private GameObject platformPrefab = null;
    [SerializeField] private Material choosenPlatformMaterial = null, normalPlatformMaterial = null;

    internal List<Platform> platforms = new List<Platform>();
    internal bool isPlatformSpawningAfterStart = false;
    internal bool isDirectionCanChange = true; // Changing direction of spawn after white line
    private int platformNumber = 0;

    public bool SpawnDirection { get; private set; }
    public Material GetWhiteMat { get { return choosenPlatformMaterial; } }
    public Material GetNormalMat { get { return normalPlatformMaterial; } }

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
        Platform.currentMaterial = normalPlatformMaterial;
        platforms.Add(platform);
        for (int i = 0; i < 5; i++)
        {
            platforms.Add(Instantiate(platformPrefab, platforms[i].SpawnPoint.position, Quaternion.identity).GetComponent<Platform>());
        }
    }

    internal void SpawnNewPlatform()
    {
        platformNumber += 1;
        if(platformNumber % 10 == 0 && !Player.player.isTapModeActive)
        {
            int rand = Random.Range(0, 100);
            if(rand > 40)
            {
                Platform.currentMaterial = choosenPlatformMaterial;
                Player.player.isTapModeActive = true;
                isPlatformSpawningAfterStart = true;
                SpawnDirection = platforms[platforms.Count - 1]._random;
                isDirectionCanChange = false;
            }
        }
        else if (platformNumber % 10 == 0 && Player.player.isTapModeActive)
        {
            Platform.currentMaterial = normalPlatformMaterial;
            Player.player.isTapModeActive = false;
            isPlatformSpawningAfterStart = false;
        }

        if (Player.player.isTapModeActive)
        {
            if (SpawnDirection)
                platforms.Add(Instantiate(platformPrefab, platforms[platforms.Count - 1].sp1.position, Quaternion.identity).GetComponent<Platform>());
            else
                platforms.Add(Instantiate(platformPrefab, platforms[platforms.Count - 1].sp2.position, Quaternion.identity).GetComponent<Platform>());
        }
        else
        {
            if (!isDirectionCanChange) // Adding 1 purple platform after white line
            {
                if (SpawnDirection)
                    platforms.Add(Instantiate(platformPrefab, platforms[platforms.Count - 1].sp1.position, Quaternion.identity).GetComponent<Platform>());
                else
                    platforms.Add(Instantiate(platformPrefab, platforms[platforms.Count - 1].sp2.position, Quaternion.identity).GetComponent<Platform>());

                isDirectionCanChange = true;
            }
            else
                platforms.Add(Instantiate(platformPrefab, platforms[platforms.Count - 1].SpawnPoint.position, Quaternion.identity).GetComponent<Platform>());
        }
    }
}
