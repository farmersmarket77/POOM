using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private SpriteRenderer theSR;

    private void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
        theSR.flipX = true;
    }

    private void Update()
    {
        transform.LookAt(PlayerController.instance.transform.position, -Vector3.forward);
    }
}