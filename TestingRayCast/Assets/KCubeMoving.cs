using UnityEngine;
using System.Collections;

public class KCubeMoving : MonoBehaviour {

    public KRayCast KRay;
    public GameObject Cube;
    public Camera UICam = null;
    public float[] fXPosArray = new float[4];
    public Vector3 TempVec3 = Vector3.zero;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < fXPosArray.Length; i++)
           fXPosArray[i] = i * 2;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            KRay.m_Hit = KRay.GetHitInfo();
            if (KRay.m_Hit.collider.tag == "Player")
            {
                int nXIndex = (int)(KRay.ClickPointVec3.x / 2f);
                TempVec3.x = fXPosArray[nXIndex];
                Cube.transform.position = TempVec3;
                Debug.Log(KRay.ClickPointVec3);
            }
        }
    }
}
