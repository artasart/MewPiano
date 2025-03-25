using System;
using static Enums;

namespace EnhancedScrollerDemos.GridSimulation
{
	[Serializable]
	public class Data
	{
		public string message;
	}

	public class InventoryData : Data
	{
		public string name;
		public string thumbnail;
		public int quantity;
	}
}