using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    private GameObject _cam;
    private ObjectPool _pool;
    [SerializeField]
    private GameObject _cop;
    [SerializeField]
    private GameObject _powerBall;
    private float _nextTime;
    [SerializeField]
    private float _platformFreq;
    [SerializeField]
    private float _creationPoint;
    private int _platformCount;
    private float _platformY;
    private float _rand;
    [SerializeField]
    private int _minPathLength;
    [SerializeField]
    private int _maxPathLength;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _pathChangeProb;
    private float _lastPlatformX;
    private float _newPlatformX;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _holeProb;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _highPathProb;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _poweBallProb;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _copFreq;

    // Use this for initialization
    void Start () {
        _platformY = 0;
        _platformCount = 10;
        _cam = Camera.main.gameObject;
        _nextTime = Time.time + _platformFreq;
        _pool = transform.GetComponent<ObjectPool>();
        _lastPlatformX = _cam.transform.position.x + _creationPoint;
    }
	
	// Update is called once per frame
	void Update () {
        


        if (Time.time > _nextTime)
        {
            _nextTime = Time.time;
            _newPlatformX = _cam.transform.position.x + _creationPoint;

            if (_newPlatformX >= _lastPlatformX + 0.32f)
            {
                

                
                _nextTime += +_platformFreq;
                _lastPlatformX += 0.32f;

                //PowerBall creation
                _rand = Random.value;
                if (_rand < _poweBallProb)
                {
                    Instantiate(_powerBall, new Vector2(_lastPlatformX, _platformY), Quaternion.identity);
                }

                //If is hole
                _rand = Random.value;
                if (_rand > _holeProb)
                {
                    //Cop creation
                    _rand = Random.value;
                    if(_rand < _copFreq)
                    {
                        Instantiate(_cop, new Vector2(_lastPlatformX, _platformY), Quaternion.identity);

                    }
                    _pool.GetObject("platform", new Vector2(_lastPlatformX, _platformY), Quaternion.identity);
                    
                }
                _platformCount--;

                if (_platformCount == 0)
                {
                    _rand = Random.value;
                    if (_rand > 1 - _pathChangeProb)
                    {
                        _rand = Random.value;
                        if(_rand < _highPathProb)
                        {
                            _platformY += 0.32f;
                        }
                        _platformY += 0.32f;
                    }
                    else if (_rand < 0 + _pathChangeProb)
                    {
                        _platformY -= 0.32f;
                        if (_platformY < 0) _platformY = 0;
                    }
                    _platformCount = (int)Random.Range(0.0f, _maxPathLength - _minPathLength) + _minPathLength;
                }
            }
            
        }
	}
}
