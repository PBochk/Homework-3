using NUnit.Framework;
using UnityEngine;
public class DiceMover : MonoBehaviour
{
    [SerializeField, UnityEngine.Range(5, 20)] private float maxPushingForce;
    [SerializeField, UnityEngine.Range(5, 20)] private float maxTorque;
    private const float MIN_PUSHING_FORCE = 5f;
    private Rigidbody rb;
    private int[] directionalCoeffitients = { -1, 1 };
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private int GetDirectionalCoefficient()
    {
        return directionalCoeffitients[Random.Range(0, 2)];
    }

    public void OnRoll()
    {
        var rollDirection = new Vector3(Random.Range(MIN_PUSHING_FORCE, maxPushingForce) * GetDirectionalCoefficient(), 
            Random.Range(MIN_PUSHING_FORCE, maxPushingForce) * GetDirectionalCoefficient(), 
            Random.Range(MIN_PUSHING_FORCE, maxPushingForce) * GetDirectionalCoefficient());
        var rotateDirection = new Vector3(Random.Range(-maxTorque, maxTorque), 
            Random.Range(-maxTorque, maxTorque), 
            Random.Range(-maxTorque, maxTorque));
        rb.AddForce(rollDirection, ForceMode.Impulse);
        rb.AddTorque(rotateDirection, ForceMode.Impulse);
    }
}
