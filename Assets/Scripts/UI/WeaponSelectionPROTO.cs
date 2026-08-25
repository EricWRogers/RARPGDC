using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponSelectionPROTO : MonoBehaviour
{
    public GameObject _mainMenu;
    public GameObject player;
    private SimplePlayerController playerScript; 
    public GameObject swordPrefab;
    public GameObject staffPrefab;
    public GameObject macePrefab;

    public List<float> defaultSword = new List<float> {2f, 0.8f, 0.1f};
    public List<float> defaultStaff = new List<float> {4f, 0.8f, 0.3f};
    public List<float> defaultMace = new List<float> {8f, 0.8f, 0.6f};
    public List<float> swordStats;
    public List<float> staffStats;
    public List<float> maceStats;
    public List<float> swordPercentDifferences;
    public List<float> staffPercentDifferences;
    public List<float> macePercentDifferences;
    private string swordStatSummary;
    private string staffStatSummary;
    private string maceStatSummary;
    public TMP_Text swordText;
    public TMP_Text spearText; // do we have a spear or staff? IDK
    public TMP_Text maceText;
    
    

    //public Button _swordBtn, _spearBtn, _maceBtn, _quit;
    //public string sword, spear, mace;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Give a variance of 50% plus or minus to each weapons stat
    void RandomizeWeapons()
    {
        swordStats = new List<float>
        {
            Random.Range(defaultSword[0] * 0.5f, defaultSword[0] * 1.5f),
            Random.Range(defaultSword[1] * 0.5f, defaultSword[1] * 1.5f),
            Random.Range(defaultSword[2] * 0.5f, defaultSword[2] * 1.5f)
        };
        staffStats = new List<float>
        {
            Random.Range(defaultStaff[0] * 0.5f, defaultStaff[0] * 1.5f),
            Random.Range(defaultStaff[1] * 0.5f, defaultStaff[1] * 1.5f),
            Random.Range(defaultStaff[2] * 0.5f, defaultStaff[2] * 1.5f)
        };
        maceStats = new List<float>
        {
            Random.Range(defaultMace[0] * 0.5f, defaultMace[0] * 1.5f),
            Random.Range(defaultMace[1] * 0.5f, defaultMace[1] * 1.5f),
            Random.Range(defaultMace[2] * 0.5f, defaultMace[2] * 1.5f)
        };
    }
    void Start()
    {
        _mainMenu.SetActive(true);
        Time.timeScale = 0;

        RandomizeWeapons();

        swordPercentDifferences = CalculatePercentDifferences(defaultSword, swordStats);
        staffPercentDifferences = CalculatePercentDifferences(defaultStaff, staffStats);
        macePercentDifferences = CalculatePercentDifferences(defaultMace, maceStats);

        swordStatSummary = $"Damage: {swordPercentDifferences[0]:+0.0;-0.0;0}% \n Range: {swordPercentDifferences[1]:+0.0;-0.0;0}% \n Attack Speed: {swordPercentDifferences[2]:+0.0;-0.0;0}%";
        staffStatSummary = $"Damage: {staffPercentDifferences[0]:+0.0;-0.0;0}% \n Range: {staffPercentDifferences[1]:+0.0;-0.0;0}% \n Attack Speed: {staffPercentDifferences[2]:+0.0;-0.0;0}%";
        maceStatSummary = $"Damage: {macePercentDifferences[0]:+0.0;-0.0;0}% \n Range: {macePercentDifferences[1]:+0.0;-0.0;0}% \n Attack Speed: {macePercentDifferences[2]:+0.0;-0.0;0}%";
        
        swordText.text = swordStatSummary;
        spearText.text = staffStatSummary;
        maceText.text = maceStatSummary;

        //_swordBtn.onClick.AddListener(WeaponChosen);
        //_spearBtn.onClick.AddListener(WeaponChosen);
        //_maceBtn.onClick.AddListener(WeaponChosen);
        
    }

    List<float> CalculatePercentDifferences(List<float> originalStats, List<float> modifiedStats)
    {
        List<float> percentDifferences = new List<float>();

        for (int i = 0; i < originalStats.Count; i++)
        {
            percentDifferences.Add((modifiedStats[i] - originalStats[i]) / originalStats[i] * 100f);
        }

        return percentDifferences;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode(); //close in editor
        #else
            Application.Ouit();  //close game
        #endif
    }

    public void WeaponChosen(GameObject weaponPrefab)
    {
        GameObject childWeapon = Instantiate(weaponPrefab, player.transform, false);

        if(weaponPrefab.name == "Sword")
        {
            childWeapon.GetComponent<Weapon>().damage = (int)swordStats[0];
            childWeapon.GetComponent<Weapon>().attackRange = swordStats[1];
            childWeapon.GetComponent<Weapon>().animationDelay = swordStats[2];
        }
        else if (weaponPrefab.name == "Staff")
        {
            childWeapon.GetComponent<Weapon>().damage = (int)staffStats[0];
            childWeapon.GetComponent<Weapon>().attackRange = staffStats[1];
            childWeapon.GetComponent<Weapon>().animationDelay = staffStats[2];
        }
        else if (weaponPrefab.name == "Mace")
        {
            childWeapon.GetComponent<Weapon>().damage = (int)maceStats[0];
            childWeapon.GetComponent<Weapon>().attackRange = maceStats[1];
            childWeapon.GetComponent<Weapon>().animationDelay = maceStats[2];
        }

        childWeapon.name = "Weapon";

        playerScript = player.GetComponent<SimplePlayerController>();
        playerScript.weapon = childWeapon;
        playerScript.weaponScript = childWeapon.GetComponent<Weapon>();


        Debug.Log("weapon selected");
        Time.timeScale = 1;
        _mainMenu.SetActive(false);
    }
}
