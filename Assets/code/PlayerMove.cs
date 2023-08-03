using code;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{

    //[SerializeField] private InputActionAsset _inputActionAsset;
    [AutofillBehavior] private Rigidbody _rigidbody;
    private InputAction _moveAction;
    private InputAction _lookAction;
    [SerializeField] 
    [Range(0f, float.MaxValue)]
    private float _speed = 5f;

    [SerializeField] private Camera _camera;
    
    protected void Start()
    {
        this.AutofillAttributes();
        _moveAction = InputManager.GetInputAction("GAME/Movement");
        _lookAction = InputManager.GetInputAction("GAME/Look");
        InputManager.QuickAddInput("DEBUG/ToggleFocus", FocusToggle);
        InputManager.QuickAddInput("DEBUG/ToggleConsole", ConsoleToggle);

    }

    /*
    private void MovementEvent(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Fired");
        var vec = ctx.ReadValue<Vector2>();
        Debug.Log($"({vec.x}, {vec.y})");
        //Vector3 vector3 = vec;
        
    }
    */

    private void FocusToggle(InputAction.CallbackContext context)
    {
        InputManager.Focus = InputManager.Focus switch
        {
            "GAME" => "UI",
            "UI" => "GAME",
            _ => InputManager.Focus
        };
    }

    private void ConsoleToggle(InputAction.CallbackContext context)
    {
        UIManager.ToggleConsole();
    }
    
    void Update()
    {
        //_rigidbody.AddForce();
        Vector2 input = _lookAction.ReadValue<Vector2>();
        transform.Rotate(0, input.x, 0);
        _camera.transform.Rotate(-input.y, 0, 0);
        
    }

    private void DoLook()
    {
        Vector3 output = Vector3.zero;
        Vector2 input = _moveAction.ReadValue<Vector2>();
        input.x *= _speed;
        input.y *= _speed;

        output.x = input.x;
        output.z = input.y;
        
        //_camera.transform.Rotate(input.);
    }

    private void FixedUpdate()
    {
        DoMovement();
    }

    private void DoMovement()
    {
        Vector3 output = Vector3.zero;
        Vector2 input = _moveAction.ReadValue<Vector2>();
        input.x *= _speed;
        input.y *= _speed;

        output.x = input.x;
        output.z = input.y;

        //output *= _rigidbody.rotation;
        //_rigidbody.rotation.
        //_rigidbody.velocity = output;
        _rigidbody.AddRelativeForce(output);
    }
}
