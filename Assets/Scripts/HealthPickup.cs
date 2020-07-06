using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 25;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.AddHealth(healthAmount);
            AudioMaster.instance.PlaySFX(pickupSound);
            Destroy(gameObject);
        }
    }
}
