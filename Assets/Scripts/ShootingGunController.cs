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
    public Transform cameraTransform;
    public Reticle reticle; //準心
    public Transform gunContainer;//槍本身的Transform
    public float damping = 0.5f;//手追攝影機的參數
    public float dampingCoef = -20f;
    public float gunContainerSmooth = 10f;
    public ShootingGalleryController shootingGalleryController;
    public VREyeRaycaster eyeRaycaster;
    //------------------------------//
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
        if (shootingGalleryController.IsPlaying == false)
            return;
        //------------------------------
        ShootingTarget shootingTarget =eyeRaycaster.CurrentInteractible ? eyeRaycaster.CurrentInteractible.GetComponent<ShootingTarget>():null;
        /*
        等同於
        if(eyeRaycaster.CurrentInteractible){
        eyeRaycaster.CurrentInteractible.GetComponent<ShootingTarget>();
        else
        return null;
        }
        */
        Transform target = shootingTarget ? shootingTarget.transform:null;
        StartCoroutine(Fire(target));//設定條件等待,條件成立繼續執行
        //------------------------------
    }

    private IEnumerator Fire(Transform target)
    {
        audioSource.Play();
        float lineLength = defaultLineLength;//雷射長度
        //todo 判斷有無設到東西
        if (target != null) {
            lineLength = Vector3.Distance(gunEnd.position,target.position);
        }
        flareParticle.Play();//粒子
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
    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, InputTracking.GetLocalRotation(VRNode.Head), damping * (1 - Mathf.Exp(dampingCoef * Time.deltaTime)));//InputTracking:API;旋轉
        transform.position = cameraTransform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(reticle.ReticleTransform.position - gunContainer.position);//手隨看的方向旋轉
        gunContainer.rotation = Quaternion.Slerp(gunContainer.rotation,lookAtRotation,gunContainerSmooth*Time.deltaTime);
    }
}
