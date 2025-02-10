using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float despawnTime;
    private float timer;
    public int Damage;
    private void Start()
    {
        timer = despawnTime;
    }
    public virtual void Update()
    {
        timer -= Time.deltaTime;
        rb.velocity = transform.forward * speed;

        if (timer < 0)
            Destroy(gameObject);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            Destroy(gameObject);
        }
    }
}
