using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]  //  MonoBehaviour가 아닌 클래스에 대해 Inspector에 나타내기.
public class TileInfo
{
	public Transform TileTm = null;
	public HTileInfo TInfo = null;
}

public class HTileGroup : MonoBehaviour {

	public List<TileInfo> TInfoList = null;

	public float fTileMoveSpeed = 50f;

	// Use this for initialization
	void Start () {
		AddTileInfoIntoList ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AddTileInfoIntoList()
	{
		TInfoList = new List<TileInfo> ();

		int nCnt = transform.childCount;
		HGameMng.I.nTilesCnt = nCnt; 

		for (int i = 0; i < nCnt; i++) {
			TileInfo Info = new TileInfo ();
			Info.TileTm = transform.FindChild ("HTile (" + i.ToString () + ")");
			Info.TInfo = Info.TileTm.GetComponent<HTileInfo> ();
			TInfoList.Add (Info);
		}
	}

	void OnDestroy()
	{
		TInfoList.Clear ();
	}
}
