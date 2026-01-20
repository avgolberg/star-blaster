using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float leftBoundPadding = 1f;
    [SerializeField] float rightBoundPadding = 1f;
    [SerializeField] float upBoundPadding = 3f;
    [SerializeField] float bottomBoundPadding = 1f;
    InputAction moveAction;
    Vector3 moveVector;
    Vector3 newPosition;
    Vector2 maxBounds;
    Vector2 minBounds;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.Enable();

        InitBounds();
    }

    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();
        newPosition = transform.position + moveVector * moveSpeed * Time.deltaTime;
        newPosition.x = Math.Clamp(newPosition.x, minBounds.x + leftBoundPadding, maxBounds.x - rightBoundPadding);
        newPosition.y = Math.Clamp(newPosition.y, minBounds.y + bottomBoundPadding, maxBounds.y - upBoundPadding);
        transform.position = newPosition;
    }
}
