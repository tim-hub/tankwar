using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankWar
{
	public class GameConstants
	{
		public  Vector2 Center= new Vector2(320,240);
		public const int WIN_WIDTH=640; //13+3
		public const int WIN_HEIGHT=520;
		public const int SHOOTING_SPEED=6;

		public const int COOL_TIME=500;


		public const int TANK_MOVING_SPEED_BASE=2;
		public const int TANK_MOVING_SPEED_PLAYER=(int)(TANK_MOVING_SPEED_BASE*1.2);
		public const int TANK_MOVING_SPEED_ArmoredCar=(int)(TANK_MOVING_SPEED_BASE*1.5);

		//map and background
		public const int LEFT_BOARDER=40;
		public const int RIGHT_BOARDER=560;
		public const int CENTER_V=300;
		public const int EAGLE_X=40*6;


		//the control keys for players
		public static Keys[] PLAYER1={Keys.W,Keys.D,Keys.S,Keys.A,Keys.F};
		public static Keys[] PLAYER2={Keys.Up,Keys.Right,Keys.Down,Keys.Left,Keys.L};
		public static Keys[] AI={Keys.Up,Keys.Right,Keys.Down,Keys.Left,Keys.L};

		//AI Control
		public const double TANK_CHANGE_DIRECTION_RATE=0.4;
		public const int TANK_MOVE_TIME=800;  //mileseconds
		public const int AI_SHOOTING_RANDOM_FACTOR=5;   //percents

		//Ai number
		public const int AI_NUMBER=20;

		//Explosion
		public const int EXPLOSION_FRAME_TIME=10;
		public const int EXPLOSION_NUM_FRAMES=9;
		public const int EXPLOSION_FRAMES_PER_ROW=3;
		public const int EXPLOSION_NUM_ROWS=3;

		//player1 number
		public const int PLAYER1_X=40+40*4;


	}
}

