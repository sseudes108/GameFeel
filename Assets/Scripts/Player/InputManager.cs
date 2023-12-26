using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public FrameInput FrameInput;
    private InputActions _inputActions;
    private InputAction _move, _jump, _jetPack, _granade;
    
    private void Awake() {
        _inputActions = new InputActions();

        _move = _inputActions.Inputs.Move;
        _jump = _inputActions.Inputs.Jump;
        _jetPack = _inputActions.Inputs.Jetpack;
        _granade = _inputActions.Inputs.Granade;
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
            JetPack = _jetPack.WasPressedThisFrame(),
            Granade = _granade.WasPressedThisFrame(),
            //JetPack = _jetPack.WasPerformedThisFrame(),
        };
    }
}

public struct FrameInput{
    public Vector2 Move;
    public bool Jump;
    public bool JetPack;
    public bool Granade;
}
