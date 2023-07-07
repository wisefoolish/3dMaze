using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GenerateMaze : MonoBehaviour
{
    public static int Long = 10;
    public static int Width = 10;
    public static int Height = 10;
    public bool isCreateMaze = false;
    public GameObject Wall;
    public GameObject Goal;
    TDP begin;
    TDP end;
    enum MazeElement
    {
        Path,
        Wall
    }
    enum Direct
    {
        Up,
        Down,
        Left,
        Right,
        Front,
        Back
    }

    //Three Demision Point
    struct TDP
    {
        public int X, Y, Z;
        public TDP GetTDP(int x = 0, int y = 0, int z = 0)
        {
            TDP result;
            result.X = x;
            result.Y = y;
            result.Z = z;
            return result;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(isCreateMaze)
        {
            SimpleMaze();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position=GetBeginPos();
        }
    }

    int PathNums(TDP p, char[,,]Auxiliary)
    {
        int result = 0;
        if (p.X - 1 >= 0 && Auxiliary[p.X - 1, p.Y, p.Z] == (char)MazeElement.Path) result++;
        if (p.X + 1 < Long && Auxiliary[p.X + 1, p.Y, p.Z] == (char)MazeElement.Path) result++;
        if (p.Y - 1 >= 0 && Auxiliary[p.X, p.Y - 1, p.Z] == (char)MazeElement.Path) result++;
        if (p.Y + 1 < Width && Auxiliary[p.X, p.Y + 1, p.Z] == (char)MazeElement.Path) result++;
        if (p.Z - 1 >= 0 && Auxiliary[p.X, p.Y, p.Z - 1] == (char)MazeElement.Path) result++;
        if (p.Z + 1 < Height && Auxiliary[p.X, p.Y, p.Z + 1] == (char)MazeElement.Path) result++;
        return result;
    }

    char[,,] CreateMaze(TDP p)
    {
        char[,,] Auxiliary = new char[Long, Width,Height];
        for(int i=0;i<Long;i++)
        {
            for(int j=0;j<Width;j++)
            {
                for (int k = 0; k < Height; k++)
                    Auxiliary[i, j, k] = (char)MazeElement.Wall;
            }
        }
        Stack<TDP> stack = new Stack<TDP>();
        stack.Push(p);
        Auxiliary[p.X, p.Y,p.Z] = (char)MazeElement.Path;
        int length = 0;
        int maxlength = 0;
        // 深度寻路
        while(stack.Count > 0)
        {
            TDP nows = stack.Peek();
            List<Direct> directs=new List<Direct>();
            // 无环路，看下一格周围是否只有一个path
            if (nows.X - 1 >= 0 && Auxiliary[nows.X - 1, nows.Y,nows.Z] == (char)MazeElement.Wall &&
                PathNums(nows.GetTDP(nows.X-1,nows.Y,nows.Z), Auxiliary) <= 1)directs.Add(Direct.Left);
            if (nows.X + 1 < Long && Auxiliary[nows.X + 1, nows.Y,nows.Z] == (char)MazeElement.Wall &&
                PathNums(nows.GetTDP(nows.X + 1, nows.Y, nows.Z), Auxiliary) <= 1) directs.Add(Direct.Right);
            if (nows.Y - 1 >= 0 && Auxiliary[nows.X, nows.Y - 1,nows.Z] == (char)MazeElement.Wall &&
                PathNums(nows.GetTDP(nows.X, nows.Y - 1, nows.Z), Auxiliary) <= 1) directs.Add(Direct.Front);
            if (nows.Y + 1 < Width && Auxiliary[nows.X, nows.Y + 1,nows.Z] == (char)MazeElement.Wall &&
                PathNums(nows.GetTDP(nows.X, nows.Y + 1, nows.Z), Auxiliary) <= 1) directs.Add(Direct.Back);
            if (nows.Z - 1 >= 0 && Auxiliary[nows.X, nows.Y, nows.Z-1] == (char)MazeElement.Wall &&
                PathNums(nows.GetTDP(nows.X, nows.Y, nows.Z - 1), Auxiliary) <= 1) directs.Add(Direct.Up);
            if (nows.Z + 1 < Height && Auxiliary[nows.X, nows.Y, nows.Z+1] == (char)MazeElement.Wall &&
                PathNums(nows.GetTDP(nows.X, nows.Y, nows.Z + 1), Auxiliary) <= 1) directs.Add(Direct.Down);
            if (directs.Count > 0)
            {
                switch (directs[UnityEngine.Random.Range(0, directs.Count - 1)])
                {
                    case Direct.Left:
                        nows.X--;
                        break;
                    case Direct.Right:
                        nows.X++;
                        break;
                    case Direct.Front:
                        nows.Y--;
                        break;
                    case Direct.Back:
                        nows.Y++;
                        break;
                    case Direct.Up:
                        nows.Z--;
                        break;
                    case Direct.Down:
                        nows.Z++;
                        break;
                }
                stack.Push(nows);
                Auxiliary[nows.X, nows.Y, nows.Z] = (char)MazeElement.Path;
                length++;
                if (length > maxlength)
                {
                    maxlength = length;
                    end = nows;
                }
            }
            else
            { 
                stack.Pop();
                length--;
            }
        }
        return Auxiliary;
    }

    void InitializeMaze(char[,,] map,float height)
    {
        float objVertical = Wall.transform.localScale.z;
        float objWidth = Wall.transform.localScale.x;
        float objHeight = Wall.transform.localScale.y;
        for(int i=0; i < map.GetLength(0); i++)
        {
            for(int j=0;j<map.GetLength(1);j++)
            {
                for(int k=0;k<map.GetLength(2);k++)
                {
                    if (map[i,j,k]==(char)MazeElement.Wall)
                    {
                        Instantiate(Wall, new Vector3(i * objWidth, k * objVertical, j * objHeight), transform.rotation);
                    }
                }
            }
        }
        GameObject floor = Instantiate(Wall, transform.position, transform.rotation);
        floor.transform.localScale = new Vector3((Long + 1) * objWidth, objVertical, (Width + 1) * objHeight);
        floor.transform.position = new Vector3(Long * objWidth / 2, -objVertical, Width * objHeight / 2);

        floor = Instantiate(Wall, transform.position, transform.rotation);
        floor.transform.localScale = new Vector3((Long + 1) * objWidth, objVertical, (Width + 1) * objHeight);
        floor.transform.position = new Vector3(Long * objWidth / 2, Height * objVertical, Width * objHeight / 2);

        floor = Instantiate(Wall, transform.position, transform.rotation);
        floor.transform.localScale = new Vector3(objWidth, (Height + 1) * objVertical, (Width + 1) * objHeight);
        floor.transform.position = new Vector3(-objWidth, Height * objVertical / 2, Width * objHeight / 2);

        floor = Instantiate(Wall, transform.position, transform.rotation);
        floor.transform.localScale = new Vector3(objWidth, (Height + 1) * objVertical, (Width + 1) * objHeight);
        floor.transform.position = new Vector3(Long * objWidth, Height * objVertical / 2, Width * objHeight / 2);

        floor = Instantiate(Wall, transform.position, transform.rotation);
        floor.transform.localScale = new Vector3((Long + 1) * objWidth, (Height + 1) * objVertical, objHeight);
        floor.transform.position = new Vector3(Long * objWidth / 2, Height * objVertical/2, -objHeight);

        floor = Instantiate(Wall, transform.position, transform.rotation);
        floor.transform.localScale = new Vector3((Long + 1) * objWidth, (Height + 1) * objVertical, objHeight);
        floor.transform.position = new Vector3(Long * objWidth / 2, Height * objVertical / 2, Width * objHeight);

        Instantiate(Goal, new Vector3(end.X * objWidth, end.Z * objVertical, end.Y * objHeight), transform.rotation);
    }

    void SimpleMaze()
    {
        begin.X = UnityEngine.Random.Range(0, Long - 1);
        begin.Y = UnityEngine.Random.Range(0, Width - 1);
        begin.Z = UnityEngine.Random.Range(0, Height - 1);
        char[,,] eachMap = CreateMaze(begin);
        InitializeMaze(eachMap, 0);
    }

    Vector3 GetBeginPos()
    {
        float objVertical = Wall.transform.localScale.z;
        float objWidth = Wall.transform.localScale.x;
        float objHeight = Wall.transform.localScale.x;
        return new Vector3(begin.X * objWidth, begin.Z * objVertical, objHeight * begin.Y);
    }

    public int GetTag(string str)
    {
        int result = 0;
        switch(str)
        {
            case "Long":
                result = Long;
                break;
            case "Width":
                result = Width;
                break;
            case "Height":
                result=Height;
                break;
        }
        return result;
    }

    public void SetTag(string str,int num)
    {
        switch (str)
        {
            case "Long":
                Long = num ;
                break;
            case "Width":
                Width = num ;
                break;
            case "Height":
                Height = num ;
                break;
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("InitialInterface");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
