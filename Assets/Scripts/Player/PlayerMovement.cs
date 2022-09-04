using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct Cmd
{
    public float forwardMove;
    public float rightMove;
    public float upMove;
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouse")]
    [SerializeField] private Transform playerView;     // Camera
    [SerializeField] private float playerViewYOffset = 0.6f; // The height at which the camera is bound to
    [SerializeField] private float xMouseSensitivity = 30.0f;
    [SerializeField] private float yMouseSensitivity = 30.0f;
    
    [Header("physics")]
    [SerializeField] private float gravity = 20.0f;
    [SerializeField] private float friction = 6; //Ground friction

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7.0f;                // Ground move speed
    [SerializeField] private float runAcceleration = 14.0f;         // Ground accel
    [SerializeField] private float runDeacceleration = 10.0f;       // Deacceleration that occurs when running on the ground                 
    [SerializeField] private float sideStrafeAcceleration = 50.0f;  // How fast acceleration occurs to get up to sideStrafeSpeed when
    [SerializeField] private float sideStrafeSpeed = 1.0f;          // What the max speed to generate when side strafing

    [Header("Jump")]
    [SerializeField] private float airAcceleration = 2.0f;          // Air accel
    [SerializeField] private float airDecceleration = 2.0f;         // Deacceleration experienced when ooposite strafing
    [SerializeField] private float airControl = 0.3f;               // How precise air control is
    [SerializeField] private float jumpSpeed = 8.0f;                // The speed at which the character's up axis gains when hitting jump
    [SerializeField] private bool holdJumpToBhop = false;           // When enabled allows player to just hold jump button to keep on bhopping perfectly. Beware: smells like casual.

    [SerializeField] private Text speedTxt;
    [SerializeField] private Text TopSpeedTxt;
    [SerializeField] private Text fpsTxt;

    [SerializeField] private float fpsDisplayRate = 4.0f;

    private int _frameCount = 0;
    private float _dt = 0.0f;
    private float _fps = 0.0f;

    private CharacterController _controller;

    // Camera rotations
    private float _rotX = 0.0f;
    private float _rotY = 0.0f;

    private Vector3 _moveDirectionNorm = Vector3.zero;
    private Vector3 _playerVelocity = Vector3.zero;
    private float _playerTopVelocity = 0.0f;

    private bool _wishJump = false;

    private float _playerFriction = 0.0f;

    private Cmd _cmd;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerView == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                playerView = mainCamera.gameObject.transform;
        }

        playerView.position = new Vector3(transform.position.x, transform.position.y + playerViewYOffset, transform.position.z);

        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _frameCount++;
        _dt += Time.deltaTime;

        if (_dt > 1.0 / fpsDisplayRate)
        {
            _fps = Mathf.Round(_frameCount / _dt);
            _frameCount = 0;
            _dt -= 1.0f / fpsDisplayRate;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (Input.GetButtonDown("Fire1")) Cursor.lockState = CursorLockMode.Locked;
        }

        _rotX -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity * 0.02f;
        _rotY += Input.GetAxisRaw("Mouse X") * yMouseSensitivity * 0.02f;

        if (_rotX < -90) 
        { 
            _rotX = -90; 
        }
        else if (_rotX > 90) 
        { 
            _rotX = 90; 
        }

        this.transform.rotation = Quaternion.Euler(0, _rotY, 0); // collider
        playerView.rotation = Quaternion.Euler(_rotX, _rotY, 0); // camera

        QueueJump();
        if (_controller.isGrounded) GroundMove();
        else if (!_controller.isGrounded) AirMove();

        _controller.Move(_playerVelocity * Time.deltaTime);

        Vector3 udp = _playerVelocity;
        udp.y = 0.0f;
        if (udp.magnitude > _playerTopVelocity)
        {
            _playerTopVelocity = udp.magnitude;
        }

        playerView.position = new Vector3(transform.position.x, transform.position.y + playerViewYOffset, transform.position.z);

        UpdateUI();
    }

    private void SetMovementDir()
    {
        _cmd.forwardMove = Input.GetAxisRaw("Vertical");
        _cmd.rightMove = Input.GetAxisRaw("Horizontal");
    }

    private void QueueJump()
    {
        if (holdJumpToBhop)
        {
            _wishJump = Input.GetButton("Jump");
            return;
        }

        if (Input.GetButtonDown("Jump") && !_wishJump)
        {
            _wishJump = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            _wishJump = false;
        }
    }

    private void AirMove()
    {
        Vector3 wishdir;
        float wishvel = airAcceleration;
        float accel;

        SetMovementDir();

        wishdir = new Vector3(_cmd.rightMove, 0, _cmd.forwardMove);
        wishdir = transform.TransformDirection(wishdir);

        float wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        wishdir.Normalize();
        _moveDirectionNorm = wishdir;

        float wishspeed2 = wishspeed;

        if (Vector3.Dot(_playerVelocity, wishdir) < 0)
        {
            accel = airDecceleration;
        }
        else
        {
            accel = airAcceleration;
        }

        if (_cmd.forwardMove == 0 && _cmd.rightMove != 0)
        {
            if (wishspeed > sideStrafeSpeed)
            {
                wishspeed = sideStrafeSpeed;
            }
            accel = sideStrafeAcceleration;
        }

        Accelerate(wishdir, wishspeed, accel);
        if (airControl > 0)
        {
            AirControl(wishdir, wishspeed2);
        }

        _playerVelocity.y -= gravity * Time.deltaTime;
    }

    private void AirControl(Vector3 wishdir, float wishspeed)
    {
        float zspeed;
        float speed;
        float dot;
        float k;

        if (Mathf.Abs(_cmd.forwardMove) < 0.001 || Mathf.Abs(wishspeed) < 0.001) return;

        zspeed = _playerVelocity.y;
        _playerVelocity.y = 0;
        speed = _playerVelocity.magnitude;

        _playerVelocity.Normalize();

        dot = Vector3.Dot(_playerVelocity, wishdir);
        k = 32;
        k *= airControl * dot * dot * Time.deltaTime;

        if (dot > 0)
        {
            _playerVelocity.x = _playerVelocity.x * speed + wishdir.x * k;
            _playerVelocity.y = _playerVelocity.y * speed + wishdir.y * k;
            _playerVelocity.z = _playerVelocity.z * speed + wishdir.z * k;

            _playerVelocity.Normalize();
            _moveDirectionNorm = _playerVelocity;
        }

        _playerVelocity.x *= speed;
        _playerVelocity.y = zspeed; 
        _playerVelocity.z *= speed;
    }

    private void GroundMove()
    {
        Vector3 wishdir;

        if (!_wishJump)
        {
            ApplyFriction(1.0f);
        }
        else
        {
            ApplyFriction(0);
        }

        SetMovementDir();

        wishdir = new Vector3(_cmd.rightMove, 0, _cmd.forwardMove);
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();

        _moveDirectionNorm = wishdir;

        var wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        Accelerate(wishdir, wishspeed, runAcceleration);

        _playerVelocity.y = -gravity * Time.deltaTime;

        if (_wishJump)
        {
            _playerVelocity.y = jumpSpeed;
            _wishJump = false;
        }
    }

    private void ApplyFriction(float t)
    {
        Vector3 vec = _playerVelocity;

        float speed;
        float newspeed;
        float control;
        float drop;

        vec.y = 0.0f;
        speed = vec.magnitude;
        drop = 0.0f;

        if (_controller.isGrounded)
        {
            control = speed < runDeacceleration ? runDeacceleration : speed;
            drop = control * friction * Time.deltaTime * t;
        }

        newspeed = speed - drop;
        _playerFriction = newspeed;

        if (newspeed < 0)
        {
            newspeed = 0;
        }

        if (speed > 0) 
        { 
            newspeed /= speed; 
        }

        _playerVelocity.x *= newspeed;
        _playerVelocity.z *= newspeed;
    }

    private void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float addspeed;
        float accelspeed;
        float currentspeed;

        currentspeed = Vector3.Dot(_playerVelocity, wishdir);

        addspeed = wishspeed - currentspeed;

        if (addspeed <= 0) return;

        accelspeed = accel * Time.deltaTime * wishspeed;

        if (accelspeed > addspeed)
        {
            accelspeed = addspeed;
        }

        _playerVelocity.x += accelspeed * wishdir.x;
        _playerVelocity.z += accelspeed * wishdir.z;
    }

    public void AddPerk()
    {
        moveSpeed *= 1.25f;
    }

    private void UpdateUI()
    {
        var ups = _controller.velocity;
        ups.y = 0;

        speedTxt.text = "Speed: " + Mathf.Round(ups.magnitude * 100) / 100 + "ups";
        TopSpeedTxt.text = "Top Speed: " + Mathf.Round(_playerTopVelocity * 100) / 100 + "ups";
        fpsTxt.text = "FPS: " + _fps;
    }
}
