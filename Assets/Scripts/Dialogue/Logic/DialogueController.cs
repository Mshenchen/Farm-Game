using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Measy.Dialogue 
{
    [RequireComponent(typeof(NPCMovement))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogueController : MonoBehaviour
    {
        private NPCMovement npc => GetComponent<NPCMovement>();
        public UnityEvent OnFinishEvent;
        public List<DialoguePiece> dialogueList = new List<DialoguePiece>();
        private Stack<DialoguePiece> dailogueStack;

        private bool canTalk;
        private bool isTalking;
        private GameObject uiSign;
        private void Awake()
        {
            uiSign = transform.GetChild(1).gameObject;
            FillDialogueStack();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canTalk = !npc.isMoving && npc.interactable;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canTalk = false;
            }
        }

        private void Update()
        {
            uiSign.SetActive(canTalk);

            if (canTalk & Input.GetKeyDown(KeyCode.Space) && !isTalking)
            {
                StartCoroutine(DailogueRoutine());
            }
        }

        /// <summary>
        /// 构建对话堆栈
        /// </summary>
        private void FillDialogueStack()
        {
            dailogueStack = new Stack<DialoguePiece>();
            for (int i = dialogueList.Count - 1; i > -1; i--)
            {
                dialogueList[i].isDone = false;
                dailogueStack.Push(dialogueList[i]);
            }
        }

        private IEnumerator DailogueRoutine()
        {
            isTalking = true;
            if (dailogueStack.TryPop(out DialoguePiece result))
            {
                //传到UI显示对话
                EventHandler.CallShowDialogueEvent(result);
                EventHandler.CallUpdateGameStateEvent(GameState.Pause);
                yield return new WaitUntil(() => result.isDone);
                isTalking = false;
            }
            else
            {
                EventHandler.CallUpdateGameStateEvent(GameState.Gameplay);
                EventHandler.CallShowDialogueEvent(null);
                FillDialogueStack();
                isTalking = false;

                OnFinishEvent?.Invoke();
                if (OnFinishEvent != null)
                {
                    OnFinishEvent.Invoke();
                    canTalk = false;
                }
            }
        }
    }
}


