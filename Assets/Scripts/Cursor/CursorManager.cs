using Measy.CropPlant;
using UnityEngine;
using Measy.Inventory;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Measy.Map;
using System.Collections;

public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed, item;

    private Sprite currentSprite;   //存储当前鼠标图片
    private Image cursorImage;
    private RectTransform cursorCanvas;
    //建造图标跟随
    private Image buildImage;
    private Camera mainCamera;
    private Grid currentGrid;
    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;
    private bool cursorEnable;
    private bool cursorPositionValid;
    private ItemDetails currentItem;
    private Transform PlayerTransform => FindObjectOfType<Player>().transform;
    private void OnEnable()
    {
        
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        //拿到建造图标
        buildImage = cursorCanvas.GetChild(1).GetComponent<Image>();
        buildImage.gameObject.SetActive(false);
        currentSprite = normal;
        SetCursorImage(normal);
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (cursorCanvas == null) return;

        cursorImage.transform.position = Input.mousePosition;

        if (!InteractWithUI()&&cursorEnable)
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
            CheckPlayerInput();
        }
        else
        {
            SetCursorImage(normal);
            buildImage.gameObject.SetActive(false);
        }
    }
    private void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0) && cursorPositionValid)
        {
            EventHandler.CallMouseClickedEvent(mouseWorldPos, currentItem);
        }
    }
    private void OnBeforeSceneUnloadEvent()
    {
        cursorEnable = false;
    }
    private void OnAfterSceneLoadedEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
    }
    /// <summary>
    /// 设置鼠标图片
    /// </summary>
    /// <param name="sprite"></param>
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    private void SetCursorValid()
    {
        cursorPositionValid = true;
        cursorImage.color = new Color(1, 1, 1, 1);
        buildImage.color = new Color(1, 1, 1, 0.5f);
    }
    private void SetCursorInValid()
    {
        cursorPositionValid = false;
        cursorImage.color = new Color(1, 0, 0, 0.4f);
        buildImage.color = new Color(1, 0, 0, 0.5f);
    }
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelect)
    {
        if (!isSelect)
        {
            currentItem = null;
            cursorEnable = false;
            currentSprite = normal;
            buildImage.gameObject.SetActive(false);
        }
        else    //物品被选中才切换图片
        {
            currentItem = itemDetails;
            
            //WORKFLOW:添加所有类型对应图片
            currentSprite = itemDetails.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.ChopTool => tool,
                ItemType.HoeTool => tool,
                ItemType.WaterTool => tool,
                ItemType.BreakTool => tool,
                ItemType.ReapTool => tool,
                ItemType.Furniture => tool,
                ItemType.CollectTool =>tool,
                _ => normal,
            };
            cursorEnable = true;
            //显示建造物品图片
            if(itemDetails.itemType == ItemType.Furniture)
            {
                buildImage.gameObject.SetActive(true);
                buildImage.sprite = itemDetails.itemOnWorldSprite;
                //StartCoroutine(FrameDelay());
                //IEnumerator FrameDelay()
                //{
                //    yield return null;
                //    buildImage.SetNativeSize();
                //}
                
                buildImage.SetNativeSize();
                float width = buildImage.GetComponent<RectTransform>().sizeDelta.x;
                float height = buildImage.GetComponent<RectTransform>().sizeDelta.y;
                buildImage.GetComponent<RectTransform>().sizeDelta = new Vector2(width / 5, height / 5);
            }
        }
    }
    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        var playerGridPos = currentGrid.WorldToCell(PlayerTransform.position);
        //建造图片跟随移动
        buildImage.rectTransform.position = Input.mousePosition;
        if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.itemUseRadius || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.itemUseRadius)
        {
            SetCursorInValid();
            return;
        }
        print(1);
        TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);
        if (currentItem != null)
        {
            CropDetails currentCrop = CropManager.Instance.GetCropDetails(currentTile.seedItemID);
            Crop crop = GridMapManager.Instance.GetCropObject(mouseWorldPos);
            switch (currentItem.itemType)
            {
                case ItemType.Seed:
                    if (currentTile.daysSinceDug > -1 && currentTile.seedItemID == -1) SetCursorValid(); else SetCursorInValid();
                    break;
                case ItemType.Commodity:
                    if (currentTile.canDropItem&&currentItem.canDropped) SetCursorValid(); else SetCursorInValid();
                    break;
                case ItemType.HoeTool:
                    if (currentTile.canDig) SetCursorValid(); else SetCursorInValid();
                    break;
                case ItemType.WaterTool:
                    if (currentTile.daysSinceDug > -1 && currentTile.daysSinceWatered == -1) SetCursorValid(); else SetCursorInValid();
                    break;
                case ItemType.BreakTool:
                case ItemType.ChopTool:
                    if (crop != null)
                    {
                        if (crop.CanHarvest&&crop.cropDetails.CheckToolAvailable(currentItem.itemID)) SetCursorValid(); else SetCursorInValid();
                    }
                    else
                        SetCursorInValid();
                    break;
                case ItemType.CollectTool:
                    if (currentCrop != null)
                    {
                        if(currentCrop.CheckToolAvailable(currentItem.itemID))
                            if (currentTile.growthDays >= currentCrop.TotalGrowthDays) SetCursorValid(); else SetCursorInValid();
                    }
                    else
                        SetCursorInValid();
                    break;
                case ItemType.ReapTool:
                    if (GridMapManager.Instance.HaveReapableItemsInRadius(mouseWorldPos,currentItem)) SetCursorValid(); else SetCursorInValid();
                    break;
                case ItemType.Furniture:
                    buildImage.gameObject.SetActive(true);    //需要添加此命令
                    var bluePrintDetails = InventoryManager.Instance.bluPrintData.GetBluePrintDetails(currentItem.itemID);
                    if (currentTile.canPlaceFurniture && InventoryManager.Instance.CheckStock(currentItem.itemID)&&!HaveFurnitureInRadius(bluePrintDetails))
                        SetCursorValid();
                    else
                        SetCursorInValid();
                    break;
            }
        }
        else
        {
            SetCursorInValid();
        }
    }
    private bool HaveFurnitureInRadius(BluePrintDetails bluePrintDetails)
    {
        var buildItem = bluePrintDetails.buildPrefab;
        Vector2 point = mouseWorldPos;
        var size = buildItem.GetComponent<BoxCollider2D>().size;
        var otherColl = Physics2D.OverlapBox(point, size, 0);
        if(otherColl!=null)
            return otherColl.GetComponent<Funiture>();
        return false;
    }
    // <summary>
    /// 是否与UI互动
    /// </summary>
    /// <returns></returns>
    private bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
}
