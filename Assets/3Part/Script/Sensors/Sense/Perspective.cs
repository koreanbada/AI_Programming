using UnityEngine;
using System.Collections;

public class Perspective : Sense
{
    public int FieldOfView = 45;
    public int ViewDistance = 100;
    private Transform playerTrans;
    private Vector3 rayDirection;

    protected override void Initialise() 
    {//플레이어 위치 찾기
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void UpdateSense() 
    {
        elapsedTime += Time.deltaTime;
        //타이머 elapsedTime

        //검출 범위에 있으면 시각 검사를 수행한다.
        if (elapsedTime >= detectionRate)
            DetectAspect();
	}

    //Detect perspective field of view for the AI Character
    //인공지능 캐릭터에 대한 시야를 검사한다. 
    void DetectAspect()
    {
        RaycastHit hit;
        //현재 위치로부터 플레이어 위치로의 방향
        rayDirection = playerTrans.position - transform.position;
        //인공지능 캐릭터의 전방 벡터와 플레이어와 인공지능 캐릭터 사이의 방향 벡터간의 각도를 검사한다.
        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)//FieldOfView = 45도 각도
        {
            // Detect if player is within the field of view
            //플레이어가 시야에 들어왔는지 검사
            if (Physics.Raycast(transform.position, rayDirection, out hit, ViewDistance))//ViewDistance = 100
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();// 맞은 물체의 Aspect를 가져온다. 
                if (aspect != null) //맞지 않았으면 없다. 
                {
                    //Check the aspect 특성검사
                    if (aspect.aspectName == aspectName)//F12누르면 Sense의 Enemy로 간다. 
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

        //Approximate perspective visualization 대략적인 시야 범위 시각화
        Vector3 leftRayPoint = frontRayPoint;
        leftRayPoint.x += FieldOfView * 0.5f;

        Vector3 rightRayPoint = frontRayPoint;
        rightRayPoint.x -= FieldOfView * 0.5f;

        Debug.DrawLine(transform.position, frontRayPoint, Color.green);
        Debug.DrawLine(transform.position, leftRayPoint, Color.green);
        Debug.DrawLine(transform.position, rightRayPoint, Color.green);
    }
}
