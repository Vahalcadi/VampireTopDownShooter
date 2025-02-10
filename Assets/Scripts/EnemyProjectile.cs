using UnityEngine;

public class EnemyProjectile : Projectile
{
    public override void Update()
    {
        base.Update();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (!player.IsInvincible)
            {
                player.TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }
}
