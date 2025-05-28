using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectButtons : MonoBehaviour
{
    public GameObject weaponDetail;

    private void OnMouseOver()
    {
        Debug.Log("MousedOver");
        weaponDetail.transform.position = transform.position - Vector3.forward;
    }
}
