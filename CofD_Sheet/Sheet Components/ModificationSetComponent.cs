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
        [XmlInclude(typeof(IntModification))]
        [XmlInclude(typeof(StringModification))]
        public class Modification
        {
            public Modification()
            { }
            public Modification(string _path)
            {
                this.path = _path;
            }

            [XmlAttribute]
            public string path = "InvalidPath";
        }

        public enum IntModificationType
        {
            [XmlEnum(Name = "Absolute")]
            Absolute,
            [XmlEnum(Name = "Delta")]
            Delta
        }

        public class IntModification : Modification
        {
            public IntModification()
            { }
            public IntModification(string _path, int _value, IntModificationType _modType) : base(_path)
            {
                this.value = _value;
                this.modType = _modType;
            }

            [XmlAttribute]
            public IntModificationType modType = IntModificationType.Absolute;

            [XmlAttribute]
            public int value = 0;
        }

        public class StringModification : Modification
        {
            public StringModification() : base()
            { }
            public StringModification(string _path, string _value) : base(_path)
            {
                this.value = _value;
            }

            [XmlAttribute]
            public string value = "";
        }

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
        ComboBox comboBox = new ComboBox() { Left = 5, Top = 5, Width = 300 };

        public ModificationSetComponent() : base("ModificationSetComponent", ColumnId.Undefined)
        { }

        public ModificationSetComponent(string componentName, List<string> ModificationSetNames, ColumnId _column) : base(componentName, _column)
        {
            foreach (string advantageName in ModificationSetNames)
            {
                sets.Add(new ModificationSet(advantageName));
            }
        }

        override public Control GetUIElement()
        {
            uiElement.RowCount = sets.Count;
            uiElement.ColumnCount = 1;
            uiElement.Dock = DockStyle.Fill;
            uiElement.Size = new Size(componentWidth, 30 * sets.Count);
            uiElement.TabIndex = 0;
            comboBox.Items.Clear();

            uiElement.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            foreach (ModificationSet set in sets)
            {
                comboBox.Items.Add(set.name);
            }
            uiElement.Controls.Add(comboBox, 0, 0);

            OnValueChanged();
            return uiElement;
        }

        void OnValueChanged(object sender = null, EventArgs e = null)
        {
            comboBox.SelectedIndex = ActiveIndex;

            OnComponentChanged();
        }
    }
}
