using UnityEngine;

public class FlipInecator : MonoBehaviour
{
    public GameObject flip_indecator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //flip_indecator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFlipInd() //call whenever play touches water
    {
        flip_indecator.SetActive(true);
    }

    public void HideFlipInd() //call whenever player is no longer toching water
    {
        flip_indecator.SetActive(true);
    }
}
