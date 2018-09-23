using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
{
	[Serializable]
	public class HealthComponent : ISheetComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlAttribute]
		public int maxValue = 10;

		[XmlAttribute]
		public int aggrivated = 0;

		[XmlAttribute]
		public int lethal = 0;

		[XmlAttribute]
		public int bashing = 0;
		
		public HealthComponent() : base("HealthComponent", -1)
		{ }

		public HealthComponent(string componentName, int componentColumnIndex) : base(componentName, componentColumnIndex)
		{ }
		
		//void valueChanged(object sender, EventArgs e)
		//{
		//	aggrivated = 0;
		//	lethal = 0;
		//	bashing = 0;
		//	for (int i = 0; i < slots.Count; i++)
		//	{
		//		string text = slots[i].Text;
		//		aggrivated += text.Count(f => f == '*');
		//		lethal += text.Count(f => f == 'x' || f == 'X');
		//		bashing += text.Count(f => f == '/' || f == '\\');
		//		int overDamage = Math.Max(0, aggrivated + lethal + bashing - maxValue);
		//		while (aggrivated < maxValue
		//			   && overDamage > 0)
		//		{
		//			if (bashing > 0)
		//			{
		//				//use bashing to upgrade least severe damage
		//				bashing--;
		//				if (bashing >= 1)
		//				{
		//					//2 bashing -> 1 lethal
		//					bashing--;
		//					lethal++;
		//				}
		//				else
		//				{
		//					//bashing + lethal -> aggrivated
		//					lethal--;
		//					aggrivated++;
		//				}
		//			}
		//			else
		//			{
		//				//no bashing damage, but still too much damage
		//				//2 lethal -> 1 aggrivated
		//				lethal -= 2;
		//				aggrivated++;
		//			}
		//			overDamage--;
		//		}
		//
		//		//in case we still have damage left after having a health track filled with aggrivated damage, remove all other damages
		//		if (aggrivated >= maxValue)
		//		{
		//			aggrivated = maxValue;
		//			lethal = 0;
		//			bashing = 0;
		//		}
		//	}
		//	onValueChanged();
		//}
		//
		//void onValueChanged()
		//{
		//	for (int i = 0; i < slots.Count; i++)
		//	{
		//		slots[i].TextChanged -= valueChanged;
		//		if (i < aggrivated)
		//		{
		//			slots[i].Text = "*";
		//		}
		//		else if (i < aggrivated + lethal)
		//		{
		//			slots[i].Text = "x";
		//		}
		//		else if (i < aggrivated + lethal + bashing)
		//		{
		//			slots[i].Text = "/";
		//		}
		//		else
		//		{
		//			slots[i].Text = "";
		//		}
		//		slots[i].TextChanged += valueChanged;
		//	}
		//
		//	onComponentChanged();
		//}
		//
		//void backSpacePressed(object sender, KeyEventArgs e)
		//{
		//	if (e.KeyData == Keys.Back)
		//	{
		//		((TextBox)sender).Text = "";
		//		valueChanged(sender, null);
		//	}
		//	else if (e.KeyData == Keys.Left
		//			 || e.KeyData == Keys.Right)
		//	{
		//		int currentFocusIndex = 0;
		//		for (int i = 0; i < slots.Count; i++)
		//		{
		//			if (sender == slots[i])
		//			{
		//				currentFocusIndex = i;
		//				break;
		//			}
		//		}
		//		if (e.KeyData == Keys.Left
		//			&& currentFocusIndex > 0)
		//		{
		//			slots[currentFocusIndex - 1].Focus();
		//		}
		//		else if (e.KeyData == Keys.Right
		//				 && currentFocusIndex < slots.Count - 1)
		//		{
		//			slots[currentFocusIndex + 1].Focus();
		//		}
		//	}
		//}
	}
}
