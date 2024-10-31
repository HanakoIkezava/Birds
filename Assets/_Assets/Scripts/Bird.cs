using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private float secondsToDeath = 3f;
    [SerializeField] private GameObject birdDeathPrefab;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private AudioSource audioSource;

    private bool hasBeenLaunched;
    private bool shouldFaceVelocityDir;


    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        rb.isKinematic = true;
        circleCollider.enabled = false;        
    }

    private void FixedUpdate()
    {
        if (hasBeenLaunched && shouldFaceVelocityDir)
        {
            transform.right = rb.velocity;
        }
    }

    public void LaunchBird(Vector2 direction, float force)
    {
        rb.isKinematic = false;
        circleCollider.enabled = true;

        // Применяем импульс один раз в направлении
        rb.velocity = Vector2.zero; // Обнуляем скорость перед каждым запуском
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        hasBeenLaunched = true;
        shouldFaceVelocityDir = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        shouldFaceVelocityDir = false;
        SoundManager.instance.PlayClip(hitClip, audioSource);
        StartCoroutine(BirdDeath());        
        //Destroy(this);
    }

    private IEnumerator BirdDeath()
    {
        yield return new WaitForSeconds(secondsToDeath);
        Instantiate(birdDeathPrefab, transform.position, Quaternion.identity);
        Destroy(this.GameObject());
    }
}
