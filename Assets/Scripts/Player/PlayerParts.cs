using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
    Melee = 0,
    Bow = 1,
    Staff = 2
}

public struct WeaponInfo
{
    public string info;
    public WeaponType weapontype;
    public float attackPower;
    public float skillPower;
    public float attackRange; // 0 ~ 25
    public float skillRange;
    public float cooltime; // skill cooltime
    public WeaponInfo(string _info, WeaponType _weapontype, float _attackPower, float _skillPower, float _attackRange, float _skillRange, float _cooltime)
    {
        info = _info;
        weapontype = _weapontype;
        attackPower = _attackPower;
        skillPower = _skillPower;
        attackRange = _attackRange;
        skillRange = _skillRange;
        cooltime = _cooltime;
    }
}

public struct ArmourInfo
{
    public string info;
    public float extrahp;
    public ArmourInfo(string _info, float _extrahp)
    {
        info = _info;
        extrahp = _extrahp;
    }
}

public class PlayerParts : MonoBehaviour
{
    // path names
    string rootPath = "Assets/Resources/SPUM/SPUM_Sprites/";
    string helmetPath1 = "Items/4_Helmet/";
    string helmetPath2 = "Packages/Ver121/4_Helmet/";
    string helmetPath3 = "Packages/Ver300/4_Helmet/";
    string armorPath1 = "Items/5_Armor/";
    string armorPath2 = "Packages/Ver121/5_Armor/";
    string armorPath3 = "Packages/Ver300/5_Armor/";
    string pantsPath1 = "Items/3_Pant/";
    string pantsPath2 = "Packages/Ver300/3_Pant/";
    string weaponPath1 = "Items/6_Weapons/";
    string weaponPath2 = "Packages/Ver121/6_Weapons/";
    string weaponPath3 = "Packages/Ver300/6_Weapons/";

    // item names
    string[] helmet1 = { "Helmet_1.png", "Helmet_2.png", "Helmet_3.png", "Helmet_4.png", "Helmet_5.png", "Helmet_6.png", "Helmet_7.png", "Helmet_8.png", "Helmet_9.png" };
    string[] helmet2 = { "Normal_Helmet1.png", "Normal_Helmet2.png", "Normal_Helmet3.png" };
    string[] helmet3 = { "New_Helmet_01.png", "New_Helmet_02.png", "New_Helmet_03.png", "New_Helmet_04.png", "New_Helmet_05.png", "New_Helmet_06.png", "New_Helmet_07.png", "New_Helmet_08.png", "New_Helmet_09.png", "New_Helmet_10.png" };
    string[] armor1 = { "Armor_1.png", "Armor_2.png", "Armor_3.png", "Armor_4.png", "Armor_5.png", "Armor_6.png", "Armor_7.png", "Armor_8.png" };
    string[] armor2 = { "Normal_Armor1.png" };
    string[] armor3 = { "New_Armor_01.png", "New_Armor_02.png", "New_Armor_03.png", "New_Armor_04.png", "New_Armor_05.png", "New_Armor_06.png", "New_Armor_07.png", "New_Armor_08.png", "New_Armor_09.png", "New_Armor_10.png", "New_Armor_12.png" };
    string[] pants1 = { "Foot_1.png", "Foot_2.png", "Foot_3.png", "Foot_4.png" };
    string[] pants2 = { "New_Pant_01.png", "New_Pant_02.png", "New_Pant_03.png", "New_Pant_04.png", "New_Pant_05.png", "New_Pant_06.png", "New_Pant_07.png", "New_Pant_08.png", "New_Pant_09.png", "New_Pant_10.png", "New_Pant_11.png", "New_Pant_12.png" };

    // weapon & shield names
    string[] melee1 = { "Axe_1.png", "Soon_Spear.png", "Spear_1.png", "Sword_1.png", "Sword_2.png", "Sword_3.png", "Sword_4.png", "Sword_5.png", "Sword_6.png" };
    string[] melee2 = { "AxeLong1.png", "AxeNormal1.png", "AxeSmall1.png" };
    string[] melee3 = { "New_Weapon_01.png", "New_weapon_02.png", "New_Weapon_04.png", "New_Weapon_05.png", "New_Weapon_06.png", "New_Weapon_07.png", "New_Weapon_08.png", "New_Weapon_09.png", "New_Weapon_11.png", "New_Weapon_13.png", "New_Weapon_14.png", "New_Weapon_16.png", "New_Weapon_17.png", "New_Weapon_19.png", "New_Weapon_20.png" };
    string[] bow1 = { "Bow_1.png" };
    string[] bow3 = { "New_Weapon_10.png", "New_Weapon_12.png", "New_Weapon_15.png" };
    string[] staff1 = { "Ward_1.png" };
    string[] staff3 = { "New_Weapon_03.png", "New_Weapon_18.png" };
    string[] shield2 = { "SteelShield1.png", "WoodShield1.png", "WoodShield2.png", "WoodShield3.png", "WoodShield4.png" };
    string[] shield3 = { "New_Shield_01.png", "New_Shield_02.png", "New_Shield_03.png", "New_Shield_04.png" };

