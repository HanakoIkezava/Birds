using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxNumberOfShots = 3;
    private int usedShots;

    [SerializeField] private float secondsTOLose = 5f;
    [SerializeField] private GameObject restartScreenObject;
    [SerializeField] private SlingshotHandler slingshotHandler;
    [SerializeField] private Image nextLvlImage;

    private IconHandler iconHandler;

    private List<Pig> pigs = new List<Pig>();
    private List<Block> blocks = new List<Block>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        iconHandler = FindAnyObjectByType<IconHandler>();

        Pig[] foundPigs = FindObjectsOfType<Pig>();
        for (int i = 0; i < foundPigs.Length; i++)
        {
            pigs.Add(foundPigs[i]);
        }

        Block[] foundBlocks = FindObjectsOfType<Block>();
        for (int i = 0;i < foundBlocks.Length; i++)
        {
            blocks.Add(foundBlocks[i]);
        }

        nextLvlImage.enabled = false;
    }

    public void UseShot()
    {
        usedShots++;
        iconHandler.UseShot(usedShots);
        ChekForLastShoot();
    }

    public bool HasEnoughtShots()
    {
        if (usedShots < maxNumberOfShots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ChekForLastShoot()
    {
        if (usedShots == maxNumberOfShots)
        {
            StartCoroutine(ChekAfterWaitTime());
        }
    }

    private IEnumerator ChekAfterWaitTime()
    {
        yield return new WaitForSeconds(secondsTOLose);

        if (pigs.Count == 0)
        {
            WinGame();
        }
        else
        {
            RestartGame();
        }
    }

    public void RemovePig(Pig pig)
    {
        pigs.Remove(pig);
        CheckForAllDeadPigs();
    }

    public void RemoveBlock(Block block)
    {
        blocks.Remove(block);
        CheckForAllDeadPigs();
    }

    private void CheckForAllDeadPigs()
    {
        if (pigs.Count == 0)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        restartScreenObject.SetActive(true);
        slingshotHandler.enabled = false;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLvls = SceneManager.sceneCountInBuildSettings;
        if (currentSceneIndex + 1 < maxLvls)
        {
            nextLvlImage.enabled = true;
        }
    }

    public void RestartGame()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLvl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
