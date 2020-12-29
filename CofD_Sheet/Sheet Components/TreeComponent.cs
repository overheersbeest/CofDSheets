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
	public class TreeComponent : ISheetComponent
	{
		public class TreeNode
		{
			public TreeNode()
			{ }

			public TreeNode(string _name)
			{
				name = _name;
			}

			public TreeNode(string _name, List<TreeNode> _children)
			{
				name = _name;
				childNodes = Program.DeepClone(_children);
			}

			[XmlAttribute]
			public string name = "TreeNode";

			[XmlElement]
			public List<string> notes = new List<string>();

			[XmlIgnore]
			public List<string> modInsertedNotes = new List<string>();

			[XmlIgnore]
			public List<string> AllNotes
			{
				get
				{
					List<string> retVal = new List<string>();
					retVal.AddRange(notes);
					retVal.AddRange(modInsertedNotes);
					return retVal;
				}
			}

			[XmlElement]
			public List<TreeNode> childNodes = new List<TreeNode>();

			[XmlIgnore]
			public List<TreeNode> modInsertedChildNodes = new List<TreeNode>();

			[XmlIgnore]
			public List<TreeNode> AllChildNodes
			{
				get
				{
					List<TreeNode> retVal = new List<TreeNode>();
					retVal.AddRange(childNodes);
					retVal.AddRange(modInsertedChildNodes);
					return retVal;
				}
			}

			public void ApplyModification(Modification mod, Sheet sheet, int pathDepth, ref bool isCurrentlyModified)
			{
				if (mod.path.Count == pathDepth)
				{
					if (mod is StringModification stringMod)
					{
						if (stringMod.modType == StringModificationType.AddElement)
						{
							string newElementName = stringMod.GetValue(sheet);
							modInsertedChildNodes.Add(new TreeNode(newElementName));
							isCurrentlyModified = true;
						}
					}
				}
				else if (mod.path.Count > pathDepth)
				{
					string target = mod.path[pathDepth];
					if (String.Equals(target, "Notes", StringComparison.OrdinalIgnoreCase))
					{
						if (mod is StringModification stringMod)
						{
							if (stringMod.modType == StringModificationType.AddElement)
							{
								modInsertedNotes.Add(stringMod.GetValue(sheet));
								isCurrentlyModified = true;
							}
						}
					}
					foreach (TreeNode node in AllChildNodes)
					{
						if (String.Equals(target, node.name, StringComparison.OrdinalIgnoreCase))
						{
							node.ApplyModification(mod, sheet, pathDepth + 1, ref isCurrentlyModified);
						}
					}
				}
			}

			public void ResetModifications()
			{
				modInsertedNotes.Clear();
				modInsertedChildNodes.Clear();
				foreach (TreeNode node in childNodes)
				{
					node.ResetModifications();
				}
			}
		}

		[XmlArray]
		public List<string> singularNames = new List<string>();

		[XmlArray]
		public List<TreeNode> nodes = new List<TreeNode>();

		[XmlIgnore]
		public List<TreeNode> modInsertedNodes = new List<TreeNode>();

		[XmlIgnore]
		public List<TreeNode> AllNodes
		{
			get
			{
				List<TreeNode> retVal = new List<TreeNode>();
				retVal.AddRange(nodes);
				retVal.AddRange(modInsertedNodes);
				return retVal;
			}
		}

		public TreeComponent() : base("TreeComponent", ColumnId.Undefined)
		{ }

		public TreeComponent(string componentName, List<string> _singularNames, List<TreeNode> _nodes, ColumnId _column, Sheet parentSheet) : base(componentName, _column)
		{
			singularNames = _singularNames;
			nodes = _nodes;

			Init(parentSheet);
		}

		override public void Init(Sheet parentSheet)
		{
			base.Init(parentSheet);
			uiElement.Dock = DockStyle.Fill;
			uiElement.ColumnCount = 1;
			uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

			ContextMenuStrip contextMenu = new ContextMenuStrip();
			ToolStripItem addTraitItem = contextMenu.Items.Add("Add " + GetSingularName(0));
			addTraitItem.Click += new EventHandler(OpenAddNodeDialog);
			uiElement.ContextMenuStrip = contextMenu;
			Form1.TransferContextMenuForControl(this);
		}

		override public Control ConstructUIElement()
		{
			OnTreeChanged();

			return uiElement;
		}

		public string GetSingularName(int depth)
		{
			if (singularNames.Count > 0)
			{
				if (depth < singularNames.Count)
				{
					return singularNames[depth];
				}
				else
				{
					return singularNames[singularNames.Count - 1];
				}
			}
			return "node";
		}

		public string GetSingularName(TreeNode node)
		{
			int nodeDepth = GetNodeDepth(node, AllNodes, 0);
			if (nodeDepth != -1)
			{
				return GetSingularName(nodeDepth);
			}
			return "node";
		}

		public int GetNodeDepth(TreeNode targetNode, List<TreeNode> nodes, int currentDepth)
		{
			foreach (TreeNode node in nodes)
			{
				if (node == targetNode)
				{
					return currentDepth;
				}
				else
				{
					int foundDepth = GetNodeDepth(targetNode, node.AllChildNodes, currentDepth + 1);
					if (foundDepth != -1)
					{
						return foundDepth;
					}
				}
			}
			return -1;
		}

		public int CountAllNodes(List<TreeNode> nodes)
		{
			int retVal = 0;
			foreach (TreeNode node in nodes)
			{
				++retVal;
				retVal += CountAllNodes(node.childNodes);
			}
			return retVal;
		}

		public TreeNode FindNodeByName(string labelText, List<TreeNode> nodes)
		{
			string nodeText = labelText;
			int notesStartIndex = labelText.IndexOf('(');
			if (notesStartIndex > 0)
			{
				nodeText = nodeText.Substring(0, notesStartIndex);
			}
			nodeText = nodeText.Trim();
			foreach (TreeNode node in nodes)
			{
				if (String.Equals(nodeText, node.name, StringComparison.OrdinalIgnoreCase)
					|| String.Equals(nodeText.Replace(' ', '_'), node.name, StringComparison.OrdinalIgnoreCase))
				{
					return node;
				}
				else
				{
					TreeNode foundNode = FindNodeByName(labelText, node.AllChildNodes);
					if (foundNode != null)
					{
						return foundNode;
					}
				}
			}
			return null;
		}

		#region Notes
		void OpenAddNoteDialog(object sender, EventArgs e)
		{
			if (!(sender is ToolStripItem menuItem))
				return;
			if (!(menuItem.Owner is ContextMenuStrip owner))
				return;
			Label nodeLabel = (Label)owner.SourceControl;
			if (nodeLabel == null)
				return;

			TreeNode node = FindNodeByName(nodeLabel.Text, nodes);
			if (node == null)
			{
				return;
			}
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Add note"
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
				string newNote = inputBox.Text;
				if (!node.notes.Contains(newNote))
				{
					node.notes.Add(newNote);
				}

				int nodeDepth = GetNodeDepth(node, AllNodes, 0);
				UpdateNodeLabel(nodeLabel, node, nodeDepth);
			}
		}

		void OpenRemoveNoteDialog(object sender, EventArgs e)
		{
			if (!(sender is ToolStripItem menuItem))
				return;
			if (!(menuItem.Owner is ContextMenuStrip owner))
				return;
			Label nodeLabel = (Label)owner.SourceControl;
			if (nodeLabel == null)
				return;

			TreeNode node = FindNodeByName(nodeLabel.Text, nodes);
			if (node == null)
			{
				return;
			}
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Remove note"
			};

			bool confirmed = false;

			ComboBox inputBox = new ComboBox() { Left = 5, Top = 5, Width = 300 };
			foreach (string note in node.notes)
			{
				inputBox.Items.Add(note);
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
				string noteToRemove = inputBox.SelectedItem as string;
				if (node.notes.Contains(noteToRemove))
				{
					node.notes.Remove(noteToRemove);
				}

				int nodeDepth = GetNodeDepth(node, AllNodes, 0);
				UpdateNodeLabel(nodeLabel, node, nodeDepth);
			}
		}

		void UpdateNodeLabel(Label label, TreeNode node, int nodeDepth)
		{
			string depthPrefix = string.Join("", Enumerable.Repeat("   ", nodeDepth).ToArray());
			if (node.AllNotes.Count > 0)
			{
				label.Text = depthPrefix + (node.name + " (" + string.Join(", ", node.AllNotes) + ")").Replace('_', ' ');
			}
			else
			{
				label.Text = depthPrefix + node.name.Replace('_', ' ');
			}

			//only allow modifications on non-mod-inserted nodes
			if (GetNodeDepth(node, nodes, 0) != -1)
			{
				ContextMenuStrip contextMenu = new ContextMenuStrip();
				ToolStripItem addNoteItem = contextMenu.Items.Add("Add note");
				addNoteItem.Click += new EventHandler(OpenAddNoteDialog);
				if (node.notes.Count > 0)
				{
					ToolStripItem removeNoteItem = contextMenu.Items.Add("Remove note");
					removeNoteItem.Click += new EventHandler(OpenRemoveNoteDialog);
				}
				ToolStripItem addNodeItem = contextMenu.Items.Add("Add " + GetSingularName(GetNodeDepth(node, nodes, 0) + 1));
				addNodeItem.Click += new EventHandler(OpenAddNodeDialog);
				ToolStripItem removeNodeItem = contextMenu.Items.Add("Remove " + node.name);
				removeNodeItem.Click += new EventHandler(OpenRemoveNodeDialog);

				label.ContextMenuStrip = contextMenu;
			}
			OnComponentChanged();
		}
		#endregion

		#region Add/Remove Nodes
		void OpenAddNodeDialog(object sender, EventArgs e)
		{
			if (!(sender is ToolStripItem menuItem))
				return;
			if (!(menuItem.Owner is ContextMenuStrip owner))
				return;
			TreeNode node = null;
			if (owner.SourceControl is Label nodeLabel)
			{
				node = FindNodeByName(nodeLabel.Text, nodes);
			}

			string singularName;
			if (node == null)
			{
				singularName = GetSingularName(0);
			}
			else
			{
				singularName = GetSingularName(GetNodeDepth(node, AllNodes, 0) + 1);
			}

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
				if (node != null)
				{
					node.childNodes.Add(new TreeNode(newTrait));
				}
				else
				{
					nodes.Add(new TreeNode(newTrait));
				}

				OnTreeChanged();
			}
		}

		void OpenRemoveNodeDialog(object sender, EventArgs e)
		{
			if (!(sender is ToolStripItem menuItem))
				return;
			if (!(menuItem.Owner is ContextMenuStrip owner))
				return;
			Label nodeLabel = (Label)owner.SourceControl;
			if (nodeLabel == null)
				return;

			TreeNode node = FindNodeByName(nodeLabel.Text, nodes);
			if (node == null)
			{
				return;
			}

			string singularName = GetSingularName(node);
			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 325,
				Height = 100,
				Text = "Remove " + singularName
			};

			bool confirmed = false;

			Label question = new Label() { Left = 5, Top = 5, Width = 300 };
			question.Text = "Are you sure you want to remove the \"" + node.name + "\" " + singularName + "?";
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
				nodes.Remove(node);

				OnTreeChanged();
			}
		}

		void OnTreeChanged()
		{
			int rowAmount = CountAllNodes(AllNodes);
			uiElement.RowCount = rowAmount;
			uiElement.RowStyles.Clear();
			for (int i = 0; i < rowAmount; ++i)
			{
				uiElement.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowAmount));
			}
			uiElement.Size = new Size(componentWidth, 17 * rowAmount);
			ResizeParentColumn();
			uiElement.Controls.Clear();

			int rowIndexCounter = 0;
			foreach (TreeNode node in AllNodes)
			{
				AddNodeControl(node, 0, ref rowIndexCounter);
			}

			OnComponentChanged();
		}

		private void AddNodeControl(TreeNode node, int depth, ref int rowIndexCounter)
		{
			Label traitNameLabel = new Label
			{
				Anchor = AnchorStyles.Left,
				AutoSize = true,
				Name = "TraitNameLabel" + node.name,
				Size = new Size(componentWidth, 17)
			};

			UpdateNodeLabel(traitNameLabel, node, depth);
			uiElement.Controls.Add(traitNameLabel, 0, rowIndexCounter);
			++rowIndexCounter;
			foreach (TreeNode childNode in node.AllChildNodes)
			{
				AddNodeControl(childNode, depth + 1, ref rowIndexCounter);
			}
		}
		#endregion

		override public void ApplyModification(Modification mod, Sheet sheet)
		{
			if (mod.path.Count == 1)
			{
				if (mod is StringModification stringMod)
				{
					if (stringMod.modType == StringModificationType.AddElement)
					{
						string newElementName = stringMod.GetValue(sheet);
						modInsertedNodes.Add(new TreeNode(newElementName));
						isCurrentlyModified = true;
					}
				}
			}
			else if (mod.path.Count > 1)
			{
				foreach (TreeNode node in AllNodes)
				{
					if (String.Equals(mod.path[1], node.name, StringComparison.OrdinalIgnoreCase))
					{
						node.ApplyModification(mod, sheet, 2, ref isCurrentlyModified);
					}
				}
			}
		}

		override public void ResetModifications()
		{
			base.ResetModifications();
			modInsertedNodes.Clear();
			foreach (TreeNode node in nodes)
			{
				node.ResetModifications();
			}
		}

		override public void OnModificationsComplete()
		{
			if (isCurrentlyModified || wasPreviouslyModified)
			{
				OnTreeChanged();
			}
		}
	}
}
