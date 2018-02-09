using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    //TestCode 클래스는 AStar 클래스를 사용해서 시작 노드부터 목적지 노드까지의 경로를 찾는다. 

    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    public ArrayList pathArray;

    GameObject objStartCube, objEndCube;
    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f;

    //일단 참조해야 하는 변수를 설정하자. pathArray는 AStar의 FindPath 메소드가 반환하는 노드 배열을 저장하는데 사용한다. 

    void Start()
    {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");

        pathArray = new ArrayList();
        FindPath();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;//1초마다 타이머 호출한다.
        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
        }
    }
    //Start 메소드에서는 Start와 End 태그를 가진 오브젝트를 찾으며 pathArray 배열도 초기화한다. 
    //시작과 도착 노드의 위치가 변경된 경우 intervalTime 속성에 정의한 주기마다 새로운 경로를 탐색한다. 그런 다음 FindPath 메소드를 호출한다. 

    void FindPath()
    {
        startPos = objStartCube.transform;
        endPos = objEndCube.transform;

        startNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        goalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));

        pathArray = AStar.FindPath(startNode, goalNode);
    }
    //경로탐색 알고리즘을 AStar 클래스에 구현한 덕분에 이제 경로탐색이 한결 쉬워졌다. 
    //일단 시작과 도착 게임 오브젝트를 가져온 후 GridManager, GetGridIndex 메소드를 사용해서 격자내에서의 각각의 행과 열의 인덱스를 계산하고 이를 사용해서 새로운 Node 오브젝트를 생성한다. 
    //이후 시작 노드와 도착 노드 정보를 가지고 AStar.FindPath메소드를 호출한 후 결과로 반환된 배열 목록을 지역 변수 pathArray 에 저장한다. 
    //다음으로 찾은 경로를 시각적으로 보여주기 위한 OnDrawGizmos 메소드를 구현하자. 

    void OnDrawGizmos()//Update처럼 작용한다.
    {
        if (pathArray == null)
            return;

        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)//pathArray가 배열이므로 foreach를 돌린다. 
            {
                if (index < pathArray.Count)//1보다 작다면.
                    // 내가 목적지까지 가기 위한 칸 숫자이다.
                {
                    Node nextNode = (Node)pathArray[index];//index는 1.

                    //Debug.DrawLine메소드는 pathArray 배열의 정보를 순회하며 각 노드를 연결하는 선을 그린다. 실행하면 시작부터 도착지점까지의 각 노드를 연결하는 녹색선을 볼 수 있다. 

                    Debug.DrawLine(node.position, nextNode.position, Color.green);//현재와 다음위치에 그린색을 그린다.
                    index++;
                }
            }
        }
    }
}
