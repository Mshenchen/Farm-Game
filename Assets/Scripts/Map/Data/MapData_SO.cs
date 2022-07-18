using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="MapData_SO",menuName = "Map/MapData")]
public class MapData_SO : ScriptableObject
{
    public string sceneName;
    public List<TilePropert> tileProperts;
}
