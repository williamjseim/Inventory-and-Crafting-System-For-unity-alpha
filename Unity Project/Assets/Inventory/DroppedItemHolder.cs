using UnityEngine;
using UnityEngine.UI;

//put this script on item on the ground so it can be picked up
public class DroppedItemHolder : MonoBehaviour
{
    public ItemClass item;
    public int amount = 1;
    public int id = -1;

    private void Awake()
    {
        id=item.id;
        this.GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}
