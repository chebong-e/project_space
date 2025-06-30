using System.Collections.Generic;
using TMPro;
using UnityEngine;



[System.Serializable]
public class BuildingLevels : SerializableDictionary<int, int[,]> { };
[CreateAssetMenu(fileName = "Player_Infomation", menuName = "Scriptble Object/Player_Infomation")]
public class PlayerInfomation : ScriptableObject
{
    public string userCode; // 유저의 고유 코드 (추후 중복되지 않는 랜덤한 값의 고유 넘버부여)
    public string userID;
    public string userPassword;
    public string userName;
    public int[][] userLocation; // 첫번째 인덱스 : 홈플래닛(0), 콜로니(1~) / (은하 - 성계 - 위치)

    // key값 : 홈플래닛과 콜로니 여부 확인하는 인덱스값
    // value값 : 첫번째 인덱스: tab1~5, 두번째 인덱스: 첫번째 인덱스에 해당하는 건물들의 레벨(순차적)
    public BuildingLevels build_Levels;
    public BuildLevels[] planets;
    

}

[System.Serializable]
public class BuildLevels
{
    public TabWindows[] tabs;
}

[System.Serializable]
public class TabWindows
{
    public int[] lv;
    public Int_Grade[] gradeLv; // tabwindow3,4,5만 활용하도록
}

[System.Serializable]
public class Int_Grade
{
    public int[] lv;
}
