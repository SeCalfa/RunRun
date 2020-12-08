using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    [SerializeField] private bool isStarterBlock = false;
    [SerializeField] private GameObject platform = null;

    private float _speed = 4;
    private bool _isPlatformDown = false;

    private void FixedUpdate()
    {
        if (_isPlatformDown)
            platform.transform.Translate(-platform.transform.up * _speed * Time.fixedDeltaTime);
    }

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
            _isPlatformDown = true;
        }
    }
}
