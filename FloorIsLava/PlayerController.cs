using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Speed and Acceleration
    [Header("Speed Variables")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] [Range(0, 1)] private float _airborneAccelerationTime = .2f;
    [SerializeField] [Range(0, 1)] private float _groundedAccelerationTime = .2f;
    #endregion

    #region Jump and Gravity Variables
    [Space]
    [SerializeField] private float _currentGravity;
    [SerializeField] private float _groundedGravity = 0f;
    [SerializeField] private float _airGravity;
    [SerializeField] private float _maxJumpVelocity;
    [SerializeField] private float _minJumpVelocity;
    [Header("Player Feel Variables")]
    [SerializeField] private float _jumpDelay = .2f;
    private float _jumpTimeElapsed = 0f;
    private float _jumpPressRemember;
    [SerializeField] private float _jumpPressedTime = .2f;

    [Space]
    [Header("Jump Variables")]
    [SerializeField] private int _originalJumpAmount = 1;
    private int _currentJumpAmount;

    [SerializeField] private float _maxJumpHeight = 5f;
    [SerializeField] private float _maxJumpTime = .5f;
    [SerializeField] private float _minJumpHeight = 2f;
    [SerializeField] private float _gravityMultiplier = 2f;

    [SerializeField] private Vector2 _wallJumpVelocity;
    [SerializeField] private float _wallSlideGravity = 5f;
    #endregion

    #region Collision Info
    [Space]
    [Header("Collision Info")]
    [SerializeField] private LayerMask _collisionLayer;
    [SerializeField] private Color _debugColor = Color.red;
    [SerializeField] private Vector3 _rightPosition = new Vector3(0.5f, 0f, 0f);
    [SerializeField] private Vector3 _leftPosition = new Vector3(-0.5f, 0f, 0f);
    [SerializeField] private Vector3 _topPosition = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private Vector3 _bottomPosition = new Vector3(0f, -0.5f, 0f);
    [SerializeField] private float _collisionRadius = 0.5f;
    private bool _onWall;
    private bool _onLeftWall;
    private bool _onRightWall;
    private bool _above;
    private bool _below;
    private bool _onWallDetected;
    #endregion

    #region Private Variables
    private CharacterController _controller;
    private PlayerDeath _playerDeath;
    private bool _cameraInTransition = false;
    private bool _isDead;
    private int _faceDirection = 1;
    public float _horizontalInput;
    private Vector2 _input;
    public Vector3 _velocity;
    private float _VelocityXSmoothing = .01f;
    private Vector2 _TempVelocity;
    #endregion

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerDeath = GetComponent<PlayerDeath>();
        _airGravity = -(_gravityMultiplier * _maxJumpHeight) / Mathf.Pow(_maxJumpTime, 2);
        _maxJumpVelocity = Mathf.Abs(_airGravity) * _maxJumpTime;
        _minJumpVelocity = Mathf.Sqrt(_gravityMultiplier * Mathf.Abs(_airGravity) * _minJumpHeight);
        _wallSlideGravity = _airGravity * .25f;
        _currentGravity = _airGravity;
        _currentJumpAmount = _originalJumpAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead || _cameraInTransition)
        {
            _velocity = Vector2.zero;
            return;
        }

        CollisionChecks();

        _horizontalInput = Input.GetAxis("Horizontal");
        _input = new Vector2(_horizontalInput, 0);

        if (_input.x > 0) _faceDirection = 1;
        else if (_input.x < 0) _faceDirection = -1;

        float targetVelocityX = _input.x * _speed;
        _velocity.x = Mathf.Lerp(_velocity.x, targetVelocityX, _below ? _groundedAccelerationTime :_airborneAccelerationTime);

        _jumpPressRemember -= Time.deltaTime;
        if (Input.GetButtonDown("Jump") )
        {
            _jumpPressRemember = _jumpPressedTime;
        }

        if (_jumpPressRemember > 0 && _currentJumpAmount > 0) Jump();

        // When the player is on the wall
        if(_jumpTimeElapsed > 0f)
        {
            _velocity.x = 0f;

            if(_horizontalInput != _faceDirection && _horizontalInput != 0)
            {
                _jumpTimeElapsed -= Time.deltaTime;
            }
            else
            {
                _jumpTimeElapsed = _jumpDelay;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            if(_velocity.y > _minJumpVelocity)
            {
                _velocity.y = _minJumpVelocity;
            }
        }

        _velocity.y += _currentGravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void Grounded()
    {
        _currentGravity = _groundedGravity;
        _currentJumpAmount = _originalJumpAmount;
    }

    private void Jump()
    {
        if (_onWall)
        {
            if (_onLeftWall)
            {
                _velocity.x = _wallJumpVelocity.x;
                _velocity.y = _wallJumpVelocity.y;
            }
            else if (_onRightWall)
            {
                _velocity.x = -_wallJumpVelocity.x;
                _velocity.y = _wallJumpVelocity.y;
            }
            else if(_onLeftWall && _onRightWall)
            {
                Debug.LogError("You have terrible level design!");
            }
        }
        else
        {
            _velocity.y = _maxJumpVelocity;
        }
        
        _currentJumpAmount--;
        _jumpPressRemember = 0f;
    }

    private void PlayerInAir()
    {
        _currentGravity = _airGravity;
    }

    private void PlayerOnWall()
    {
        _currentJumpAmount = _originalJumpAmount;

        if(_velocity.y > 0)
        {
            _currentGravity = _airGravity * 1.5f;
        }
        else
        {
            _currentGravity = _wallSlideGravity;
        }
        
    }

    private void CollisionChecks()
    {
        Vector3 bottomPos = _bottomPosition;
        bottomPos.x += transform.position.x;
        bottomPos.y += transform.position.y;
        _below = Physics.CheckSphere(bottomPos, _collisionRadius, _collisionLayer);

        Vector3 topPos = _topPosition;
        topPos.x += transform.position.x;
        topPos.y += transform.position.y;
        _above = Physics.CheckSphere(topPos, _collisionRadius, _collisionLayer);

        Vector3 rightPos = _rightPosition;
        rightPos.x += transform.position.x;
        rightPos.y += transform.position.y;
        _onRightWall = Physics.CheckSphere(rightPos, _collisionRadius, _collisionLayer);

        Vector3 leftPos = _leftPosition;
        leftPos.x += transform.position.x;
        leftPos.y += transform.position.y;

        _onLeftWall = Physics.CheckSphere(leftPos, _collisionRadius, _collisionLayer);

        _onWall = _onRightWall || _onLeftWall;

        if (_below) Grounded();

        if(!_below && !_above && !_onWall) PlayerInAir();

        if (_onWall)
        {
            PlayerOnWall();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _debugColor;

        Vector3 bottomPos = _bottomPosition;
        bottomPos.x += transform.position.x;
        bottomPos.y += transform.position.y;

        Gizmos.DrawWireSphere(bottomPos, _collisionRadius);

        Vector3 topPos = _topPosition;
        topPos.x += transform.position.x;
        topPos.y += transform.position.y;

        Gizmos.DrawWireSphere(topPos, _collisionRadius);

        Vector3 rightPos = _rightPosition;
        rightPos.x += transform.position.x;
        rightPos.y += transform.position.y;

        Gizmos.DrawWireSphere(rightPos, _collisionRadius);

        Vector3 leftPos = _leftPosition;
        leftPos.x += transform.position.x;
        leftPos.y += transform.position.y;

        Gizmos.DrawWireSphere(leftPos, _collisionRadius);
    }

    public void PlayerDied()
    {
        _playerDeath.PlayerRespawn();
        _isDead = true;

    }

    public void PlayerRespawn()
    {
        _isDead = false;
    }

    public void SetCameraTransition(bool cameraTransitioning)
    {
        _cameraInTransition = cameraTransitioning;
    }

    public void AddJump()
    {
        _currentJumpAmount++;
    }
}
