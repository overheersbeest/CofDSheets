using System.Drawing;

namespace CofD_Sheet
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.BehaviourStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.HeaderTable = new System.Windows.Forms.TableLayoutPanel();
			this.ChronicleTextBox = new System.Windows.Forms.TextBox();
			this.PlayerTextBox = new System.Windows.Forms.TextBox();
			this.NameLabel = new System.Windows.Forms.Label();
			this.PlayerLabel = new System.Windows.Forms.Label();
			this.ChronicleLabel = new System.Windows.Forms.Label();
			this.NameTextBox = new System.Windows.Forms.TextBox();
			this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
			this.LeftComponentTable = new System.Windows.Forms.TableLayoutPanel();
			this.SubSplitContainer = new System.Windows.Forms.SplitContainer();
			this.MiddleComponentTable = new System.Windows.Forms.TableLayoutPanel();
			this.RightComponentTable = new System.Windows.Forms.TableLayoutPanel();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.HeaderTable.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
			this.MainSplitContainer.Panel1.SuspendLayout();
			this.MainSplitContainer.Panel2.SuspendLayout();
			this.MainSplitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SubSplitContainer)).BeginInit();
			this.SubSplitContainer.Panel1.SuspendLayout();
			this.SubSplitContainer.Panel2.SuspendLayout();
			this.SubSplitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.BehaviourStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(955, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
			this.newToolStripMenuItem.Text = "New";
			// 
			// loadToolStripMenuItem
			// 
			this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
			this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
			this.loadToolStripMenuItem.Text = "Load";
			this.loadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
			// 
			// BehaviourStripMenuItem
			// 
			this.BehaviourStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoSaveToolStripMenuItem,
            this.autoLoadToolStripMenuItem});
			this.BehaviourStripMenuItem.Name = "BehaviourStripMenuItem";
			this.BehaviourStripMenuItem.Size = new System.Drawing.Size(72, 20);
			this.BehaviourStripMenuItem.Text = "Behaviour";
			// 
			// autoSaveToolStripMenuItem
			// 
			this.autoSaveToolStripMenuItem.Checked = true;
			this.autoSaveToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.autoSaveToolStripMenuItem.Name = "autoSaveToolStripMenuItem";
			this.autoSaveToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
			this.autoSaveToolStripMenuItem.Text = "AutoSave";
			this.autoSaveToolStripMenuItem.Click += new System.EventHandler(this.AutoSaveToolStripMenuItem_Click);
			// 
			// autoLoadToolStripMenuItem
			// 
			this.autoLoadToolStripMenuItem.Name = "autoLoadToolStripMenuItem";
			this.autoLoadToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
			this.autoLoadToolStripMenuItem.Text = "AutoLoad";
			this.autoLoadToolStripMenuItem.Click += new System.EventHandler(this.AutoLoadToolStripMenuItem_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.HeaderTable);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.MainSplitContainer);
			this.splitContainer1.Size = new System.Drawing.Size(955, 806);
			this.splitContainer1.SplitterDistance = 25;
			this.splitContainer1.TabIndex = 3;
			// 
			// HeaderTable
			// 
			this.HeaderTable.AutoSize = true;
			this.HeaderTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.HeaderTable.ColumnCount = 6;
			this.HeaderTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.HeaderTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.HeaderTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
			this.HeaderTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.HeaderTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
			this.HeaderTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.HeaderTable.Controls.Add(this.ChronicleTextBox, 5, 0);
			this.HeaderTable.Controls.Add(this.PlayerTextBox, 3, 0);
			this.HeaderTable.Controls.Add(this.NameLabel, 0, 0);
			this.HeaderTable.Controls.Add(this.PlayerLabel, 2, 0);
			this.HeaderTable.Controls.Add(this.ChronicleLabel, 4, 0);
			this.HeaderTable.Controls.Add(this.NameTextBox, 1, 0);
			this.HeaderTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.HeaderTable.Location = new System.Drawing.Point(0, 0);
			this.HeaderTable.Name = "HeaderTable";
			this.HeaderTable.RowCount = 1;
			this.HeaderTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.HeaderTable.Size = new System.Drawing.Size(953, 23);
			this.HeaderTable.TabIndex = 0;
			// 
			// ChronicleTextBox
			// 
			this.ChronicleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChronicleTextBox.Location = new System.Drawing.Point(691, 3);
			this.ChronicleTextBox.Name = "ChronicleTextBox";
			this.ChronicleTextBox.Size = new System.Drawing.Size(259, 20);
			this.ChronicleTextBox.TabIndex = 5;
			this.ChronicleTextBox.TextChanged += new System.EventHandler(this.ChronicleChanged);
			// 
			// PlayerTextBox
			// 
			this.PlayerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PlayerTextBox.Location = new System.Drawing.Point(363, 3);
			this.PlayerTextBox.Name = "PlayerTextBox";
			this.PlayerTextBox.Size = new System.Drawing.Size(257, 20);
			this.PlayerTextBox.TabIndex = 4;
			this.PlayerTextBox.TextChanged += new System.EventHandler(this.PlayerChanged);
			// 
			// NameLabel
			// 
			this.NameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.NameLabel.AutoSize = true;
			this.NameLabel.Location = new System.Drawing.Point(9, 5);
			this.NameLabel.Name = "NameLabel";
			this.NameLabel.Size = new System.Drawing.Size(38, 13);
			this.NameLabel.TabIndex = 0;
			this.NameLabel.Text = "Name:";
			// 
			// PlayerLabel
			// 
			this.PlayerLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.PlayerLabel.AutoSize = true;
			this.PlayerLabel.Location = new System.Drawing.Point(318, 5);
			this.PlayerLabel.Name = "PlayerLabel";
			this.PlayerLabel.Size = new System.Drawing.Size(39, 13);
			this.PlayerLabel.TabIndex = 1;
			this.PlayerLabel.Text = "Player:";
			// 
			// ChronicleLabel
			// 
			this.ChronicleLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.ChronicleLabel.AutoSize = true;
			this.ChronicleLabel.Location = new System.Drawing.Point(631, 5);
			this.ChronicleLabel.Name = "ChronicleLabel";
			this.ChronicleLabel.Size = new System.Drawing.Size(54, 13);
			this.ChronicleLabel.TabIndex = 2;
			this.ChronicleLabel.Text = "Chronicle:";
			// 
			// NameTextBox
			// 
			this.NameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NameTextBox.Location = new System.Drawing.Point(53, 3);
			this.NameTextBox.Name = "NameTextBox";
			this.NameTextBox.Size = new System.Drawing.Size(257, 20);
			this.NameTextBox.TabIndex = 3;
			this.NameTextBox.TextChanged += new System.EventHandler(this.NameChanged);
			// 
			// MainSplitContainer
			// 
			this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainSplitContainer.IsSplitterFixed = true;
			this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
			this.MainSplitContainer.Margin = new System.Windows.Forms.Padding(0);
			this.MainSplitContainer.Name = "MainSplitContainer";
			// 
			// MainSplitContainer.Panel1
			// 
			this.MainSplitContainer.Panel1.Controls.Add(this.LeftComponentTable);
			this.MainSplitContainer.Panel1MinSize = 315;
			// 
			// MainSplitContainer.Panel2
			// 
			this.MainSplitContainer.Panel2.Controls.Add(this.SubSplitContainer);
			this.MainSplitContainer.Panel2MinSize = 634;
			this.MainSplitContainer.Size = new System.Drawing.Size(953, 775);
			this.MainSplitContainer.SplitterDistance = 315;
			this.MainSplitContainer.TabIndex = 0;
			// 
			// LeftComponentTable
			// 
			this.LeftComponentTable.ColumnCount = 1;
			this.LeftComponentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.LeftComponentTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LeftComponentTable.Location = new System.Drawing.Point(0, 0);
			this.LeftComponentTable.Margin = new System.Windows.Forms.Padding(0);
			this.LeftComponentTable.Name = "LeftComponentTable";
			this.LeftComponentTable.RowCount = 1;
			this.LeftComponentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 775F));
			this.LeftComponentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 775F));
			this.LeftComponentTable.Size = new System.Drawing.Size(315, 775);
			this.LeftComponentTable.TabIndex = 0;
			// 
			// SubSplitContainer
			// 
			this.SubSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SubSplitContainer.IsSplitterFixed = true;
			this.SubSplitContainer.Location = new System.Drawing.Point(0, 0);
			this.SubSplitContainer.Margin = new System.Windows.Forms.Padding(0);
			this.SubSplitContainer.Name = "SubSplitContainer";
			// 
			// SubSplitContainer.Panel1
			// 
			this.SubSplitContainer.Panel1.Controls.Add(this.MiddleComponentTable);
			this.SubSplitContainer.Panel1MinSize = 315;
			// 
			// SubSplitContainer.Panel2
			// 
			this.SubSplitContainer.Panel2.Controls.Add(this.RightComponentTable);
			this.SubSplitContainer.Panel2MinSize = 315;
			this.SubSplitContainer.Size = new System.Drawing.Size(634, 775);
			this.SubSplitContainer.SplitterDistance = 315;
			this.SubSplitContainer.TabIndex = 1;
			// 
			// MiddleComponentTable
			// 
			this.MiddleComponentTable.ColumnCount = 1;
			this.MiddleComponentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.MiddleComponentTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MiddleComponentTable.Location = new System.Drawing.Point(0, 0);
			this.MiddleComponentTable.Margin = new System.Windows.Forms.Padding(0);
			this.MiddleComponentTable.Name = "MiddleComponentTable";
			this.MiddleComponentTable.RowCount = 1;
			this.MiddleComponentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 775F));
			this.MiddleComponentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 775F));
			this.MiddleComponentTable.Size = new System.Drawing.Size(315, 775);
			this.MiddleComponentTable.TabIndex = 1;
			// 
			// RightComponentTable
			// 
			this.RightComponentTable.ColumnCount = 1;
			this.RightComponentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.RightComponentTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RightComponentTable.Location = new System.Drawing.Point(0, 0);
			this.RightComponentTable.Margin = new System.Windows.Forms.Padding(0);
			this.RightComponentTable.Name = "RightComponentTable";
			this.RightComponentTable.RowCount = 1;
			this.RightComponentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 775F));
			this.RightComponentTable.Size = new System.Drawing.Size(315, 775);
			this.RightComponentTable.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(955, 830);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MaximumSize = new System.Drawing.Size(971, 1440);
			this.MinimumSize = new System.Drawing.Size(971, 39);
			this.Name = "Form1";
			this.Text = "CofD Sheet";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.HeaderTable.ResumeLayout(false);
			this.HeaderTable.PerformLayout();
			this.MainSplitContainer.Panel1.ResumeLayout(false);
			this.MainSplitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
			this.MainSplitContainer.ResumeLayout(false);
			this.SubSplitContainer.Panel1.ResumeLayout(false);
			this.SubSplitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.SubSplitContainer)).EndInit();
			this.SubSplitContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TableLayoutPanel HeaderTable;
		private System.Windows.Forms.Label NameLabel;
		private System.Windows.Forms.TextBox ChronicleTextBox;
		private System.Windows.Forms.TextBox PlayerTextBox;
		private System.Windows.Forms.Label PlayerLabel;
		private System.Windows.Forms.Label ChronicleLabel;
		private System.Windows.Forms.TextBox NameTextBox;
		private System.Windows.Forms.SplitContainer MainSplitContainer;
		private System.Windows.Forms.TableLayoutPanel LeftComponentTable;
		private System.Windows.Forms.TableLayoutPanel RightComponentTable;
		private System.Windows.Forms.ToolStripMenuItem BehaviourStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem autoSaveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem autoLoadToolStripMenuItem;
		private System.Windows.Forms.SplitContainer SubSplitContainer;
		private System.Windows.Forms.TableLayoutPanel MiddleComponentTable;
	}
}

