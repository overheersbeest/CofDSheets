using GalaSoft.MvvmLight;
using System.Xml;
using System.Xml.Serialization;

namespace CofD_Sheet_WPF.Models
{
	[XmlInclude(typeof(AspirationsComponent))]
	[XmlInclude(typeof(AttributesComponent))]
	[XmlInclude(typeof(ExperienceComponent))]
	[XmlInclude(typeof(HealthComponent))]
	[XmlInclude(typeof(MeritComponent))]
	[XmlInclude(typeof(SimpleComponent))]
	[XmlInclude(typeof(SkillsComponent))]
	[XmlInclude(typeof(StatComponent))]
	public abstract class ISheetComponent : ViewModelBase
	{
		[XmlIgnore]
		public const int componentWidth = 315;
		
		[XmlAttribute]
		public string name { get; set; } = "ComponentName";

		[XmlAttribute]
		public int columnIndex;

		public ISheetComponent(string componentName, int componentColumnIndex)
		{
			name = componentName;
			columnIndex = componentColumnIndex;
		}
	}
}
