using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform[] checkpointArray;
    public int currentLap = 0;
    public Vector3 startPos;

    private int index;
    private LapComplete[] lapComplete = new LapComplete[4];

    public GameObject[] playerPrefab;
    public GameObject player2Prefab;
    public GameObject AIPrefab;

    int i;
    public List<GameObject> players = new List<GameObject>();

    private GameObject[] countdown;
    private GameObject[] UIElements;

    void Start()
    {
        ModeSelector modeSelector = FindObjectOfType<ModeSelector>();
        if (modeSelector != null)
        {
            if (modeSelector.mode == "ai")
                playerPrefab[1] = AIPrefab;
            else
                playerPrefab[1] = player2Prefab;
        }

        for (i = 0; i < playerPrefab.Length; i++)
        {
            GameObject spawnPlayer = Instantiate(playerPrefab[i], new Vector3(i * 4, 4f, -28.7f), Quaternion.identity);
            players.Add(spawnPlayer);
        }
        UIElements = GameObject.FindGameObjectsWithTag("UIElement");
        countdown = GameObject.FindGameObjectsWithTag("Countdown");
        index = 0;
        foreach (Transform obj in checkpointArray)
        {
            lapComplete[index] = obj.GetComponent<LapComplete>();
            index++;
        }

        StartCoroutine("LoseTime");

        foreach (GameObject element in UIElements)
        {
            element.SetActive(false);
        }
        startPos = transform.position;
    }

    IEnumerator LoseTime()
    {
        Time.timeScale = 0;
        float pausetime = Time.realtimeSinceStartup + 4f;
        while (Time.realtimeSinceStartup < pausetime)
            yield return 0;

        foreach (GameObject countdown in countdown)
        {
            countdown.SetActive(false);
        }

        foreach (GameObject obj in UIElements)
        {
            obj.SetActive(true);
        }

        Time.timeScale = 1;
        index = 0;
        foreach (Transform obj in checkpointArray)
        {
            lapComplete[index].AfterCountdown();
            index++;
        }
    }
}