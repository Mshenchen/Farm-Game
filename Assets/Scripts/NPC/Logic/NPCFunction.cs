using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFunction : MonoBehaviour
{
    public InventoryBag_SO shopData;
    private bool isOpen;
    void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            //�رձ���
            CloseShop();
        }   
    }
    public void OpenShop()
    {
        isOpen = true;
        EventHandler.CallBaseBagOpenEvent(SlotType.Shop, shopData);
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
    }
    public void CloseShop()
    {
        isOpen = false;
        EventHandler.CallBaseBagCloseEvent(SlotType.Shop, shopData);
        EventHandler.CallUpdateGameStateEvent(GameState.Gameplay);
    }
}
