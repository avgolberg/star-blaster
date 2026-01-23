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

    Shooter playerShooter;
    InputAction moveAction;
    InputAction fireAction;
    Vector3 moveVector;
    Vector3 newPosition;
    Vector2 maxBounds;
    Vector2 minBounds;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.Enable();

        playerShooter = GetComponent<Shooter>();
        fireAction = InputSystem.actions.FindAction("Fire");
        fireAction.Enable();

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
        MovePlayer();
        FireShooter();
    }

    void MovePlayer()
    {
        moveVector = moveAction.ReadValue<Vector2>();
        newPosition = transform.position + moveVector * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x + leftBoundPadding, maxBounds.x - rightBoundPadding);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y + bottomBoundPadding, maxBounds.y - upBoundPadding);
        transform.position = newPosition;
    }
    void FireShooter()
    {
        playerShooter.isFiring = fireAction.IsPressed();
    }
}
