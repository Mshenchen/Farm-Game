using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    public LightPattenList_SO lightData;
    private Light2D currentLight;
    private LightDetails currentLightDetails;
    private void Awake()
    {
        currentLight = GetComponent<Light2D>();
    }
    //Êµ¼ÊÇÐ»»µÆ¹â
    public void ChangeLightShift(Season season, LightShift lightShift, float timeDifference)
    {
        currentLightDetails = lightData.GetLightDetails(season, lightShift);
        if (timeDifference < Settings.lightChangeDuration)
        {
            var colorOffset = (currentLightDetails.lightColor - currentLight.color) / Settings.lightChangeDuration;
            currentLight.color += colorOffset;
            DOTween.To(() => currentLight.color, c => currentLight.color = c, currentLightDetails.lightColor,200f);
            DOTween.To(() => currentLight.intensity, i => currentLight.intensity = i, currentLightDetails.lightAmount,200f);
        }
        if (timeDifference >= Settings.lightChangeDuration)
        {
            
            currentLight.color = currentLightDetails.lightColor;
            currentLight.intensity = currentLightDetails.lightAmount;
        }
    }
}
