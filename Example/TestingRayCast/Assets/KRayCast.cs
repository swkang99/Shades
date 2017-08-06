using UnityEngine;
using System.Collections;

public class KRayCast : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Camera UICam = null;
    public RaycastHit m_Hit;
    public Vector3 ClickPointVec3 = Vector3.zero;

    public RaycastHit GetHitInfo()
    {
        Ray ClickRay = UICam.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Physics.Raycast(ClickRay, out Hit, 1000f))
            ClickPointVec3 = Hit.point;

        if (Hit.collider != null)
            return Hit;
        else
            return Hit;
    }

    //public Camera UICam = null;
    //public RaycastHit2D m_Hit2D;

    //public RaycastHit2D GetHitInfo()
    //{
    //    Vector2 ClickPosVec2 = UICam.ScreenToWorldPoint(Input.mousePosition);

    //    Ray2D ClickRay = new Ray2D(ClickPosVec2, Vector2.zero);
    //    RaycastHit2D Hit2D = Physics2D.Raycast(ClickRay.origin, ClickRay.direction);

    //    if (Hit2D.collider != null)
    //        return Hit2D;
    //    else
    //        return Hit2D;
    //}
}
