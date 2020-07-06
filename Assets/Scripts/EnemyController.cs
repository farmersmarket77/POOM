using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Animator anim;
    public GameObject bullet;
    public Transform firePoint;
    public AudioClip audioHit;
    public AudioClip audioDead;

    private int health = 3;
    private float playerRange = 10f;
    private float moveSpeed = 1f;
    private float fireRate = 1.5f;
    private float shootCounter;
    private bool shouldShoot;
    private bool isDead;

    private void Start()
    {
        StartCoroutine(Alive());
    }

    private IEnumerator Alive()
    {
        while (true)
        {
            if (isDead)
            {
                shouldShoot = false;
                rigid.velocity = Vector2.zero;
                anim.SetBool("isWalk", false);
                break;
            }

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < playerRange)
            {
                anim.SetBool("isWalk", true);
                Vector3 playerDirection = PlayerController.instance.transform.position - transform.position;

                rigid.velocity = playerDirection.normalized * moveSpeed;
                shouldShoot = true;
                Shoot();
            }
            else
            {
                shouldShoot = false;
                rigid.velocity = Vector2.zero;
                anim.SetBool("isWalk", false);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    //private void Update()
    //{
    //    if (isDead)
    //    {
    //        return;
    //    }

    //    if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < playerRange)
    //    {
    //        anim.SetBool("isWalk", true);
    //        Vector3 playerDirection = PlayerController.instance.transform.position - transform.position;

    //        theRB.velocity = playerDirection.normalized * moveSpeed;
    //        shouldShoot = true;
    //        Shoot();
    //    }
    //    else
    //    {
    //        shouldShoot = false;
    //        theRB.velocity = Vector2.zero;
    //        anim.SetBool("isWalk", false);
    //    }
    //}

    private void Shoot()
    {
        if (shouldShoot)
        {
            shootCounter -= Time.deltaTime;
            if (shootCounter <= 0)
            {
                anim.SetTrigger("Attack");
                
                var bullet = BulletObjectPool.GetBullet();
                bullet.transform.position = firePoint.position;
                shootCounter = fireRate;
            }
        }
    }

    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            isDead = true;
            anim.SetBool("isDead", true);
            anim.SetBool("isWalk", false);
            AudioMaster.instance.PlaySFX(audioDead);
            Destroy(gameObject, 5f);

            transform.GetComponent<CircleCollider2D>().enabled = false;
            transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            anim.SetTrigger("Hit");
            AudioMaster.instance.PlaySFX(audioHit);
        }
    }
}
