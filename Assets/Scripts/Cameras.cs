using UnityEngine;

public class Cameras : MonoBehaviour
{
    public Camera[] cameras;
    public static int currentCameraIndex;
    public static int i;

    public float distanceAway;
    public float distanceUp;
    public float smooth;

    public bool actualCamera;
    public bool cameraSwitcher;

    public Transform objectToFollow;
    private Vector3 targetPosition;

    public KeyCode toggleCam;

    GameObject AIPlayer;

    void Start()
    {
        Camera[] cams = FindObjectsOfType<Camera>();
        AIPlayer = GameObject.FindGameObjectWithTag("AI");
        if (AIPlayer != null)
        {
            for (int i = 0; i < cams.Length; i++)
            {
                cams[i].rect = new Rect(0, 0, 1, 1);
            }
        }
        if (cameraSwitcher == true)
        {
            currentCameraIndex = 0;

            for (i = 1; i < cameras.Length; i++)
            {
                cameras[i].gameObject.SetActive(false);
            }
            if (cameras.Length > 0)
            {
                cameras[0].gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
        Camera[] cams = FindObjectsOfType<Camera>();

        if (AIPlayer != null)
        {
            for (int i = 0; i < cams.Length; i++)
            {
                cams[i].rect = new Rect(0, 0, 1, 1);
            }
        }

        if (cameraSwitcher == true)
        {
            if (Input.GetKeyDown(toggleCam))
            {
                currentCameraIndex++;
                if (currentCameraIndex < cameras.Length)
                {
                    cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                    cameras[currentCameraIndex].gameObject.SetActive(true);
                }
                else
                {
                    cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                    currentCameraIndex = 0;
                }
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
        }
    }

    void LateUpdate()
    {
        if (actualCamera == true)
        {
            targetPosition = objectToFollow.position + objectToFollow.up * distanceUp - objectToFollow.forward * distanceAway;

            transform.position = Vector3.Lerp(transform.position, targetPosition, smooth * Time.deltaTime);

            transform.LookAt(objectToFollow);
        }
    }
}