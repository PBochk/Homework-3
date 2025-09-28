using UnityEngine;

public class Side : MonoBehaviour
{
    [SerializeField] private SideNumber Number;
    public bool IsBottom { get; private set; }

    public int GetNumber() => (int)Number;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ScoreManager>(out _))
        {
            IsBottom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ScoreManager>(out _))
        {
            IsBottom = false;
        }
    }
}

