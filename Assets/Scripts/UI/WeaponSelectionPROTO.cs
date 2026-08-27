using TMPro;
using UnityEngine;

public class WeaponSelectionPROTO : MonoBehaviour
{
    public GameObject _mainMenu;
    public GameObject player;
    private SimplePlayerController playerScript; 
    public TMP_Text abilityText;
    //ability images
    public GameObject dashImg;
    public GameObject chargeImg;
    public GameObject invisImg;

    // public List<float> defaultSword = new List<float> {2f, 0.8f, 0.1f};
    // public List<float> defaultStaff = new List<float> {4f, 0.8f, 0.3f};
    // public List<float> defaultMace = new List<float> {8f, 0.8f, 0.6f};
    

    //public Button _swordBtn, _spearBtn, _maceBtn, _quit;
    //public string sword, spear, mace;
    void Start()
    {
        playerScript = FindFirstObjectByType<SimplePlayerController>();
        player = GameObject.FindGameObjectWithTag("Player");
        _mainMenu.SetActive(true);
        Time.timeScale = 0;

        ChangeAbilityText();
        
        //_swordBtn.onClick.AddListener(WeaponChosen);
        //_spearBtn.onClick.AddListener(WeaponChosen);
        //_maceBtn.onClick.AddListener(WeaponChosen);
        
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode(); //close in editor
        #else
            Application.Quit();  //close game
        #endif
    }

    public void WeaponChosen(GameObject weaponPrefab)
    {
        GameObject childWeapon = Instantiate(weaponPrefab, player.transform, false);

        // if(weaponPrefab.name == "Sword")
        // {
        //     childWeapon.GetComponent<Weapon>().damage = (int)defaultSword[0];
        //     childWeapon.GetComponent<Weapon>().attackRange = defaultSword[1];
        //     childWeapon.GetComponent<Weapon>().animationStepLength = defaultSword[2];
        // }
        // else if (weaponPrefab.name == "Staff")
        // {
        //     childWeapon.GetComponent<Weapon>().damage = (int)defaultStaff[0];
        //     childWeapon.GetComponent<Weapon>().attackRange = defaultStaff[1];
        //     childWeapon.GetComponent<Weapon>().animationStepLength = defaultStaff[2];
        // }
        // else if (weaponPrefab.name == "Mace")
        // {
        //     childWeapon.GetComponent<Weapon>().damage = (int)defaultMace[0];
        //     childWeapon.GetComponent<Weapon>().attackRange = defaultMace[1];
        //     childWeapon.GetComponent<Weapon>().animationStepLength = defaultMace[2];
        // }

        childWeapon.name = "Weapon";

        
        playerScript.weapon = childWeapon;
        playerScript.weaponScript = childWeapon.GetComponent<Weapon>();


        Debug.Log("weapon selected");
        Time.timeScale = 1;
        _mainMenu.SetActive(false);
        
    }
    public void ChangeAbilityText()
    {
        if (playerScript.myASkill == ActionSkill.Dash) //dash ability
        {
            abilityText.text = "Dash Ability";
            dashImg.SetActive(true);
            chargeImg.SetActive(false);
            invisImg.SetActive(false);
        }
        else if (playerScript.myASkill == ActionSkill.Charge) //charge ablilty
        {
            abilityText.text = "Charge Ability";
            dashImg.SetActive(false);
            chargeImg.SetActive(true);
            invisImg.SetActive(false);
        }
        else if (playerScript.myASkill == ActionSkill.Invis) //invisability
        {
            abilityText.text = "Invisibility";
            dashImg.SetActive(false);
            chargeImg.SetActive(false);
            invisImg.SetActive(true);
        }
    }


}
