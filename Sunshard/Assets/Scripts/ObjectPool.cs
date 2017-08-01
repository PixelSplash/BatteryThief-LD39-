using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    private GameObject[] _pool;

    [SerializeField]
    private GameObject _gameObject;
    
    [SerializeField]
    private int _maxSize;
    private int _count = 0;
    private GameObject _obj;

    // Use this for initialization
    void Start () {
        
        _pool = new GameObject[_maxSize];
        _count = _maxSize/2;
        for (int i = 0; i<_maxSize/2; i++)
        {
            _obj = Instantiate(_gameObject);
            _obj.SetActive(false);
            _pool[i] = _obj;

        }
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject GetObject(string name, Vector3 pos, Quaternion quat)
    {
        switch (name)
        {
            case "platform":

                if (_pool.Length > 0)
                {
                    _count--;
                    _obj = _pool[_count];
                    _pool[_count + 1] = null;
                    _obj.transform.position = pos;
                    _obj.transform.rotation = quat;
                    _obj.SetActive(true);
                    return _obj;
                }
                else { return null; }
            default:
                return null;
        }
        
        
    }
    public void ReturnObject( GameObject obj )
    {
        _pool[_count] = obj;
        _count++;
        obj.SetActive(false);
    }
}
