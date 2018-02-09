using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue 
{
    private ArrayList nodes = new ArrayList();

    public int Length
    {
        get
        {
            return this.nodes.Count;//this는 생략해도 된다. 
        }
    }

    public bool Contains(object node)
    {
        return this.nodes.Contains(node);
    }

    public Node First()//노드(리스트)값을 첫번째 값을 가져온다.
    {
        if(this.nodes.Count >0 )
        {
            return (Node)this.nodes[0];//0번을 첫번째로 바꿔준다. 
        }
        return null;
    }
            
    public void Push(Node node)//스택에 push가 있다. 스택(Stack)은 후입선출, 큐(Queue)는 선입선출. 
    {
        this.nodes.Add(node);//ArrayList에 밀어넣는다. 배열을 가져올수 있다. 
        this.nodes.Sort();//ArrayList를 정렬한다.(Sorting) 
    }

    public void Remove(Node node)
    {
        this.nodes.Remove(node);
        //리스트를 제거하고 확실하게 정렬한다. 
        this.nodes.Sort();
    }
}
