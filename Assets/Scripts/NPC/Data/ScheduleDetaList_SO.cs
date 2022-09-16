using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ScheduleDataList_SO",menuName = "NPC Schedule/ScheduleDataList")]
public class ScheduleDetaList_SO : ScriptableObject
{
    public List<ScheduleDetails> scheduleList;
}
