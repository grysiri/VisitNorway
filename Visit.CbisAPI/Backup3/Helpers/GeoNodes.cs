using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visit.CbisAPI.Products;

namespace Visit.CbisAPI.Helpers
{
	public static class GeoNodes
	{
		private static List<TreeNodeOfGeoNode> GetGeoNodes(TreeNodeOfGeoNode node)
		{
			List<TreeNodeOfGeoNode> categories = new List<TreeNodeOfGeoNode>() { node };

			foreach (var subNode in node.Children)
			{
				categories.AddRange(GetGeoNodes(subNode));
			}

			return categories;
		}
	
		public static List<GeoNode> ToGeoNodeList(this TreeOfGeoNode tree)
		{
			List<GeoNode> result = new List<GeoNode>();

			foreach (TreeNodeOfGeoNode node in tree.Nodes)
			{
				result.AddRange(GetGeoNodes(node).Select(n => n.Data).ToList());
			}

			return result;
		}

		private static string GetDropDownOptions(this TreeNodeOfGeoNode node, int selectedValue, int level)
		{
			string pre = "";
			for (int i = 0; i < level; ++i)
				pre += "&nbsp;&nbsp;";
			if (level > 0)
				pre += "- ";

			string result = string.Format("<option value=\"{0}\"{1}>" + pre + "{2}</option>", node.Data.Id, selectedValue == node.Data.Id ? " selected=\"selected\"" : "", node.Data.Name);

			foreach (TreeNodeOfGeoNode subNode in node.Children)
			{
				result += GetDropDownOptions(subNode, selectedValue, level + 1);
			}

			return result;
		}

		public static string GetDropDownOptions(this TreeOfGeoNode tree, int selectedValue)
		{
			string result = "";
			foreach (TreeNodeOfGeoNode node in tree.Nodes)
			{
				result += GetDropDownOptions(node, selectedValue, 0);
			}

			return result;
		}

	}
}
