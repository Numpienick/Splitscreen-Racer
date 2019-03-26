using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelBL;
    public WheelCollider WheelBR;

    public GameObject FL;
    public GameObject FR;
    public GameObject BL;
    public GameObject BR;

    public float topSpeed = 250f;
    public float maxTorque = 200f;
    private float maxSteerAngle = 45f;
    [HideInInspector]
    public float currentSpeed;
    public float maxBrakeTorque = 2200;

    [HideInInspector]
    public TextMeshProUGUI speedText;

    private float Forward;

    [HideInInspector]
    public float Turn;
    [HideInInspector]
    public float Brake;

    private Rigidbody rb;

    [HideInInspector]
    public float FLSteer;
    [HideInInspector]
    public float FRSteer;

    public string inputVertical;
    public string inputHorizontal;
    public string inputBrake;

    [HideInInspector]
    public int currentCheckpoint;

    [HideInInspector]
    public float bestTime;
    [HideInInspector]
    public TextMeshProUGUI ellapsedTimeText;
    public GameObject ellapsedTimeBox;
    [HideInInspector]
    public TextMeshProUGUI bestTimeText;
    [HideInInspector]
    public string bestTimeString;
    [HideInInspector]
    public GameObject bestTimeBox;

    [HideInInspector]
    public int currentLap;

    private LapComplete LapCompleteRef;

    [HideInInspector]
    public float startTime;
    [HideInInspector]
    public float ellapsedTime;

    [HideInInspector]
    public string timeText;

    [HideInInspector]
    public int minutes;
    [HideInInspector]
    public int seconds;
    [HideInInspector]
    public int milliseconds;

    [HideInInspector]
    public bool finished = false;

    public KeyCode enableLight;

    void Start()
    {
        ellapsedTimeText = ellapsedTimeBox.GetComponent<TextMeshProUGUI>();
        bestTimeText = bestTimeBox.GetComponent<TextMeshProUGUI>();
        rb = GetComponent<Rigidbody>();
        currentCheckpoint = 0;

        TimerStart();
        bestTime = float.MaxValue;
    }

    public void TimerStart()
    {
        startTime = Time.time;
    }

    void FixedUpdate()
    {
        Forward = Input.GetAxis(inputVertical);
        Turn = Input.GetAxis(inputHorizontal);
        Brake = Input.GetAxis(inputBrake);

        if (speedText != null)
        {
            speedText.GetComponent<TextMeshProUGUI>();
            speedText.SetText("Speed: " + Mathf.Round(currentSpeed).ToString() + " km/h");
        }

        FLSteer = WheelFL.steerAngle = maxSteerAngle * Turn;
        FRSteer = WheelFR.steerAngle = maxSteerAngle * Turn;

        currentSpeed = rb.velocity.magnitude * 3.6f;

        if (currentSpeed < topSpeed)
        {
            WheelBL.motorTorque = maxTorque * Forward;
            WheelBR.motorTorque = maxTorque * Forward;
        }

        WheelBL.brakeTorque = maxBrakeTorque * Brake;
        WheelBR.brakeTorque = maxBrakeTorque * Brake;
        WheelFL.brakeTorque = maxBrakeTorque * Brake;
        WheelFR.brakeTorque = maxBrakeTorque * Brake;
    }

    void Update()//update is called once per frame
    {
        Quaternion flq;//rotation of wheel collider
        Vector3 flv;//position of wheel collider
        WheelFL.GetWorldPose(out flv, out flq);//get wheel collider position and rotation
        FL.transform.position = flv;
        FL.transform.rotation = flq;

        Quaternion Blq;//rotation of wheel collider
        Vector3 Blv;//position of wheel collider
        WheelBL.GetWorldPose(out Blv, out Blq);//get wheel collider position and rotation
        BL.transform.position = Blv;
        BL.transform.rotation = Blq;

        Quaternion fRq;//rotation of wheel collider
        Vector3 fRv;//position of wheel collider
        WheelFR.GetWorldPose(out fRv, out fRq);//get wheel collider position and rotation
        FR.transform.position = fRv;
        FR.transform.rotation = fRq;

        Quaternion BRq;//rotation of wheel collider
        Vector3 BRv;//position of wheel collider
        WheelBR.GetWorldPose(out BRv, out BRq);//get wheel collider position and rotation
        BR.transform.position = BRv;
        BR.transform.rotation = BRq;

        ellapsedTime = Time.time - startTime;
        minutes = Mathf.FloorToInt(ellapsedTime / 60);
        seconds = Mathf.FloorToInt(ellapsedTime) % 60;
        milliseconds = Mathf.FloorToInt((ellapsedTime - Mathf.FloorToInt(ellapsedTime)) * 10);

        timeText = string.Format("{0:D2}:{1:D2}:{2:D1}", minutes, seconds, milliseconds);

        ellapsedTimeText.SetText(timeText);
    }
}