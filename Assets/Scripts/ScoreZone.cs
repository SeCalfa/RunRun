using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    [SerializeField] private bool isStarterBlock = false;
    [SerializeField] private GameObject platform = null;

    public GameObject GetPlatform { get { return platform; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isStarterBlock)
        {
            Player.player.GetSpawner.SpawnNewPlatform();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            platform.GetComponent<Animation>().clip = platform.GetComponent<Animation>().GetClip("PlatformDisappear");
            platform.GetComponent<Animation>().Play();
        }
    }
}
