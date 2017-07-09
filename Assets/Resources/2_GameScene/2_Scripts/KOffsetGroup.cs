using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KOffsetGroup : MonoBehaviour {

    public List<Transform> OffsetTmList = new List<Transform>();

	// Use this for initialization
	void Start () {
        InitList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void InitList ()
    {
        int nCount = transform.childCount;

        // Offset 개수만큼 반복
        for (int i = 0; i < nCount; i++) 
        {
            Transform Tm = transform.FindChild("KOffset (" + i.ToString() + ")");
            OffsetTmList.Add(Tm);
            // Offset 개수만큼 Tm을 만들고 거기에 KOffset의 Transform을 차례대로 넣은 다음 List에 추가
        }
    }
}
