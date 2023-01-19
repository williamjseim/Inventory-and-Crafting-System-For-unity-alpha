using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeable", menuName = "Inventory/Item/Placeable")]
public class PlaceableItemClass : ItemClass
{
    [SerializeField] private GameObject placableObjectPrefab;
    public override void Use(PlayerController playerController)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = 0;
        Instantiate(placableObjectPrefab, point, new Quaternion());
    }
}
