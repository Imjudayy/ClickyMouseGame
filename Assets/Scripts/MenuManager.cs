using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> targets;
    private float spawnRate = 1.0f;

    private Coroutine spawnRoutine;

    public void Start()
    {
        spawnRate = spawnRate / 2;
        spawnRoutine = StartCoroutine(SpawnTargets());
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main"); 
    }

    IEnumerator SpawnTargets()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            Instantiate(targets[Random.Range(0, targets.Count)]);
        }
    }

}
