using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoverPreview : MonoBehaviour
{
    public Vector3 OriginalPos;
    public Vector3 TargetPosition;
    public float TargetScale;
    public GameObject PreviewGameObject;

    private static HoverPreview _currentlyViewing = null;

    private static bool _previewsAllowed = true;
    public static bool PreviewsAllowed { get => _previewsAllowed;
        set 
        {
            _previewsAllowed = value;
            if (!_previewsAllowed)
                StopAllPreviews();
        } 
    }

    private bool _thisPreviewEnabled = true;
    public bool ThisPreviewEnabled { get => _thisPreviewEnabled;
        set 
        {
            _thisPreviewEnabled = value;
            if (!_thisPreviewEnabled)
                StopThisPreview();
        } 
    }

    public bool OverCollider { get; set; }

    private void OnMouseEnter()
    {
        OverCollider = true;
        if (PreviewsAllowed && ThisPreviewEnabled)
        {
            PreviewThisObject();
        }
    }

    private void OnMouseExit()
    {
        OverCollider = false;
        //if (!PreviewingSomeCard())
        //    StopAllPreviews();
        //if (_currentlyViewing != null)
            StopAllPreviews();
    }


    private static bool PreviewingSomeCard()
    {
        if (!PreviewsAllowed)
            return false;
        HoverPreview[] allHoverBlowups = GameObject.FindObjectsOfType<HoverPreview>();
        foreach(HoverPreview hb in allHoverBlowups)
        {
            if (hb.OverCollider && hb.ThisPreviewEnabled)
                return true;
        }
        return false;
    }

    private void PreviewThisObject()
    {
        StopAllPreviews();
        StaticData.Instance.GameSlowDown();
        _currentlyViewing = this;
        PreviewGameObject.SetActive(true);
        ResetPreviewSize();
        PreviewDotween();
    }

    private void PreviewDotween()
    {
        Tween tween1 = PreviewGameObject.transform.DOLocalMove(TargetPosition, .5f).SetEase(Ease.OutQuint);
        Tween tween2 = PreviewGameObject.transform.DOScale(TargetScale, .5f).SetEase(Ease.OutQuint);
        tween1.SetUpdate(true);
        tween2.SetUpdate(true);
    }


    private void ResetPreviewSize()
    {
        PreviewGameObject.transform.localScale = Vector3.one;
        PreviewGameObject.transform.localPosition = OriginalPos;
    }


    public void StopThisPreview()
    {
        StaticData.Instance.GameSpeedResume();
        PreviewGameObject.SetActive(false);
        ResetPreviewSize();
    }

    private static void StopAllPreviews()
    {
        if (_currentlyViewing != null)
        {
            _currentlyViewing.StopThisPreview();
        }
    }


}
