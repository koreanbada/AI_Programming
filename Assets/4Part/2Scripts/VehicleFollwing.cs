using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFollwing : MonoBehaviour {

    public Path path;
    public float speed = 20.0f;
    public float mass = 5.0f;

    public bool isLooping = true;//반복 

    private float curSpeed;//current speed 약자

    private int curPathIndex;//current path index 현재 패스 
    private float pathLength;
    private Vector3 targetPoint;

    Vector3 velocity;

	void Start ()
    {
        pathLength = path.Length;
        curPathIndex = 0;//초기화 0번

        velocity = transform.forward;//Z축으로 방향	
	}
		
	void Update ()
    {
        curSpeed = speed * Time.deltaTime;
        targetPoint = path.GetPoint(curPathIndex);

        if(Vector3.Distance(transform.position, targetPoint) < path.Radius)
            //목적지의 반지름 내에 들어오면 경로의 다음 지점으로 이동
            //다음 포인트가 2보다 작아진다면,
        {
            if (curPathIndex < pathLength - 1) curPathIndex++;
            //6-1 = 5보다 작다면
            else if (isLooping) curPathIndex = 0;
            else return;
        }

        if (curPathIndex >= pathLength) return;//최종 지점에 도착하지 않았다면 계속이동.

        if (curPathIndex >= pathLength - 1 && !isLooping)//경로를 따라 다음 Velocity를 계산.
            velocity += Steer(targetPoint, true);
        else velocity += Steer(targetPoint);

        //속도에 따라 차량 이동.
        transform.position += velocity;
        //원하는 방향(Velocity)으로 차량을 회전
        transform.rotation = Quaternion.LookRotation(velocity);		
	}
    //-------------------------------------------------
    //목적지로 벡터의 방향을 바꾸는 조향 알고리즘
    public Vector3 Steer (Vector3 target, bool bFinalPoint = false)
    {
        //현재 위치에서 목적지 방향으로 방향 벡터를 계산한다. 
        Vector3 desiredVelocity = (target - transform.position);
        float dist = desiredVelocity.magnitude;

        //원하는 Velocity를 정규화
        desiredVelocity.Normalize();

        //속력에 따라 속도를 계산.
        //10보다 작아지면 나눠준다. 
        if (bFinalPoint && dist < 10.0f) desiredVelocity *= (curSpeed * (dist / 10.0f));
        else desiredVelocity *= curSpeed;
        
        //힘 Vector계산
        Vector3 steeringForce = desiredVelocity - velocity;
        Vector3 acceleration = steeringForce / mass;

        return acceleration;
    }
    //-------------------------------------------------
}
