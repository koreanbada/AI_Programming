using UnityEngine;
using System.Collections;

public class PlayerTank : MonoBehaviour 
{
    public Transform targetTransform;
    //private float movementSpeed, rotSpeed;//������ �� ���
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
        if (Vector3.Distance(transform.position, targetTransform.position) < 5f)//��Ÿ��� 5�� ����
        {
            return;
        }

        Vector3 tarPos = targetTransform.position;//Ÿ���� 5���� ������ ������ ����
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;

        Quaternion tarRot = Quaternion.LookRotation(dirRot);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);
        //���� ������ �ش�. Slerp��ŭ.

        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
        //new Vector3 ��� ��.
	}
}
