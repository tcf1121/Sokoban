using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Day06_Sokoban
{
    class Program
    {
        struct Position
        {
            public int x;
            public int y;
        }

        enum tileMap
        {
            blank,
            wall,
            box,
            boxgoal,
            cleargoal
        }

        static void Main(string[] args)
        {
            bool gameOver = false;
            Position playerPos = new Position();
            int[,] map;
            ConsoleKey key = ConsoleKey.UpArrow;
            Start(ref playerPos, out map);


            while (gameOver == false)
            {
                Render(playerPos, map, key);
                key = Input();
                Update(key, ref playerPos, map, ref gameOver);
            }
            End();
        }

        // 시작 작업
        static void Start(ref Position playerPos, out int[,] map)
        {

            Console.CursorVisible = false;
            playerPos.x = 4;
            playerPos.y = 4;


            map = new int[8, 8]
            {   //
                {0, 0,  1,  1,  1,  0,  0, 0},
                {0, 1,  0,  3,  1,  1,  1, 0},
                {0, 1,  0,  2,  0,  0,  3, 1},
                {1, 1,  0,  2,  0,  2,  3, 1},
                {1, 3,  0,  2,  0,  0,  0, 1},
                {1, 0,  0,  0,  2,  0,  1, 0},
                {0, 1,  1,  1,  3,  1,  0, 0},
                {0, 0,  0,  1,  1,  1,  0, 0}
            };

            ShowTitle();
        }
        static void ShowTitle()
        {
            Console.SetCursorPosition(2, 2);
            Console.WriteLine("-----------");
            Console.SetCursorPosition(4, 4);
            Console.WriteLine("Sokoban");
            Console.SetCursorPosition(2, 6);
            Console.WriteLine("-----------");
            Console.WriteLine();
            Console.WriteLine("아무키나 눌러서\n\n 시작하세요....");

            Console.ReadKey(true);
            Console.Clear();
        }
        static void Restart(ref Position playerPos, int[,] map)
        {
            Console.Clear();
            playerPos.x = 4;
            playerPos.y = 4;

            int[,] remap = new int[8, 8]
            {   //
                {0, 0,  1,  1,  1,  0,  0, 0},
                {0, 1,  0,  3,  1,  1,  1, 0},
                {0, 1,  0,  0,  0,  0,  3, 1},
                {1, 1,  0,  2,  3,  2,  3, 1},
                {1, 3,  0,  2,  0,  0,  0, 1},
                {1, 0,  0,  0,  2,  0,  1, 0},
                {0, 1,  1,  1,  3,  1,  0, 0},
                {0, 0,  0,  1,  1,  1,  0, 0}
            };
            for (int i = 0; i < remap.GetLength(0); i++)
            {
                for (int j = 0; j < remap.GetLength(1); j++)
                    map[i, j] = remap[i, j];
            }
        }
        // 종료 작업
        static void End()
        {
            Console.Clear();
            Console.SetCursorPosition(2, 2);
            Console.WriteLine("-----------");
            Console.SetCursorPosition(5, 4);
            Console.WriteLine("Clear");
            Console.SetCursorPosition(2, 6);
            Console.WriteLine("-----------");
            Console.WriteLine();
            Console.SetCursorPosition(2, 8);
            Console.WriteLine("모든 박스를");
            Console.SetCursorPosition(2, 10);
            Console.WriteLine("옮겼습니다!");
        }

        // 출력 작업
        static void Render(Position playerPos, int[,] map, ConsoleKey key)
        {
            // 콘솔 지우기
            //Console.Clear();
            //Clear()대신 SetCursorPosition을 쓰면 덮어씌운다
            Console.SetCursorPosition(0, 0);
            PrintMap(map);
            PrintPlayer(playerPos, key);
        }


        // 맵 출력
        static void PrintMap(int[,] map)
        {

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Console.SetCursorPosition(x, y);
                    switch (map[y, x])
                    {
                        case (int)tileMap.blank:
                            Console.Write(' ');
                            break;
                        case (int)tileMap.wall:
                            Console.Write('▦');
                            break;
                        case (int)tileMap.box:
                            Console.Write('■');
                            break;
                        case (int)tileMap.boxgoal:
                            Console.Write('□');
                            break;
                        case (int)tileMap.cleargoal:
                            Console.Write('▣');
                            break;
                    }
                }
            }
        }


        // 플레이어 출력
        static void PrintPlayer(Position playerPos, ConsoleKey key)
        {
            // 플레이어 위치로 커서 옮기기
            Console.SetCursorPosition(playerPos.x, playerPos.y);
            // 플레이어 출력
            Console.ForegroundColor = ConsoleColor.Green;
            switch (key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    Console.WriteLine('◀');
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    Console.WriteLine('▶');
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    Console.WriteLine('▲');
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                default:
                    Console.WriteLine('▼');
                    break;
            }
            Console.ResetColor();
        }

        // 입력 작업
        static ConsoleKey Input()
        {
            return Console.ReadKey(true).Key;
        }


        // 플레이어 움직임
        static void Move(ConsoleKey key, ref Position playerPos, int[,] map)
        {
            Position targetPos = new Position();
            Position overPos = new Position();

            switch (key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    targetPos.x = playerPos.x - 1;
                    targetPos.y = playerPos.y;
                    overPos.x = playerPos.x - 2;
                    overPos.y = playerPos.y;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    targetPos.x = playerPos.x + 1;
                    targetPos.y = playerPos.y;
                    overPos.x = playerPos.x + 2;
                    overPos.y = playerPos.y;
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    targetPos.x = playerPos.x;
                    targetPos.y = playerPos.y - 1;
                    overPos.x = playerPos.x;
                    overPos.y = playerPos.y - 2;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    targetPos.x = playerPos.x;
                    targetPos.y = playerPos.y + 1;
                    overPos.x = playerPos.x;
                    overPos.y = playerPos.y + 2;
                    break;
                case ConsoleKey.R:
                    Restart(ref playerPos, map);
                    return;

                default:
                    return;
            }
            // 이동 하려는 곳이 벽이 아닐경우
            if (map[targetPos.y, targetPos.x] != (int)tileMap.wall)
            {
                // 그것이 박스인지 확인
                if (map[targetPos.y, targetPos.x] == (int)tileMap.box)
                {
                    // 박스를 밀 수 있는 경우
                    if (map[overPos.y, overPos.x] == (int)tileMap.blank ||
                        map[overPos.y, overPos.x] == (int)tileMap.boxgoal)
                    {

                        map[targetPos.y, targetPos.x] = (int)tileMap.blank;
                        // 박스 골 구역이면 골 안에 박스를 넣기
                        if (map[overPos.y, overPos.x] == (int)tileMap.boxgoal)
                            map[overPos.y, overPos.x] = (int)tileMap.cleargoal;
                        // 골 구역이 아니면 그냥 밀기
                        else
                        {
                            map[overPos.y, overPos.x] = (int)tileMap.box;
                        }
                        playerPos.x = targetPos.x;
                        playerPos.y = targetPos.y;
                    }
                }
                // 그것이 골 안에 있는 박스인지 확인
                else if (map[targetPos.y, targetPos.x] == (int)tileMap.cleargoal)
                {
                    // 박스를 밀 수 있는 경우
                    if (map[overPos.y, overPos.x] == (int)tileMap.blank ||
                        map[overPos.y, overPos.x] == (int)tileMap.boxgoal)
                    {

                        map[targetPos.y, targetPos.x] = (int)tileMap.boxgoal;
                        // 박스 골 구역이면 골 안에 박스를 넣기
                        if (map[overPos.y, overPos.x] == (int)tileMap.boxgoal)
                            map[overPos.y, overPos.x] = (int)tileMap.cleargoal;
                        // 골 구역이 아니면 그냥 밀기
                        else
                        {
                            map[overPos.y, overPos.x] = (int)tileMap.box;
                        }
                        playerPos.x = targetPos.x;
                        playerPos.y = targetPos.y;
                    }
                }
                //빈 공간일 경우 그냥 이동하기
                else
                {
                    playerPos.x = targetPos.x;
                    playerPos.y = targetPos.y;
                }
            }

            #region 최적화전
            //switch (key)
            //{
            //    case ConsoleKey.A:
            //    case ConsoleKey.LeftArrow:
            //        // 이동 하려는 곳이 벽이 아닐경우
            //        if (map[playerPos.y, playerPos.x - 1] != (int)tileMap.wall)
            //        {
            //            // 그것이 박스 인지확인
            //            if (map[playerPos.y, playerPos.x - 1] == (int) tileMap.box)
            //            {
            //                // 박스를 밀 수 있는 경우
            //                if (map[playerPos.y, playerPos.x - 2] != (int)tileMap.wall &&
            //                    map[playerPos.y, playerPos.x - 2] != (int)tileMap.box &&
            //                    map[playerPos.y, playerPos.x - 2] != (int)tileMap.cleargoal)
            //                {

            //                    map[playerPos.y, playerPos.x - 1] = (int)tileMap.blank;
            //                    // 박스 골 구역이면 골 안에 박스를 넣기
            //                    if (map[playerPos.y, playerPos.x - 2] == (int)tileMap.boxgoal)
            //                        map[playerPos.y, playerPos.x - 2] = (int)tileMap.cleargoal;
            //                    // 골 구역이 아니면 그냥 밀기
            //                    else
            //                    {
            //                        map[playerPos.y, playerPos.x - 2] = (int)tileMap.box;
            //                    }
            //                    playerPos.x--;
            //                }
            //            }
            //            // 그것이 골 안에 있는 박스인지 확인
            //            else if (map[playerPos.y, playerPos.x - 1] == (int)tileMap.cleargoal)
            //            {
            //                // 박스를 밀 수 있는 경우
            //                if (map[playerPos.y, playerPos.x - 2] != (int)tileMap.wall &&
            //                    map[playerPos.y, playerPos.x - 2] != (int)tileMap.box)
            //                {

            //                    map[playerPos.y, playerPos.x - 1] = (int)tileMap.boxgoal;
            //                    // 박스 골 구역이면 골 안에 박스를 넣기
            //                    if (map[playerPos.y, playerPos.x - 2] == (int)tileMap.boxgoal)
            //                        map[playerPos.y, playerPos.x - 2] = (int)tileMap.cleargoal;
            //                    // 골 구역이 아니면 그냥 밀기
            //                    else
            //                    {
            //                        map[playerPos.y, playerPos.x - 2] = (int)tileMap.box;
            //                    }
            //                    playerPos.x--;
            //                }
            //            }
            //            //빈 공간일 경우 그냥 이동하기
            //            else
            //            {
            //                playerPos.x--;
            //            }
            //        }   
            //        break;
            //    case ConsoleKey.D:
            //    case ConsoleKey.RightArrow:
            //        if (map[playerPos.y, playerPos.x + 1] != (int)tileMap.wall)
            //        {
            //            if (map[playerPos.y, playerPos.x + 1] == (int)tileMap.box)
            //            {
            //                if (map[playerPos.y, playerPos.x + 2] != (int)tileMap.wall &&
            //                    map[playerPos.y, playerPos.x + 2] != (int)tileMap.box &&
            //                    map[playerPos.y, playerPos.x + 2] != (int)tileMap.cleargoal)
            //                {
            //                    map[playerPos.y, playerPos.x + 1] = (int)tileMap.blank;
            //                    if (map[playerPos.y, playerPos.x + 2] == (int)tileMap.boxgoal)
            //                        map[playerPos.y, playerPos.x + 2] = (int)tileMap.cleargoal;
            //                    else
            //                    {
            //                        map[playerPos.y, playerPos.x + 2] = (int)tileMap.box;
            //                    }
            //                    playerPos.x++;
            //                }
            //            }
            //            else if (map[playerPos.y, playerPos.x + 1] == (int)tileMap.cleargoal)
            //            {
            //                if (map[playerPos.y, playerPos.x + 2] != (int)tileMap.wall &&
            //                    map[playerPos.y, playerPos.x + 2] != (int)tileMap.box)
            //                {
            //                    map[playerPos.y, playerPos.x + 1] = (int)tileMap.boxgoal;
            //                    if (map[playerPos.y, playerPos.x + 2] == (int)tileMap.boxgoal)
            //                        map[playerPos.y, playerPos.x + 2] = (int)tileMap.cleargoal;
            //                    else
            //                    {
            //                        map[playerPos.y, playerPos.x + 2] = (int)tileMap.box;
            //                    }
            //                    playerPos.x++;
            //                }
            //            }
            //            else
            //            {
            //                playerPos.x++;
            //            }
            //        }
            //        break;
            //    case ConsoleKey.W:
            //    case ConsoleKey.UpArrow:
            //        if (map[playerPos.y - 1, playerPos.x] != (int)tileMap.wall)
            //        {
            //            if (map[playerPos.y - 1, playerPos.x] == (int)tileMap.box)
            //            {
            //                if (map[playerPos.y - 2, playerPos.x] != (int)tileMap.wall &&
            //                    map[playerPos.y - 2, playerPos.x] != (int)tileMap.box &&
            //                    map[playerPos.y - 2, playerPos.x] != (int)tileMap.cleargoal)
            //                {
            //                    map[playerPos.y - 1, playerPos.x] = (int)tileMap.blank;
            //                    if (map[playerPos.y - 2, playerPos.x] == (int)tileMap.boxgoal)
            //                        map[playerPos.y - 2, playerPos.x] = (int)tileMap.cleargoal;
            //                    else
            //                    {
            //                        map[playerPos.y - 2, playerPos.x] = (int)tileMap.box;
            //                    }
            //                    playerPos.y--;
            //                }
            //            }
            //            else if (map[playerPos.y - 1, playerPos.x] == (int)tileMap.cleargoal)
            //            {
            //                if (map[playerPos.y - 2, playerPos.x] != (int)tileMap.wall &&
            //                    map[playerPos.y - 2, playerPos.x] != (int)tileMap.box)
            //                {
            //                    map[playerPos.y - 1, playerPos.x] = (int)tileMap.boxgoal;
            //                    if (map[playerPos.y - 2, playerPos.x] == (int)tileMap.boxgoal)
            //                        map[playerPos.y - 2, playerPos.x] = (int)tileMap.cleargoal;
            //                    else
            //                    {
            //                        map[playerPos.y - 2, playerPos.x] = (int)tileMap.box;
            //                    }
            //                    playerPos.y--;
            //                }
            //            }
            //            else
            //            {
            //                playerPos.y--;
            //            }
            //        }
            //        break;
            //    case ConsoleKey.S:
            //    case ConsoleKey.DownArrow:
            //        if (map[playerPos.y + 1, playerPos.x] != (int)tileMap.wall)
            //        {
            //            if (map[playerPos.y + 1, playerPos.x] == (int)tileMap.box)
            //            {
            //                if (map[playerPos.y + 2, playerPos.x] != (int)tileMap.wall &&
            //                    map[playerPos.y + 2, playerPos.x] != (int)tileMap.box &&
            //                    map[playerPos.y + 2, playerPos.x] != (int)tileMap.cleargoal)
            //                {
            //                    map[playerPos.y + 1, playerPos.x] = (int)tileMap.blank;
            //                    if (map[playerPos.y + 2, playerPos.x] == (int)tileMap.boxgoal)
            //                        map[playerPos.y + 2, playerPos.x] = (int)tileMap.cleargoal;
            //                    else
            //                    {
            //                        map[playerPos.y + 2, playerPos.x] = (int)tileMap.box;
            //                    }
            //                    playerPos.y++;
            //                }
            //            }
            //            else if (map[playerPos.y + 1, playerPos.x] == (int)tileMap.cleargoal)
            //            {
            //                if (map[playerPos.y + 2, playerPos.x] != (int)tileMap.wall &&
            //                    map[playerPos.y + 2, playerPos.x] != (int)tileMap.cleargoal)
            //                {
            //                    map[playerPos.y + 1, playerPos.x] = (int)tileMap.boxgoal;
            //                    if (map[playerPos.y + 2, playerPos.x] == (int)tileMap.boxgoal)
            //                        map[playerPos.y + 2, playerPos.x] = (int)tileMap.cleargoal;
            //                    else
            //                    {
            //                        map[playerPos.y + 2, playerPos.x] = (int)tileMap.box;
            //                    }
            //                    playerPos.y++;
            //                }
            //            }
            //            else
            //            {
            //                playerPos.y++;
            //            }
            //        }
            //        break;
            //}
            #endregion
        }

        //
        static void Update(ConsoleKey key, ref Position playerPos, int[,] map, ref bool gameOver)
        {

            Move(key, ref playerPos, map);
            bool isClear = IsClear(map);
            if (isClear)
            {
                gameOver = true;
            }
        }

        // 게임 클리어
        static bool IsClear(int[,] map)
        {
            foreach (var s in map)
            {
                if (s == (int)tileMap.box)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
