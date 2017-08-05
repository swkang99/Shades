using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class HTileOffsetGroup : MonoBehaviour {

	public List<Transform> OffsetTmList = null;

	// Use this for initialization
	void Start () {
		
		AddOffsetTmIntoList ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AddOffsetTmIntoList()
	{
		OffsetTmList = new List<Transform>();

		int nCnt = transform.childCount;

		for(int i=0; i<nCnt; i++)
		{
			Transform Tm = transform.FindChild("HOffset ("+i.ToString()+")");
			OffsetTmList.Add(Tm);
		}
	}


	public void CreateTile()
	{
		int nCnt = HGameMng.I.nTilesCnt;
		int nOffstIndex = HGameMng.I.GetOffsetRandomIndex ();

		for (int i = 0; i < nCnt; i++) 
		{
			if (HGameMng.I.TGroupSc.TInfoList[i].TInfo.bUse == false) 
			{
				HGameMng.I.TGroupSc.TInfoList [i].TInfo.bUse = true;
				HGameMng.I.TGroupSc.TInfoList[i].TInfo.E_TSTATE = E_TILE_STATE.E_DOWN;
				HGameMng.I.TGroupSc.TInfoList [i].TileTm.position = OffsetTmList [nOffstIndex].position;

				int nTileType = HGameMng.I.GetRandomIndex ();

				HGameMng.I.TGroupSc.TInfoList[i].TInfo.UserSprite.spriteName = HGameMng.I.GetSpriteName (nTileType);
				HGameMng.I.TGroupSc.TInfoList[i].TInfo.nTiletype = nTileType;
				HGameMng.I.TGroupSc.TInfoList[i].TInfo.CalcDestPos ();
				break;
			}
		}

	}
}
