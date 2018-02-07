using UnityEngine;
using System.Collections;

public class PlayerTank : MonoBehaviour 
{
    public Transform targetTransform;
    //private float movementSpeed, rotSpeed;//좀비할 때 사용
    private float movementSpeed;
    private float rotSpeed;

    void Start () 
    {
        movementSpeed = 10.0f;
        rotSpeed = 2.0f;
        targetTransform = GameObject.Find("Target").transform;
	}
	
	void Update () 
    {
        if (Vector3.Distance(transform.position, targetTransform.position) < 5f)//사거리를 5로 잡음
        {
            return;
        }

        Vector3 tarPos = targetTransform.position;//타겟이 5보다 작으면 동작이 안함
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;

        Quaternion tarRot = Quaternion.LookRotation(dirRot);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);
        //선형 보간해 준다. Slerp만큼.

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
        //new Vector3 없어도 됨.
	}
}
