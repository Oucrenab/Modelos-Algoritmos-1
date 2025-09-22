using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpsSpawner : MonoBehaviour
{
    //a
    [SerializeField] float _spawnCD;
    [SerializeField] float _offset;

    [SerializeField] List<BasicGravityObject> _allPowerUps = new List<BasicGravityObject>();

    float _lastSpawn;
    float _cd;

    private void Awake()
    {
        EventManager.Subscribe("MementoLoad", MementoLoad);
    }

    private void Update()
    {
        if(Time.time > _lastSpawn + _cd)
            SpawnPowerUp();
    }

    void MementoLoad(params object[] noUse)
    {
        _lastSpawn = Time.time - (_spawnCD * 0.75f);
    }

    void SpawnPowerUp()
    {
        _lastSpawn = Time.time;
        _cd =1;
        if (!(_allPowerUps.Count > 0)) return;

        _cd = _spawnCD;

        DoChoose();
        TurnOn(_lastChoose);
    }

    public void TurnOff(BasicGravityObject other)
    {
        other.gameObject.SetActive(false);
        _allPowerUps.Add(other);
    }

    void TurnOn((BasicGravityObject, Vector3) other)
    {
        if (other.Item1.gameObject.activeSelf) return;

        other.Item1.transform.position = other.Item2;
        other.Item1.gameObject.SetActive(true);
        other.Item1.SetSpawner(this);
        _allPowerUps.Remove(other.Item1);
    }
    //Martin

    (BasicGravityObject powerUp, Vector3 position) _lastChoose;

    void DoChoose()
    {
        List<Vector3> pos = new();
        pos.Add(ChooseRandomPos(_lastChoose.position));

        System.Random rand = new System.Random();
        _lastChoose = _allPowerUps.Where(x => x != _lastChoose.powerUp)
            .OrderBy(x => rand.Next())
            .Zip(pos, (power, pos) => (power, pos))
            .First();

    }

    BasicGravityObject ChooseRandomPowerUp()
    {
        var i = Random.Range(0, _allPowerUps.Count);
        return _allPowerUps[i];
    }

    Vector3 ChooseRandomPos(Vector3 pos)
    {
        var x = Random.Range(BorderManager.Instance.Left + _offset, BorderManager.Instance.Right - _offset);
        if (x == pos.x)
            return ChooseRandomPos(pos);
        return new Vector3(x, BorderManager.Instance.Top + _offset, 0);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("MementoLoad", MementoLoad);
        
    }
}
