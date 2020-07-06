using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{
    private BulletObjectPool() { }
    public static BulletObjectPool instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Initialize(20);
    }

    public GameObject bullet;
    private Queue<EnemyBullet> bulletQueue = new Queue<EnemyBullet>();

    private void Initialize(int _initCount)
    {
        for (int i = 0; i < _initCount; i++)
        {
            bulletQueue.Enqueue(CreateNewBullet());
        }
    }

    private EnemyBullet CreateNewBullet()
    {
        var newObj = Instantiate(bullet).GetComponent<EnemyBullet>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static EnemyBullet GetBullet()
    {
        if (instance.bulletQueue.Count > 0)
        {
            var obj = instance.bulletQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            obj.GetComponent<EnemyBullet>().InitBullet();
            return obj;
        }
        else
        {
            var newObj = instance.CreateNewBullet();
            newObj.gameObject.SetActive(true);
            newObj.GetComponent<EnemyBullet>().InitBullet();
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnBullet(EnemyBullet _obj)
    {
        _obj.gameObject.SetActive(false);
        _obj.transform.SetParent(instance.transform);
        instance.bulletQueue.Enqueue(_obj);
    }
}
