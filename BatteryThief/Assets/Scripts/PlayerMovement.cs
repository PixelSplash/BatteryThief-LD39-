using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerMovement : MonoBehaviour {
    public string FileName; // This contains the name of the file. Don't add the ".txt"
                            // Assign in inspector
    private TextAsset asset; // Gets assigned through code. Reads the file.
    private StreamWriter writer; // This is the writer that writes to the file
    [SerializeField]
    private TextAsset _scoreFile;
    [SerializeField]
    private GameObject _energyBar;
    private Rigidbody2D _rig;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _velocity;
    public LayerMask groundLayer;
    private bool _canBounce;
    private float _canBounceTime;
    [SerializeField]
    private float _canBounceReload;
    [SerializeField]
    private int _maxCharge;
    private int _charge;
    private float _energyLoseTime;
    [SerializeField]
    private float _energyLoseFreq;
    [SerializeField]
    private UnityEngine.UI.Text _score;
    [SerializeField]
    private UnityEngine.UI.Text _highScore;
    private bool _canMove;
    private AudioSource[] _sounds;
    private Animator _animator;
    [SerializeField]
    private ParticleSystem _particles;
    [SerializeField]
    private float _deadDelay;
    private bool _dead;
    private float _deadTime;

    // Use this for initialization
    void Start () {
        _dead = false;
        _animator = transform.GetComponent<Animator>();
        _sounds = transform.GetComponents<AudioSource>();
        _charge = _maxCharge;
        _rig = transform.GetComponent<Rigidbody2D>();
        _canBounce = true;
        _energyLoseTime = Time.time + _energyLoseFreq;
        if (File.Exists("./Score.txt"))
        {
            _highScore.text = System.IO.File.ReadAllText("./Score.txt");
        }
        else
        {
            System.IO.File.WriteAllText("./Score.txt", "0");
            _highScore.text = "0";
        }

        _canMove = true;
        _rig.velocity = new Vector2(_velocity, _rig.velocity.y);
    }
	
	// Update is called once per frame
	void Update () {
        if (!_dead)
        {
            //Dead
            if (_charge <= 0 || transform.position.y < -0.17f)
            {
                
                //transform.Rotate(new Vector3(0, 0,90));
                
                _dead = true;

            }

            //Setting score UI
            _score.text = "" + (int)transform.position.x;

            //Running out of power
            if (Time.time > _energyLoseTime)
            {
                if (_charge > 0)
                {
                    _sounds[4].Play();
                    _charge--;
                    _energyBar.transform.localScale = new Vector2(_energyBar.transform.localScale.x, _charge);

                }
                _energyLoseTime = Time.time + _energyLoseFreq;
            }

            //constant velocity
            _rig.AddForce(Vector2.right * _velocity * 4);
            if (_rig.velocity.x > _velocity) _rig.velocity = new Vector2(_velocity, _rig.velocity.y);


            //Inputs
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //_highScore.text = _scoreFile.text;
                if (IsGrounded())
                {
                    _sounds[1].Play();
                    _rig.AddForce(Vector2.up * _jumpForce);
                }

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_charge > 0)
                {
                    _particles.Play();
                    ParticleSystem.EmissionModule em = _particles.emission;
                    em.enabled = true;
                    _sounds[2].Play();
                    _charge--;
                    _energyBar.transform.localScale = new Vector2(_energyBar.transform.localScale.x, _charge);
                    _rig.AddForce(Vector2.up * _jumpForce * 1.5f);
                    _canMove = true;
                }


            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {

            }

            //Audio
            if (IsGrounded())
            {
                if (!_sounds[0].isPlaying) _sounds[0].Play();
            }
            else
            {
                if (_sounds[0].isPlaying) _sounds[0].Stop();
            }

            //animation
            _animator.SetBool("isGrounded", IsGrounded());
            _animator.SetFloat("yVel", _rig.velocity.y);
        }
        else
        {
            if(_deadTime == 0)_deadTime = Time.time + _deadDelay;
            if (_rig != null)
            {
                _sounds[5].Play();
                Destroy(_rig);
                _rig = null;
                _animator.SetBool("isGrounded", false);
                _animator.SetFloat("yVel", -1);
               
            }
            if (Time.time > _deadTime) Die();


        }
        

    }



    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.4f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }
    bool IsStucked()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.right;
        float distance = 0.4f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "PowerUp")
        {
            _sounds[3].Play();
            if (_charge < _maxCharge)
            {
                
                _charge++;
                _energyBar.transform.localScale = new Vector2(_energyBar.transform.localScale.x,_charge);
            }
            Destroy(other.gameObject);
        }else if (other.tag == "Danger")
        {
            _dead = true;
           
        }
    }

    private void Die()
    {
        //Debug.Log((int)transform.position.x);
        //Debug.Log(IntParseFast(_scoreFile.text));
        
        if (IntParseFast(_highScore.text) < (int)transform.position.x)
        {
            System.IO.File.WriteAllText("./Score.txt", ((int)transform.position.x).ToString());
            //AppendString(((int)transform.position.x).ToString());
        }
        Application.LoadLevel(Application.loadedLevel);
    }
    public static int IntParseFast(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
           
            char letter = value[i];

            result = 10 * result + (letter - 48);
            //Debug.Log(letter);
            //Debug.Log(result);
        }
        return result;
    }

    void AppendString(string appendString)
    {
        asset = Resources.Load(FileName + ".txt") as TextAsset;
        writer = new StreamWriter("Assets/Resources/" + FileName + ".txt"); // Does this work?
        writer.WriteLine(appendString);
        writer.Close();
    }
}
