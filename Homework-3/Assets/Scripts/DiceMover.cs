using UnityEngine;
public class DiceMover : MonoBehaviour
{
    [SerializeField, Range(1, 20)] private float maxPushingForce;
    [SerializeField, Range(1, 20)] private float maxTorque;
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnRoll()
    {
        var rollDirection = new Vector3(Random.Range(-maxPushingForce, maxPushingForce), 
            Random.Range(-maxPushingForce, maxPushingForce), 
            Random.Range(-maxPushingForce, maxPushingForce));
        var rotateDirection = new Vector3(Random.Range(-maxTorque, maxTorque), 
            Random.Range(-maxTorque, maxTorque), 
            Random.Range(-maxTorque, maxTorque));
        rb.AddForce(rollDirection, ForceMode.Impulse);
        rb.AddTorque(rotateDirection, ForceMode.Impulse);
    }
}
