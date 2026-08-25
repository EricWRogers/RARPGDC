using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelectionPROTO : MonoBehaviour
{
    public GameObject _mainMenu;
    public GameObject player;
    private SimplePlayerController playerScript; 
    public GameObject swordPrefab;
    public GameObject staffPrefab;
    public GameObject macePrefab;

    //public Button _swordBtn, _spearBtn, _maceBtn, _quit;
    //public string sword, spear, mace;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainMenu.SetActive(true);
        Time.timeScale = 0;

        //_swordBtn.onClick.AddListener(WeaponChosen);
        //_spearBtn.onClick.AddListener(WeaponChosen);
        //_maceBtn.onClick.AddListener(WeaponChosen);
        
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
        childWeapon.name = "Weapon";

        playerScript = player.GetComponent<SimplePlayerController>();
        playerScript.weapon = childWeapon;
        playerScript.weaponScript = childWeapon.GetComponent<Weapon>();


        Debug.Log("weapon selected");
        Time.timeScale = 1;
        _mainMenu.SetActive(false);
    }
}
