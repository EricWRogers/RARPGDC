using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Fireball : MonoBehaviour
{
    public float forceMagnitude = 3f;
    void Awake()
    {
        GetComponent<Rigidbody>().AddForce(-transform.forward * forceMagnitude);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAI>().TakeDamage(
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Weapon>().damage);
        }

        if(!other.GetComponentInChildren<SimplePlayerController>() && !other.GetComponentInParent<SimplePlayerController>())
            Destroy(gameObject);
    }
}
