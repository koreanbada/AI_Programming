using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    private static GridManager s_Instance = null;

    public static GridManager instance
    {
        get
        {
            if(s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GridManager)) as GridManager;
                if(s_Instance == null)
                {
                    Debug.Log("Could not locate a GrideManager" + "object. \n You have to have exactly" + "one GridManager in the scene");
                }
            }
            return s_Instance;
        }
    }

    public int numOfRows;
    public int numOfColumns;
    public float gridCellSize;
    public bool showGrid = true;
    public bool showObstacleBlocks = true;

    private Vector3 origin = new Vector3();
    private GameObject[] obstacleList;
    public Node[,] nodes { get; set; }
    public Vector3 Origin
    {
        get
        {
            return origin;
        }
    }

    void Awake()
    {
        obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
        CalculateObstacles();
    }

    //맵상의 모든 장애물을 찾는다. 
    void CalculateObstacles()
    {
        nodes = new Node[numOfColumns, numOfRows];
        int index = 0;
        for(int i=0; i < numOfColumns; i++)
        {
            for (int j = 0; j < numOfColumns; j++)
            {
                Vector3 cellPos = GetGridCellCenter(index);
                Node node = new global::Node(cellPos);
                nodes[i, j] = node;
                index++;
            }
        }

        if(obstacleList != null && obstacleList.Length > 0)
        {
            // 맵에서 발견한 각 장애물을 리스트에 기록한다. 
            foreach(GameObject data in obstacleList)
            {
                int indexCell = GetGridIndex(data.transform.position);
                int col = GetColumn(indexCell);
                int row = GetRow(indexCell);
                nodes[row, col].MarkAsObstacle();
            }
        }
    }

    //GetGridCellCenter메소드는 격자 셀의 위치를 월드좌표 기준으로 반환한다. 
    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 cellPosition = GetGridCellPosition(index);
        cellPosition.x += (gridCellSize / 2.0f);
        cellPosition.z += (gridCellSize / 2.0f);
        return cellPosition;
    }

    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float zPosInGrid = row * gridCellSize;
        return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
    }

    //GetGridIndex메소드는 주어진 좌표로붙터 격자 셀 인덱스를 반환한다. 
    public int GetGridIndex(Vector3 pos)
    {
        if(!IsInBounds(pos))
        {
            return -1;
        }
        pos -= Origin;
        int col = (int)(pos.x / gridCellSize);
        int row = (int)(pos.z / gridCellSize);
        return (row * numOfColumns + col);
    }

    public bool IsInBounds(Vector3 pos)
    {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;
        return (pos.x >= Origin.x && pos.x <= Origin.x + width &&
            pos.x <= Origin.z + height && pos.z >= Origin.z);
    }

    //GetRow와 GetColumn 메소드는 주어진 인덱스로부터 격자 셀의 열과 행 데이터를 반환한다. 
    public int GetRow(int index)
    {
        int row = index / numOfColumns;
        return row;
    }
    public int GetColumn(int index)
    {
        int col = index % numOfColumns;
        return col;
    }

    // GetNeighbours 메소드는 AStar 클래스에서 이 메소드를 사용해서 특정노드의 이웃 노드들을 구한다. 
    public void GetNeighbours(Node node, ArrayList neighbors)
    {
        Vector3 neighborPos = node.position;
        int neighborIndex = GetGridIndex(neighborPos);

        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);
        //아래
        int leftNodeRow = row - 1;
        int leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //위
        leftNodeRow = row + 1;
        leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //오른쪽
        leftNodeRow = row;
        leftNodeColumn = column + 1; 
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //왼쪽
        leftNodeRow = row;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
    }

     void AssignNeighbour(int row, int column, ArrayList neighbors)
    {
        if(row != -1 && column != -1 && row < numOfRows && column < numOfColumns)
        {
            Node nodeToAdd = nodes[row, column];
            if(!nodeToAdd.bObstacle)
            {
                neighbors.Add(nodeToAdd);
            }
        }
    }
    //일단 현재 노드의 왼쪽과 오른쪽, 위, 아래 4방향에 있는 이웃 노드를 가져오며, AssignNeighbour 메소드 내부에서 장애물인지 검사한다. 
    //만일 장애물이 아니면 해당 이웃 노드를 참조배열 목록인 neighbors에 추가한다. 

    //다음은 디버그를 돕는 OnDrawGizmos 메소드는 격자와 장애물 블록을 시각화 한다. 
    void OnDrawGizmos()
    {
        if(showGrid)
        {
            DebugDrawGrid(transform.position, numOfRows, numOfColumns, gridCellSize, Color.blue);
        }
        Gizmos.DrawSphere(transform.position, 0.5f);
        if(showObstacleBlocks)
        {
            Vector3 cellSize = new Vector3(gridCellSize, 1.0f, gridCellSize);
            if(obstacleList != null && obstacleList.Length > 0)
            {
                foreach(GameObject data in obstacleList)
                {
                    Gizmos.DrawCube(GetGridCellCenter(
                        GetGridIndex(data.transform.position)), cellSize);
                }
            }
        }
    }

    public void DebugDrawGrid(Vector3 origin, int numRows, int numCols, float cellSize, Color color)
    {
        float width = (numCols * cellSize);
        float height = (numRows * cellSize);
        // 수평 격자 라인을 그린다. 
        for (int i = 0; i < numRows + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
        // 수직 격자 라인을 그린다.
        for (int i = 0; i < numCols + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f, 1.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
    }
}
// 기즈모를 사용하면 편집기 씬 뷰에서 시각적 디버깅과 설정 도움을 받을 수 있다. 
//OnDrawGizmos는 엔진에서 매 프레임을 호출한다. 
//따라서, 디버그 플래그인 showGrid와 showObstacleBlocks가 선택된 상태면 선으로 격자를 그리고 큐브오브젝트를 정육면체로 그린다. 
// DebugDrawGrid 메소드는 매우 간단하므로 설명은 생략한다. 