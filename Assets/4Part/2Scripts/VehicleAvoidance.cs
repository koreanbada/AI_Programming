using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAvoidance : MonoBehaviour {

    public float speed = 20.0f;
    public float mass = 5.0f;
    public float force = 50.0f;
    public float minimumDistToAvoid = 20.0f;//피하기 위한 속도

    private float curSpeed;//차량의 실제 속도
    private Vector3 targetPoint;

	void Start ()
    {
        mass = 5.0f;
        targetPoint = Vector3.zero;
	}

    void OnGUI()
    {
        GUILayout.Label("플레이어를 이동하려면 아무곳이나 클릭하세요.");
            //("Click anywhere to move the vehicle");        
    }

    void Update ()
    {    // 차량은 마우스 클릭으로 이동한다. 
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay
            (Input.mousePosition);

        if(Input.GetMouseButtonDown(0) && 
            Physics.Raycast(ray, out hit, 100.0f))
        {
            targetPoint = hit.point;
        }
        //목표지점을 향하는 방향 벡터
        Vector3 dir = (targetPoint - transform.position);
        dir.Normalize();

        AvoidObstacles(ref dir);//장애물 회피적용

        if(Vector3.Distance(targetPoint, 
            transform.position) < 3.0f)
        {
            return;
        }
        curSpeed = speed * Time.deltaTime;

        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp
            (transform.rotation, rot, 5.0f * 
            Time.deltaTime);

        transform.position += transform.forward 
            * curSpeed;
	}

    public void AvoidObstacles (ref Vector3 dir)//장애물 회피를 위해 새 방향 벡터를 계산
    {
        RaycastHit hit;

        int layerMask = 1 << 8;//레이어 8(Obstacles) 만 검사

        //회피 최소거리 이내에서 장애물과 차량이 충돌했는지 검사 수행
        if (Physics.Raycast(transform.position, transform.forward, out hit, minimumDistToAvoid, layerMask))
           
        {
            Vector3 hitNormal = hit.normal;//새 방향을 계산하기 위해 충돌 지점에서 법선을 구한다. 
            hitNormal.y = 0.0f;//Don't want to move in Y-Space

            dir = transform.forward + hitNormal * force;//차량의 현재 전방 벡터에 force를 더해 새로운 방향 벡터를 얻는다. 
        }
    }
}
