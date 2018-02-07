using UnityEngine;
using System.Collections;

public class TankAi : MonoBehaviour {

    // General state machine variables 범용 상태 기계 변수
    private GameObject player;//플레이어
    private Animator animator;//애니메이터
    private Ray ray;//xray 선
    private RaycastHit hit;//선에 맞음
    private float maxDistanceToCheck = 6.0f;//6미터 감지거리
    private float currentDistance;//현재거리
    private Vector3 checkDirection;//체크좌표값

    // Patrol state variables Patrol 상태 변수
    public Transform pointA;//포인트A
    public Transform pointB;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;//책은 없으나 맨 위에 using UnityEngine.AI를 단다. 또는 앞부분에 단다.  

    private int currentTarget;//현재 타켓
    private float distanceFromTarget;//타켓과의 거리
    private Transform[] waypoints = null;//웨이포인트를 배열로 선언함. 여러개 사용하려고 함

    private void Awake() {//Start보다 먼저 실행
        player = GameObject.FindWithTag("Player");//player 태그를 가진 player를 찾아준다.
        animator = gameObject.GetComponent<Animator>();//탱크에 애니메이터를 달아준다.
        pointA = GameObject.Find("p1").transform;//p1과 p2를 만든다. 
        pointB = GameObject.Find("p2").transform;
        navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        waypoints = new Transform[2]{ pointA, pointB };
        //위와 같다.
        //waypoints = new Transform[2];
        //waypoints = pointA;
        //waypoints = pointB;

        currentTarget = 0;
        navMeshAgent.SetDestination(waypoints[currentTarget].position);
    }

    private void FixedUpdate() {
        //First we check distance from the player 일단 플레이어와의 거리를 검사한다.
        currentDistance = Vector3.Distance(player.transform.position, transform.position);
        animator.SetFloat("distanceFromPlayer", currentDistance);

        //Then we check for visibility 그런 다음 시야에 들어왔는지 확인한다. 
        checkDirection = player.transform.position - transform.position;
        ray = new Ray(transform.position, checkDirection);//위치와 방향이 필요. 내 현재 위치값과 방향을 플레이어쪽으로 쏜다.
        if (Physics.Raycast(ray, out hit, maxDistanceToCheck)) {//레이를 6만큼 쐈을때 맞으면 뺀다.
            if(hit.collider.gameObject == player){//만약 히트가 플레이어라면
                animator.SetBool("isPlayerVisible", true);//공격하는 상황이 된다.
            } else {
                animator.SetBool("isPlayerVisible", false);//공격하지 않는다.
            }
        } else {
            animator.SetBool("isPlayerVisible", false);//6보다 멀때 플레이어를 공격하지 않는다. 
        }

        //Lastly, we get the distance to the next waypoint target
        //마지막으로, 다음 웨이포인트 대상까지의 거리를 구한다. 
        distanceFromTarget = Vector3.Distance(waypoints[currentTarget].position, transform.position);
        animator.SetFloat("distanceFromWaypoint", distanceFromTarget);
    }

    public void SetNextPoint() {
        switch (currentTarget) {
            case 0:
                currentTarget = 1;
                break;
            case 1:
                currentTarget = 0;
                break;
        }
        navMeshAgent.SetDestination(waypoints[currentTarget].position);
    }
}
