using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet.Sheet_Components
{
	[Serializable]
	public class AdvantagesComponent : ISheetComponent
	{
		[XmlInclude(typeof(NumericAdvantage))]
		[XmlInclude(typeof(StringAdvantage))]
		[XmlInclude(typeof(ArmorAdvantage))]
		public abstract class Advantage
		{
			public Advantage(string _name)
			{
				this.name = _name;
			}

			[XmlAttribute]
			public string name = "Advantage";

			public abstract Control ConstructUIElement(AdvantagesComponent Parent);

			public abstract void OnValueChanged();
		}

		public class NumericAdvantage : Advantage
		{
			public NumericAdvantage() : base ("NumericAdvantage")
			{ }
			public NumericAdvantage(string _name) : base (_name)
			{ }
			public NumericAdvantage(string _name, int _value) : base(_name)
			{
				this.value = _value;
			}

			[XmlAttribute]
			public int value = 0;

			[XmlIgnore]
			private Label CachedLabel;

			[XmlIgnore]
			private AdvantagesComponent CachedParent;

			public override Control ConstructUIElement(AdvantagesComponent Parent)
			{
				Label label = new Label
				{
					Anchor = AnchorStyles.Left,
					Text = value.ToString()
				};

				ContextMenuStrip contextMenu = new ContextMenuStrip();
				ToolStripItem ChangeValueItem = contextMenu.Items.Add("Change value");
				ChangeValueItem.Click += new EventHandler(OpenChangeValueDialog);
				label.ContextMenuStrip = contextMenu;

				CachedLabel = label;
				CachedParent = Parent;
				return label;
			}

			public override void OnValueChanged()
			{
				CachedLabel.Text = value.ToString();
			}

			private void OpenChangeValueDialog(object sender, EventArgs e)
			{
				Form prompt = new Form
				{
					StartPosition = FormStartPosition.CenterParent,
					Width = 325,
					Height = 100,
					Text = "Change value"
				};
				NumericUpDown inputBox = new NumericUpDown() { Left = 5, Top = 5, Width = 300 };
				inputBox.Value = value;
				inputBox.TabIndex = 0;
				inputBox.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { prompt.Close(); } };
				Button confirmation = new Button() { Text = "Confirm", Left = 205, Width = 100, Top = 30 };
				confirmation.TabIndex = 1;
				confirmation.Click += (sender2, e2) => { prompt.Close(); };
				Button cancel = new Button() { Text = "Cancel", Left = 100, Width = 100, Top = 30 };
				cancel.TabIndex = 2;
				cancel.Click += (sender2, e2) => { inputBox.Value = value; prompt.Close(); };
				prompt.Controls.Add(inputBox);
				prompt.Controls.Add(confirmation);
				prompt.Controls.Add(cancel);
				prompt.ShowDialog();

				value = (int)inputBox.Value;
				CachedParent.OnValueChanged();
			}
		}

		public class StringAdvantage : Advantage
		{
			public StringAdvantage() : base("StringAdvantage")
			{ }
			public StringAdvantage(string _name) : base(_name)
			{ }
			public StringAdvantage(string _name, string _value) : base(_name)
			{
				this.value = _value;
			}

			[XmlAttribute]
			public string value = "";

			[XmlIgnore]
			private Label CachedLabel;

			[XmlIgnore]
			private AdvantagesComponent CachedParent;

			public override Control ConstructUIElement(AdvantagesComponent Parent)
			{
				Label label = new Label
				{
					Anchor = AnchorStyles.Left,
					Text = value.ToString()
				};

				ContextMenuStrip contextMenu = new ContextMenuStrip();
				ToolStripItem ChangeValueItem = contextMenu.Items.Add("Change value");
				ChangeValueItem.Click += new EventHandler(OpenChangeValueDialog);
				label.ContextMenuStrip = contextMenu;

				CachedLabel = label;
				CachedParent = Parent;
				return label;
			}

			public override void OnValueChanged()
			{
				CachedLabel.Text = value;
			}

			private void OpenChangeValueDialog(object sender, EventArgs e)
			{
				Form prompt = new Form
				{
					StartPosition = FormStartPosition.CenterParent,
					Width = 325,
					Height = 100,
					Text = "Change value"
				};
				TextBox inputBox = new TextBox() { Left = 5, Top = 5, Width = 300 };
				inputBox.Text = value;
				inputBox.TabIndex = 0;
				inputBox.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { prompt.Close(); } };
				Button confirmation = new Button() { Text = "Confirm", Left = 205, Width = 100, Top = 30 };
				confirmation.TabIndex = 1;
				confirmation.Click += (sender2, e2) => { prompt.Close(); };
				Button cancel = new Button() { Text = "Cancel", Left = 100, Width = 100, Top = 30 };
				cancel.TabIndex = 2;
				cancel.Click += (sender2, e2) => { inputBox.Text = value; prompt.Close(); };
				prompt.Controls.Add(inputBox);
				prompt.Controls.Add(confirmation);
				prompt.Controls.Add(cancel);
				prompt.ShowDialog();

				value = inputBox.Text;
				CachedParent.OnValueChanged();
			}
		}

		public class ArmorAdvantage : Advantage
		{
			public ArmorAdvantage() : base("StringAdvantage")
			{ }
			public ArmorAdvantage(string _name) : base(_name)
			{ }
			public ArmorAdvantage(string _name, int _general, int _ballistic) : base(_name)
			{
				this.general = _general;
				this.ballistic = _ballistic;
			}

			[XmlAttribute]
			public int general = 0;

			[XmlAttribute]
			public int ballistic = 0;

			[XmlIgnore]
			private Label CachedLabel;

			[XmlIgnore]
			private AdvantagesComponent CachedParent;

			public override Control ConstructUIElement(AdvantagesComponent Parent)
			{
				Label label = new Label
				{
					Anchor = AnchorStyles.Left,
					Text = general.ToString() + "/" + ballistic.ToString()
				};

				ContextMenuStrip contextMenu = new ContextMenuStrip();
				ToolStripItem ChangeValueItem = contextMenu.Items.Add("Change values");
				ChangeValueItem.Click += new EventHandler(OpenChangeValueDialog);
				label.ContextMenuStrip = contextMenu;

				CachedLabel = label;
				CachedParent = Parent;
				return label;
			}

			public override void OnValueChanged()
			{
				CachedLabel.Text = general.ToString() + "/" + ballistic.ToString();
			}

			private void OpenChangeValueDialog(object sender, EventArgs e)
			{
				Form prompt = new Form
				{
					StartPosition = FormStartPosition.CenterParent,
					Width = 325,
					Height = 100,
					Text = "Change values"
				};

				NumericUpDown inputBoxGeneral = new NumericUpDown() { Left = 5, Top = 5, Width = 120 };
				inputBoxGeneral.Value = general;
				inputBoxGeneral.TabIndex = 0;
				inputBoxGeneral.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { prompt.Close(); } };

				Label label = new Label { Left = 130, Top = 5, Width = 55 };
				label.TextAlign = ContentAlignment.MiddleCenter;
				label.Text = "/";

				NumericUpDown inputBoxBallistic = new NumericUpDown() { Left = 185, Top = 5, Width = 120 };
				inputBoxBallistic.Value = ballistic;
				inputBoxBallistic.TabIndex = 0;
				inputBoxBallistic.KeyDown += (sender2, e2) => { if (e2.KeyCode == Keys.Return) { prompt.Close(); } };

				Button confirmation = new Button() { Text = "Confirm", Left = 205, Width = 100, Top = 30 };
				confirmation.TabIndex = 1;
				confirmation.Click += (sender2, e2) => { prompt.Close(); };
				Button cancel = new Button() { Text = "Cancel", Left = 100, Width = 100, Top = 30 };
				cancel.TabIndex = 2;
				cancel.Click += (sender2, e2) => { inputBoxGeneral.Value = general; inputBoxBallistic.Value = ballistic; prompt.Close(); };

				prompt.Controls.Add(inputBoxGeneral);
				prompt.Controls.Add(label);
				prompt.Controls.Add(inputBoxBallistic);
				prompt.Controls.Add(confirmation);
				prompt.Controls.Add(cancel);
				prompt.ShowDialog();

				general = (int)inputBoxGeneral.Value;
				ballistic = (int)inputBoxBallistic.Value;
				CachedParent.OnValueChanged();
			}
		}

		[XmlArray]
		public List<Advantage> advantages = new List<Advantage>();

		public AdvantagesComponent() : base("AdvantagesComponent", ColumnId.Undefined)
		{ }

		public AdvantagesComponent(string componentName, List<Advantage> _advantages, ColumnId _column) : base(componentName, _column)
		{
			this.advantages = _advantages;
		}

		override public Control ConstructUIElement()
		{
			uiElement.RowCount = advantages.Count;
			uiElement.ColumnCount = 1;
			uiElement.Dock = DockStyle.Fill;
			uiElement.Size = new Size(componentWidth, rowHeight * advantages.Count);
			uiElement.TabIndex = 0;

			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			for (int a = 0; a < advantages.Count; a++)
			{
				Advantage advantage = advantages[a];
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / advantages.Count));

				TableLayoutPanel advantageElement = new TableLayoutPanel
				{
					Name = "advantageTable" + advantage.name,
					Size = new Size(componentWidth, rowHeight)
				};

				Label label = new Label
				{
					Anchor = AnchorStyles.Left,
					Text = advantage.name + ": "
				};
				advantageElement.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, label.Width));
				advantageElement.Controls.Add(label, 0, 0);

				advantageElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
				advantageElement.Controls.Add(advantage.ConstructUIElement(this), 1, 0);

				uiElement.Controls.Add(advantageElement, 0, a);
			}
			OnValueChanged();
			return uiElement;
		}

		void OnValueChanged(object sender = null, EventArgs e = null)
		{
			foreach (Advantage advantage in advantages)
			{
				advantage.OnValueChanged();
			}

			OnComponentChanged();
		}
	}
}
