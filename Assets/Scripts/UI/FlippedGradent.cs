using UnityEngine;

public class FlippedGradent : MonoBehaviour
{
    public bool flipped;
    public GameObject flippedGradent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flippedGradent.SetActive(false);
        flipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        flippedGradent.SetActive(flipped);
    }

    public void FlipWorld() //change the bool
    {
        if(flipped == true)
            flipped = false;

        if(flipped == false)
            flipped = true;
        
    }

    public void CheckFlippedGradent() //call whenever player flips
    {
        if(flipped == true)
            flippedGradent.SetActive(true);

        if(flipped == false)
            flippedGradent.SetActive(false);
        

    }
}
