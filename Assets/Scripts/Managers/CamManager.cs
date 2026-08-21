using UnityEngine;

public class CamManager : MonoBehaviour
{
    public GameObject target;
    public int type;
    public float smoothTime = 0.3f;    // Damping time
    private Vector3 velocity = Vector3.zero;
    Vector3 targetTransform;
    //public float yOffset = 1;


    // NEW: stop following flag
    public bool stopFollowing = false;

    void Update()
    {
        if (stopFollowing) return; // freeze camera when player dies

        targetTransform = target.transform.position;
        // targetTransform.z = transform.position.z;
        // targetTransform.y = target.transform.position.y + yOffset;

        // Smooth follow
        transform.position = Vector3.SmoothDamp(transform.position, 
            new Vector3(targetTransform.x - 18, transform.position.y, targetTransform.z), ref velocity, smoothTime);

        // if (transform.position.y + (GetComponent<Camera>().orthographicSize / 2) > maxHeight || camLock)
        // {
        //     camLock = true;
        //     transform.position = new Vector3(transform.position.x, 1.07f, transform.position.z);
        //     transform.GetComponent<Camera>().orthographicSize = 6.93f;
        // }
    }
}