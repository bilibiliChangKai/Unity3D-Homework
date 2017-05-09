using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.DoTween;
using My.DoTween.Core;

public class test3 : MonoBehaviour {
    // Use this for initialization
    void Start () {
        Tween tween = transform.DORotate(new Vector3(0, 0, 360), 2.5f).SetLoopTime(-1);
        transform.DOMoveY(10, 5).SetEaseType(My.LerpFunctionSpace.LerpFunctionType.SineBoth);
        Sequence se2 = MyDoTween.Sequence();
        se2.Append(transform.DOMoveX(5, 2.5f).SetEaseType(My.LerpFunctionSpace.LerpFunctionType.SineOut))
            .Append(transform.DOMoveX(0, 2.5f).SetEaseType(My.LerpFunctionSpace.LerpFunctionType.SineIn))
            .AppendInterval(1f)
            .AppendCallback((t, u) =>
            {
                tween.Stop();
                transform.DOScale(3, 1);
                transform.DOMoveY(0, 1.5f).SetEaseType(My.LerpFunctionSpace.LerpFunctionType.BounceOut);
            });
    }
	
	// Update is called once per frame
	void Update () {
	}
}
