using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenTest : MonoBehaviour
{
    public bool isPunch = false;
    Sequence sequence;
    private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        sequence = DOTween.Sequence();

        sequence.Append(transform.DOMoveX(5, 2));
        sequence.Append(transform.DORotate(new Vector3(0, 0, 180), 2));
        sequence.Append(transform.DOScale(new Vector3(2, 2, 2), 2));
        sequence.SetLoops(-1, LoopType.Yoyo);

        // transform.DOMoveX(5, 2).SetEase(Ease.OutBounce).OnComplete(DeactivateObject);
        // transform.DOShakeRotation(2, new Vector3(0, 0, 180), 10, 90);
        
        renderer = GetComponent<Renderer>();
    }

    void DeactivateObject()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            sequence.Kill();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isPunch)
            {
                isPunch = true;
                transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.1f, 10, 1).OnComplete(EndPunch);

                Color color = new Color(Random.value, Random.value, Random.value);

                renderer.material.DOColor(color, 0.1f)
                    .SetEase(Ease.InOutQuad)
                    .SetAutoKill(false);

                renderer.material.DOPlay();
            }
        }
    }

    void EndPunch()
    {
        isPunch = false;
    }
}
