using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankWar
{

	/// <summary>
	/// AI. the sub class of tank. Auto Move and Auto Shoot
	/// </summary>
	public class AI: Tank
	{

		protected Random Rand;
//		int seed = Guid.NewGuid().GetHashCode();
//		protected Random Rand = new Random (seed);
		protected double ChangeDirectionRate;
		protected bool CanMove;
		protected int ContinuousMovingTime;
		protected int MoveDirection  ;
		protected bool Halt;
		protected int CurrentMovingTimeAI;


		//AI shoot time Random delay


		#region constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="TankWar.AI"/> class.
		/// The Sub class need cool time, moving time
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="spriteName">Sprite name.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public AI(ContentManager content, String spriteName, int x, int y) :base(content,spriteName,x,y){
			byte[] randomBytes = new byte[4];
			RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider ();
			rngServiceProvider.GetBytes (randomBytes);
			Int32 seed = BitConverter.ToInt32 (randomBytes, 0);
			//Console.WriteLine (result);


			Rand = new Random (seed);
			ChangeDirectionRate = GameConstants.TANK_CHANGE_DIRECTION_RATE;

			ContinuousMovingTime = Rand.Next (GameConstants.TANK_MOVE_TIME/2, GameConstants.TANK_MOVE_TIME*2);
			CurrentMovingTimeAI = 0;
			this.MoveDirection = Rand.Next (4);
			this.Halt = false;

		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TankWar.AI"/> class. With Map
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="spriteName">Sprite name.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="map">Map.</param>
		public AI(ContentManager content, String spriteName, int x, int y, Map map) :this(content,spriteName,x,y){
			CubesInMap = map.Cubes;
		}

		#endregion


		#region Methods
		//Update
		public void Update(GameTime gameTime){
			AutoUpdate (gameTime);
		}

		//AutoUpdate
		public void AutoUpdate(GameTime gameTime)
		{



			//do not move out
			Bound ();
			//move smoothly
			MoveSmoothly (gameTime);
			//Wether the tank is halted
			Halt = CheckHalt ();

			//When Desactive explode
			if (!Active) {

				Projectiles.Clear();

			}

			//running time
			if (Active) {
				if (!Halt) {


					CurrentMovingTimeAI += gameTime.ElapsedGameTime.Milliseconds;
					if (CurrentMovingTimeAI < ContinuousMovingTime) {
						StartMove ();

					} else {
						MoveDirection = GetDirection (MoveDirection);

						CurrentMovingTimeAI = 0;
						ContinuousMovingTime = Rand.Next (GameConstants.TANK_MOVE_TIME / 2, GameConstants.TANK_MOVE_TIME * 2);
					}
				} else {


					MoveDirection = GetDirection (MoveDirection);
					Halt = CheckHalt ();
					//StartMove ();

				}


				//Set Shooting Delay
				if (CanShoot) {
					Shooting ();
					CanShoot = false;
					//Use Random dactor make ai tank shoot more clearly
					CoolTime = GameConstants.COOL_TIME * (Rand.Next (100 - GameConstants.AI_SHOOTING_RANDOM_FACTOR,
					                                            100 + GameConstants.AI_SHOOTING_RANDOM_FACTOR) / 100);
				}
				if (!CanShoot) {
					CurrentShootingTime += gameTime.ElapsedGameTime.Milliseconds;
					if (CurrentShootingTime >= CoolTime) {
						CanShoot = true;
						CurrentShootingTime = 0;
					}

				}

				//Update Projectiles and remove the deactive one
				ProjectilesUpdate (gameTime);


			}
		}




		public int GetDirection(int oldDirection){


			int newDirection = Rand.Next (4);
			if (newDirection != oldDirection) {
				return newDirection;

			} else {

				//not always another direction
				newDirection = Rand.Next (4);
			}
		
			return newDirection;

		}


		/// <summary>
		/// AI Starts the move.
		/// </summary>
		public void StartMove(){


			switch (MoveDirection) {
				case  0:
				MoveUp ();
				GunDirection = Direction.Up;
				break;
				case 1:
				MoveRight ();
				GunDirection = Direction.Right;
				break;
				case 2:
				MoveDown ();
				GunDirection = Direction.Down;
				break;
				case 3:
				MoveLeft ();
				GunDirection = Direction.Left;
				break;
			}
		}





		#endregion
	}
	/// <summary>
	/// Medium tank. The same with the ai
	/// </summary>
	public class MediumTank:AI{

		public MediumTank(ContentManager content, String spriteName, int x, int y, Map map)
			:base(content,spriteName,x,y, map){

		}


	}

	public class ArmoredCar:AI{

		public ArmoredCar(ContentManager content, String spriteName, int x, int y, Map map)
			:base(content,spriteName,x,y, map){
			this.MovingSpeed = GameConstants.TANK_MOVING_SPEED_ArmoredCar;
		

		}

		public void AutoUpdate(GameTime gameTime)
		{

			//Wether the tank is halted
			Halt = CheckHalt ();

			//running time
			if (!Halt) {
				this.Rand = new Random ();
				CurrentMovingTimeAI += gameTime.ElapsedGameTime.Milliseconds;
				if (CurrentMovingTimeAI < ContinuousMovingTime) {
					StartMove ();

				} else {
					MoveDirection = GetDirection (MoveDirection);

					CurrentMovingTimeAI = 0;
					ContinuousMovingTime = Rand.Next (GameConstants.TANK_MOVE_TIME / 2, GameConstants.TANK_MOVE_TIME * 2);
				}
			} else {
				//When Halted, check the tank whether or not is at the border
				Bound ();
				MoveDirection = Rand.Next (4);
				Halt = false;
				StartMove ();

			}


			//Set Shooting Delay
			if (CanShoot) {
				Shooting ();
				CanShoot = false;
				//Use Random dactor make ai tank shoot more clearly
				CoolTime =GameConstants.COOL_TIME*(Rand.Next(100-GameConstants.AI_SHOOTING_RANDOM_FACTOR,
				                                             100+GameConstants.AI_SHOOTING_RANDOM_FACTOR)/100);
			}
			if (!CanShoot) {
				CurrentShootingTime += gameTime.ElapsedGameTime.Milliseconds;
				if (CurrentShootingTime >= CoolTime) {
					CanShoot = true;
					CurrentShootingTime = 0;
				}

			}

			//Update Projectiles and remove the deactive one
			ProjectilesUpdate (gameTime);

		}
	}
	
}

