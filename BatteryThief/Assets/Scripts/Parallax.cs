using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject _cam;

    [SerializeField]
    private float _recyclingPoint;


    // Use this for initialization
    void Start()
    {
        _cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < _cam.transform.position.x - _recyclingPoint)
        {
            transform.position = new Vector3(transform.position.x + (5.8613f * 12), transform.position.y, transform.position.z);
        }
    }
}
