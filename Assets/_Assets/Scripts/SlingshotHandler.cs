using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SlingshotHandler : MonoBehaviour
{
    [Header("LineRenderers")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;

    [Header("Transforms")]
    [SerializeField] private Transform leftStartPos;
    [SerializeField] private Transform rightStartPos;
    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform idlePos;
    [SerializeField] private Transform elasticPos;

    [Header("Stats")]
    [SerializeField] private float maxDistance = 3.5f;
    [SerializeField] private float shotForce = 5f;
    [SerializeField] private float timeBetweenBirdRespawn = 2f;
    [SerializeField] private float elasticDevider = 1.2f;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea slingShotArea;
    [SerializeField] private CameraManager cameraManager;

    [Header("Birds")]
    [SerializeField] private Bird birdPrefab;
    [SerializeField] private float birdPositionOffset = 2f;

    [Header("Sounds")]
    [SerializeField] private AudioClip elasticPulledClip;
    [SerializeField] private AudioClip[] elasticReleaseClips;

    private Vector2 slingShootLinesPosition;
    private Vector2 direction;
    private Vector2 directionNormalized;

    private bool clikedWithinArea;
    private bool birdOnSlingshot;

    private Bird spawnedBird;

    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;

        SpawnBird();
    }
    private void Update()
    {
        if (InputManager.wasMousePressed && slingShotArea.IsWithinSlingshotArea())
        {
            clikedWithinArea = true;
            if (birdOnSlingshot) 
            {
                SoundManager.instance.PlayClip(elasticPulledClip, audioSource);
                cameraManager.SwitchCameraToFollow(spawnedBird.transform);
            }
        }

        if (InputManager.isMousePressed && clikedWithinArea && birdOnSlingshot)
        {
            DrawSlingShot();
        }

        if (InputManager.wasMouseReleased && clikedWithinArea && birdOnSlingshot)
        {
            if (GameManager.instance.HasEnoughtShots())
            {
                clikedWithinArea = false;
                birdOnSlingshot = false;

                spawnedBird.LaunchBird(direction, shotForce);

                SoundManager.instance.PlayRandomClip(elasticReleaseClips, audioSource);
                GameManager.instance.UseShot();
                //SetLines(centerPos.position);
                AnimateSlingshoot();

                if (GameManager.instance.HasEnoughtShots())
                {
                    StartCoroutine(SpawnBirdAfterTime());
                }
            }
        }
    }
    #region Slingshot metods
    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.mousePosition);

        slingShootLinesPosition = centerPos.position + Vector3.ClampMagnitude(touchPosition - centerPos.position, maxDistance);

        SetLines(slingShootLinesPosition);

        direction = (Vector2)centerPos.position - slingShootLinesPosition;
        directionNormalized = direction.normalized;

        // ќбновл€ем положение птицы в процессе нат€гивани€
        spawnedBird.transform.position = slingShootLinesPosition + directionNormalized * birdPositionOffset;
        spawnedBird.transform.right = directionNormalized;
    }

    private void SetLines(Vector2 position)
    {
        if (!leftLineRenderer.enabled && !rightLineRenderer.enabled) 
        {
            leftLineRenderer.enabled = true;
            rightLineRenderer.enabled = true;        
        }

        leftLineRenderer.SetPosition(0, position);
        leftLineRenderer.SetPosition(1, leftStartPos.position);

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1, rightStartPos.position);
    }
    #endregion
    #region birds metods
    private void SpawnBird()
    {
        elasticPos.DOComplete();
        SetLines(idlePos.position);

        Vector2 dir = (centerPos.position - idlePos.position).normalized;
        Vector2 spawnPos = (Vector2)idlePos.position + dir * birdPositionOffset;

        spawnedBird = Instantiate(birdPrefab, spawnPos, Quaternion.identity);
        spawnedBird.transform.right = dir;

        birdOnSlingshot = true;
    }

    private void OnPostRender()
    {
        spawnedBird.transform.position = slingShootLinesPosition + directionNormalized * birdPositionOffset;
        spawnedBird.transform.right = directionNormalized;
    }

    private IEnumerator SpawnBirdAfterTime()
    {
        yield return new WaitForSeconds(timeBetweenBirdRespawn);

        SpawnBird();
        cameraManager.SwitchCameraToIdle();
    }

    #endregion
    #region Animation of slingshoot
    private void AnimateSlingshoot()
    {
        elasticPos.position = leftLineRenderer.GetPosition(0);

        float dist = Vector2.Distance(elasticPos.position, centerPos.position);

        float time = dist / elasticDevider;

        elasticPos.DOMove(centerPos.position, time).SetEase(Ease.OutElastic);
        StartCoroutine(AnimateSlingshotLines(elasticPos, time));
    }

    private IEnumerator AnimateSlingshotLines(Transform pos, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time) 
        {
            elapsedTime += Time.deltaTime;
            SetLines(pos.position);
            yield return null;
        }
    }
    
    #endregion
}
