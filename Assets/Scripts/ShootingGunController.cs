using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.VR;
public class ShootingGunController : MonoBehaviour
{
    public AudioSource audioSource;
    public VRInput vrInput;
    public float defaultLineLength = 70f;
    public ParticleSystem flareParticle;
    public LineRenderer gunFlare;
    public float gunFlareVisibleSeconds = 0.07f;
    public Transform gunEnd;
    private void OnEnable()
    {
        vrInput.OnDown += HandleDown;//事件的用法+=
    }

    private void OnDisable()
    {
        vrInput.OnDown -= HandleDown;//有+=就要有-=
    }
    private void HandleDown()//方法
    {
        StartCoroutine(Fire());//設定條件等待,條件成立繼續執行
    }

    private IEnumerator Fire()
    {
        audioSource.Play();
        float lineLength = defaultLineLength;//雷射長度為與物體的距離
        //todo 判斷有無設到東西
        flareParticle.Play();
        gunFlare.enabled = true;
        yield return StartCoroutine(MoveLineRenderer(lineLength));//等條件完成才繼續往下走
        gunFlare.enabled = false;

    }
    private IEnumerator MoveLineRenderer(float lineLength)
    {
        float timer = 0f;
        while (timer < gunFlareVisibleSeconds)
        {
            gunFlare.SetPosition(0, gunEnd.position);//射線起點
            gunFlare.SetPosition(1, gunEnd.position + gunEnd.forward * lineLength);//gunEnd.forward槍管的正方向
            yield return null;//跳出
            timer += Time.deltaTime;
        }
    }
}
