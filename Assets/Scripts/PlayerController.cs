using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    InputAction moveAction;
    Vector3 moveVector;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.Enable();
    }

    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
}
