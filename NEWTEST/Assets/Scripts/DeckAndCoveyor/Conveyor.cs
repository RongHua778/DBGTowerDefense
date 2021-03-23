using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private Deck _deck;

    [SerializeField]
    private float _spawnNodeInterval = 1f;
    private float _spawnTimer;

    [SerializeField]
    Transform _spawnPos;
    [SerializeField]
    Transform _unSpawnPos;

    [SerializeField]
    private GameObject _nodePrefab;
    [SerializeField]
    private GameObject _cardPrefab;

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
        node.SetNode(_unSpawnPos.position,cardObj);
    }

    private GameObject SpawnCard(GameObject nodeObj)
    {
        GameObject cardObj = ObjectPool.Instance.Spawn(_cardPrefab);
        cardObj.transform.position = nodeObj.transform.position;
        Card card = cardObj.GetComponent<Card>();
        card.HandleNode = nodeObj;
        card.CardAsset = _deck.DrawCard();
        card.ReadCardFromAsset();
        return cardObj;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _spawnTimer >= _spawnNodeInterval)
        {
            SpawnNode();
            _spawnTimer = Time.time;
        }
    }
}
