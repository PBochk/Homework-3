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
        bindingPath.text = action.bindings[0].effectivePath;
    }

    public void OnRebind()
    {
        bindingPath.text = "Press any key";
        action.Disable();
        action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(operation =>
            {
                bindingPath.text = action.bindings[0].effectivePath;
                action.Enable();
                operation.Dispose();
            })
            .Start();
    }
}










//var rebindOperation = myAction.PerformInteractiveRebinding()
//    .WithControlsExcluding("Mouse") // Exclude mouse input during rebinding
//    .OnMatchWaitForAnother(0.1f) // Wait a bit if multiple inputs are detected
//    .OnComplete(operation => {
//        // Binding complete, save the new binding path
//        Debug.Log($"Rebound to: {myAction.bindings[0].effectivePath}");
//        operation.Dispose(); // Clean up the operation
//    })
//    .OnCancel(operation => {
//        Debug.Log("Rebinding cancelled.");
//        operation.Dispose();
//    })
//    .Start(); // Start the interactive rebinding process