
public static class PlayerAbilityInfo
{
    private static float GetLevelSpeed(int tabIndex)
    {
        return Build_Manager.instance.scriptable_Group.tab2Groups[tabIndex].level * 0.01f;
    }

    private static float Get_HighTechMachine(int abilityIndex)
    {
        return Build_Manager.instance.scriptable_Group.tab2Groups[6].spacilAbility[abilityIndex];
    }

    public static float BuildingSpeed => GetLevelSpeed(0);
    public static float ResearchSpeed => GetLevelSpeed(1);
    public static float MakingSpeed => GetLevelSpeed(4);

    public static float SpacialBuildingSpeed => Get_HighTechMachine(0);
    public static float SpacialShipMakingSpeed => Get_HighTechMachine(1);

}
