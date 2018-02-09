
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{

    public GameObject bullet;
    public Transform firePos;

    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }
    void Fire()
    {
        StartCoroutine(this.CreateBullet());
    }
    IEnumerator CreateBullet() { 
        Instantiate(bullet, firePos.position, firePos.rotation);
        yield return null;
    }
}