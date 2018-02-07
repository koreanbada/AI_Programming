using UnityEngine;
using System.Collections;

public class Perspective : Sense
{
    public int FieldOfView = 45;
    public int ViewDistance = 100;
    private Transform playerTrans;
    private Vector3 rayDirection;

    protected override void Initialise() 
    {//�÷��̾� ��ġ ã��
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void UpdateSense() 
    {
        elapsedTime += Time.deltaTime;
        //Ÿ�̸� elapsedTime

        //���� ������ ������ �ð� �˻縦 �����Ѵ�.
        if (elapsedTime >= detectionRate)
            DetectAspect();
	}

    //Detect perspective field of view for the AI Character
    //�ΰ����� ĳ���Ϳ� ���� �þ߸� �˻��Ѵ�. 
    void DetectAspect()
    {
        RaycastHit hit;
        //���� ��ġ�κ��� �÷��̾� ��ġ���� ����
        rayDirection = playerTrans.position - transform.position;
        //�ΰ����� ĳ������ ���� ���Ϳ� �÷��̾�� �ΰ����� ĳ���� ������ ���� ���Ͱ��� ������ �˻��Ѵ�.
        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)//FieldOfView = 45�� ����
        {
            // Detect if player is within the field of view
            //�÷��̾ �þ߿� ���Դ��� �˻�
            if (Physics.Raycast(transform.position, rayDirection, out hit, ViewDistance))//ViewDistance = 100
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();// ���� ��ü�� Aspect�� �����´�. 
                if (aspect != null) //���� �ʾ����� ����. 
                {
                    //Check the aspect Ư���˻�
                    if (aspect.aspectName == aspectName)//F12������ Sense�� Enemy�� ����. 
                    {
                        print("Enemy Detected");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Show Debug Grids and obstacles inside the editor
    /// </summary>
    void OnDrawGizmos()
    {
        if (playerTrans == null)
            return;

        Debug.DrawLine(transform.position, playerTrans.position, Color.red);

        Vector3 frontRayPoint = transform.position + (transform.forward * ViewDistance);

        //Approximate perspective visualization �뷫���� �þ� ���� �ð�ȭ
        Vector3 leftRayPoint = frontRayPoint;
        leftRayPoint.x += FieldOfView * 0.5f;

        Vector3 rightRayPoint = frontRayPoint;
        rightRayPoint.x -= FieldOfView * 0.5f;

        Debug.DrawLine(transform.position, frontRayPoint, Color.green);
        Debug.DrawLine(transform.position, leftRayPoint, Color.green);
        Debug.DrawLine(transform.position, rightRayPoint, Color.green);
    }
}
