using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] GameObject _circleRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
