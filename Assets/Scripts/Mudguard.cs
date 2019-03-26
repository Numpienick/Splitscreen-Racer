using UnityEngine;

public class Mudguard : MonoBehaviour
{
    public CarController carController;
    public AICarEngine AICarEngine;
    private Quaternion m_OriginalRotation;

    public bool FL;
    public bool FR;

    private void Start()
    {
        m_OriginalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (FL == true && !AICarEngine)
            transform.localRotation = m_OriginalRotation * Quaternion.Euler(0, carController.FLSteer, 0);

        if (FR == true && !AICarEngine)
            transform.localRotation = m_OriginalRotation * Quaternion.Euler(0, carController.FRSteer, 0);

        if (FL == true && !carController)
            transform.localRotation = m_OriginalRotation * Quaternion.Euler(0, AICarEngine.FLSteer, 0);

        if (FR == true && !carController)
            transform.localRotation = m_OriginalRotation * Quaternion.Euler(0, AICarEngine.FRSteer, 0);
    }
}