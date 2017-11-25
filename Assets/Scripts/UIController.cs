using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using VRStandardAssets.Common;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public UIFader introUI;//開始UI
    public UIFader outroUI;//結束UI
    public UIFader playerUI;//Visual按ctrl+c、+v會複製一行
    public Text totalScore;
    public Text hightScore;

    public IEnumerator ShowIntroUI()
    {
        yield return StartCoroutine(introUI.InteruptAndFadeIn());
    }
    public IEnumerator HideIntroUI()
    {
        yield return StartCoroutine(introUI.InteruptAndFadeOut());
    }
    public IEnumerator ShowOutroUI()
    {
        totalScore.text = SessionData.Score.ToString();//內建簡單資料庫 上面要有VRStandardAssets.Common;才能使用
        hightScore.text = SessionData.HighScore.ToString();
        yield return StartCoroutine(outroUI.InteruptAndFadeIn());
    }
    public IEnumerator HideOutroUI()
    {
        yield return StartCoroutine(outroUI.InteruptAndFadeOut());
    }
    public IEnumerator ShowPlayerUI()
    {
        yield return StartCoroutine(playerUI.InteruptAndFadeIn());
    }
    public IEnumerator HidePlayerUI()
    {
        yield return StartCoroutine(playerUI.InteruptAndFadeOut());
    }
}