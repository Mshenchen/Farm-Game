using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Settings 
{
    public const float itemFadeDuration = 0.35f;
    public const float targetAlpha = 0.45f;

    //时间相关
    public const float secondThreshold = 0.1f;  //数值越小时间越快
    public const int secondHold = 59;
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 10;
    public const int seasonHold = 3;
    public const float fadeDuration = 1.5f; //transition
    public const int reapAmount = 3;
    public const float gridCellSize = 1;
    public const float gridCellDiagonalSize = 1.41f;
    public const float pixelSize = 0.05f;   //20*20占 1 unit
    public const float animationBreakTime = 5f;
    public const int maxGridSize = 9998;
    public const float lightChangeDuration = 25f;
    public static TimeSpan morningTime = new TimeSpan(5, 0, 0);
    public static TimeSpan nightTime = new TimeSpan(9, 0, 0);
    public static Vector3 playerStartPos = new Vector3(2.5f, -11.6f,0);
    public const int playerStartMoney = 100;
}
