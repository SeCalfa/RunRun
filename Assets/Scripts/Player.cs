using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IPlayer
{
    #region Fields/Properties

    public static Player player; // Player singelton

    [SerializeField] private Spawner spawner = null;
    [SerializeField] private Transform _laser1 = null, _laser2 = null;
    [SerializeField] private MainUI mainUI = null;

    private Rigidbody _playerRB;
    private Vector3 _direction = Vector3.zero;
    private Vector3 _leftDir = new Vector3(-1, 0, 0), _rightDir = new Vector3(0, 0, 1);
    private float _speed = 2;
    private bool _isRunStarted = false;
    private Vector3 _startCameraPosition, _startCameraRotation;
    private Vector3 _cameraRightDirectionPos = new Vector3(0, 9.3f, -4), _cameraLeftDirectionPos = new Vector3(4, 9.3f, 0);
    private Vector3 _cameraRightDirectionRot = new Vector3(50, 350, 0), _cameraLeftDirectionRot = new Vector3(50, 280, 0);
    private float cameraLerpAlpha = 0;
    private bool isTappingOn = false;

    internal int _score = 0;
    internal bool canMove = false;
    internal bool isTapModeActive = false;

    public delegate void AddScore();
    public event AddScore addScore;
    public delegate void FirstTap();
    public event FirstTap firstTap;

    public Spawner GetSpawner { get { return spawner; } }

    #endregion

    #region MonoBehaviour Events

    private void Awake() => player = this;

    private void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
        _startCameraPosition = Camera.main.transform.localPosition;
        _startCameraRotation = Camera.main.transform.localRotation.eulerAngles;

        addScore += ScoreAdd; // Event subscription
    }

    private void FixedUpdate()
    {
        Move();
        TapMode();
    }

    private void Update()
    {
        RayCasting();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ScoreZone")
        {
            if (other.gameObject.GetComponent<ScoreZone>().GetPlatform.GetComponent<MeshRenderer>().materials[0].color == GetSpawner.GetWhiteMat.color)
            {
                isTappingOn = true;
                mainUI.GetTapText.gameObject.SetActive(true);
            }
            else if (other.gameObject.GetComponent<ScoreZone>().GetPlatform.GetComponent<MeshRenderer>().materials[0].color == GetSpawner.GetNormalMat.color)
            {
                isTappingOn = false;
                mainUI.GetTapText.gameObject.SetActive(false);
                addScore.Invoke();
            }
        }
    }

    #endregion

    #region Interface Implementation

    public void Tap()
    {
        if (!_isRunStarted && canMove && !isTappingOn)
        {
            _direction = spawner.GetFirstPlatform._random ? _leftDir : _rightDir;
            _isRunStarted = true;
            firstTap.Invoke();
        }
        else if (_isRunStarted && canMove && !isTappingOn)
        {
            if (_direction == _leftDir)
                _direction = _rightDir;
            else
                _direction = _leftDir;
        }
        else if (_isRunStarted && canMove && isTappingOn)
        {
            addScore.Invoke();
        }
    }

    public void Move() => _playerRB.MovePosition(transform.position + _direction * _speed * Time.fixedDeltaTime);

    public void RayCasting()
    {
        RaycastHit hit1;
        RaycastHit hit2;

        if (Physics.Raycast(_laser1.position, -Vector3.up, out hit1, 200))
        {
            if (hit1.distance > 2)
                LoseGame();
        }

        if (Physics.Raycast(_laser2.position, -Vector3.up, out hit2, 200))
        {
            if (hit2.distance > 2)
                LoseGame();
        }
    }

    public void TapMode()
    {
        if (isTapModeActive && isTappingOn)
        {
            cameraLerpAlpha = Mathf.Clamp(cameraLerpAlpha, 0, 1) + Time.fixedDeltaTime * 4;
        }
        else if (!isTappingOn)
        {
            cameraLerpAlpha = Mathf.Clamp(cameraLerpAlpha, 0, 1) - Time.fixedDeltaTime * 4;
        }

        if (GetSpawner.SpawnDirection)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(_startCameraPosition, _cameraRightDirectionPos, cameraLerpAlpha);
            Quaternion cameraRotation = Camera.main.transform.localRotation;
            cameraRotation.eulerAngles = Vector3.Lerp(_startCameraRotation, _cameraRightDirectionRot, cameraLerpAlpha);
            Camera.main.transform.localRotation = cameraRotation;
        }
        else
        {
            Camera.main.transform.localPosition = Vector3.Lerp(_startCameraPosition, _cameraLeftDirectionPos, cameraLerpAlpha);
            Quaternion cameraRotation = Camera.main.transform.localRotation;
            cameraRotation.eulerAngles = Vector3.Lerp(_startCameraRotation, _cameraLeftDirectionRot, cameraLerpAlpha);
            Camera.main.transform.localRotation = cameraRotation;
        }
    }

    #endregion

    private void ScoreAdd()
    {
        if(_score > PlayerPrefs.GetInt("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", _score);
        _score += 1;

        if (_score % 10 == 0)
            _speed += 0.05f;
    }

    private void LoseGame() => SceneManager.LoadScene("SampleScene");

}
