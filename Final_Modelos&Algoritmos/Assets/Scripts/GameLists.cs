using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    //6 funciones linq
        //1 time-slicing
        //1 generador
    //2 tuplas
    //2 anonimos
    //2 agregatte

public class GameLists : MonoBehaviour
{
    List<BaseBullet> _activeBullets = new();
    List<BaseEnemy> _activeEnemies = new();


    public static GameLists Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            BlankEffect(Vector3.zero, 3f);
    }

    private void FixedUpdate()
    {
        OutOfScreen();
    }

    public void AddToBulletList(BaseBullet newBullet)
    {
        _activeBullets.Add(newBullet);
    }

    public void RemoveFromBulletList(BaseBullet newBullet)
    {
        _activeBullets.Remove(newBullet);
    }
    public void AddToEnemyList(BaseEnemy newEnemy)
    {
        _activeEnemies.Add(newEnemy);
    }

    public void RemoveFromEnemyList(BaseEnemy newEnemy)
    {
        _activeEnemies.Remove(newEnemy);
    }



    public void BlankEffect(Vector3 pos, float radius)
    {
        //var finalBullets = activeBullets
        //    .Where(x => (x.transform.position - pos).sqrMagnitude < (radius * radius) && x.Team != Team.Player)
        //    .OrderBy(x => (x.transform.position - pos).sqrMagnitude)
        //    .ToList();

        var bullets = _activeBullets
            .Where(x => (x.transform.position - pos).sqrMagnitude < (radius * radius) && x.Team != Team.Player)
            .OrderBy(x => (x.transform.position - pos).sqrMagnitude)
            .Select(x => x.SetMovement(BulletMovementType.Static, 0 , x.transform.up))
            .ToList();

        StartCoroutine(DespawnBullets(bullets));
    }

    void OutOfScreen()
    {
        var bullets = _activeBullets.Where(x => x.transform.position.x < BorderManager.Instance.Left 
        || x.transform.position.x > BorderManager.Instance.Right
        || x.transform.position.y < BorderManager.Instance.Bottom 
        || x.transform.position.y > BorderManager.Instance.Top)
            .ToArray();

        foreach (var bullet in bullets)
        {
            bullet.TurnOff();
        }

        //var x = _activeEnemies.Select(y => y.transform.position.x < BorderManager.Instance.Left
        //|| y.transform.position.x > BorderManager.Instance.Right
        //|| y.transform.position.y < BorderManager.Instance.Bottom
        //|| y.transform.position.y > BorderManager.Instance.Top);
        var enemiesTuple =
            _activeEnemies.Zip(_activeEnemies.Select(y => !(y.transform.position.x < BorderManager.Instance.Left
            || y.transform.position.x > BorderManager.Instance.Right
            || y.transform.position.y < BorderManager.Instance.Bottom
            || y.transform.position.y > BorderManager.Instance.Top)), (e, b) => (e, b))
            .ToArray();

        foreach(var enemy in enemiesTuple)
        {
            enemy.e.SetCanShoot(enemy.b);
        }
    }

    IEnumerator DespawnBullets(List<BaseBullet> bullets)
    {
        foreach (var bullet in bullets)
        {
            bullet.TurnOff();

            yield return new WaitForSeconds(0.01f);
        }
    }
}
