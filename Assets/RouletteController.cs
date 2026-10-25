using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class RouletteController : MonoBehaviour
{
    [Header("旋轉設定")]
    public float totalDuration = 3f;
    public float maxSpinSpeed = 1000f;
    
    [Tooltip("控制轉速的曲線。X軸為正規化時間(0~1)，Y軸為速度比例(0~1)")]
    public AnimationCurve speedCurve;

    [Header("組件綁定")]
    public AudioSource spinAudio;

    private bool isSpinning = false;

    // 透過鼠標點擊觸發
    private void OnMouseDown()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinRoutine());
        }
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        float elapsedTime = 0f;

        if (spinAudio != null) 
        {
            spinAudio.Play();
        }

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // 計算正規化時間 (0.0 到 1.0)
            float normalizedTime = elapsedTime / totalDuration;
            
            // 透過 AnimationCurve 取得當前時間點的速度倍率
            float currentSpeed = speedCurve.Evaluate(normalizedTime) * maxSpinSpeed;

            // 執行 Z 軸旋轉 (負值為順時針旋轉)
            transform.Rotate(0, 0, -currentSpeed * Time.deltaTime);

            yield return null;
        }

        if (spinAudio != null) 
        {
            spinAudio.Stop();
        }
        
        isSpinning = false;
    }
}