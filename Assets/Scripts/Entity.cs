using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator Anim { get; private set; }
    public Rigidbody Rb { get; private set; }


    [Header("Stats")]
    [SerializeField] private GameObject bloodVfx;
    [SerializeField] private bool destroyOnDeath;
    [SerializeField] private GameObject corpse;
    [SerializeField] private GameObject mesh;
    [SerializeField] private int scoreOnDeath;
    [SerializeField] private float maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    private float currentHP;

    [Header("Loot")]
    [SerializeField] private List<LootSystem> lootSystems;

    /*[Header("Knockback info")]
    [SerializeField] protected float knockbackPower;
    [SerializeField] protected float knockbackDuration = .07f;
    protected bool isKnocked;*/

    public int Damage { get { return damage; } }
    public float CurrentHP { get { return currentHP; } set { currentHP = value; } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    public float Speed { get { return speed; } set { speed = value; } }

    public bool IsDead { get; private set; }
    public bool IsInvincible { get; private set; }

    private Collider colliderEntity;

    protected virtual void Awake()
    {
        colliderEntity = GetComponent<Collider>();
        currentHP = maxHealth;
    }

    protected virtual void Update()
    {

    }

    protected virtual void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        //anim = this.GetComponentsInChildren<Animator>().First(x => x.gameObject.transform.parent != transform.parent);
        Rb = GetComponent<Rigidbody>();

        IsDead = false;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHealth);
        if (!IsDead)
        {
            GameObject vfx = Instantiate(bloodVfx, transform.position, Quaternion.identity);
            Destroy(vfx, .5f);
        }
        //StartCoroutine(HitKnockback());

        if (currentHP == 0)
            Die();
    }



    /*public IEnumerator HitKnockback()
    {
        isKnocked = true;

        //rb.velocity = -transform.forward * knockbackPower;

        rb.AddForce(-transform.forward * knockbackPower, ForceMode.VelocityChange);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }*/

    public void MakeInvincible(bool _invincible) => IsInvincible = _invincible;

    public virtual void SetZeroVelocity()
    {
        Rb.velocity = Vector3.zero;
        Rb.angularVelocity = Vector3.zero;
    }

    public virtual void Die()
    {
        IsDead = true;
        EventManager.IncreaseScore?.Invoke(scoreOnDeath);

        if (lootSystems.Count > 0)
        {
            int probability = UnityEngine.Random.Range(1, 101);

            foreach (LootSystem loot in lootSystems)
            {
                if (loot.startRange <= probability && loot.endRange >= probability) // sr <= p <= er   es. 1 - 10 : hp, 11 - 20: grenade, 21 - 30: altro
                {
                    Instantiate(loot.loot, transform.position, transform.rotation);
                    break;
                }
            }
        }

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            GameManager.Instance.AddEnemyToTheDeadList(gameObject);
            enabled = false;
            corpse.SetActive(true);
            mesh.SetActive(false);
            Rb.useGravity = false;
            Rb.constraints = RigidbodyConstraints.FreezeAll;
            colliderEntity.isTrigger = true;
        }
    }

    public virtual void ReviveEntity()
    {
        IsDead = false;
        enabled = true;
        corpse.SetActive(false);
        mesh.SetActive(true);
        colliderEntity.isTrigger = false;
        Rb.constraints = RigidbodyConstraints.FreezeRotation;
        Rb.useGravity = true;
    }

    protected virtual void OnDrawGizmos()
    {
    }

    #region PickUp


    #endregion
}

[Serializable]
public class LootSystem
{
    public GameObject loot;
    public int startRange;
    public int endRange;
}