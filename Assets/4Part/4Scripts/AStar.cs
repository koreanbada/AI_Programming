using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상속이 안되어 있다. 
public class AStar {

    public static PriorityQueue closedList, openList;

    //두 노드 사이의 비용을 계산하기 위해 HeuristicEstimateCost 메소드를 구현하자.
    // 하나의 위치 벡터에서 나머지를 빼면 둘 사이의 방향 벡터를 찾을 수 있으며, 이 벡터의 크기가 현재 노드와 목적지 노드 사이의 거리다. 
    private static float HeuristicEstimateCost(Node curNode, Node goalNode)
    {
        Vector3 vecCost = curNode.position - goalNode.position;//현재 노드 Vector3 - 목적지 노드 Vector 3
        return vecCost.magnitude;//vecCost에 담는다. Distance로 대체가능하다. 
    }

    public static ArrayList FindPath(Node start, Node goal)//길을 찾는다.
    {
        openList = new PriorityQueue();
        openList.Push(start);//Push(node)이며 node는 start이다.
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);//estimatedCost는 예상비용

        closedList = new PriorityQueue();
        Node node = null;
        
        //열린 리스트와 닫힌 리스트를 초기화하고 시작 노트부터 시작하면서 이를 열린 리스트를 넣는다. 그런후 열린 리스트를 대상으로 처리를 시작한다. 
        while (openList.Length !=0)
        {
            node = openList.First();

            //현재 노드가 목적지 노드인지 확인한다. 
            if(node.position == goal.position)
            {
                return CalculatePath(node);
            }

            //이웃 노드를 저장하기 위해 ArrayList를 neighbours로 생성한다. 
            ArrayList neighbours = new ArrayList();


            GridManager.instance.GetNeighbours(node, neighbours);// GridManager는 싱글톤, neighbours는 ArrayList.


            //-----------------for 반복문 시작 -------------------//길 찾을때까지 계속 돌린다. 비용이 제일 적은 것으로 한다. While의 For문으로 돌려준다. 
            for (int i = 0; i < neighbours.Count; i++)//for 반복문
            {
                Node neighbourNode = (Node)neighbours[i];//첫번은 0번째.

                if(!closedList.Contains(neighbourNode))//bool값이 true가 아니라면.
                {
                    float cost = HeuristicEstimateCost(node, neighbourNode);
                    float totalCost = node.nodeTotalCost + cost;
                    float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

                    neighbourNode.nodeTotalCost = totalCost;
                    neighbourNode.parent = node;
                    neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Push(neighbourNode);
                    }
                }
            }
            //------------------for 반복문 끝------------------

            //현재 노드를 closedList에 추가한다. 
            closedList.Push(node);
            //그리고 openList에서는 제거한다. 
            openList.Remove(node);
        }

        if(node.position != goal.position)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }
        return CalculatePath(node);
    }

    //1. openList에서 첫 노드를 가져온다. openList의 노드는 항상 정렬된 상태임을 기억하자. 따라서 첫노드는 항상 목적지 노드까지의 추정비용이 가장 적다. 
    //2. 현재 노드가 이미 목적지 노드인지 검사한다. 만일 그렇다면 while 반복문을 탈출하고 path배열을 만든다.
    //3. 현재 노드의 이웃 노드를 저장할 배열 리스트를 생성한다. 격자에서 이웃을 가져오기 위해서는 GetNeighbours 메소드를 사용한다.
    //4. 이웃 배열의 모든 노드에 대해 이미 closedList에 있는지 검사한다. 
    //   만일 closedList에 없다면 비용을 계산하고 노드 속성을 새로 계산한 값으로 부모노드 데이터와 함께 갱신하고 openList에 추가한다.
    //5. 현재 노드를 closedList에 넣고 이를 openList에서 제거한다. 그리고 1단계로 돌아간다. 



    //openList에 더 이상 노드가 없고 유효한 경로가 존재한다면 현재 노드는 대상 노드 위치에 놓인다. 
    //그러면 현재 노드를 매개변수로 CalculatePath 메소드를 호출만 하면 된다. 
    private static ArrayList CalculatePath(Node node)
    {
        ArrayList list = new ArrayList();
        while(node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }
}

    //CalculatePath 메소드는 각 노드의 부모 노드 오브젝트를 추적해 배열 리스트를 만든다. 
    //리스트는 대상 노드로부터 시작 노트까지의 배열 목록이다. 
    //실제 필요한 건 시작 노드부터 대상 노드까지의 경로 배열이므로 Reverse 메소드를 호출하면 된다. 