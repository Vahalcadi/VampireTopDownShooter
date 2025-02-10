using UnityEngine;

public class GrenadeBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float forceToLaunch;
    [SerializeField] private float timerToExplode;
    [SerializeField] private float deathRange;
    [SerializeField] private float explosionLifeTime;
    [SerializeField] private GameObject vfxExplosion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.AddForce(transform.forward * forceToLaunch, ForceMode.Impulse);
    }

    void Update()
    {
        timerToExplode -= Time.deltaTime;

        if (timerToExplode <= 0)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Explode();
        }
    }

    private void Explode()
    {
        var enemiesCollider = Physics.OverlapSphere(transform.position, deathRange);
        Debug.DrawLine(transform.position, transform.position + (Vector3.up * deathRange), Color.yellow, 20);
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * deathRange), Color.yellow, 20);
        Debug.DrawLine(transform.position, transform.position + (-Vector3.up * deathRange), Color.yellow, 20);
        Debug.DrawLine(transform.position, transform.position + (-Vector3.right * deathRange), Color.yellow, 20);
        Debug.DrawLine(transform.position, transform.position + (Vector3.forward * deathRange), Color.yellow, 20);
        Debug.DrawLine(transform.position, transform.position + (-Vector3.forward * deathRange), Color.yellow, 20);
        var vfx = Instantiate(vfxExplosion, transform.position, transform.rotation);

        foreach (var enemy in enemiesCollider)
        {
            if (enemy.gameObject.tag == "Enemy")
            {
                enemy.GetComponent<Enemy>().TakeDamage(int.MaxValue);
            }
        }

        Destroy(vfx, explosionLifeTime);
        Destroy(gameObject);
    }
}
