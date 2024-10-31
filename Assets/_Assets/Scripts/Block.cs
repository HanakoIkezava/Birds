using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float maxHp = 3f;
    [SerializeField] private float damageTreshold = 0.5f;
    [SerializeField] private GameObject blockDeathPrefab;

    private float currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void DamageBlock(float damageAmount)
    {
        currentHp -= damageAmount;

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.instance.RemoveBlock(this);

        GameObject particles = Instantiate(blockDeathPrefab, transform.position, Quaternion.identity);

        ParticleSystem particleSystem = particles.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            var shapeModule = particleSystem.shape;

            shapeModule.scale = transform.localScale;

            particles.transform.rotation = transform.rotation;
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity > damageTreshold)
        {
            DamageBlock(impactVelocity);
        }
    }
}
