using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Measy.Inventory;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Text valueText;
    [SerializeField] private GameObject bottomPart;
    [Header("����")]
    public GameObject resourcePanle;
    [SerializeField] private Image[] resourceItem;

    public void SetupToolTip(ItemDetails itemDetails,SlotType slotType)
    {
        nameText.text = itemDetails.itemName;
        typeText.text = GetItemType(itemDetails.itemType);
        descriptionText.text = itemDetails.itemDescription;
        if (itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity || itemDetails.itemType == ItemType.Furniture)
        {
            bottomPart.SetActive(true);
            var price = itemDetails.itemPrice;
            if (slotType == SlotType.Bag)
            {
                price = (int)(price * itemDetails.sellPercentage);
            }
            valueText.text = price.ToString();
        }
        else
        {
            bottomPart.SetActive(false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "����",
            ItemType.Commodity => "��Ʒ",
            ItemType.Furniture => "�Ҿ�",
            ItemType.BreakTool => "����",
            ItemType.ChopTool => "����",
            ItemType.CollectTool => "����",
            ItemType.HoeTool => "����",
            ItemType.ReapTool => "����",
            ItemType.WaterTool => "����",
            _ => "��"
        };
    }
    public void SetupResourcePanle(int ID)
    {
        var bluePrintDetails = InventoryManager.Instance.bluPrintData.GetBluePrintDetails(ID);
        for (int i = 0; i < resourceItem.Length; i++)
        {
            if (i < bluePrintDetails.resourceItem.Length)
            {
                Debug.Log("555");
                var item = bluePrintDetails.resourceItem[i];
                resourcePanle.gameObject.SetActive(true);
                resourceItem[i].sprite = InventoryManager.Instance.GetItemDetails(item.itemID).itemIcon;
                resourceItem[i].transform.GetChild(0).GetComponent<Text>().text = item.itemAmount.ToString();
            }
            else
            {
                resourceItem[i].gameObject.SetActive(false);
            }
        }
    }
}
