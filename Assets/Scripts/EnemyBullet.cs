using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 direction;
    private int damageAmount = 10;
    private float bulletSpeed = 0.2f;

    private void Start()
    {
        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize();
        direction = direction * bulletSpeed;
    }

    private void Update()
    {
        transform.position += direction * bulletSpeed;
    }

    public void InitBullet()
    {
        Invoke("DestroyBullet", 2f);
    }

    private void DestroyBullet()
    {
        BulletObjectPool.ReturnBullet(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.TakeDamage(damageAmount);
            DestroyBullet();
        }
    }
}
