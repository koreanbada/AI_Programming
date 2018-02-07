using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour 
{
    private Vector3 tarPos;

    private float movementSpeed = 5.0f;
    private float rotSpeed = 2.0f;
    private float minX, maxX, minZ, maxZ;

	void Start () 
    {
        minX = -45.0f;
        maxX = 45.0f;

        minZ = -45.0f;
        maxZ = 45.0f;

        //Get Wander Position ���ƴٴ� ��ġ ���
        GetNextPosition();
	}
	
	void Update () 
    {//���� ���� ��ó���� �˻�
        if(Vector3.Distance(tarPos, transform.position) <= 5.0f)//
            GetNextPosition();
        
        //������ ���������� ȸ���� ���� ���ʹϿ� ����
        Quaternion tarRot = Quaternion.LookRotation(tarPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);
        //Slerp ���� ������ �����.
        
        //ȸ���� Ʈ�������̼� ����
        transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
	}

    void GetNextPosition()
    {
        tarPos = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));//-45���� 45��ŭ �̵��Ѵ�. 
    }
}
