using UnityEngine;

public class PickUpHealthBehaviour : MonoBehaviour
{
    [SerializeField] private int health;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player.CurrentHP == player.MaxHealth)
            {
                return;
            }

            player.HealPlayer(health);

            Destroy(gameObject);
        }
    }
}
