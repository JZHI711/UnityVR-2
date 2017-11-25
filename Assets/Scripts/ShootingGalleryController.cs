using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;
using UnityEngine.UI;//namespace 命名空間
public class ShootingGalleryController : MonoBehaviour//Class類別
{
    public UIController uiController;//Field 欄位
    public Reticle reticle;
    public SelectionRadial selectionRadial;
    public SelectionSlider selectionSlider;

    public Image timerBar;
    public float gameDuration=30f;
    public float endDelay =1.5f;
    public bool IsPlaying//Property屬性  判斷遊戲正在玩還是結束(只有自己可以更改，外部只能讀取)
    {
        private set;
        get;
    }
    private IEnumerator Start()//Method 方法
    {
        SessionData.SetGameType(SessionData.GameType.SHOOTER180);
        while (true)//無窮迴圈
        {
            Debug.Log("Start StartPhase");
            yield return StartCoroutine(StartPhase());
            Debug.Log("Start PlayerPhase");
            yield return StartCoroutine(PlayPhase());
            Debug.Log("Start EndPhase");
            yield return StartCoroutine(EndPhase());
            Debug.Log("Complete");


        }

    }
    private IEnumerator StartPhase()
    {
        yield return StartCoroutine(uiController.ShowIntroUI());
        reticle.Show();
        selectionRadial.Hide();
        yield return StartCoroutine(selectionSlider.WaitForBarToFill());
        yield return StartCoroutine(uiController.HideIntroUI());
        
    }
    private IEnumerator PlayPhase()
    {
        yield return StartCoroutine(uiController.ShowPlayerUI());
        IsPlaying = true;
        reticle.Show();
        SessionData.Restart();
        float gameTimer = gameDuration;
        while (gameTimer > 0f)
        {
            yield return null;//停 一 ㄓㄣ-
            gameTimer -= Time.deltaTime;
            timerBar.fillAmount = gameTimer / gameDuration;
        }
        IsPlaying = false;
        yield return StartCoroutine(uiController.HidePlayerUI());
    }
    public IEnumerator EndPhase()
    {
        reticle.Hide();
        yield return StartCoroutine(uiController.ShowOutroUI());
        yield return new WaitForSeconds(endDelay);
        yield return StartCoroutine(selectionRadial.WaitForSelectionRadialToFill());
        yield return StartCoroutine(uiController.HideOutroUI());
    }
}
