using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


/**
 * This component moves a player controlled with a CharacterController using the keyboard.
 */
[RequireComponent(typeof(CharacterController))]
public class CharacterKeyboardMover : MonoBehaviour {
    [Tooltip("Speed of player keyboard-movement, in meters/second")] [SerializeField]
    float walkSpeed = 3.5f;
    private bool idle;
    private float speed;

    private CharacterController cc;

    private Actions actions;
    
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float runSpeed = 7f;

    [SerializeField] InputAction moveAction;
    [SerializeField] InputAction runAction = new InputAction(type: InputActionType.Button);
    
    Vector3 velocity = new Vector3(0, 0, 0);

    private void OnEnable() {
        moveAction.Enable();
        runAction.Enable();
    }

    private void OnDisable() {
        moveAction.Disable();
        runAction.Disable();
    }

    void OnValidate() {
        // Provide default bindings for the input actions.
        // Based on answer by DMGregory: https://gamedev.stackexchange.com/a/205345/18261
        if (moveAction == null)
            moveAction = new InputAction(type: InputActionType.Button);

        if (moveAction.bindings.Count == 0)
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");


        if (runAction == null){
            runAction = new InputAction(type: InputActionType.Button);
        }
        if (runAction.bindings.Count == 0) {// Add a default binding if none is present.
            runAction.AddBinding("<Keyboard>/leftShift");
          }
    }

    void Start() {
        cc = GetComponent<CharacterController>();
        actions = GetComponent<Actions>();
        idle = true;
    }


    void Update() {
        Vector3 movement = moveAction.ReadValue<Vector2>();
        if (runAction.ReadValue<float>() > 0f) {
            speed = runSpeed;
            actions.Run();

            velocity.x = movement.x * speed;
            velocity.z = movement.y * speed;
        }
        else {
            speed = walkSpeed;
            if (moveAction.ReadValue<Vector2>() == Vector2.zero) {
                velocity.x = 0f;
                velocity.z = 0f;
                if (!idle) {
                    actions.Stay();
                    idle = true;
                }
                
            }
            else {
                
                velocity.x = movement.x * speed;
                velocity.z = movement.y * speed;
                actions.Walk();
                idle = false;
            }
        }

        if (cc.isGrounded) {
            // Apply gravity only when the character is grounded.
            velocity.y = -gravity * Time.deltaTime;
        }
        else {
            // Apply gravity while the character is in the air.
            velocity.y -= gravity * Time.deltaTime;
        }

        // Move in the direction you look:
        velocity = transform.TransformDirection(velocity);
        cc.Move(velocity * Time.deltaTime);
    }
}