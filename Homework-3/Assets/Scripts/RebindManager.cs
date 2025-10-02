using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class RebindManager : MonoBehaviour
{
    [SerializeField] private TMP_Text bindingPath;
    private PlayerInput playerInput;
    private InputAction action;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        action = playerInput.actions["Roll"];
        //bindingPath.text = action.bindings[0].effectivePath;
    }

    public void OnRebind()
    {
        //bindingPath.text = "Press any key";
        action.Disable();
        action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(operation =>
            {
                //bindingPath.text = action.bindings[0].effectivePath;
                action.Enable();
                operation.Dispose();
            })
            .Start();
    }
}



