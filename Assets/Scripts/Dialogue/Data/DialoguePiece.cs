using UnityEngine;

namespace Measy.Dialogue
{
    [System.Serializable]
    public class DialoguePiece
    {
        [Header("�Ի�����")]
        public Sprite faceImage;
        public bool onLeft;
        public string name;
        [TextArea]
        public string dialogueText;
        public bool hasToPause;
        [HideInInspector] public bool isDone;
    }
}

