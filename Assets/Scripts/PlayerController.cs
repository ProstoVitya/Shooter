using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _speed = 6f;
    private float _jumpSpeed = 8f;
    private float _mouseSensitivity = 100f;
    private float _gravity = 20f;

    private Vector3 _moveDirection = Vector3.zero;

    private CharacterController _controller;
    private Transform _playerCamera;

    private float _xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponent<CharacterController>();
        _playerCamera = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        MouseLook();
        Move();
    }

    private void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Move()
    {
        _moveDirection = new Vector3(Input.GetAxis("Horizontal") * _speed,
                           _moveDirection.y, Input.GetAxis("Vertical") * _speed);

        _moveDirection = transform.TransformDirection(_moveDirection);

        if (_controller.isGrounded)
            _moveDirection.y = Input.GetButton("Jump") ? _jumpSpeed : 0;

        _moveDirection.y -= _gravity * Time.deltaTime;
        _controller.Move(_moveDirection * Time.deltaTime);
    }
}
