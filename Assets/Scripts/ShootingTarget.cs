using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VRStandardAssets.Utils;
using VRStandardAssets.Common;

public class ShootingTarget : MonoBehaviour
{
    public int score = 1;
    public float destroyTimeOutDuration = 2f;
    public event Action<ShootingTarget> onRemove;//event Action<事件必須符合的參數> 名稱;
    public float timeOutDuration = 2f;
    private Transform cameraTransform;
    private AudioSource audioSource;
    private VRInteractiveItem vrInteractiveItem;
    private Renderer mRenderer;
    private Collider mCollider;
    public AudioClip destroyClip;//音效
    public GameObject destroyPrefab;
    private bool isEnding;
    public AudioClip spawnClip;
    public AudioClip missedClip;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
        vrInteractiveItem = GetComponent<VRInteractiveItem>();
        mRenderer = GetComponent<Renderer>();
        mCollider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        vrInteractiveItem.OnDown +=HandleDown;
    }
    private void OnDisable()
    {
        vrInteractiveItem.OnDown -= HandleDown;
    }
    private void OnDestroy()
    {
        onRemove = null;
    }
    private void HandleDown()
    {
        StartCoroutine(OnHit());
    }
    private IEnumerator OnHit()
    {
        if (isEnding)
        yield break;
        isEnding = true;
        mRenderer.enabled = false;
        mCollider.enabled = false;
        audioSource.clip = destroyClip;
        audioSource.Play();
        SessionData.AddScore(score);
        GameObject desroyedTarget = Instantiate<GameObject>(destroyPrefab, transform.position, transform.rotation);//Instantiate實例化物件
        Destroy(desroyedTarget, destroyTimeOutDuration);
        yield return new WaitForSeconds(destroyClip.length);
        if (onRemove!= null)
        {
            onRemove(this);//因為上面的Action<ShootingTarget> onRemove;
        }
    }
    public void Restart(float gameTimeRemaining)
    {
        mRenderer.enabled = true;
        mCollider.enabled = true;
        isEnding = false;
        audioSource.clip = spawnClip;
        audioSource.Play();
        transform.LookAt(cameraTransform.position);
        StartCoroutine(MissTarget());
        StartCoroutine(GameOver(gameTimeRemaining));

    }

    private IEnumerator MissTarget()
    {
        yield return new WaitForSeconds(timeOutDuration);
        if (isEnding)
            yield break;
        isEnding = true;
        mRenderer.enabled = false;
        mCollider.enabled = false;
        audioSource.clip = missedClip;
        audioSource.Play();
        yield return new WaitForSeconds(missedClip.length);
        if (onRemove != null)
        
            onRemove(this);
        
    }
    private IEnumerator GameOver(float gameTimeRemaining)
    {
        yield return new WaitForSeconds(gameTimeRemaining);
        if (isEnding) {
            yield break;
          
        }
        isEnding = true;
        mRenderer.enabled = false;
        mCollider.enabled = false;
        if (onRemove != null)
        {
            onRemove(this);

        }
    }
}
