using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeRender : MonoBehaviour
{
    public Text seed_UI_Text;

    [Header("CONFIGURACIÓN")]

    [SerializeField]
    [Range(1, 50)]
    private int width = 10;

    [SerializeField]
    [Range (1,50)]
    private int height = 10;

    [SerializeField]
    private int seed = 9999;

    [SerializeField]
    private float size = 1f;

    //Partes del labertinto
    [Header("Components")]
    [SerializeField]
    private Transform wallPrefab = null;
    [SerializeField]
    private Transform floorPrefab = null;
    [SerializeField]
    private GameObject trapGO;
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject finish;

    [Header("Trampas")]
    [SerializeField]
    [Range(1, 100)]
    private int porcenTrampas;
    private Vector2 lastPosition;

    public bool test;

    // Start is called before the first frame update
    void Start()
    {
        if (test)
        {
            var maze = MazeGenerator.Generate(width, height, seed);
            seed_UI_Text.text = "Seed: " + seed;
            Draw(maze);
        }
    }

    public void startDraw()
    {
        seed = ServiceLocator.Instance.GetService<IMazeInfo>().getSeed();
        var maze = MazeGenerator.Generate(width, height, seed);
        //seed_UI_Text.text = "Seed: " + seed;
        Draw(maze);
    }

    public void Draw(WallState[,] maze)
    {
        var rng = new System.Random(seed);
        Debug.Log("SEED: " + seed);
        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width / 2, 1, height/ 2);

        GameObject newStart = Instantiate(start, transform);
        newStart.transform.position = new Vector3(-width / 2, 0, -height / 2);
        newStart.name = "Start";

        for(int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {

                int probTrampas = rng.Next(0, 100);
                lastPosition = new Vector2(i, j);

                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                if (cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size/2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if(i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(wallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(+size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }

                if(j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }

                if (porcenTrampas >= probTrampas)
                {
                    GameObject newTrap = Instantiate(trapGO, transform.Find("Traps"));
                    newTrap.transform.position = new Vector3(-width / 2 + i, 0, -height / 2 + j);
                    int trapType = rng.Next(0, 4);
                    newTrap.GetComponent<Trap>().setTipo(trapType);
                }

            }
        }
        GameObject newFinish = Instantiate(finish, transform);
        newFinish.transform.position = new Vector3(-width / 2 + lastPosition.x, 0, -height / 2 + lastPosition.y);
        newFinish.name = "Finish";
    }

    // Update is called once per frame
    void Update()
    {
        if (ServiceLocator.Instance.GetService<Common.Installer>()._getMazeIniciated && !test)
        {
            ServiceLocator.Instance.GetService<Common.Installer>()._getMazeIniciated = false;
            startDraw();
        }
    }

}
