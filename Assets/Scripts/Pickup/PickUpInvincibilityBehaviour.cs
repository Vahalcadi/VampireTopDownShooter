using UnityEngine;

public class PickUpInvincibilityBehaviour : MonoBehaviour
{
    [SerializeField] private float invincibilityTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.MakeInvincible(invincibilityTimer, other.gameObject.GetComponent<Player>());
            other.GetComponent<Player>().InvincibilityCoroutineVFX(invincibilityTimer);
            Destroy(gameObject);
        }
    }
}
