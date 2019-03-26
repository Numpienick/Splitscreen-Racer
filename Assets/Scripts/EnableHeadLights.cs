using UnityEngine;

public class EnableHeadLights : MonoBehaviour
{
    private Light myLight;
    [HideInInspector]
    public MeshRenderer glow;

    public bool mesh;
    public bool headLight;
    private bool headlightEnabled = false;

    private CarController carControllerScript;

    private bool braking = false;

    void Start()
    {
        myLight = GetComponent<Light>();
        glow = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (carControllerScript != null)
        {
            if (mesh == true && carControllerScript.Brake > 0 && !glow.enabled)
            {
                glow.enabled = true;
                braking = true;
            }
            if (mesh == true && carControllerScript.Brake <= 0 && braking == true && headlightEnabled == false)
                glow.enabled = false;

            if (mesh == true && carControllerScript.Brake <= 0 && braking == true)
                braking = false;

            if (Input.GetKeyDown(carControllerScript.enableLight))
            {
                headlightEnabled = !headlightEnabled;

                if (headLight == true)
                    myLight.enabled = !myLight.enabled;

                if (mesh == true)
                    glow.enabled = !glow.enabled;
            }
        }
    }

    public void GetCarController()
    {
        carControllerScript = transform.root.GetComponentInChildren<CarController>();
    }
}