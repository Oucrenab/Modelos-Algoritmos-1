using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//6 funciones linq revisar
//      Boss ChoosePatter Gonzalo linq 3 grupos
//2 time-slicing (Usar linq adentro)
//      GameList DespawnBullets Gonzalo linq 3 grupos
//      PatternSpawner StartPattern Martin
//2 generador
//      GameList EnemyOutOfScreen() Gonzalo linq 3 grupos
//      GameList BulletOutOfScreen() Martin linq 3 grupos
//2 tuplas
//      tupla (BaseEnemy, bool) en GameList EnemyOutOfScreen() Gonzalo
//      tupla (PowerUp, Position) en PowerUpSpawner DoChoose() Martin linq 3 grupos
//2 anonimos
//      GameList DespawnBullets Gonzalo
//      GameList FinalData Martin linq 3 grupos
//2 agregatte falta 1
//      GameList CheckLife Gonzalo
//      GameList FinalData martin

public class GameLists : MonoBehaviour
{
    List<BaseBullet> _activeBullets = new();
    List<BaseEnemy> _activeEnemies = new();
    List<IFactory> _factories = new();


    public static GameLists Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        EventManager.Subscribe("BossDeath", UpdateFinalScreen);
        EventManager.Subscribe("PlayerDeath", UpdateFinalScreen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            BlankEffect(Vector3.zero, 3f);
        if (Input.GetKeyDown(KeyCode.F2))
            FinalData();

        CheckLife();
    }

    private void FixedUpdate()
    {
        OutOfScreen();
    }

    //Gonzalo
    public void CheckLife()
    {
        //lista de enemigo a 0 hp
        //hacerlos morir
        //decirle al boss que spawnee mas cuando este en 0

        if (!_activeEnemies.Aggregate(false, (acum, current) =>
        {
            if (current.Life < 0) current.TriggerDeath();
            acum = current.Life > 0 || acum;

            return acum;
        }))
        {
            EventManager.Trigger("SpawnEnemies");
        }
    }

    //Martin con ayuda XD

    [SerializeField] TMP_Text kills, shots, timeText, points;
    [SerializeField] Image rank;
    [SerializeField] Sprite[] _ranksImages;

    void UpdateFinalScreen(params object[] noUSe)
    {
        if (!_factories.Any()) return;
        var data = FinalData();

        kills.text = "Kills: " + data.kills;
        shots.text = "Shots: " + data.shots;
        timeText.text = "In Game Time: " + data.time;
        points.text = "Points: " + data.points;
        switch (data.rank)
        {
            case Rank.F:
                rank.sprite = _ranksImages[0];
                break;
            case Rank.D:
                rank.sprite = _ranksImages[1];
                break;
            case Rank.C:
                rank.sprite = _ranksImages[2];
                break;
            case Rank.B:
                rank.sprite = _ranksImages[3];
                break;
            case Rank.A:
                rank.sprite = _ranksImages[4];
                break;
            case Rank.S:
                rank.sprite = _ranksImages[5];
                break;
        }
    }

