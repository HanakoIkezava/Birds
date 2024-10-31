using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Pig : MonoBehaviour
{
    [SerializeField] private float maxHp = 3f;
    [SerializeField] private float damageTreshold = 0.5f;
    [SerializeField] private GameObject pigDeathPrefab;
    [SerializeField] private AudioClip deathClip;

    private float currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void DamagePig(float damageAmount)
    {
        currentHp -= damageAmount;

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.instance.RemovePig(this);

        Instantiate(pigDeathPrefab, transform.position, Quaternion.identity);

        AudioSource.PlayClipAtPoint(deathClip, transform.position);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity > damageTreshold) 
        {
            DamagePig(impactVelocity);
        }
    }
}
