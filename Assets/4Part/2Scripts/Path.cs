using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//itween Path와 흡사하다.

public class Path : MonoBehaviour {

    public bool bDebug = true;//디버그인지 확인
    public float Radius = 2.0f;//돌아가는 반경표시
    public Vector3[] pointA;//배열로 처리

    public float Length//굳이 할 필요는 없다. 
    {
        get
        {
            return pointA.Length;//포인트A의 길이, 배열
        }
    }

    public Vector3 GetPoint(int index)//포인트를 가져온다. 좌표값을.
    {
        return pointA[index];//몇번째의 좌표값인지 가져온다. 
    }

    void OnDrawGizmos()//기즈모를 그려준다. 
    {
        if (!bDebug) return;//꺼져 있으면 안그린다.
        for (int i = 0; i < pointA.Length; i++)
        //기즈모 위에 스크립트가 생성된다. 
        //위의 기즈모를 체크해제 해도 보여진다. 
        //스크립트의 Path를 체크해제하면 안보이지만 돌아다닌다. 
        {
            if (i + 1 <pointA.Length)//1회전 6개포인트 지남.
            {
                Debug.DrawLine(pointA[i], pointA[i+1], Color.red);//빨강색으로 그려줌
            }
        }
    }
}