using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] GameObject _circleRange;
    [SerializeField] CircleCollider2D _cicleCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCircleRange(float range)
    {
        _circleRange.transform.localScale = Vector2.one * 2f * range;
    }

    public void Show()
    {
        _circleRange.SetActive(true);
    }

    public void Hide()
    {
        _circleRange.SetActive(false);
    }
}
