using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRecycling : MonoBehaviour {
    private GameObject _cam;
 
    private ObjectPool _pool;
    [SerializeField]
    private float _recyclingPoint;

    // Use this for initialization
    void Start () {
        _cam = Camera.main.gameObject;
        _pool = GameObject.Find("GameManager").transform.GetComponent<ObjectPool>();
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.x < _cam.transform.position.x - _recyclingPoint)
        {
            _pool.ReturnObject(transform.gameObject);
        }
	}
}
