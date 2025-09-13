using System.Collections;
using System.Collections.Generic;
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

        var powerUp = ChooseRandomPowerUp();
        TurnOn(powerUp);
    }

    public void TurnOff(BasicGravityObject other)
    {
        other.gameObject.SetActive(false);
        _allPowerUps.Add(other);
    }

    void TurnOn(BasicGravityObject other)
    {
        if (other.gameObject.activeSelf) return;

        other.transform.position = ChooseRandomPos();
        other.gameObject.SetActive(true);
        other.SetSpawner(this);
        _allPowerUps.Remove(other);
    }

    BasicGravityObject ChooseRandomPowerUp()
    {
        var i = Random.Range(0, _allPowerUps.Count);
        return _allPowerUps[i];
    }

    Vector3 ChooseRandomPos()
    {
        var x = Random.Range(BorderManager.Instance.Left + _offset, BorderManager.Instance.Right - _offset);
        return new Vector3(x, BorderManager.Instance.Top + _offset, 0);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("MementoLoad", MementoLoad);
        
    }
}
