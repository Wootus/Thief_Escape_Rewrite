/*
* Created by: Zachary T. Vig
* Date Created: 06/15/2015
* Personal version of CIT195 group project Thief-Escape
* Original Creators: Zachary T. Vig, Jamie Gleason, Keegon Cabinaw
*/

/*
* Cell Class .cs File
* Used as memory object to represent each game cell on the map
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ThiefEscape
{
	//-----------------------------------------------------------------------------------------------------
	[Serializable]
	class Cell
	{
		//Enumerations used to define the cell type and sub-types
		//-----------------------------------------------------------------------------------------------------
		#region [ Enumerations ]
		//-----------------------------------------------------------------------------------------------------
		//Enum for each of the main cell types
		//-----------------------------------------------------------------------------------------------------
		public enum Archetypes
		{
			DEFAULT,
			WALL,
			FLOOR,
			DOOR,
			STAIR
		}

		//Enum for each type of wall
		//-----------------------------------------------------------------------------------------------------
		public enum WallType
		{
			DEFAULT,
			ITEM,
			SECRET,
			NULL
		}

		//Enum for each type of Floor
		//-----------------------------------------------------------------------------------------------------
		public enum FloorType
		{
			DEFAULT,
			ITEM,
			SECRET,
			NULL
		}

		//Enum for each type of Door
		//-----------------------------------------------------------------------------------------------------
		public enum DoorType
		{
			DEFAULT,
			LOCKED,
			UNLOCKED,
			EXIT,
			NULL
		}

		//Enum for each type of Stairs
		//-----------------------------------------------------------------------------------------------------
		public enum StairsType
		{
			DEFAULT,
			UP,
			DOWN,
			NULL
		}

		//Enum for each type of item that a cell can contain
		//-----------------------------------------------------------------------------------------------------
		public enum CellContents
		{
			DEFAULT,
			KEY,
			KITTEN,
			SECRET,
			NULL
		}

		//-----------------------------------------------------------------------------------------------------
		#endregion
		//-----------------------------------------------------------------------------------------------------

		//-----------------------------------------------------------------------------------------------------
		#region [ Fields ]
		//-----------------------------------------------------------------------------------------------------
		//internal vars for the cell type for each cell object
		private Archetypes _cellArchetype;
		private WallType _wallType;
		private FloorType _floorType;
		private DoorType _doorType;
		private StairsType _stairsType;
		private CellContents _cellContents;
		private int[ ] _destinationCoords;

		//Misc cell variables
		private bool _containsKey;
		private bool _containsKitten;
		private bool _containsItem;
		private bool _containsSecret;

		//-----------------------------------------------------------------------------------------------------
		#endregion
		//-----------------------------------------------------------------------------------------------------

		//-----------------------------------------------------------------------------------------------------
		#region [ Properties ]
		//-----------------------------------------------------------------------------------------------------

		//Gets or Sets the cell's Archetype
		//-----------------------------------------------------------------------------------------------------
		public Archetypes Archetype
		{
			get { return _cellArchetype; }
			set
			{
				//Set the archetype
				_cellArchetype = value;

				//Sets default values for each cell Archetype
				ArchetypeDefaults(_cellArchetype);
			}
		}

		//Gets or Sets the cell's WallType
		//-----------------------------------------------------------------------------------------------------
		public WallType CellWallType
		{
			get { return _wallType; }
			set
			{
				//Assigns wall type
				_wallType = value;

				//if wall type is an item or a secret then the internal item/secret bools are assigned
				if(_wallType == WallType.ITEM)
				{
					_containsItem = true;
					_cellContents = CellContents.DEFAULT;
				}

				else if(_wallType == WallType.SECRET)
				{
					_containsSecret = true;
					_cellContents = CellContents.DEFAULT;
				}

			}
		}

		//Gets or Sets the cell's FloorType
		//-----------------------------------------------------------------------------------------------------
		public FloorType CellFloorType
		{
			get { return _floorType; }
			set
			{
				//Assigns Walltype
				_floorType = value;

				//if floor type is an item or a secret then the internal item/secret bools are assigned
				if(_floorType == FloorType.ITEM)
				{
					_containsItem = true;
					_cellContents = CellContents.DEFAULT;
				}

				else if(_floorType == FloorType.SECRET)
				{
					_containsSecret = true;
					_cellContents = CellContents.DEFAULT;
				}
			}
		}

		//Gets or Sets the cell's DoorType
		//-----------------------------------------------------------------------------------------------------
		public DoorType CellDoorType
		{
			get { return _doorType; }
			set
			{
				//Checks if current cell is of Archetype DOOR
				if(this._cellArchetype == Archetypes.DOOR)
				{
					//if so then doortype is assigned
					_doorType = value;
				}

				//if not then sets DoorType to default and throws an exception
				else
				{
					_doorType = DoorType.NULL;
					throw new ArgumentException("Only Cell's of Archetype DOOR can have a DoorType");
				}
			}
		}

		//Gets or Sets the cell's StairsType
		//-----------------------------------------------------------------------------------------------------
		public StairsType CellStairsType
		{
			get { return _stairsType; }
			set
			{
				//Checks if current cell is of Archetype Stair
				if(this._cellArchetype == Archetypes.STAIR)
				{
					//if so then stairstype assigned
					_stairsType = value;
				}

				//If not then throws an esception
				else
				{
					_stairsType = StairsType.NULL;
					throw new ArgumentException("Only Cell's of Archetype Stair can have a StairsType.");
				}
			}
		}

		//Gets or Sets the cell's Contents
		//-----------------------------------------------------------------------------------------------------
		public CellContents CellContent
		{
			get { return _cellContents; }
			set
			{
				//Checks if the current cell type can contain items
				if(this._cellArchetype == Archetypes.FLOOR || this._cellArchetype == Archetypes.WALL)
				{

					//If cell can hold an Item then it's assigned
					_cellContents = value;

					//pre-assigned contents is reverted in preparation
					_containsItem = false;
					_containsKey = false;
					_containsKitten = false;
					_containsSecret = false;

					//Switches between the types of contents and assigns bools
					switch(_cellContents)
					{
						//case CellContents.DEFAULT:
						//	break;

						case CellContents.KEY:
							_containsItem = true;
							_containsKey = true;
							break;

						case CellContents.KITTEN:
							_containsItem = true;
							_containsKitten = true;
							break;

						case CellContents.SECRET:
							_containsItem = true;
							_containsSecret = true;
							break;

						//case CellContents.NULL:
						//	break;

						//default:
						//	break;
					}
				}

				//If not then exception is thrown
				else
					throw new ArgumentException("Only Cell's of Archetype FLOOR and DOOR can contain Items/Keys/Kittens/Secrets.");
			}
		}

		//Gets or Sets whether the cell contains a key or not
		//-----------------------------------------------------------------------------------------------------
		public bool CellContainsKey
		{
			get { return _containsKey; }
			set
			{
				//Only Cell's of archetype FLOOR and WALL can contain a key
				if(this._cellArchetype == Archetypes.FLOOR || this._cellArchetype == Archetypes.WALL)
				{
					//assigns value
					_containsKey = value;

					//Defaults depending on value
					if(this._containsKey == true)
						_containsItem = true;
					else
						_containsItem = false;
				}

				//if not then throws an exception
				else
					throw new ArgumentException("Only Cell's of Archetype FLOOR and WALL can contain a Key.");
			}
		}

		//Gets or Sets whether the cell contains a kitten or not
		//-----------------------------------------------------------------------------------------------------
		public bool CellContainsKitten
		{
			get { return _containsKitten; }
			set
			{
				//Only Cell's of archetype FLOOR and WALL can contain a kitten
				if(this._cellArchetype == Archetypes.FLOOR || this._cellArchetype == Archetypes.WALL)
				{
					//assigns value
					_containsKitten = value;

					//Defaults depending on value
					if(this._containsKitten == true)
						_containsItem = true;
					else
						_containsItem = false;
				}

				//if not then throws an exception
				else
					throw new ArgumentException("Only Cell's of Archetype FLOOR and WALL can contain a Kitten.");
			}
		}

		//Gets or Sets whether the cell contains an item or not
		//-----------------------------------------------------------------------------------------------------
		public bool CellContainsItem
		{
			get { return _containsItem; }
			set
			{
				//Only Cell's of archetype FLOOR and WALL can contain an item
				if(this._cellArchetype == Archetypes.FLOOR || this._cellArchetype == Archetypes.WALL)
				{
					//assigns value
					_containsItem = value;

					//Defaults depending on value
					if(this._containsItem == false)
						_containsItem = false;
				}

				//if not then throws an exception
				else
					throw new ArgumentException("Only Cell's of Archetype FLOOR and WALL can contain an Item.");
			}
		}

		//Gets or Sets whether the cell contains a secret
		//-----------------------------------------------------------------------------------------------------
		public bool CellContainsSecret
		{
			get { return _containsSecret; }
			set
			{
				//Only Cell's of archetype FLOOR and WALL can contain a Secret
				if(this._cellArchetype == Archetypes.FLOOR || this._cellArchetype == Archetypes.WALL)
				{
					//assigns value
					_containsSecret = value;

					//Defaults depending on value
					if(this._containsSecret == true)
						_containsItem = true;
					else
						_containsItem = false;
				}

				//if not then throws an exception
				else
					throw new ArgumentException("Only Cell's of Archetype FLOOR and WALL can contain a Secret.");
			}
		}

		//Gets or sets the destination coords of a stair cell
		//-----------------------------------------------------------------------------------------------------
		public int[ ] StairDestinationCoords
		{
			get { return _destinationCoords; }
			set
			{
				//Checks if the current cell is of archetype stair
				if(this._cellArchetype == Archetypes.STAIR)
				{
					_destinationCoords = value;
				}

				//if not then esception is thrown
				else
					throw new ArgumentException("Only Cell's of Archetype STAIR can have destination coords.");
			}
		}
		//-----------------------------------------------------------------------------------------------------
		#endregion
		//-----------------------------------------------------------------------------------------------------

		//-----------------------------------------------------------------------------------------------------
		#region [ Constructors ]
		//-----------------------------------------------------------------------------------------------------

		//Default constructor
		//-----------------------------------------------------------------------------------------------------
		public Cell( )
		{
			//Default cell type is FLOOR
			_cellArchetype = Archetypes.FLOOR;
		}

		//Overloaded constructor for passed Archetype
		//-----------------------------------------------------------------------------------------------------
		public Cell(Archetypes type)
		{
			//assigned archetype
			_cellArchetype = type;

			//Sets default values for each cell Archetype
			ArchetypeDefaults(type);
		}

		//Overloaded constructor for passed archetype, floortype
		//-----------------------------------------------------------------------------------------------------
		public Cell(Archetypes type, FloorType floor)
		{
			//assigns archetype
			_cellArchetype = type;

			//Sets default values for each cell Archetype
			ArchetypeDefaults(type);

			//assigns FLOOR type
			_floorType = floor;
		}

		//Overloaded constructor for passed archetype, floortype, Contents
		//-----------------------------------------------------------------------------------------------------
		public Cell(Archetypes type, FloorType floor, CellContents contents)
		{
			//assigns archetype
			_cellArchetype = type;

			//Sets default values for each cell Archetype
			ArchetypeDefaults(type);

			//assigns FLOOR type
			_floorType = floor;

			//Switch for different content types
			switch(contents)
			{
				case CellContents.DEFAULT:
					_cellContents = CellContents.DEFAULT;
					_containsItem = false;
					_containsKey = false;
					_containsKitten = false;
					_containsSecret = false;
					break;

				case CellContents.KEY:
					_cellContents = CellContents.KEY;
					_containsItem = true;
					_containsKey = true;
					_containsKitten = false;
					_containsSecret = false;
					break;

				case CellContents.KITTEN:
					_cellContents = CellContents.KITTEN;
					_containsItem = true;
					_containsKitten = true;
					_containsKey = false;
					_containsSecret = false;
					break;

				case CellContents.SECRET:
					_cellContents = CellContents.SECRET;
					_containsItem = true;
					_containsSecret = true;
					_containsKey = false;
					_containsKitten = false;
					break;

				//case CellContents.NULL:
				//	break;
				//default:
				//	break;
			}
		}

		//overloaded constructor for passed archetype, walltype
		//-----------------------------------------------------------------------------------------------------
		public Cell(Archetypes type, WallType wall)
		{
			//assigns archetype
			_cellArchetype = type;

			//Sets default values for each cell Archetype
			ArchetypeDefaults(type);

			//assigns wall type
			_wallType = wall;
		}

		//overloaded constructor for passed archetype, walltype, contents
		//-----------------------------------------------------------------------------------------------------
		public Cell(Archetypes type, WallType wall, CellContents contents)
		{
			//assigns archetype
			_cellArchetype = type;

			//Sets default values for each cell Archetype
			ArchetypeDefaults(type);

			//assigns wall type
			_wallType = wall;

			//Switch for different content types
			switch(contents)
			{
				case CellContents.DEFAULT:
					_cellContents = CellContents.DEFAULT;
					_containsItem = false;
					_containsKey = false;
					_containsKitten = false;
					_containsSecret = false;
					break;

				case CellContents.KEY:
					_cellContents = CellContents.KEY;
					_containsItem = true;
					_containsKey = true;
					_containsKitten = false;
					_containsSecret = false;
					break;

				case CellContents.KITTEN:
					_cellContents = CellContents.KITTEN;
					_containsItem = true;
					_containsKitten = true;
					_containsKey = false;
					_containsSecret = false;
					break;

				case CellContents.SECRET:
					_cellContents = CellContents.SECRET;
					_containsItem = true;
					_containsSecret = true;
					_containsKey = false;
					_containsKitten = false;
					break;

				//case CellContents.NULL:
				//	break;
				//default:
				//	break;
			}
		}

		//Overloaded Constructor for passed archetype, doortype
		//-----------------------------------------------------------------------------------------------------
		public Cell(Archetypes type, DoorType door)
		{
			//assigns archetype
			_cellArchetype = type;

			//Sets default values for each cell Archetype
			ArchetypeDefaults(type);

			//assigns door type
			_doorType = door;
		}

		//Overloaded Constructor for passed archetype, stairstype
		//-----------------------------------------------------------------------------------------------------
		public Cell(Archetypes type, StairsType stairs)
		{
			//assigns archetype
			_cellArchetype = type;

			//Sets default values for each cell Archetype
			ArchetypeDefaults(type);

			//assigns Stairs type
			_stairsType = stairs;
		}

		//-----------------------------------------------------------------------------------------------------
		#endregion
		//-----------------------------------------------------------------------------------------------------

		//-----------------------------------------------------------------------------------------------------
		#region [ Methods ]
		//-----------------------------------------------------------------------------------------------------

		//-----------------------------------------------------------------------------------------------------
		private void ArchetypeDefaults(Archetypes type)
		{
			//Bool defaults
			_containsItem = false;
			_containsKey = false;
			_containsKitten = false;
			_containsSecret = false;

			//switches between the Archetypes and assigns defaults accordingly
			switch(type)
			{
				//case Archetypes.DEFAULT:
				//	break;

				//If Wall
				case Archetypes.WALL:
					_wallType = WallType.DEFAULT;
					_floorType = FloorType.NULL;
					_doorType = DoorType.NULL;
					_stairsType = StairsType.NULL;
					_cellContents = CellContents.DEFAULT;
					break;

				//If Floor
				case Archetypes.FLOOR:
					_wallType = WallType.NULL;
					_floorType = FloorType.DEFAULT;
					_doorType = DoorType.NULL;
					_stairsType = StairsType.NULL;
					_cellContents = CellContents.DEFAULT;
					break;

				//If Door
				case Archetypes.DOOR:
					_wallType = WallType.NULL;
					_floorType = FloorType.DEFAULT;
					//Doors unlocked by default
					_doorType = DoorType.UNLOCKED;
					_stairsType = StairsType.NULL;
					//Door and Stair cells can't contain items
					_cellContents = CellContents.NULL;
					break;

				//If Stairs
				case Archetypes.STAIR:
					_wallType = WallType.NULL;
					_floorType = FloorType.DEFAULT;
					_doorType = DoorType.NULL;
					_stairsType = StairsType.DEFAULT;
					//Door and Stair cells can't contain items
					_cellContents = CellContents.NULL;
					break;

				//default:
				//	break;
			}
		}
		//-----------------------------------------------------------------------------------------------------
		#endregion
		//-----------------------------------------------------------------------------------------------------

	}
}


