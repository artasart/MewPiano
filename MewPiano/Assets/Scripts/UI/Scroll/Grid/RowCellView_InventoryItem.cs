using EnhancedScrollerDemos.GridSimulation;
using TMPro;
using UnityEngine;

public class RowCellView_InventoryItem : RowCellView
{
	TMP_Text txtmp_Quantity;
	TMP_Text txtmp_Name;

	private void Awake()
	{
		txtmp_Name = Util.FindTMPText(this.gameObject, nameof(txtmp_Name));
		txtmp_Quantity = Util.FindTMPText(this.gameObject, nameof(txtmp_Quantity));
	}

	public override void SetData(EnhancedScrollerDemos.GridSimulation.Data data)
	{
		base.SetData(data);

		var invenData = data as InventoryData;

		if (invenData != null)
		{
			txtmp_Name.text = invenData.name;
			txtmp_Quantity.text = invenData.quantity.ToString();

			container.GetComponent<CanvasGroup>().alpha = 1f;
			container.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}

		else
		{
			container.SetActive(true);
			container.GetComponent<CanvasGroup>().alpha = 0f;
			container.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}
}
