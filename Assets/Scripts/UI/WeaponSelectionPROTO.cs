using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionPROTO : MonoBehaviour
{
    public Button _swordBtb, _spearBtn, _maceBtn, _quit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_quit.onClick.(QuitGame);
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
}
