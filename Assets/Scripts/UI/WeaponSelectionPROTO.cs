using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics.Tracing;

public class WeaponSelectionPROTO : MonoBehaviour
{
    public GameObject _mainMenu;
    public Button _swordBtn, _spearBtn, _maceBtn, _quit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainMenu.SetActive(true);

        _swordBtn.onClick.AddListener(WeaponChosen);
        _spearBtn.onClick.AddListener(WeaponChosen);
        _maceBtn.onClick.AddListener(WeaponChosen);

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

    public void WeaponChosen()
    {
        Debug.Log("weapon selected");
        _mainMenu.SetActive(false);
        /* if this button selected
            do this (change stats yadda yadda)
            
        if other button selected 
            do this yeayeah
        */
    }
}
