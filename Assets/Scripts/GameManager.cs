using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject [] levels;
    private int level = 0;
    private GameObject currentLevel;
    void Start()
    {
       NextLevel();

    }

    public void NextLevel()
    {
        if(currentLevel)
        {
            Destroy(currentLevel);
        }
        StartCoroutine(StartNewLevel());
    }

    private IEnumerator StartNewLevel()
    {
        yield return new WaitForSeconds(1f);
		currentLevel = Instantiate(levels[level]);
        level++;
    }
}

