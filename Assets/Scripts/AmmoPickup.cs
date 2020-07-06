using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 25;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.currentAmmo += ammoAmount;
            PlayerController.instance.UpdateAmmoUI();
            AudioMaster.instance.PlaySFX(pickupSound);
            Destroy(gameObject);
        }
    }
}
