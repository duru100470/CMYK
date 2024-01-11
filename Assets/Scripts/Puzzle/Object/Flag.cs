using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MapObject, IObtainable
{

    public void Obtain()
    {
        Debug.Log("Clear!!");
        //TODO: 스테이지 클리어 후 처리
        
        MapModel.RemoveMapObject(this);
    }
}
