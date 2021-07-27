using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CofD_Sheet.Helpers
{
	class StaticErrorLogger
	{
		private static List<string> errorLog = new List<string>();

		private static List<string> ignoreList = new List<string>();
		public static void AddQueryError(List<string> path)
		{
			string errorMsg = "A query could not resolve: \"" + path.Aggregate((i, j) => i + "." + j) + "\", please fix.";

			AddGenericError(errorMsg);
		}

		public static void AddGenericError(string errorMsg)
		{
			errorLog.Add(errorMsg);

			if (ignoreList.Contains(errorMsg))
			{
				return;
			}

			Form prompt = new Form
			{
				StartPosition = FormStartPosition.CenterParent,
				Width = 350,
				Height = 100,
				MinimumSize = new Size(350, 100),
				Text = "Query Error"
			};

			bool ignoreQueryInFuture = false;

			Label question = new Label() { Left = 5, Top = 5, AutoSize = true, MaximumSize = new Size(300, 0) };
			question.Text = errorMsg;
			Button confirmation = new Button() { Text = "Ok", Left = 225, Width = 100, Top = 30, Anchor = (AnchorStyles.Bottom | AnchorStyles.Right) };
			confirmation.Click += (sender2, e2) => { prompt.Close(); };
			Button ignore = new Button() { Text = "OK, but ignore this error in the future", Left = 20, Width = 200, Top = 30, Anchor = (AnchorStyles.Bottom | AnchorStyles.Right) };
			ignore.Click += (sender2, e2) => { ignoreQueryInFuture = true; prompt.Close(); };
			prompt.Controls.Add(question);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(ignore);
			prompt.ShowDialog();

			if (ignoreQueryInFuture)
			{
				ignoreList.Add(errorMsg);
			}
		}
	}
}
