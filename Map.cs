using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TankWar
{
	public class Map
	{	
		Texture2D eagleSprite;
		Rectangle eagleRectangle;

		public List<Cube> Cubes=new List<Cube>();
		List<Cube> walls=new List<Cube>();
		List<Cube> grasses=new List<Cube>();
		List<Cube> waters=new List<Cube>();
		List<Cube> irons=new List<Cube>();
		int wallNum;
		int ironNum;
		int grassNum;
		int waterNum;
		//public int [] cubesLocation{ get; }
		//get should be deleted
		int[,] wallsLocation;
		int[,] grassesLocation;
		int[,] watersLocation;
		int[,] ironsLocation;
		WallCube cubeTest;

		public Rectangle Home{
			get { return eagleRectangle;}
		}




		#region Constructors
		public Map (ContentManager content,Random rand)
		{
			//random map
		}
		public Map(ContentManager content){

			//default map
			//walls
			walls = new List<Cube> ();
			wallsLocation =ArrayDimension(MapLayout1.WallCubesLocation) ;


			for (int i=0; i<wallsLocation.GetLength(0); i++) {

				walls.Add (new WallCube (content, wallsLocation [i, 0], wallsLocation [i, 1]));
			}
			//grass
			grasses = new List<Cube> ();
			grassesLocation = ArrayDimension (MapLayout1.GrassCubesLocation);
			for (int i=0; i<grassesLocation.GetLength(0); i++) {

				grasses.Add (new GrassCube (content, grassesLocation [i, 0], wallsLocation [i, 1]));
			}
			//and iron grass water
			irons = new List<Cube> ();
			ironsLocation = ArrayDimension (MapLayout1.IronCubesLocation);
			for (int i=0; i<ironsLocation.GetLength(0); i++) {
				irons.Add (new IronCube (content, ironsLocation [i, 0], ironsLocation [i, 1]));
			}

			waters = new List<Cube> ();
			watersLocation = ArrayDimension (MapLayout1.WaterCubesLocation);
			for (int i=0; i<watersLocation.GetLength(0); i++) {
				waters.Add (new WaterCube (content, watersLocation [i, 0], watersLocation [i, 1]));
			}

			//set cubes list
			// add home firstly
			Cubes.Add(new EagleCube(content,GameConstants.EAGLE_X, GameConstants.WIN_HEIGHT-40));
			Cubes.AddRange (walls);
			Cubes.AddRange (grasses);
			Cubes.AddRange (irons);
			Cubes.AddRange (waters);


		}

		public Map (ContentManager content,int wallNum, int ironNum, int grassNum, int waterNum){
			this.wallNum = wallNum;
			this.ironNum = ironNum;
			this.grassNum = grassNum;
			this.waterNum = waterNum;


		}
		#endregion


		#region Method
		public void Draw(SpriteBatch spriteBatch){
			//spriteBatch.Draw (eagleSprite, eagleRectangle, Color.White);


			foreach (Cube cube in Cubes) {
				cube.Draw (spriteBatch);
			}
			

		}

		public void Update(GameTime gameTime){
		}

		/// <summary>
		/// Arraies the dimension.  Change a 1 dimension array to 2 dimensions
		/// </summary>
		/// <returns>The dimension.</returns>
		/// <param name="arr1">Arr1.</param>

		public int[,] ArrayDimension(int[] arr1){

			//set a temp 2 dimensions array
			int [,] arr2=new int[arr1.Length/2,2];

			//set j value to show the 2 dimensions array
			int j = 0;

			//use for loop to change 1 dimension array to 2 dimensions array
			for (int i=0;i<arr1.Length-1;i+=2) {
				arr2 [j, 0] = arr1 [i];
				arr2 [j, 1] = arr1 [i+1];
				j++;
			}
		
			return arr2;
		}


		#endregion
	}
}

