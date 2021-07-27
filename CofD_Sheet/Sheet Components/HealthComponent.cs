using CofD_Sheet.Helpers;
using CofD_Sheet.Modifications;
using CofD_Sheet.Modifyables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public enum DamageCombinationType
	{
		[XmlEnum(Name = "Rollover")]
		Rollover,
		[XmlEnum(Name = "OverwriteLesser")]
		OverwriteLesser
	}

	[Serializable]
	public class HealthComponent : ISheetComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;

		[XmlIgnore]
		const float separatorProportion = 0.5F;

		[XmlElement]
		public ModifiableInt MaxValue = new ModifiableInt(0);

		[XmlElement]
		public bool AllowAggravated = true;

		[XmlElement]
		public DamageCombinationType CombinationType = DamageCombinationType.Rollover;

		[XmlAttribute]
		public int aggravated = 0;

		[XmlAttribute]
		public int lethal = 0;

		[XmlAttribute]
		public int bashing = 0;
		
		[XmlIgnore]
		private readonly List<TextBox> slots = new List<TextBox>();

		public HealthComponent() : base("HealthComponent", ColumnId.Undefined)
		{ }

		public HealthComponent(string componentName, int _MaxValue, bool _AllowAggravated, DamageCombinationType _CombinationType, ColumnId _column, Sheet parentSheet) : base(componentName, _column)
		{
			MaxValue.CurrentValue = _MaxValue;
			AllowAggravated = _AllowAggravated;
			CombinationType = _CombinationType;

			Init(parentSheet);
		}

		override public void Init(Sheet parentSheet)
		{
			base.Init(parentSheet);
			uiElement.Dock = DockStyle.Fill;
			uiElement.TabIndex = 0;

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripItem changeMaxValueItem = contextMenu.Items.Add("Change maximum value");
			changeMaxValueItem.Click += new EventHandler(OpenChangeMaxValueDialog);
			uiElement.ContextMenuStrip = contextMenu;
			Form1.TransferContextMenuForControl(this);
		}

		override public Control ConstructUIElement()
		{
			OnMaxValueChanged(false);
			return uiElement;
		}

		void OpenChangeMaxValueDialog(object sender, EventArgs e)
		{
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Change maximum value"
			};

			bool confirmed = false;

			NumericUpDown inputBox = new NumericUpDown() { Left = 5, Top = 5, Width = 300 };
			inputBox.Minimum = Math.Min(0, MaxValue.CurrentValue);
			inputBox.Value = MaxValue.CurrentValue;
			inputBox.TabIndex = 0;
			inputBox.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { confirmed = true; prompt.Close(); } };
			Button confirmation = new Button() { Text = "Confirm", Left = 205, Width = 100, Top = 30 };
			confirmation.TabIndex = 1;
			confirmation.Click += (sender2, e2) => { confirmed = true; prompt.Close(); };
			Button cancel = new Button() { Text = "Cancel", Left = 100, Width = 100, Top = 30 };
			cancel.TabIndex = 2;
			cancel.Click += (sender2, e2) => { prompt.Close(); };
			prompt.Controls.Add(inputBox);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(cancel);
			prompt.ShowDialog();

			if (confirmed)
			{
				MaxValue.CurrentValue = (int)inputBox.Value;

				OnMaxValueChanged(true);
			}
		}

		void OnMaxValueChanged(bool CanChangeValue)
		{
			if (CanChangeValue)
			{
				switch (CombinationType)
				{
					case DamageCombinationType.Rollover:
						HandleDamageRollover();
						break;
					case DamageCombinationType.OverwriteLesser:
						CapDamageToMax();
						break;
				}
			}

			int rowAmount = Convert.ToInt32(Math.Ceiling(MaxValue.CurrentValue / Convert.ToSingle(maxPerRow)));
			int checkBoxRows = Math.Min(MaxValue.CurrentValue, maxPerRow);
			int columnSeparatorCount = (checkBoxRows - 1) / 5;
			int columnAmount = checkBoxRows + columnSeparatorCount;

			foreach (TextBox slot in slots)
			{
				slot.Dispose();
			}
			slots.Clear();
			uiElement.RowStyles.Clear();
			uiElement.ColumnStyles.Clear();
			uiElement.RowCount = rowAmount;
			uiElement.ColumnCount = columnAmount;
			uiElement.Size = new Size(componentWidth, rowAmount * inputBoxHeight);
			ResizeParentColumn();

			float columnWidth = 100F / (checkBoxRows + (columnSeparatorCount * separatorProportion));
			float separatorWidth = columnWidth * separatorProportion;

			for (int r = 0; r < rowAmount; ++r)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
				for (int c = 0; c < columnAmount; ++c)
				{
					if ((c + 1) % 6 == 0)
					{
						//break, to separate groups of 5
						if (r == 0)
						{
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, separatorWidth));
						}
					}
					else
					{
						if (r == 0)
						{
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, columnWidth));
						}
						int slotNr = slots.Count;
						if (slotNr < MaxValue.CurrentValue)
						{
							TextBox slot = new TextBox
							{
								Anchor = AnchorStyles.None,
								AutoSize = true,
								Size = new Size(15, 14),
								TabIndex = 0
							};
							slot.KeyDown += new KeyEventHandler(OnKeyDown);
							slot.TextChanged += RecomputeValues;
							slots.Add(slot);
							uiElement.Controls.Add(slot, c, r);
						}
					}
				}
			}
			OnValueChanged();
		}

		void RecomputeValues(object sender, EventArgs e)
		{
			switch (CombinationType)
			{
				case DamageCombinationType.Rollover:
					aggravated = 0;
					lethal = 0;
					bashing = 0;
					for (int i = 0; i < slots.Count; ++i)
					{
						string text = slots[i].Text;
						if (AllowAggravated) aggravated += text.Count(f => f == '*');
						lethal += text.Count(f => f == 'x' || f == 'X');
						bashing += text.Count(f => f == '/' || f == '\\');
						HandleDamageRollover();
					}
					break;
				case DamageCombinationType.OverwriteLesser:
					//check new values, so we can check the delta
					int newAggravated = 0;
					int newLethal = 0;
					int newBashing = 0;
					for (int i = 0; i < slots.Count; ++i)
					{
						string text = slots[i].Text;
						if (AllowAggravated) newAggravated += text.Count(f => f == '*');
						newLethal += text.Count(f => f == 'x' || f == 'X');
						newBashing += text.Count(f => f == '/' || f == '\\');
					}

					//least severe damage upgrades first, and each damage overwrites the most severe damage they can. 
					int lethalIncrease = Math.Max(0, newLethal - lethal);
					int aggIncrease = Math.Max(0, newAggravated - aggravated);

					// upgrade lethal
					newBashing = Math.Max(0, newBashing - lethalIncrease);

					//upgrade aggravated
					int spilloverToBashing = Math.Max(0, aggIncrease - newLethal);
					newLethal = Math.Max(0, newLethal - aggIncrease);
					newBashing = Math.Max(0, newBashing - spilloverToBashing);

					aggravated = newAggravated;
					lethal = newLethal;
					bashing = newBashing;

					CapDamageToMax();
					break;
			}

			OnValueChanged();
		}

		void HandleDamageRollover()
		{
			int overDamage = Math.Max(0, aggravated + lethal + bashing - MaxValue.CurrentValue);
			while (aggravated < MaxValue.CurrentValue
				   && overDamage > 0)
			{
				if (bashing > 0)
				{
					//use bashing to upgrade least severe damage
					bashing--;
					if (bashing >= 1)
					{
						//2 bashing -> 1 lethal
						bashing--;
						++lethal;
					}
					else
					{
						//bashing + lethal -> aggravated
						lethal--;
						++aggravated;
					}
				}
				else
				{
					//no bashing damage, but still too much damage
					//2 lethal -> 1 aggravated
					lethal -= 2;
					++aggravated;
				}
				overDamage--;
			}

			//in case we still have damage left after having a health track filled with aggravated damage, remove all other damages
			if (aggravated >= MaxValue.CurrentValue)
			{
				aggravated = MaxValue.CurrentValue;
				lethal = 0;
				bashing = 0;
			}
		}

		void CapDamageToMax()
		{
			int overDamage = Math.Max(0, aggravated + lethal + bashing - MaxValue.CurrentValue);
			//Bashing
			int bashingReduction = Math.Min(bashing, overDamage);
			bashing -= bashingReduction;
			overDamage -= bashingReduction;
			//Lethal
			int lethalReduction = Math.Min(lethal, overDamage);
			bashing -= lethalReduction;
			overDamage -= lethalReduction;
			//Aggravated
			int aggReduction = Math.Min(aggravated, overDamage);
			bashing -= aggReduction;
			//overDamage -= aggReduction;
		}

		void OnValueChanged()
		{
			for (int i = 0; i < slots.Count; ++i)
			{
				slots[i].TextChanged -= RecomputeValues;
				if (i < aggravated)
				{
					slots[i].Text = "*";
				}
				else if (i < aggravated + lethal)
				{
					slots[i].Text = "x";
				}
				else if (i < aggravated + lethal + bashing)
				{
					slots[i].Text = "/";
				}
				else
				{
					slots[i].Text = "";
				}
				slots[i].TextChanged += RecomputeValues;
			}

			OnComponentChanged();
		}

		void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Back)
			{
				((TextBox)sender).Text = "";
				RecomputeValues(sender, null);
			}
			else if (e.KeyData == Keys.Left
					 || e.KeyData == Keys.Right)
			{
				int currentFocusIndex = 0;
				for (int i = 0; i < slots.Count; ++i)
				{
					if (sender == slots[i])
					{
						currentFocusIndex = i;
						break;
					}
				}
				if (e.KeyData == Keys.Left
					&& currentFocusIndex > 0)
				{
					slots[currentFocusIndex - 1].Focus();
				}
				else if (e.KeyData == Keys.Right
						 && currentFocusIndex < slots.Count - 1)
				{
					slots[currentFocusIndex + 1].Focus();
				}
			}
		}

		public override int QueryInt(List<string> path)
		{
			if (path.Count > 1)
			{
				if (String.Equals(path[1], "MaxValue", StringComparison.OrdinalIgnoreCase))
				{
					isCurrentlyIncludedInModFormula = true;
					return MaxValue.CurrentValue;
				}
				if (String.Equals(path[1], "Bashing", StringComparison.OrdinalIgnoreCase))
				{
					isCurrentlyIncludedInModFormula = true;
					return bashing;
				}
				if (String.Equals(path[1], "Lethal", StringComparison.OrdinalIgnoreCase))
				{
					isCurrentlyIncludedInModFormula = true;
					return lethal;
				}
				if (String.Equals(path[1], "Aggravated", StringComparison.OrdinalIgnoreCase))
				{
					isCurrentlyIncludedInModFormula = true;
					return aggravated;
				}
			}

			StaticErrorLogger.AddQueryError(path);
			return 0;
		}

		override public void ApplyModification(Modification mod, Sheet sheet)
		{
			if (mod is IntModification intMod)
			{
				if (mod.path.Count > 1)
				{
					if (String.Equals(mod.path[1], "MaxValue", StringComparison.OrdinalIgnoreCase))
					{
						MaxValue.ApplyModification(intMod, sheet);
						isCurrentlyModified = true;
					}
				}
			}
		}

		override public void ResetModifications()
		{
			base.ResetModifications();
			MaxValue.Reset();
		}

		override public void OnModificationsComplete()
		{
			if (isCurrentlyModified || wasPreviouslyModified)
			{
				OnMaxValueChanged(true);
			}
		}
	}
}
