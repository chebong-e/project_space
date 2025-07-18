using UnityEngine;

public static class PlayerAbilityInfo
{
    private static int GetLevelSpeed(int tabIndex)
    {
        return Build_Manager.instance.scriptable_Group.tab2Groups[tabIndex].level;
    }

    private static int GetMakingResourceLevel(int index)
    {
        return Build_Manager.instance.scriptable_Group.tab3Groups[0].researches[index + 4].level;
    }

    private static int GetMiningSpeed(int index)
    {
        int lv = Build_Manager.instance.scriptable_Group.tab3Groups[0].researches[index].level;
        if (lv > 0 && lv < 10)
        {

        }

        return lv;
    }


    public static int BuildingSpeed => GetLevelSpeed(0);
    public static int ResearchSpeed => GetLevelSpeed(1);
    public static int MakingSpeed => GetLevelSpeed(4);
    public static int MiningSpeed => GetMiningSpeed(7);
    public static int ResourceMakingSpeed(int index) => GetMakingResourceLevel(index);
    


    // �Ϲݰǹ� �� ������ ���� �ð� ���� ���ʽ� ��� ����
    public static float GetCalculatedTime(string category, float baseTime, float percentReductionPerLevel = 0.01f)
    {
        float timer = 0f;

        switch (category)
        {
            case "Build":
                // Pow(1f - ���ҵ� ��ġ, ���ҵ� ����)
                // Tab2�� �ǹ� ������ ���� �ɷ�ġ
                timer = baseTime * Mathf.Pow(1f - percentReductionPerLevel, BuildingSpeed);

                // ���� ������ ���� �ɷ�ġ ���
                if (Build_Manager.instance.scriptable_Group.GetTargetListByResearch(0)[1].level > 0)
                {
                    timer = timer * Mathf.Pow(
                        1f - percentReductionPerLevel,
                        Build_Manager.instance.scriptable_Group.GetTargetListByResearch(0)[1].level);
                }

                // ������ũ ������ ���� �ɷ�ġ ���
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

    
    /*public static float Electricity_Production()
    {

        float e_Bonus = Build_Manager.instance.scriptable_Group.tab3Groups[0].researches[0].level * 0.01f;


    }*/


}
