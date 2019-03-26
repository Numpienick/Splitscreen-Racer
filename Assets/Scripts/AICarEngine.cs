using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarEngine : MonoBehaviour
{

    public Transform path;
    public float maxSteeringAngle = 45f;
    public float turnSpeed = 5f;

    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelBL;
    public WheelCollider WheelBR;

    public GameObject FL;
    public GameObject FR;
    public GameObject BL;
    public GameObject BR;

    public float FLSteer;
    public float FRSteer;

    public float maxMotorTorque = 500;
    public float currentSpeed;
    public float maxSpeed = 100f;

    public bool isBraking;

    public MeshRenderer brakeLight;

    public float maxBrakeTorque = 150f;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public Vector3 frontSensorPos = new Vector3(0f, 0.5f, 2.2f);
    public float frontSideSensorPos = 1f;
    public float frontSensorAngle = 30f;

    private List<Transform> nodes;
    public int currentNode = 0;
    public bool avoiding = false;
    private float targetSteeringAngle = 0;

    void Start()
    {
        GameObject pathObject = GameObject.FindGameObjectWithTag("path");
        path = pathObject.transform;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    private void Update()
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
    }

    void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        Drive();
        CheckWaypointDistance();
        LerpToSteerAngle();
        if ((FLSteer >= maxSteeringAngle - 20 || FRSteer >= maxSteeringAngle - 20) && currentSpeed >= 10)
        {
            isBraking = true;
        }
        else
        {
            isBraking = false;
        }
        Braking();
    }

    //Sensors are not enabled because there are no objects to avoid in the scene because this wasn't needed. It works though
    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPos.z;
        sensorStartPos += transform.up * frontSensorPos.y;
        float avoidMultiplier = 0;
        avoiding = false;


        //frontRightSensor
        sensorStartPos += transform.right * frontSideSensorPos;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.blue);
                avoiding = true;
                avoidMultiplier -= 1f;
            }
        }


        //frontRightAngleSensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.green);
                avoiding = true;
                avoidMultiplier -= .5f;
            }
        }


        //frontLeftSensor
        sensorStartPos -= transform.right * frontSideSensorPos * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.red);
                avoiding = true;
                avoidMultiplier += 1f;
            }
        }


        //frontLeftAngleSensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.yellow);
                avoiding = true;
                avoidMultiplier += .5f;
            }
        }


        //frontCenterSensor
        sensorStartPos += transform.right * frontSideSensorPos;
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    if (hit.normal.x < 0)
                    {
                        avoidMultiplier = -1;
                    }
                    else
                    {
                        avoidMultiplier = 1;
                    }
                }
            }
        }


        if (avoiding)
        {
            targetSteeringAngle = maxSteeringAngle * avoidMultiplier;
        }
    }

    private void ApplySteer()
    {
        if (avoiding) return;
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
        targetSteeringAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * WheelFL.radius * WheelFL.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed && !isBraking)
        {
            WheelFL.motorTorque = maxMotorTorque;
            WheelFR.motorTorque = maxMotorTorque;
        }
        else
        {
            WheelFL.motorTorque = 0;
            WheelFR.motorTorque = 0;
        }
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 5f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void Braking()
    {
        {
            if (isBraking)
            {
                brakeLight.enabled = true;
                WheelBL.brakeTorque = maxBrakeTorque;
                WheelBR.brakeTorque = maxBrakeTorque;
            }
            else
            {
                brakeLight.enabled = false;
                WheelBL.brakeTorque = 0;
                WheelBR.brakeTorque = 0;
            }
        }
    }

    private void LerpToSteerAngle()
    {
        FLSteer = WheelFL.steerAngle = Mathf.Lerp(WheelFL.steerAngle, targetSteeringAngle, Time.deltaTime * turnSpeed);
        FRSteer = WheelFR.steerAngle = Mathf.Lerp(WheelFL.steerAngle, targetSteeringAngle, Time.deltaTime * turnSpeed);
    }
}
