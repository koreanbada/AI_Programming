using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bugs : MonoBehaviour {

    public float moveSpeed;
    public float rotSpeed;
    public float senseLength;
    public float turnCool;
    public float turnDelay;

    public enum BUGSTATE
    {
        IDLE,
        LEFT,
        RIGHT,
        FORWARD
    }
    public BUGSTATE bugState;

    void Update()
    {
        RaycastHit hit;//1번
        if (Physics.Raycast(transform.position, transform.forward, out hit, senseLength))
         { 
            if(hit.transform == null)
            {
                bugState = BUGSTATE.FORWARD;
            }

            if (hit.transform.tag == "Wall" && bugState != BUGSTATE.LEFT && bugState != BUGSTATE.RIGHT)
            {//벽에 부딪히거나 오른쪽, 왼쪽일때 
                int ranState = Random.Range(0, 2);
                switch (ranState)
                {
                    case 0:
                        bugState = BUGSTATE.LEFT;
                        break;
                    case 1:
                        bugState = BUGSTATE.RIGHT;
                        break;
                }
            }
           // else
           // {
           //     bugState = BUGSTATE.FORWARD;
           // }
        }
        
        switch (bugState)
        {
            case BUGSTATE.IDLE:
                break;

            case BUGSTATE.LEFT:
                transform.Rotate(0, -rotSpeed, 0);
                turnCool += Time.deltaTime;
                if(turnCool > turnDelay)
                {
                    bugState = BUGSTATE.FORWARD;
                    turnCool = 0;
                }
                break;

            case BUGSTATE.RIGHT:
                transform.Rotate(0, rotSpeed, 0);
                turnCool += Time.deltaTime;
                if (turnCool > turnDelay)
                {
                    bugState = BUGSTATE.FORWARD;
                    turnCool = 0;
                }
                break;

            case BUGSTATE.FORWARD:
                turnCool = 0;
                transform.Translate(0, 0, moveSpeed * Time.deltaTime);
                break;
        }
	}
}
