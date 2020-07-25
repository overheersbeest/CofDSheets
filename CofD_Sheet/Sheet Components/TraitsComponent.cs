using CofD_Sheet.Modifications;
using CofD_Sheet.Modifyables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class TraitsComponent : ISheetComponent
	{
		public class Trait
		{
			public Trait()
			{ }
			public Trait(string _name)
			{
				this.name = _name;
			}
			public Trait(string _name, int _value)
			{
				this.name = _name;
				this.Value.CurrentValue = _value;
			}

			[XmlAttribute]
			public string name = "Trait";

			[XmlElement]
			public ModifiableInt Value = new ModifiableInt(0);

			[XmlElement]
			public List<string> specialties = new List<string>();

			[XmlIgnore]
			public readonly List<RadioButton> pips = new List<RadioButton>();
		}

		[XmlIgnore]
		const int maxDotsPerRow = 10;

		[XmlIgnore]
		const int nameLabelWidth = 160;

		[XmlAttribute]
		public int maxValue = 5;

		[XmlAttribute]
		public bool canHaveSpecialties = false;

		[XmlAttribute]
		public string specialtyName = "specialty";

		[XmlAttribute]
		public bool canModifyTraits = true;

		[XmlAttribute]
		public string singularName = "trait";

		[XmlAttribute]
		public bool canModifyMaxValue = true;

		[XmlIgnore]
		public int maxValueVisible = -1;

		[XmlArray]
		public List<Trait> Traits = new List<Trait>();

		public TraitsComponent() : base("TraitsComponent", ColumnId.Undefined)
		{ }

		public TraitsComponent(string componentName, bool _canHaveSpecialties, string _specialtyName, bool _canModifyTraits, string _singularName, bool _canModifyMaxValue, List<string> TraitNames, int _startValue, int _maxValue, ColumnId _column) : base(componentName, _column)
		{
			canHaveSpecialties = _canHaveSpecialties;
			specialtyName = _specialtyName;
			canModifyTraits = _canModifyTraits;
			singularName = _singularName;
			canModifyMaxValue = _canModifyMaxValue;
			maxValue = _maxValue;

			for (int i = 0; i < TraitNames.Count; ++i)
			{
				Traits.Add(new Trait(TraitNames[i], _startValue));
			}

			Init();
		}

		override public void Init()
		{
			uiElement.Dock = DockStyle.Fill;

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			if (canModifyMaxValue)
			{
				ToolStripItem changeMaxValueItem = contextMenu.Items.Add("Change maximum value");
				changeMaxValueItem.Click += new EventHandler(OpenChangeMaxValueDialog);
			}

			if (canModifyTraits)
			{
				ToolStripItem addTraitItem = contextMenu.Items.Add("Add " + singularName);
				addTraitItem.Click += new EventHandler(OpenAddTraitDialog);
			}

			uiElement.ContextMenuStrip = contextMenu;
		}

		override public Control ConstructUIElement()
		{
			OnMaxValuePossiblyChanged();

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
			inputBox.Value = maxValue;
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
				maxValue = (int)inputBox.Value;

				OnMaxValuePossiblyChanged();
			}
		}

