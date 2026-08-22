using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;

public class WeaponSelectionPROTO : MonoBehaviour
{
    public GameObject _mainMenu;
    public GameObject player;
    public GameObject swordPrefab;
    public GameObject staffPrefab;
    public GameObject macePrefab;

    public Button _swordBtn, _spearBtn, _maceBtn, _quit;
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

    // Update is called once per frame
    void Update()
    {
        
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
        // Source - https://stackoverflow.com/a/70591393
// Posted by Saif
// Retrieved 2026-08-22, License - CC BY-SA 4.0

        GameObject childGameObject = Instantiate(weaponPrefab, player.transform, true);
        childGameObject.name = "Weapon";

        Debug.Log("weapon selected");
        Time.timeScale = 1;
    }

    public void Sword()
    {
        //sword stats
    }
    public void Spear()
    {
        //spear stats
    }
    public void Mace()
    {
        //mace stats
    }
}
