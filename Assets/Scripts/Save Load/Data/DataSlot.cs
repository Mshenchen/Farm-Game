using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Measy.Save
{
    public class DataSlot
    {
        /// <summary>
        /// ��������String��GUID
        /// </summary>
        public Dictionary<string, GameSaveData> dataDict = new Dictionary<string, GameSaveData>();
    }
}

