using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public FrameInput FrameInput;
    private InputActions _inputActions;
    private InputAction _move;
    private InputAction _jump;
    
    private void Awake() {
        _inputActions = new InputActions();

        _move = _inputActions.Inputs.Move;
        _jump = _inputActions.Inputs.Jump;
    }

    private void OnEnable() {
        _inputActions.Enable();
    }
    private void OnDisable() {
        _inputActions.Disable();
    }

    void Update(){
        FrameInput = GatherInput();
    }

    private FrameInput GatherInput(){
        return new FrameInput{
            Move = _move.ReadValue<Vector2>(),
            Jump = _jump.WasPressedThisFrame(),
        };
    }
}

public struct FrameInput{
    public Vector2 Move;
    public bool Jump;
}
