using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : ReusableObject
{
    private Vector2 _targetPos;
    private GameObject _holdingCard;

    public float NodeSpeed
    {
        get => StaticData.Instance.NodeSpeed;
    }

    private float _minDistance = .1f;
    public override void OnSpawn()
    {
    }

    public override void OnUnSpawn()
    {
        _holdingCard = null;
    }

    public void SetNode(Vector2 targetPos, GameObject cardObj = null)
    {
        _targetPos = targetPos;
        _holdingCard = cardObj;
    }

    private void Update()
    {
        float distance = ((Vector2)transform.position - _targetPos).magnitude;
        if (distance > _minDistance)
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, NodeSpeed * Time.deltaTime);
        else
        {
            if (_holdingCard != null)
            {
                ObjectPool.Instance.UnSpawn(_holdingCard);
            }
            ObjectPool.Instance.UnSpawn(this.gameObject);
        }    
    }

}
