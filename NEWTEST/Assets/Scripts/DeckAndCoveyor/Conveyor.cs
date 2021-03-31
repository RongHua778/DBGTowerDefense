using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private Deck _deck;
    private float _spawnTimer;
    public float NodeSpeed => StaticData.Instance.NodeSpeed;
    public float NodeSpawnInterval => StaticData.Instance.NodeSpawnInterval;

    [SerializeField] Transform _spawnPos;
    [SerializeField] Transform _unSpawnPos;

    [Header("Prefab Setting")]
    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] private GameObject _turretCardPrefab;
    [SerializeField] private GameObject _magicCardPrefab;
    [SerializeField] private GameObject _noTargetMagicCardPrefab;

    private void Awake()
    {
        _deck = this.GetComponent<Deck>();
    }

    public void SpawnNode()
    {
        GameObject cardObj = null;
        GameObject nodeObj = ObjectPool.Instance.Spawn(_nodePrefab);
        nodeObj.transform.position = _spawnPos.position;
        Node node = nodeObj.GetComponent<Node>();
        if (_deck._drawPile.Count > 0)//≈–∂œ «∑Òªπ”– £”‡ø®≈∆
        {
            cardObj = SpawnCard(nodeObj);
        }
        node.SetNode(_unSpawnPos.position, NodeSpeed, cardObj);
    }

    private GameObject SpawnCard(GameObject nodeObj)
    {
        CardSO cardAsset = _deck.DrawCard();
        GameObject cardObj = null;
        switch (cardAsset.CardType)
        {
            case CardType.Tower:
                cardObj = ObjectPool.Instance.Spawn(_turretCardPrefab);
                break;
            case CardType.Magic:
                cardObj = ObjectPool.Instance.Spawn(_magicCardPrefab);
                break;
            case CardType.NoTargetMagic:
                cardObj = ObjectPool.Instance.Spawn(_noTargetMagicCardPrefab);
                break;
        }
        cardObj.transform.position = nodeObj.transform.position;
        Card card = cardObj.GetComponent<Card>();
        card.HandleNode = nodeObj;
        card.CardAsset = cardAsset;
        card.ReadCardFromAsset();
        return cardObj;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _spawnTimer >= NodeSpawnInterval)
        {
            SpawnNode();
            _spawnTimer = Time.time;
        }
    }
}
