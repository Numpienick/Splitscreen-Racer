using UnityEngine;

public class LapComplete : MonoBehaviour
{

    private CarController carControllerRef;

    private EnableHeadLights enableHeadLightsRef;

    private GameObject[] lights;

    public GameController gameController;
    private GameObject[] carController;
    private CarController[] player;
    private Transform[] checkpointA;

    private BoxCollider finishLine;

    void OnTriggerExit(Collider other)
    {
        int index = 0;

        if (carController != null)
        {
            foreach (GameObject obj in carController)
            {
                if (obj.transform.root == other.gameObject.transform.root && other.CompareTag("CheckpointCol"))
                {
                    if (transform == checkpointA[player[index].currentCheckpoint].transform)
                    {
                        if (player[index].currentCheckpoint + 1 < gameController.checkpointArray.Length)
                        {
                            player[index].currentCheckpoint++;
                            finishLine.enabled = true;
                            player[index].finished = false;
                        }
                        else
                        {
                            player[index].currentCheckpoint = 0;
                        }
                    }
                    if (player[index].currentCheckpoint == 0 && player[index].ellapsedTime <= player[index].bestTime && player[index].finished == false)
                    {
                        player[index].bestTimeString = string.Format("{0:D2}:{1:D2}:{2:D1}", player[index].minutes, player[index].seconds, player[index].milliseconds);
                        player[index].bestTimeString = player[index].timeText;
                        player[index].bestTime = player[index].ellapsedTime;
                        player[index].bestTimeText.SetText(player[index].bestTimeString);
                        player[index].ellapsedTime = 0;
                        player[index].TimerStart();
                        player[index].currentLap++;
                        player[index].finished = true;
                    }
                    else if (player[index].currentCheckpoint == 0 && player[index].ellapsedTime >= player[index].bestTime && player[index].finished == false)
                    {
                        player[index].ellapsedTime = 0;
                        player[index].TimerStart();
                        player[index].currentLap++;
                        player[index].finished = true;
                    }
                    break;
                }
                index++;
            }
        }
    }

    public void AfterCountdown()
    {
        ModeSelector modeSelect = FindObjectOfType<ModeSelector>();

        player = new CarController[gameController.players.Count];
        carController = GameObject.FindGameObjectsWithTag("carController");
        lights = GameObject.FindGameObjectsWithTag("CarLight");

        int index = 0;
        foreach (GameObject element in carController)
        {
            carControllerRef = player[index] = element.GetComponent<CarController>();
            if (modeSelect.mode == "vs")
                index++;
        }

        gameController.GetComponent<GameController>();
        checkpointA = gameController.checkpointArray;
        carControllerRef.currentLap = 0;

        carControllerRef.bestTime = float.MaxValue;
        finishLine = checkpointA[3].GetComponent<BoxCollider>();
        finishLine.enabled = false;

        foreach (GameObject obj in lights)
        {
            enableHeadLightsRef = obj.GetComponent<EnableHeadLights>();

            if (enableHeadLightsRef != null)
                enableHeadLightsRef.GetCarController();
        }
    }
}