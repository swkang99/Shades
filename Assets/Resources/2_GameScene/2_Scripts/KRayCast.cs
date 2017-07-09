using UnityEngine;
using System.Collections;

public class KRayCast : MonoBehaviour {

    public Camera UICam = null;
    public RaycastHit m_Hit;
    public Vector3 ClickPointVec3 = Vector3.zero;

    public RaycastHit GetHitInfo()
    {
        Ray ClickRay = UICam.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Physics.Raycast(ClickRay, out Hit, 10f))
            ClickPointVec3 = Hit.point;

        if (Hit.collider != null)
            return Hit;
        else
            return Hit;
    }
}
