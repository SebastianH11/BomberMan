﻿/* Sebastion Horton
 * Tuesday, May 14, 2019
 * Class that creates the game
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BomberMan_2._0
{
    class Game
    {
        Map map;
        public static int playerDead = 0;
      
        Player player1;
        Point player1Point;

        Player player2;
        Point player2Point;

        int bombFuse;
        List<Bomb> bombs;

        List<Player> players;
        public Game(Canvas c)
        {
            map = new Map(c);
            player1Point = new Point(0, 0);
            player1 = new Player(c, Brushes.DarkRed, player1Point);  //construct player1 in top left corner

            player2Point = new Point(896, 512);
            player2 = new Player(c, Brushes.DarkBlue, player2Point); //construct player2 in bottom right corner

            players = new List<Player>() { player1, player2};
            bombs = new List<Bomb>();

        }
        public void updateGame()
        {

            player1.updatePlayer(Key.W, Key.S, Key.A, Key.D);
            player2.updatePlayer(Key.Up, Key.Down, Key.Left, Key.Right);

            placeBomb(Key.RightCtrl, player2);
            placeBomb(Key.LeftShift, player1);
           

            
          
            
        }
        /// <summary>
        /// Authors
        /// Sebastian Horton, Logan Ellis
        /// if the player presses the "place" key it will check if this player already has a bomb placed that hasn't exploded.
        /// If there is no bomb placed then it runs the Bomb class' function armBomb.
        /// </summary>
        private bool placeBomb(Key place, Player player)
        {
            if (player.bombPlaced == false)
            {
                if (Keyboard.IsKeyDown(place))
                {
                    bombs.Add(new Bomb(player.getPlayerPos()));
                    bombFuse = 15;
                    return player.bombPlaced = true;
                }
            }
            foreach (Bomb b in bombs)
            {
               
                    if (player.bombPlaced == true && bombFuse >= -5)
                    {
                        bombFuse--;
                        if (bombFuse == 0)
                        {
                            b.explosion();
                        playerDead = 1;
                        foreach (Player pl in players)
                        {
                            if (isPlayerDead(pl.getPlayerPos()) == true)
                            {
                                if (playerDead == 1)
                                {
                                    Menu.playerNumber = "2";
                                }
                                else
                                    Menu.playerNumber = "1";

                                MainWindow.gamestate = MainWindow.GameState.gameOver;
                            }
                            playerDead++;

                        }
                            return player.bombPlaced = true;
                        }
                        else if (bombFuse == -5)
                        {
                            b.resetBomb();
                            return player.bombPlaced = false;
                        }
                        return player.bombPlaced = true;
                    }
                    else
                        return player.bombPlaced = false;
            }
            return false;
        }

        /// <summary>
        /// Authors
        /// Sebastion Horton
        /// checks if the player is in the blast radius
        /// </summary>
        private bool isPlayerDead(Point p)
        {
            
                if (Matrices.bomb[(int)p.X / 64, (int)p.Y / 64] == 1)
                {
                    return true;
                }
                else
                    return false;
        }
    }
}
