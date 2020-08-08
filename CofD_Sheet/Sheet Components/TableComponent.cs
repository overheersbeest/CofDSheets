using CofD_Sheet.Modifications;
using CofD_Sheet.Modifyables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class TableComponent : ISheetComponent
	{
		public class TableColumn
		{
			public TableColumn()
			{ }

			public TableColumn(string _columnName, TableValue _defaultValue)
			{
				name = _columnName;
				defaultValue = _defaultValue;
			}

			[XmlAttribute]
			public string name = "TableColumn";

			[XmlElement]
			public TableValue defaultValue;
		}

		[XmlInclude(typeof(TableValue_String))]
		[XmlInclude(typeof(TableValue_Numeric))]
		[XmlInclude(typeof(TableValue_NumericSuffix))]
		[XmlInclude(typeof(TableValue_Range))]
		public abstract class TableValue
		{
			[XmlIgnore]
			protected const int uiMargin = 20;

			abstract public void BuildValueControls(string valueName, int left, int width, ref List<Control> valueControls, ref List<Control> nonValueControls, ref int valueControlsHeight, ref int tabIndexCounter);
			abstract public void SetValueWithControls(List<Control> valueControls);

			virtual public void ResetModifications() { }

			abstract public override string ToString();
		}

		public class TableValue_String : TableValue
		{
			public TableValue_String()
			{ }

			public TableValue_String(string _value)
			{
				value = _value;
			}

			[XmlAttribute]
			public string value = "";

			override public void BuildValueControls(string valueName, int left, int width, ref List<Control> valueControls, ref List<Control> nonValueControls, ref int valueControlsHeight, ref int tabIndexCounter)
			{
				int controlWidth = (width - uiMargin) / 2;
				Label columnNameLabel = new Label() { Left = left, Top = valueControlsHeight, Width = controlWidth, Text = valueName, Font = new Font(Label.DefaultFont, FontStyle.Bold) };
				nonValueControls.Add(columnNameLabel);

				TextBox inputBox = new TextBox() { Left = left + controlWidth + uiMargin, Top = valueControlsHeight, Width = controlWidth, Text = value, TabIndex = tabIndexCounter++ };
				valueControls.Add(inputBox);
				valueControlsHeight += rowHeight;
			}
			override public void SetValueWithControls(List<Control> valueControls)
			{
				if (valueControls.Count >= 1
					&& valueControls[0] is TextBox valueBox)
				{
					value = valueBox.Text;
				}
			}

			public override string ToString()
			{
				return value;
			}
		}

		public class TableValue_Numeric : TableValue
		{
			public TableValue_Numeric()
			{ }

			public TableValue_Numeric(int _value, bool _alwaysIncludeSign, bool _negativeSignOnZero)
			{
				value.CurrentValue = _value;
				alwaysIncludeSign = _alwaysIncludeSign;
				negativeSignOnZero = _negativeSignOnZero;
			}

			[XmlElement]
			public ModifiableInt value = new ModifiableInt();

			[XmlAttribute]
			public bool alwaysIncludeSign = false;

			[XmlAttribute]
			public bool negativeSignOnZero = false;

			override public void BuildValueControls(string valueName, int left, int width, ref List<Control> valueControls, ref List<Control> nonValueControls, ref int valueControlsHeight, ref int tabIndexCounter)
			{
				int controlWidth = (width - uiMargin) / 2;
				Label columnNameLabel = new Label() { Left = left, Top = valueControlsHeight, Width = controlWidth, Text = valueName, Font = new Font(Label.DefaultFont, FontStyle.Bold) };
				nonValueControls.Add(columnNameLabel);

				NumericUpDown valueInputBox = new NumericUpDown() { Left = left + controlWidth + uiMargin, Top = valueControlsHeight, Width = controlWidth, Minimum = int.MinValue, Value = value.CurrentValue, TabIndex = tabIndexCounter++};
				valueControls.Add(valueInputBox);
				valueControlsHeight += rowHeight;
			}
			override public void SetValueWithControls(List<Control> valueControls)
			{
				if (valueControls.Count >= 1
					&& valueControls[0] is NumericUpDown valueInputBox)
				{
					value.CurrentValue = (int)valueInputBox.Value;
				}
			}

			override public void ResetModifications()
			{
				value.Reset();
			}

			public override string ToString()
			{
				if (negativeSignOnZero
					&& value.CurrentValue == 0)
				{
					return "-" + value.CurrentValue.ToString();
				}
				else if (alwaysIncludeSign
					&& value.CurrentValue >= 0)
				{
					return "+" + value.CurrentValue.ToString();
				}
				else
				{
					return value.CurrentValue.ToString();
				}
			}
		}

		public class TableValue_NumericSuffix : TableValue_Numeric
		{
			public TableValue_NumericSuffix()
			{ }

			public TableValue_NumericSuffix(int _value, bool _alwaysIncludeSign, bool _negativeSignOnZero, string _suffix) : base(_value, _alwaysIncludeSign, _negativeSignOnZero)
			{
				suffix = _suffix;
			}

			[XmlAttribute]
			public string suffix = "";

			override public void BuildValueControls(string valueName, int left, int width, ref List<Control> valueControls, ref List<Control> nonValueControls, ref int valueControlsHeight, ref int tabIndexCounter)
			{
				base.BuildValueControls(valueName, left, width, ref valueControls, ref nonValueControls, ref valueControlsHeight, ref tabIndexCounter);

				int controlWidth = (width - uiMargin) / 2;
				Label suffixLabel = new Label() { Left = left, Top = valueControlsHeight, Width = controlWidth, Text = "suffix" };
				nonValueControls.Add(suffixLabel);
				TextBox suffixInputBox = new TextBox() { Left = left + controlWidth + uiMargin, Top = valueControlsHeight, Width = controlWidth, Text = suffix, TabIndex = tabIndexCounter++ };
				valueControls.Add(suffixInputBox);
				valueControlsHeight += rowHeight;
			}
			override public void SetValueWithControls(List<Control> valueControls)
			{
				if (valueControls.Count >= 2
					&& valueControls[0] is NumericUpDown valueInputBox
					&& valueControls[1] is TextBox suffixInputBox)
				{
					value.CurrentValue = (int)valueInputBox.Value;
					suffix = suffixInputBox.Text;
				}
			}

			public override string ToString()
			{
				return base.ToString() + suffix;
			}
		}

		public class TableValue_Range : TableValue
		{
			public TableValue_Range()
			{ }

			public TableValue_Range(int _shortRange, int _mediumRange, int _longRange)
			{
				shortRange.CurrentValue = _shortRange;
				mediumRange.CurrentValue = _mediumRange;
				longRange.CurrentValue = _longRange;
			}

			[XmlElement]
			public ModifiableInt shortRange = new ModifiableInt();

			[XmlElement]
			public ModifiableInt mediumRange = new ModifiableInt();

			[XmlElement]
			public ModifiableInt longRange = new ModifiableInt();

			override public void BuildValueControls(string valueName, int left, int width, ref List<Control> valueControls, ref List<Control> nonValueControls, ref int valueControlsHeight, ref int tabIndexCounter)
			{
				int nameWidth = (width - uiMargin) / 2;
				int inputWidth = ((nameWidth + uiMargin) / 3) - uiMargin;
				Label columnNameLabel = new Label() { Left = left, Top = valueControlsHeight, Width = nameWidth, Text = valueName, Font = new Font(Label.DefaultFont, FontStyle.Bold) };
				nonValueControls.Add(columnNameLabel);

				NumericUpDown shortInputBox = new NumericUpDown() { Left = left + nameWidth + uiMargin, Top = valueControlsHeight, Width = inputWidth, Value = shortRange.CurrentValue, TabIndex = tabIndexCounter++ };
				valueControls.Add(shortInputBox);

				NumericUpDown mediumInputBox = new NumericUpDown() { Left = left + nameWidth + (2 * uiMargin) + inputWidth, Top = valueControlsHeight, Width = inputWidth, Value = mediumRange.CurrentValue, TabIndex = tabIndexCounter++ };
				shortInputBox.ValueChanged += (sender2, e2) => { mediumInputBox.Value = shortInputBox.Value * 2; };
				valueControls.Add(mediumInputBox);

				NumericUpDown longInputBox = new NumericUpDown() { Left = left + width - inputWidth, Top = valueControlsHeight, Width = inputWidth, Value = longRange.CurrentValue, TabIndex = tabIndexCounter++ };
				mediumInputBox.ValueChanged += (sender2, e2) => { longInputBox.Value = mediumInputBox.Value * 2; };
				valueControls.Add(longInputBox);
				valueControlsHeight += rowHeight;
			}
			override public void SetValueWithControls(List<Control> valueControls)
			{
				if (valueControls.Count >= 3
					&& valueControls[0] is NumericUpDown shortInputBox
					&& valueControls[1] is NumericUpDown mediumInputBox
					&& valueControls[2] is NumericUpDown longInputBox)
				{
					shortRange.CurrentValue = (int)shortInputBox.Value;
					mediumRange.CurrentValue = (int)mediumInputBox.Value;
					longRange.CurrentValue = (int)longInputBox.Value;
				}
			}

			override public void ResetModifications()
			{
				shortRange.Reset();
				mediumRange.Reset();
				longRange.Reset();
			}

			public override string ToString()
			{
				if (shortRange.CurrentValue <= 0
					&& mediumRange.CurrentValue <= 0
					&& longRange.CurrentValue <= 0)
				{
					return "-";
				}
				else
				{
					return shortRange.CurrentValue + "/" + mediumRange.CurrentValue + "/" + longRange.CurrentValue;
				}
			}
		}

		public class TableRow
		{
			public TableRow()
			{ }

			[XmlArray]
			public List<TableValue> values = new List<TableValue>();

			[XmlIgnore]
			public List<Label> labels = new List<Label>();

			public override string ToString()
			{
				string retVal = "";
				for (int i = 0; i < values.Count; ++i)
				{
					retVal += values[i].ToString();
					if (i + 1 < values.Count)
					{
						retVal += "    ";
					}
				}
				return retVal;
			}
		}

		[XmlAttribute]
		public bool canModifyRows = true;

		[XmlAttribute]
		public string rowName = "Row";

		[XmlArray]
		public List<TableColumn> columns = new List<TableColumn>();

		[XmlArray]
		public List<TableRow> rows = new List<TableRow>();

		[XmlIgnore]
		private const int minColumnWidth = 25;

		public TableComponent() : base("TableComponent", ColumnId.Undefined)
		{ }

		public TableComponent(string componentName, bool _canModifyRows, string _singulairRowName, List<Tuple<string, TableValue>> _columns, ColumnId _column) : base(componentName, _column)
		{
			canModifyRows = _canModifyRows;
			rowName = _singulairRowName;

			foreach (Tuple<string, TableValue> Column in _columns)
			{
				columns.Add(new TableColumn(Column.Item1, Column.Item2));
			}
			
			Init();
		}

		override public void Init()
		{
			uiElement.Dock = DockStyle.Fill;

			UpdateContextMenu();
		}

		private void UpdateContextMenu()
		{
			ContextMenuStrip contextMenu = new ContextMenuStrip();
			if (canModifyRows)
			{
				ToolStripItem addTraitItem = contextMenu.Items.Add("Add " + rowName);
				addTraitItem.Click += new EventHandler(OpenAddRowDialog);
				if (rows.Count > 0)
				{
					ToolStripItem removeTraitItem = contextMenu.Items.Add("Remove " + rowName);
					removeTraitItem.Click += new EventHandler(OpenRemoveRowDialog);
				}
			}
			uiElement.ContextMenuStrip = contextMenu;
			Form1.TransferContextMenuForControl(uiElement);
		}

		override public Control ConstructUIElement()
		{
			OnTableChanged();

			return uiElement;
		}

		#region Add/Remove Row
		void OpenAddRowDialog(object sender, EventArgs e)
		{
			List<List<Control>> valueControls = new List<List<Control>>();
			List<Control> nonValueControls = new List<Control>();
			int valueControlsHeight = 5;
			int tabIndexCounter = 0;
			foreach (TableColumn column in columns)
			{
				List<Control> columnControls = new List<Control>();
				column.defaultValue.BuildValueControls(column.name, 5, 300, ref columnControls, ref nonValueControls, ref valueControlsHeight, ref tabIndexCounter);
				valueControls.Add(columnControls);
				valueControlsHeight += 10;
			}

			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = valueControlsHeight + 70,
				Text = "Add " + rowName
			};

			foreach (List<Control> columnControls in valueControls)
			{
				foreach (Control valueControl in columnControls)
				{
					prompt.Controls.Add(valueControl);
				}
			}
			foreach (Control nonValueControl in nonValueControls)
			{
				prompt.Controls.Add(nonValueControl);
			}

			bool confirmed = false;

			Button confirmation = new Button() { Text = "Add", Left = 205, Width = 100, Top = valueControlsHeight };
			confirmation.TabIndex = tabIndexCounter;
			confirmation.Click += (sender2, e2) => { confirmed = true; prompt.Close(); };
			prompt.Controls.Add(confirmation);
			prompt.ShowDialog();

			if (confirmed)
			{
				TableRow newRow = new TableRow();
				for (int i = 0; i < columns.Count; ++i)
				{
					TableValue newValue = columns[i].defaultValue;
					newValue.SetValueWithControls(valueControls[i]);
					newRow.values.Add(newValue);
				}
				rows.Add(newRow);

				OnTableChanged();
			}
		}

		void OpenRemoveRowDialog(object sender, EventArgs e)
		{
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Remove " + rowName
			};

			bool confirmed = false;

			ComboBox inputBox = new ComboBox() { Left = 5, Top = 5, Width = 300 };
			foreach (TableRow row in rows)
			{
				inputBox.Items.Add(row.ToString());
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
				string rowStringtoRemove = inputBox.SelectedItem as string;
				TableRow rowtoRemove = rows.Find(x => rowStringtoRemove == x.ToString());
				if (rowtoRemove != null)
				{
					rows.Remove(rowtoRemove);
					OnTableChanged();
				}
			}
		}
		#endregion

		private void OpenChangeValueDialog(object sender, EventArgs e)
		{
			if (!(sender is ToolStripItem menuItem))
				return;
			if (!(menuItem.Owner is ContextMenuStrip owner))
				return;
			Label senderLabel = (Label)owner.SourceControl;
			if (senderLabel == null)
				return;

			TableLayoutPanelCellPosition position = uiElement.GetPositionFromControl(senderLabel);
			int c = position.Column;
			int r = position.Row;

			List<Control> valueControls = new List<Control>();
			List<Control> nonValueControls = new List<Control>();
			int valueControlsHeight = 5;
			int tabIndexCounter = 0;

			TableValue value = rows[r - 1].values[c];
			value.BuildValueControls(columns[c].name, 5, 300, ref valueControls, ref nonValueControls, ref valueControlsHeight, ref tabIndexCounter);

			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = valueControlsHeight + 70,
				Text = "Change value"
			};

			bool confirmed = false;

			foreach (Control valueControl in valueControls)
			{
				valueControl.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { confirmed = true; prompt.Close(); } };
				prompt.Controls.Add(valueControl);
			}
			foreach (Control nonValueControl in nonValueControls)
			{
				prompt.Controls.Add(nonValueControl);
			}

			Button confirmation = new Button() { Text = "Add", Left = 205, Width = 100, Top = valueControlsHeight };
			confirmation.TabIndex = tabIndexCounter;
			confirmation.Click += (sender2, e2) => { confirmed = true; prompt.Close(); };
			prompt.Controls.Add(confirmation);
			prompt.ShowDialog();

			if (confirmed)
			{
				value.SetValueWithControls(valueControls);
				OnTableChanged();
			}
		}

		void OnTableChanged()
		{
			uiElement.RowStyles.Clear();
			uiElement.ColumnStyles.Clear();
			uiElement.Controls.Clear();
			uiElement.RowCount = rows.Count + 1;
			uiElement.ColumnCount = columns.Count;

			List<float> columnWidths = new List<float>();
			for (int c = 0; c < columns.Count; ++c)
			{
				TableColumn column = columns[c];

				Label columnTitle = new Label
				{
					Anchor = AnchorStyles.None,
					AutoSize = true,
					Name = "ColumnTitle" + column.name,
					Text = column.name,
					Font = new Font(Label.DefaultFont, FontStyle.Underline)
				};
				uiElement.Controls.Add(columnTitle, c, 0);

				//cache width so we can set column styles later on
				float columnWidth = (float)Math.Ceiling(columnTitle.CreateGraphics().MeasureString(columnTitle.Text, columnTitle.Font).Width);
				columnWidth = Math.Max(columnWidth, minColumnWidth);
				columnWidths.Add(columnWidth);
			}

			uiElement.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
			for (int r = 0; r < rows.Count; ++r)
			{
				TableRow row = rows[r];
				row.labels.Clear();
				for (int c = 0; c < row.values.Count; ++c)
				{
					Label valueLabel = new Label
					{
						Anchor = AnchorStyles.None,
						AutoSize = true
					};

					ContextMenuStrip contextMenu = new ContextMenuStrip();
					if (canModifyRows)
					{
						ToolStripItem addTraitItem = contextMenu.Items.Add("Change value");
						addTraitItem.Click += new EventHandler(OpenChangeValueDialog);
					}
					valueLabel.ContextMenuStrip = contextMenu;

					row.labels.Add(valueLabel);
					uiElement.Controls.Add(valueLabel, c, r + 1);

					columnWidths[c] = Math.Max(columnWidths[c], (float)Math.Ceiling(valueLabel.CreateGraphics().MeasureString(valueLabel.Text, valueLabel.Font).Width));
				}
				uiElement.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
			}

			OnValueChanged();

			float widthSum = 0;
			for (int c = 0; c < columnWidths.Count; ++c)
			{
				foreach (TableRow row in rows)
				{
					columnWidths[c] = Math.Max(columnWidths[c], (float)Math.Ceiling(row.labels[c].CreateGraphics().MeasureString(row.labels[c].Text, row.labels[c].Font).Width));
				}
				widthSum += columnWidths[c];
			}
			float columnScalar = componentWidth / widthSum;
			for (int c = 0; c < columnWidths.Count; ++c)
			{
				uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, columnWidths[c] * columnScalar));
			}

			uiElement.Size = new Size(componentWidth, inputBoxHeight + (rowHeight * rows.Count));

			ResizeParentColumn();
			UpdateContextMenu();
		}

		void OnValueChanged()
		{
			foreach (TableRow row in rows)
			{
				for (int i = 0; i < row.labels.Count; ++i)
				{
					row.labels[i].Text = row.values[i].ToString();
				}
			}

			OnComponentChanged();
		}

		public override int QueryInt(List<string> path)
		{
			//only allow this operation if the first column contains string values, so we can use them as a key to find the row we want to modify
			if (path.Count > 2
				&& columns.Count > 0
				&& columns[0].defaultValue is TableValue_String)
			{
				string targetRow = path[1];
				string targetColumn = path[2];
				for (int c = 0; c < columns.Count; ++c)
				{
					TableColumn column = columns[c];
					if (String.Equals(column.name, targetColumn, StringComparison.OrdinalIgnoreCase))
					{
						for (int r = 0; r < rows.Count; ++r)
						{
							TableRow row = rows[r];
							if (row.values[0] is TableValue_String rowName)
							{
								if (String.Equals(rowName.value, targetRow, StringComparison.OrdinalIgnoreCase))
								{
									if (row.values[c] is TableValue_Numeric targetNumericValue)
									{
										isCurrentlyIncludedInModFormula = true;
										return targetNumericValue.value.CurrentValue;
									}
									else if (row.values[c] is TableValue_Range targetRangeValue)
									{
										if (path.Count > 3)
										{
											if (String.Equals(path[3], "Short", StringComparison.OrdinalIgnoreCase))
											{
												isCurrentlyIncludedInModFormula = true;
												return targetRangeValue.shortRange.CurrentValue;
											}
											else if(String.Equals(path[3], "Medium", StringComparison.OrdinalIgnoreCase))
											{
												isCurrentlyIncludedInModFormula = true;
												return targetRangeValue.mediumRange.CurrentValue;
											}
											else if(String.Equals(path[3], "Long", StringComparison.OrdinalIgnoreCase))
											{
												isCurrentlyIncludedInModFormula = true;
												return targetRangeValue.longRange.CurrentValue;
											}
										}
									}
									break;
								}
							}
						}
						break;
					}
				}
			}

			throw new Exception("Component could not complete Query: " + path.ToString());
		}

		override public void ApplyModification(Modification mod, Sheet sheet)
		{
			//only allow this operation if the first column contains string values, so we can use them as a key to find the row we want to modify
			if (mod.path.Count > 2
				&& columns.Count > 0
				&& columns[0].defaultValue is TableValue_String)
			{
				string targetRow = mod.path[1];
				string targetColumn = mod.path[2];
				for (int c = 0; c < columns.Count; ++c)
				{
					TableColumn column = columns[c];
					if (column.name == targetColumn)
					{
						for (int r = 0; r < rows.Count; ++r)
						{
							TableRow row = rows[r];
							if (row.values[0] is TableValue_String rowName)
							{
								if (rowName.value == targetRow)
								{
									if (row.values[c] is TableValue_Numeric targetValue)
									{
										if (mod is IntModification intMod)
										{
											targetValue.value.ApplyModification(intMod, sheet);
											isCurrentlyModified = true;
										}
									}
									else if (row.values[c] is TableValue_Range targetRangeValue)
									{
										if (mod is IntModification intMod)
										{
											if (mod.path.Count > 3)
											{
												string targetValueName = mod.path[3];
												if (String.Equals(targetValueName, "Short", StringComparison.OrdinalIgnoreCase))
												{
													targetRangeValue.shortRange.ApplyModification(intMod, sheet);
													isCurrentlyModified = true;
												}
												else if (String.Equals(targetValueName, "Medium", StringComparison.OrdinalIgnoreCase))
												{
													targetRangeValue.mediumRange.ApplyModification(intMod, sheet);
													isCurrentlyModified = true;
												}
												else if (String.Equals(targetValueName, "Long", StringComparison.OrdinalIgnoreCase))
												{
													targetRangeValue.longRange.ApplyModification(intMod, sheet);
													isCurrentlyModified = true;
												}
											}
										}
									}
									break;
								}
							}
						}
						break;
					}
				}
			}
		}

		override public void ResetModifications()
		{
			base.ResetModifications();
			foreach (TableRow Row in rows)
			{
				foreach (TableValue Value in Row.values)
				{
					Value.ResetModifications();
				}
			}
		}

		override public void OnModificationsComplete()
		{
			if (isCurrentlyModified || wasPreviouslyModified)
			{
				OnTableChanged();
			}
			base.OnModificationsComplete();
		}
	}
}
