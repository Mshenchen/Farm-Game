using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Measy.Inventory 
{
    public class Box : MonoBehaviour
    {
        public InventoryBag_SO boxBagTemplate;
        public InventoryBag_SO boxBagData;
        public GameObject mouseIcon;
        private bool canOpen;
        private bool isOpen;
        public int index;
        private void OnEnable()
        {
            if (boxBagData == null)
            {
                boxBagData = Instantiate(boxBagTemplate);
            }
            //var key = this.name + index;
            //if (InventoryManager.Instance.GetBoxDataList(key) != null)  //ˢ�µ�ͼ��ȡ����
            //{
            //    boxBagData.itemList = InventoryManager.Instance.GetBoxDataList(key);
            //}
            //else //�½�����
            //{
            //    if (index == 0)
            //    {
            //        index = InventoryManager.Instance.BoxDataAmount;
            //    }
            //    InventoryManager.Instance.AddBoxDataDict(this);
            //}
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canOpen = true;
                mouseIcon.SetActive(true);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canOpen = false;
                mouseIcon.SetActive(false);
            }
        }
        private void Update()
        {
            if (!isOpen && canOpen && Input.GetMouseButtonDown(1))
            {
                //������
                EventHandler.CallBaseBagOpenEvent(SlotType.Box, boxBagData);
                isOpen = true;
            }
            if (!canOpen && isOpen)
            {
                //�ر�����
                EventHandler.CallBaseBagCloseEvent(SlotType.Box, boxBagData);
                isOpen = false;
            }
            if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                //�ر�����
                EventHandler.CallBaseBagCloseEvent(SlotType.Box, boxBagData);
                isOpen = false;
            }
        }
        /// <summary>
        /// ��ʼ��Box������
        /// </summary>
        /// <param name="boxIndex"></param>
        public void InitBox(int boxIndex)
        {
            index = boxIndex;
            var key = this.name + index;
            if (InventoryManager.Instance.GetBoxDataList(key) != null)  //ˢ�µ�ͼ��ȡ����
            {
                boxBagData.itemList = InventoryManager.Instance.GetBoxDataList(key);
            }
            else //�½�����
            {
                InventoryManager.Instance.AddBoxDataDict(this);
            }
        }
    }
}


