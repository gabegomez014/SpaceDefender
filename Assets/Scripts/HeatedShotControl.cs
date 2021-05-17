using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatedShotControl : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private AudioClip _explosionSFX;

    private Animator _animator;
    private UIManager _uiManager;
    private AudioSource _audioSource;

    void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_uiManager == null)
        {
            Debug.LogWarning("Could not find the UI Manager script");
        }

        if (_animator == null)
        {
            Debug.LogWarning("Could not find the animator script");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _speed);
    }

    public void Explode()
    {
        _uiManager.UpdateScore();
        _speed = 0;
        _animator.SetBool("explode", true);
        _audioSource.PlayOneShot(_explosionSFX);
        Destroy(this.gameObject, 2.0f);
    }

}
