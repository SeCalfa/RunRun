using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IPlayer
{
    public static Player player; // Player singelton

    [SerializeField] private Spawner spawner = null;
    [SerializeField] private Transform _laser1 = null, _laser2 = null;

    private Rigidbody _playerRB;
    private Vector3 _direction = Vector3.zero;
    private Vector3 _leftDir = new Vector3(-1, 0, 0), _rightDir = new Vector3(0, 0, 1);
    private float _speed = 2;
    private bool _isRunStarted = false;
    private Vector3 _startPlayerPosition, _startCameraPosition;

    internal int _score = 0;

    public delegate void AddScore();
    public event AddScore addScore;

    public Spawner GetSpawner
    {
        get { return spawner; }
    }

    private void Awake()
    {
        player = this;
    }

    private void Start()
    {
        _playerRB = GetComponent<Rigidbody>();
        _startPlayerPosition = transform.position;
        _startCameraPosition = Camera.main.transform.position;

        addScore += ScoreAdd; // Event subscription
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Tap();
        RayCasting();
        CameraMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ScoreZone")
        {
            addScore.Invoke();
        }
    }

    private void ScoreAdd()
    {
        _score++;
        if(_score > PlayerPrefs.GetInt("MaxScore"))
        {

        }
    }

    public void Tap()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && !_isRunStarted)
            {
                _direction = spawner.GetFirstPlatform._random ? _leftDir : _rightDir;
                _isRunStarted = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Began && _isRunStarted)
            {
                if (_direction == _leftDir)
                    _direction = _rightDir;
                else
                    _direction = _leftDir;
            }
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && !_isRunStarted)
        {
            _direction = spawner.GetFirstPlatform._random ? _leftDir : _rightDir;
            _isRunStarted = true;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && _isRunStarted)
        {
            if (_direction == _leftDir)
                _direction = _rightDir;
            else
                _direction = _leftDir;
        }
    }

    public void Move() => _playerRB.MovePosition(transform.position + _direction * _speed * Time.fixedDeltaTime);

    public void CameraMove() => Camera.main.transform.position = transform.position - _startPlayerPosition + _startCameraPosition;

    public void RayCasting()
    {
        RaycastHit hit1;
        RaycastHit hit2;

        if (Physics.Raycast(_laser1.position, -Vector3.up, out hit1, 200))
        {
            if (hit1.distance > 2)
                SceneManager.LoadScene("SampleScene");
        }

        if (Physics.Raycast(_laser2.position, -Vector3.up, out hit2, 200))
        {
            if (hit2.distance > 2)
                SceneManager.LoadScene("SampleScene");
        }
    }


    private void LoseGame() => print("You lost");
}
