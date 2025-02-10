using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpArmorBehaviour : MonoBehaviour
{
    [SerializeField] private int armor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player.CurrentArmor == Player.OnMaxArmor?.Invoke())
            {
                return;
            }

            player.GainArmor(armor);

            Destroy(gameObject);
        }
    }
}