#region Specialties
		void OpenAddSpecialtyDialog(object sender, EventArgs e)
		{
			ContextMenuStrip owner = (sender as ToolStripItem).Owner as ContextMenuStrip;
			Label TraitLabel = owner.SourceControl as Label;
			Trait Trait = Traits.Find(x => TraitLabel.Text.StartsWith(x.name));
			if (Trait == null)
			{
				return;
			}
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Add " + specialtyName
			};

			bool confirmed = false;

			TextBox inputBox = new TextBox() { Left = 5, Top = 5, Width = 300 };
			inputBox.TabIndex = 0;
			inputBox.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { confirmed = true; prompt.Close(); } };
			Button confirmation = new Button() { Text = "Add", Left = 205, Width = 100, Top = 30 };
			confirmation.TabIndex = 1;
			confirmation.Click += (sender2, e2) => { confirmed = true; prompt.Close(); };
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(inputBox);
			prompt.ShowDialog();

			if (confirmed)
			{
				string newSpecialty = inputBox.Text;
				if (!Trait.specialties.Contains(newSpecialty))
				{
					Trait.specialties.Add(newSpecialty);
				}

				OnSpecialtiesChanged(TraitLabel, Trait);
			}
		}

		void OpenRemoveSpecialtyDialog(object sender, EventArgs e)
		{
			ContextMenuStrip owner = (sender as ToolStripItem).Owner as ContextMenuStrip;
			Label TraitLabel = owner.SourceControl as Label;
			Trait Trait = Traits.Find(x => TraitLabel.Text.StartsWith(x.name));
			if (Trait == null)
			{
				return;
			}
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Remove " + specialtyName
			};

			bool confirmed = false;

			ComboBox inputBox = new ComboBox() { Left = 5, Top = 5, Width = 300 };
			foreach (string specialty in Trait.specialties)
			{
				inputBox.Items.Add(specialty);
			}
			inputBox.TabIndex = 0;
			inputBox.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { confirmed = true; prompt.Close(); } };
			Button confirmation = new Button() { Text = "Remove", Left = 205, Width = 100, Top = 30 };
			confirmation.TabIndex = 1;
			confirmation.Click += (sender2, e2) => { confirmed = true; prompt.Close(); };
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(inputBox);
			prompt.ShowDialog();

			if (confirmed)
			{
				string specialtytoRemove = inputBox.SelectedItem as string;
				if (Trait.specialties.Contains(specialtytoRemove))
				{
					Trait.specialties.Remove(specialtytoRemove);
				}

				OnSpecialtiesChanged(TraitLabel, Trait);
			}
		}

		void OnSpecialtiesChanged(Label label, Trait Trait)
		{
			if (Trait.specialties.Count > 0)
			{
				label.Text = (Trait.name + " (" + string.Join(", ", Trait.specialties) + ")").Replace('_', ' ');
			}
			else
			{
				label.Text = Trait.name.Replace('_', ' ');
			}

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			if (canHaveSpecialties)
			{
				ToolStripItem addSpecialtyItem = contextMenu.Items.Add("Add " + specialtyName);
				addSpecialtyItem.Click += new EventHandler(OpenAddSpecialtyDialog);
				if (Trait.specialties.Count > 0)
				{
					ToolStripItem removeSpecialtyItem = contextMenu.Items.Add("Remove " + specialtyName);
					removeSpecialtyItem.Click += new EventHandler(OpenRemoveSpecialtyDialog);
				}
			}
			if (canModifyTraits)
			{
				ToolStripItem addSpecialtyItem = contextMenu.Items.Add("Remove " + Trait.name);
				addSpecialtyItem.Click += new EventHandler(OpenRemoveTraitDialog);
			}

			label.ContextMenuStrip = contextMenu;

			OnComponentChanged();
		}
#endregion

#region Add/Remove Traits
		void OpenAddTraitDialog(object sender, EventArgs e)
		{
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Add " + singularName
			};

			bool confirmed = false;

			TextBox inputBox = new TextBox() { Left = 5, Top = 5, Width = 300 };
			inputBox.TabIndex = 0;
			inputBox.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { confirmed = true; prompt.Close(); } };
			Button confirmation = new Button() { Text = "Add", Left = 205, Width = 100, Top = 30 };
			confirmation.TabIndex = 1;
			confirmation.Click += (sender2, e2) => { confirmed = true; prompt.Close(); };
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(inputBox);
			prompt.ShowDialog();

			if (confirmed)
			{
				string newTrait = inputBox.Text;
				Traits.Add(new Trait(newTrait));

				OnTraitsChanged();
			}
		}

		void OpenRemoveTraitDialog(object sender, EventArgs e)
		{
			ContextMenuStrip owner = (sender as ToolStripItem).Owner as ContextMenuStrip;
			Label traitLabel = owner.SourceControl as Label;
			Trait trait = Traits.Find(x => traitLabel.Text.StartsWith(x.name));
			if (trait == null)
			{
				return;
			}
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Remove " + singularName
			};

			bool confirmed = false;

			Label question = new Label() { Left = 5, Top = 5, Width = 300 };
			question.Text = "Are you sure you want to remove the \"" + trait.name + "\" " + singularName + "?";
			Button confirmation = new Button() { Text = "Yes", Left = 205, Width = 100, Top = 30 };
			confirmation.Click += (sender2, e2) => { confirmed = true; prompt.Close(); };
			Button cancel = new Button() { Text = "No", Left = 100, Width = 100, Top = 30 };
			cancel.Click += (sender2, e2) => { prompt.Close(); };
			prompt.Controls.Add(question);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(cancel);
			prompt.ShowDialog();

			if (confirmed)
			{
				Traits.Remove(trait);

				OnTraitsChanged();
			}
		}

		void OnTraitsChanged()
		{
			int rowsPerTrait = Math.Max(1, Convert.ToInt32(Math.Ceiling(maxValueVisible / Convert.ToSingle(maxDotsPerRow))));
			int rowAmount = Traits.Count * rowsPerTrait;
			int columnAmount = 1 + Math.Min(maxValueVisible, maxDotsPerRow);

			uiElement.RowCount = rowAmount;
			uiElement.ColumnCount = columnAmount;
			uiElement.Size = new Size(componentWidth, rowHeight * rowAmount);
			ResizeParentColumn();
			uiElement.RowStyles.Clear();
			uiElement.ColumnStyles.Clear();
			uiElement.Controls.Clear();

			//column styles
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, nameLabelWidth));
			for (int c = 1; c < columnAmount; ++c)
			{
				uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / (columnAmount - 1)));
			}

			for (int a = 0; a < Traits.Count; ++a)
			{
				Trait trait = Traits[a];
				for (int ar = 0; ar < rowsPerTrait; ++ar)
				{
					uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
					if (ar == 0)
					{
						Label TraitNameLabel = new Label
						{
							Anchor = AnchorStyles.None,
							AutoSize = true,
							Name = "TraitNameLabel" + trait.name,
							Size = new Size(nameLabelWidth, 20)
						};

						OnSpecialtiesChanged(TraitNameLabel, trait);
						uiElement.Controls.Add(TraitNameLabel, 0, a * rowsPerTrait);
					}
				}

				//pips
				trait.pips.Clear();
				for (int p = 0; p < maxValueVisible; ++p)
				{
					RadioButton pip = new RadioButton
					{
						Anchor = System.Windows.Forms.AnchorStyles.None,
						AutoSize = true,
						Size = new System.Drawing.Size(18, 18),
						TabIndex = 0,
						UseVisualStyleBackColor = true,
						Padding = new Padding(0),
						Margin = new Padding(0),
						Dock = DockStyle.Fill
					};
					pip.Click += new EventHandler(RecomputeValues);
					pip.AutoCheck = false;
					trait.pips.Add(pip);

					//insert pip in table
					int rowIndexOfTrait = Convert.ToInt32(Math.Floor(p / Convert.ToSingle(maxDotsPerRow)));
					int column = (p - (rowIndexOfTrait * maxDotsPerRow)) + 1;
					int row = a * rowsPerTrait + rowIndexOfTrait;
					uiElement.Controls.Add(pip, column, row);
				}
			}

			OnValueChanged();
		}
