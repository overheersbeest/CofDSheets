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
	public class HealthComponent : ISheetComponent
	{
		[XmlIgnore]
		const int maxPerRow = 15;

		[XmlIgnore]
		const float separatorProportion = 2F;

		[XmlElement]
		public ModifiableInt MaxValue = new ModifiableInt(10);

		[XmlAttribute]
		public int aggrivated = 0;

		[XmlAttribute]
		public int lethal = 0;

		[XmlAttribute]
		public int bashing = 0;

		[XmlIgnore]
		List<TextBox> slots = new List<TextBox>();

		public HealthComponent() : base("HealthComponent", ColumnId.Undefined)
		{
			Init();
		}

		public HealthComponent(string componentName, ColumnId _column) : base(componentName, _column)
		{
			Init();
		}

		void Init()
		{
			uiElement.Dock = DockStyle.Fill;
			uiElement.TabIndex = 0;

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripItem changeMaxValueItem = contextMenu.Items.Add("Change maximum value");
			changeMaxValueItem.Click += new EventHandler(OpenChangeMaxValueDialog);
			uiElement.ContextMenuStrip = contextMenu;
		}

		override public Control ConstructUIElement()
		{
			OnMaxValueChanged();
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

				OnMaxValueChanged();
			}
		}

		void OnMaxValueChanged()
		{
			HandleDamageRollover();

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

			float separatorWidth = 100F / (checkBoxRows * separatorProportion + columnSeparatorCount);

			for (int r = 0; r < rowAmount; r++)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
				for (int c = 0; c < columnAmount; c++)
				{
					if ((c + 1) % 6 == 0)
					{
						//break, to separate groups of 5
						if (r == 0)
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, separatorWidth));
					}
					else
					{
						if (r == 0)
							uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, separatorWidth * separatorProportion));
						int slotNr = slots.Count;
						if (slotNr < MaxValue.CurrentValue)
						{
							TextBox slot = new TextBox();
							slot.Anchor = System.Windows.Forms.AnchorStyles.None;
							slot.KeyDown += new KeyEventHandler(OnKeyDown);
							slot.AutoSize = true;
							slot.Size = new System.Drawing.Size(15, 14);
							slot.TabIndex = 0;
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
			aggrivated = 0;
			lethal = 0;
			bashing = 0;
			for (int i = 0; i < slots.Count; i++)
			{
				string text = slots[i].Text;
				aggrivated += text.Count(f => f == '*');
				lethal += text.Count(f => f == 'x' || f == 'X');
				bashing += text.Count(f => f == '/' || f == '\\');
				HandleDamageRollover();
			}

			OnValueChanged();
		}

		void HandleDamageRollover()
		{
			int overDamage = Math.Max(0, aggrivated + lethal + bashing - MaxValue.CurrentValue);
			while (aggrivated < MaxValue.CurrentValue
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
						lethal++;
					}
					else
					{
						//bashing + lethal -> aggrivated
						lethal--;
						aggrivated++;
					}
				}
				else
				{
					//no bashing damage, but still too much damage
					//2 lethal -> 1 aggrivated
					lethal -= 2;
					aggrivated++;
				}
				overDamage--;
			}

			//in case we still have damage left after having a health track filled with aggrivated damage, remove all other damages
			if (aggrivated >= MaxValue.CurrentValue)
			{
				aggrivated = MaxValue.CurrentValue;
				lethal = 0;
				bashing = 0;
			}
		}

		void OnValueChanged()
		{
			for (int i = 0; i < slots.Count; i++)
			{
				slots[i].TextChanged -= RecomputeValues;
				if (i < aggrivated)
				{
					slots[i].Text = "*";
				}
				else if (i < aggrivated + lethal)
				{
					slots[i].Text = "x";
				}
				else if (i < aggrivated + lethal + bashing)
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
				for (int i = 0; i < slots.Count; i++)
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

		override public void ApplyModification(ModificationSetComponent.Modification mod)
		{
			if (mod is ModificationSetComponent.IntModification intMod)
			{
				if (mod.path.Count > 1)
				{
					if (mod.path[1] == "MaxValue")
					{
						MaxValue.ApplyModification(intMod.modType, intMod.value);
					}
				}
			}
		}

		override public void ResetModifications()
		{
			MaxValue.Reset();
		}

		override public void OnModificationsComplete()
		{
			OnMaxValueChanged();
		}
	}
}