    public (int kills, int shots, float time, int points, Rank rank) FinalData()
    {

        List<(int kill, int shot, int point)> finalPoint = new();
        finalPoint.Add(_factories.Aggregate((0,0,0), (acum, current) =>
        {
            //item1 kills
            //item2 disparos
            //item3 puntos
            var num = current.GetCount();
            if (current.GetFactory().TryGetComponent<BulletFactory>(out var bFac))
            {
                acum.Item2 += num; //si es fabrica de balas conseguir la cantidad de tiros
                acum.Item3 += num / 100; //conversion a puntos
            }
            else
            {
                acum.Item1 += num; //si fabrica de enemigos, conseguir cantidad de kills
                acum.Item3 += num / 5; //conversion a puntos
            }

            return acum;
        }));

        List<float> points = new();
        points.Add( finalPoint.First().point - (Time.timeSinceLevelLoad * 0.01f) * 12);//restar puntos por tiempo

        //pasar de puntos a rango
        List<Rank> rank = new();
        switch (points.First())
        {
            case < 100:
                rank.Add(Rank.F);
                break;
            case < 200:
                rank.Add(Rank.D);
                break;
            case < 300:
                rank.Add(Rank.C);
                break;
            case < 400:
                rank.Add(Rank.B);
                break;
            case < 500:
                rank.Add(Rank.A);
                break;
            default:
                rank.Add(Rank.S);
                break;
        }

        //List<(int kills, int shots, float time, float points,  Rank rank)> finalData = new();

        //unir las listas
        var finalData = finalPoint.Zip(points, (fpo, pts) => new {kill = fpo.kill, shot = fpo.shot ,time = Time.timeSinceLevelLoad, p = pts })
            .Zip(rank, (fd, r) => new ValueTuple<int,int,float,int,Rank>(fd.kill,fd.shot,fd.time,(int)fd.p, r));//fd final data

        return finalData.FirstOrDefault();
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
    public void AddToFactoryList(IFactory newF)
    {
        _factories.Add(newF);
    }

    public void RemoveFromfFactoryList(IFactory newF)
    {
        _factories.Remove(newF);
    }



    public void BlankEffect(Vector3 pos, float radius)
    {
        //var finalBullets = activeBullets
        //    .Where(x => (x.transform.position - pos).sqrMagnitude < (radius * radius) && x.Team != Team.Player)
        //    .OrderBy(x => (x.transform.position - pos).sqrMagnitude)
        //    .ToList();

        //var stop = new { a = BulletMovementType.Static, b = 0, c = transform.up };

        //var bullets = _activeBullets
        //    .Where(x => (x.transform.position - pos).sqrMagnitude < (radius * radius) && x.Team != Team.Player)
        //    .OrderBy(x => (x.transform.position - pos).sqrMagnitude)
        //    .Select(x => {
        //        var stop = new { a = BulletMovementType.Static, b = 0, c = x.transform.up };
        //        x.SetMovement(BulletMovementType.Static, 0, x.transform.up);

        //        return x;
        //        })
        //    .ToList();

        StartCoroutine(DespawnBullets(_activeBullets, pos, radius));
    }

    void OutOfScreen()
    {
        //var bullets = _activeBullets.Where(x => x.transform.position.x < BorderManager.Instance.Left 
        //|| x.transform.position.x > BorderManager.Instance.Right
        //|| x.transform.position.y < BorderManager.Instance.Bottom 
        //|| x.transform.position.y > BorderManager.Instance.Top)
        //    .ToArray();

        //var enemiesTuple =
        //    _activeEnemies.Zip(_activeEnemies.Select(y => !(y.transform.position.x < BorderManager.Instance.Left
        //    || y.transform.position.x > BorderManager.Instance.Right
        //    || y.transform.position.y < BorderManager.Instance.Bottom
        //    || y.transform.position.y > BorderManager.Instance.Top)), (e, b) => (e, b))
        //    .ToArray();

        foreach (var enemy in EnemyOutOfScreen(_activeEnemies).OfType<(BaseEnemy e, bool b)>())
        {
            enemy.e.SetCanShoot(enemy.b);
        }

        foreach (var bullet in BulletOutOfScreen().OfType<BaseBullet>())
        {
            bullet.TurnOff();
        }
    }

    //Esto esta bien
    //Time slicing
    //Anonimo
    //Gonzalo
    IEnumerator DespawnBullets(List<BaseBullet> allBullets, Vector3 pos, float radius)
    {
        var bullets = allBullets
            .Where(x => (x.transform.position - pos).sqrMagnitude < (radius * radius) && x.Team != Team.Player)
            .OrderBy(x => (x.transform.position - pos).sqrMagnitude)
            .Select(x => {
                var stop = new
                {
                    type = BulletMovementType.Static,
                    speed = 0,
                    dir = x.transform.up,
                    ren = x.GetComponentInChildren<SpriteRenderer>(),
                    col = x.GetComponent<Collider2D>()
                };
                x.SetMovement(stop.type, stop.speed, stop.dir);
                stop.col.enabled = false;
                stop.ren.color = Color.gray;

                return x;
            })
            .ToList();

        foreach (var bullet in bullets)
        {
            var stop = new
            {
                ren = bullet.GetComponentInChildren<SpriteRenderer>(),
                col = bullet.GetComponent<Collider2D>()
            };

            stop.ren.color = Color.white;

            yield return new WaitForSeconds(0.1f);

            bullet.TurnOff();
            stop.col.enabled = true;

        }
    }

    IEnumerable<(BaseEnemy e, bool b)> EnemyOutOfScreen(List<BaseEnemy> enemies)
    {
        var enemiesTuple =
            enemies.Zip(_activeEnemies.Select(y => !(y.transform.position.x < BorderManager.Instance.Left
            || y.transform.position.x > BorderManager.Instance.Right
            || y.transform.position.y < BorderManager.Instance.Bottom
            || y.transform.position.y > BorderManager.Instance.Top)), (e, b) => (e, b));

        //if (enemiesTuple.Any())
        foreach (var enemy in enemiesTuple)
            yield return enemy;
    }

    //Martin
    //Generador
    //
    IEnumerable<BaseBullet> BulletOutOfScreen()
    {
        var bullets = _activeBullets.Where(x => x.transform.position.x < BorderManager.Instance.Left
        || x.transform.position.x > BorderManager.Instance.Right
        || x.transform.position.y < BorderManager.Instance.Bottom
        || x.transform.position.y > BorderManager.Instance.Top).OrderBy(x => x.Team);

        //if(bullets.Any()) 
        foreach (var bullet in bullets)
            yield return bullet;

    }
}

public enum Rank
{
    F,
    D,
    C,
    B,
    A,
    S
}