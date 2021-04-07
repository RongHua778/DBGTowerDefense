using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private Deck _deck;
    public static float _spawnTimer;

    [SerializeField] Transform _spawnPos = default;
    [SerializeField] Transform _unSpawnPos = default;

    [Header("Prefab Setting")]
    [SerializeField] private GameObject _nodePrefab = default;
    [SerializeField] private GameObject _turretCardPrefab = default;
    [SerializeField] private GameObject _magicCardPrefab = default;
    [SerializeField] private GameObject _noTargetMagicCardPrefab = default;

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
        node.SetNode(_unSpawnPos.position, cardObj);
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

    void Update()
    {
        if (Time.time - _spawnTimer >= StaticData.Instance.NodeSpawnInterval)
        {
            SpawnNode();
            _spawnTimer = Time.time;
        }
    }
}
