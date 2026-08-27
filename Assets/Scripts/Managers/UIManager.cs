using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("Inversion")]
    public GameObject flipIndicator;
    public GameObject flipGradient;
    private bool isFlipped;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(true);

        flipIndicator.SetActive(false);
        flipGradient.SetActive(false);
    }

    public void ShowFlipInd() //call whenever play touches water
    {
        flipIndicator.SetActive(true);
    }

    public void HideFlipInd() //call whenever player is no longer toching water
    {
        flipIndicator.SetActive(false);
    }

    public void FlipWorld() //change the bool
    {
        if (!isFlipped)
        {
            isFlipped = true;
            flipGradient.SetActive(true);
        }
        else
        {
            isFlipped = false;
            flipGradient.SetActive(false);
        }
    }
}
