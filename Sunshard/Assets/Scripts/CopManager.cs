using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopManager : MonoBehaviour {
    private GameObject _cam;
    [SerializeField]
    private GameObject _bullet;
    private GameObject _bulletInstance;
    [SerializeField]
    private GameObject _gun;
    [SerializeField]
    private float _bulletSpeed;
    private float _shootTime;
    [SerializeField]
    private float _shootFreq;
    private AudioSource[] _sounds;
    [SerializeField]
    private float _recyclingPoint;

    // Use this for initialization
    void Start () {
        _cam = Camera.main.gameObject;

        _shootTime = Time.time + _shootFreq/2;
        _sounds = transform.GetComponents<AudioSource>();
        _bulletInstance = Instantiate(_bullet, _gun.transform.position, Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
        

        if (Time.time > _shootTime)
        {
            Shoot();
            _shootTime = Time.time + _shootFreq;
        }
        if (transform.position.x < _cam.transform.position.x - _recyclingPoint)
        {
            Die();
        }
    }
    private void Shoot()
    {
        _sounds[0].Play();
        _bulletInstance.transform.position = _gun.transform.position;
        float a = Random.Range(170.0f,210.0f);
        Vector2 v = DegreeToVector2(a) * _bulletSpeed;
        _bulletInstance.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        _bulletInstance.transform.GetComponent<Rigidbody2D>().AddForce(v);

    }
    private static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    private static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
    private void Die()
    {
        Destroy(_bulletInstance);
        Destroy(gameObject);
    }
}
