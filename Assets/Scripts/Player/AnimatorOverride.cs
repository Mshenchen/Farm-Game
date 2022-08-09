using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Measy.Inventory;

public class AnimatorOverride : MonoBehaviour
{
    private Animator[] animators;

    public SpriteRenderer holdItem;

    [Header("各部分动画列表")]
    public List<AnimatorType> animatorTypes;

    private Dictionary<string, Animator> animatorNameDict = new Dictionary<string, Animator>();
    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
        foreach (var anim in animators)
        {
            animatorNameDict.Add(anim.name, anim);
        }
    }
    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforSceneUnloadEvent;
        EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
    }

   

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforSceneUnloadEvent;
        EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
    }

    private void OnHarvestAtPlayerPosition(int ID)
    {
        Sprite itemSprite = InventoryManager.Instance.GetItemDetails(ID).itemOnWorldSprite;
        if (holdItem.enabled == false)
        {
            StartCoroutine(ShowItem(itemSprite));
        }
    }
    private IEnumerator ShowItem(Sprite itemSprite)
    {
        holdItem.sprite = itemSprite;
        holdItem.enabled = true;
        yield return new WaitForSeconds(1f);
        holdItem.enabled = false;
    }
    private void OnBeforSceneUnloadEvent()
    {
        holdItem.enabled = false;
        SwitchAnimator(PartType.None);
    }
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        //WORKFLOW:不同的工具返回不同的动画在这里补全
        PartType currentType = itemDetails.itemType switch
        {
            ItemType.Seed => PartType.Carry,
            ItemType.Commodity => PartType.Carry,
            ItemType.HoeTool => PartType.Hoe,
            ItemType.WaterTool =>PartType.Water,
            ItemType.CollectTool =>PartType.Collect,
            ItemType.ChopTool => PartType.Chap,
            ItemType.BreakTool=>PartType.Break,
            ItemType.ReapTool => PartType.Reap,
            _ => PartType.None
        };
        if (isSelected == false)
        {
            currentType = PartType.None;
            holdItem.enabled = false;
        }
        else
        {
            if (currentType == PartType.Carry)
            {
                holdItem.sprite = itemDetails.itemOnWorldSprite;
                holdItem.enabled = true;
            }
            else
            {
                holdItem.enabled = false;
            }
        }
        SwitchAnimator(currentType);
    }
    private void SwitchAnimator(PartType partType)
    {
        foreach (var item in animatorTypes)
        {
            if (item.partType == partType)
            {
                animatorNameDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }
}
