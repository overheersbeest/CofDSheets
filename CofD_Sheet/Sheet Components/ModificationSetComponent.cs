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
	public class ModificationSetComponent : ISheetComponent
	{
		[Serializable]
		public class ModificationSet
		{
			public ModificationSet()
			{ }
			public ModificationSet(string _name)
			{
				this.name = _name;
			}
			public ModificationSet(string _name, List<Modification> _modifications)
			{
				this.name = _name;
				this.modifications = _modifications;
			}

			public void Apply()
			{
				Sheet sheet = Form1.instance.sheet;
				foreach (Modification modification in modifications)
				{
					if (modification.path.Count > 0)
					{
						string targetComponentName = modification.path[0];
						foreach (ISheetComponent component in sheet.components)
						{
							if (component.name == targetComponentName)
							{
								component.ApplyModification(modification, sheet);
							}
						}
					}
				}
			}

			[XmlAttribute]
			public string name = "ModificationSet";

			[XmlArray]
			public List<Modification> modifications = new List<Modification>();
		}

		[XmlArray]
		public List<ModificationSet> sets = new List<ModificationSet>();

		[XmlAttribute]
		public int ActiveIndex = 0;

		[XmlIgnore]
		private readonly ComboBox selectionComboBox = new ComboBox() { Left = 5, Top = 5, Width = 300 };

		public ModificationSetComponent() : base("ModificationSetComponent", ColumnId.Undefined)
		{ }

		public ModificationSetComponent(string componentName, List<string> ModificationSetNames, ColumnId _column) : base(componentName, _column)
		{
			foreach (string advantageName in ModificationSetNames)
			{
				sets.Add(new ModificationSet(advantageName));
			}
		}

		override public Control ConstructUIElement()
		{
			uiElement.RowCount = sets.Count;
			uiElement.ColumnCount = 1;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Size = new Size(componentWidth, 30 * sets.Count);
			uiElement.TabIndex = 0;
			selectionComboBox.Items.Clear();

			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

			foreach (ModificationSet set in sets)
			{
				selectionComboBox.Items.Add(set.name);
			}
			selectionComboBox.SelectedIndex = ActiveIndex;
			selectionComboBox.SelectedIndexChanged += OnValueChanged;
			uiElement.Controls.Add(selectionComboBox, 0, 0);

			return uiElement;
		}

		void OnValueChanged(object sender = null, EventArgs e = null)
		{
			ActiveIndex = selectionComboBox.SelectedIndex;
			Form1.instance.sheet.RefreshModifications();

			OnComponentChanged();
		}

		override public void ApplyModification(Modification mod, Sheet sheet)
		{
			throw new Exception("trying to modify a modification component");
		}
	}
}
