using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

    public GameObject ExplosionMobile;

    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag =="BULLET")
        {
            Instantiate(ExplosionMobile, transform.position, Quaternion.identity);
            Destroy(col.gameObject);//총알만 삭제됨
            //Destroy(this.gameObject);//맞은 벽까지 삭제됨
        }
    }
}
