using UnityEngine;

public class PickUpGrenadeBehaviour : MonoBehaviour
{
    [SerializeField] private int granadePickUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().TakeGrenade(granadePickUp))
            {
                Destroy(gameObject);
            }
        }
    }
}