    // parts lists
    public List<ArmourInfo> helmetsList = new List<ArmourInfo>();
    public List<ArmourInfo> armorsList = new List<ArmourInfo>();
    public List<ArmourInfo> pantsList = new List<ArmourInfo>();
    public List<WeaponInfo> weaponsList = new List<WeaponInfo>();
    public List<ArmourInfo> shieldsList = new List<ArmourInfo>();

    // called before start
    private void Awake()
    {
        // helmet list init
        for (int i = 0; i < helmet1.Length; i++)
            helmetsList.Add(new ArmourInfo(rootPath + helmetPath1 + helmet1[i], 1.0f));

        for (int i = 0; i < helmet2.Length; i++)
            helmetsList.Add(new ArmourInfo(rootPath + helmetPath2 + helmet2[i], 1.0f));

        for (int i = 0; i < helmet3.Length; i++)
            helmetsList.Add(new ArmourInfo(rootPath + helmetPath3 + helmet3[i], 1.0f));

        // armors list init
        for (int i = 0; i < armor1.Length; i++)
            armorsList.Add(new ArmourInfo(rootPath + armorPath1 + armor1[i], 1.0f));

        for (int i = 0; i < armor2.Length; i++)
            armorsList.Add(new ArmourInfo(rootPath + armorPath2 + armor2[i], 1.0f));

        for (int i = 0; i < armor3.Length; i++)
            armorsList.Add(new ArmourInfo(rootPath + armorPath3 + armor3[i], 1.0f));

        // pants list init
        for (int i = 0; i < pants1.Length; i++)
            pantsList.Add(new ArmourInfo(rootPath + pantsPath1 + pants1[i], 1.0f));

        for (int i = 0; i < pants2.Length; i++)
            pantsList.Add(new ArmourInfo(rootPath + pantsPath2 + pants2[i], 1.0f));

        // weapon list init
        weaponsList.Add(new WeaponInfo("", WeaponType.Melee, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));

        for (int i = 0; i < melee1.Length; i++)
            weaponsList.Add(new WeaponInfo(rootPath + weaponPath1 + melee1[i], WeaponType.Melee, 15.0f, 25.0f, 1.0f, 2.0f, 5.0f));

        for (int i = 0; i < melee2.Length; i++)
            weaponsList.Add(new WeaponInfo(rootPath + weaponPath2 + melee2[i], WeaponType.Melee, 20.0f, 30.0f, 3.0f, 5.0f, 10.0f));

        for (int i = 0; i < melee3.Length; i++)
            weaponsList.Add(new WeaponInfo(rootPath + weaponPath3 + melee3[i], WeaponType.Melee, 25.0f, 40.0f, 5.0f, 7.0f, 15.0f));

        for (int i = 0; i < bow1.Length; i++)
            weaponsList.Add(new WeaponInfo(rootPath + weaponPath1 + bow1[i], WeaponType.Bow, 15.0f, 25.0f, 1.0f, 2.0f, 5.0f));

        for (int i = 0; i < bow3.Length; i++)
            weaponsList.Add(new WeaponInfo(rootPath + weaponPath3 + bow3[i], WeaponType.Bow, 15.0f, 25.0f, 1.0f, 2.0f, 5.0f));

        for (int i = 0; i < staff1.Length; i++)
            weaponsList.Add(new WeaponInfo(rootPath + weaponPath1 + staff1[i], WeaponType.Staff, 15.0f, 25.0f, 1.0f, 2.0f, 5.0f));

        for (int i = 0; i < staff3.Length; i++)
            weaponsList.Add(new WeaponInfo(rootPath + weaponPath3 + staff3[i], WeaponType.Staff, 15.0f, 25.0f, 1.0f, 2.0f, 5.0f));

        // shield list init
        shieldsList.Add(new ArmourInfo("", 0.0f));

        for (int i = 0; i < shield2.Length; i++)
            shieldsList.Add(new ArmourInfo(rootPath + weaponPath2 + shield2[i], 1.0f));

        for (int i = 0; i < shield3.Length; i++)
            shieldsList.Add(new ArmourInfo(rootPath + weaponPath3 + shield3[i], 1.0f));
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
