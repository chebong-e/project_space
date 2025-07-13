using UnityEngine;

public static class PlayerAbilityInfo
{
    private static int GetLevelSpeed(int tabIndex)
    {
        return Build_Manager.instance.scriptable_Group.tab2Groups[tabIndex].level;
    }

    /*private static float Get_HighTechMachine(int abilityIndex)
    {
        return Build_Manager.instance.scriptable_Group.tab2Groups[6].spacilAbility[abilityIndex];
    }*/


    public static int BuildingSpeed => GetLevelSpeed(0);
    public static int ResearchSpeed => GetLevelSpeed(1);
    public static int MakingSpeed => GetLevelSpeed(4);


    public static float GetCalculatedTime(string category, float baseTime, float percentReductionPerLevel = 0.01f)
    {
        float timer = 0f;

        switch (category)
        {
            case "Build":
                // Tab2의 건물 레벨에 따른 능력치
                timer = baseTime * Mathf.Pow(1f - percentReductionPerLevel, BuildingSpeed);

                // 연구 레벨에 따른 능력치 계산
                if (Build_Manager.instance.scriptable_Group.GetTargetListByResearch(0)[1].level > 0)
                {
                    timer = timer * Mathf.Pow(
                        1f - percentReductionPerLevel,
                        Build_Manager.instance.scriptable_Group.GetTargetListByResearch(0)[1].level);
                }

                // 하이테크 레벨에 따른 능력치 계산
                if (Build_Manager.instance.scriptable_Group.tab2Groups[6].level > 0)
                {
                    timer = timer * Mathf.Pow(
                        1f - Build_Manager.instance.scriptable_Group.tab2Groups[6].spacilAbility[0],
                        Build_Manager.instance.scriptable_Group.tab2Groups[6].level);
                }
                break;
            case "Research":
                timer = baseTime * Mathf.Pow(1f - percentReductionPerLevel, ResearchSpeed);

                if (Build_Manager.instance.scriptable_Group.GetTargetListByResearch(0)[2].level > 0)
                {
                    timer = timer * Mathf.Pow(
                        1f - percentReductionPerLevel,
                        Build_Manager.instance.scriptable_Group.GetTargetListByResearch(0)[2].level);
                }

                if (Build_Manager.instance.scriptable_Group.tab2Groups[7].level > 0)
                {
                    timer = timer * Mathf.Pow(
                        1f - (Build_Manager.instance.scriptable_Group.tab2Groups[7].buildAbility / 100f),
                        Build_Manager.instance.scriptable_Group.tab2Groups[7].level);
                }
                break;
            case "Resource":
                /*timer = baseTime * Mathf.Pow(1f - percentReductionPerLevel, level);*/
                break;
            case "Ship":
                timer = baseTime * Mathf.Pow(1f - percentReductionPerLevel, MakingSpeed);

                if (Build_Manager.instance.scriptable_Group.tab2Groups[6].level > 0)
                {
                    timer = timer * Mathf.Pow(
                        1f - Build_Manager.instance.scriptable_Group.tab2Groups[6].spacilAbility[1],
                        Build_Manager.instance.scriptable_Group.tab2Groups[6].level);
                }

                break;
        }
        

        return timer;
    }
}
