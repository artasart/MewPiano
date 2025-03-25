using EnhancedScrollerDemos.GridSimulation;
using EnhancedUI;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScrollController : Controller
{
	List<InventoryData> lists = new List<InventoryData>();

	public void Refresh(List<InventoryData> _list)
	{
		lists = _list;

		Application.targetFrameRate = 60;

		scroller.Delegate = this;

		LoadData();
	}

	protected override void LoadData()
	{
		_data = new SmallList<EnhancedScrollerDemos.GridSimulation.Data>();

		for (int i = 0; i < lists.Count; i++)
		{
			_data.Add(lists[i]);
		}

		scroller.ReloadData();
	}
}