#endregion

		void RecomputeValues(object sender, EventArgs e)
		{
			foreach (Trait trait in Traits)
			{
				for (int i = 0; i < trait.pips.Count; ++i)
				{
					if (sender == trait.pips[i])
					{
						if (trait.Value.CurrentValue == i + 1)
						{
							//when clicking the last pip, reduce value by 1
							trait.Value.CurrentValue = i;
						}
						else
						{
							trait.Value.CurrentValue = i + 1;
						}
					}
				}
			}
			OnValueChanged();
		}

		void OnMaxValuePossiblyChanged()
		{
			int newVisibleMaxValue = maxValue;
			foreach (Trait trait in Traits)
			{
				trait.Value.maxValue = maxValue;
				newVisibleMaxValue = Math.Max(newVisibleMaxValue, trait.Value.CurrentValue);
			}

			if (newVisibleMaxValue != maxValueVisible)
			{
				maxValueVisible = newVisibleMaxValue;
				OnTraitsChanged();
			}
			else
			{
				OnValueChanged();
			}
		}

		void OnValueChanged()
		{
			foreach (Trait trait in Traits)
			{
				for (int i = 0; i < trait.pips.Count; ++i)
				{
					trait.pips[i].Checked = i < trait.Value.CurrentValue;
				}
			}

			OnComponentChanged();
		}

		public override int QueryInt(List<string> path)
		{
			if (path.Count > 1)
			{
				if (path[1] == "MaxValue")
				{
					isCurrentlyIncludedInModFormula = true;
					return maxValue;
				}
				string targetTrait = path[1];
				foreach (Trait trait in Traits)
				{
					if (trait.name == targetTrait)
					{
						isCurrentlyIncludedInModFormula = true;
						return trait.Value.CurrentValue;
					}
				}
			}

			throw new Exception("Component could not complete Query: " + path.ToString());
		}

		override public void ApplyModification(Modification mod, Sheet sheet)
		{
			if (mod is IntModification intMod)
			{
				if (mod.path.Count > 1)
				{
					string targetTrait = mod.path[1];
					foreach (Trait Trait in Traits)
					{
						if (Trait.name == targetTrait)
						{
							Trait.Value.ApplyModification(intMod, sheet);
							isCurrentlyModified = true;
							break;
						}
					}
				}
			}
		}

		override public void ResetModifications()
		{
			base.ResetModifications();
			foreach (Trait Trait in Traits)
			{
				Trait.Value.Reset();
			}
		}

		override public void OnModificationsComplete()
		{
			if (isCurrentlyModified || wasPreviouslyModified)
			{
				OnMaxValuePossiblyChanged();
			}
			base.OnModificationsComplete();
		}
	}
}
