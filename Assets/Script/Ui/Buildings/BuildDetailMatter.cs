using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

[System.Serializable]
public class values : SerializableDictionary<string, GameObject> { };
public class BuildDetailMatter : MonoBehaviour
{
    public Infomations infos;
    public float buildTimer;
    public bool ccBuild;
    public float targetTimer;

    void Start()
    {
        infos = transform.GetChild(2).GetComponent<Infomations>();
    }

    public void TimeSlide()
    {
        buildTimer = 0f;
        targetTimer = infos.buildResource.building_Time[0];

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        infos.slider.maxValue = targetTimer;

        while (buildTimer < targetTimer)
        {
            buildTimer += Time.deltaTime;
            float remainingTime = Mathf.Clamp(targetTimer - buildTimer, 0f, targetTimer);
            infos.slider.value = buildTimer;

            int curTime = Mathf.FloorToInt(remainingTime);

            string timeStr = "";

            if (curTime >= 3600)
            {
                int hours = curTime / 3600;
                int minutes = (curTime % 3600) / 60;
                int seconds = curTime % 60;
                timeStr = string.Format("{0:D2}h {1:D2}m {2:D2}s", hours, minutes, seconds);
            }
            else if (curTime >= 60)
            {
                int minutes = curTime / 60;
                int seconds = curTime % 60;
                timeStr = string.Format("{0:D2}m {1:D2}s", minutes, seconds);
            }
            else
            {
                timeStr = string.Format("{0:D2}s", curTime);
            }

            infos.timeText.text = timeStr;

            yield return null;

        }
    }

}
