using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingCard : MonoBehaviour
{
    [SerializeField] private Image output;
    [SerializeField] private TextMeshProUGUI outputName;
    [SerializeField] private TextMeshProUGUI outputText;

    [SerializeField] private Image[] item;
    [SerializeField] private TextMeshProUGUI[] itemText;
    public void SetupCard(ItemClass output,CraftingResources[] resources, int[] resourceArray)
    {
        if(output != null)
        {
            this.output.sprite = output.sprite;
            this.outputName.text = output.name;
            this.outputText.text = output.description;
        }
        for (int i = 0; i < resources.Length; i++)
        {
            item[i].gameObject.SetActive(true);
            item[i].sprite = resources[i].item.sprite;
            itemText[i].text = $"{resourceArray[i]}/{resources[i].amount}";
            if(resourceArray[i] >= resources[i].amount)
            {
                itemText[i].color = Color.green;
            }
            else
            {
                itemText[i].color = Color.red;
            }
        }
    }
}
