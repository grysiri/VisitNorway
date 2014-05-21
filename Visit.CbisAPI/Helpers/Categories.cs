using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visit.CbisAPI.Categories;

namespace Visit.CbisAPI.Helpers
{
	public static class Categories
	{
		private static List<TreeNodeOfCategory> GetCategories(TreeNodeOfCategory node)
		{
			List<TreeNodeOfCategory> categories = new List<TreeNodeOfCategory>() { node };

			foreach (var subNode in node.Children)
			{
				categories.AddRange(GetCategories(subNode));
			}

			return categories;
		}

		public static List<Category> ToCategoryList(this TreeNodeOfCategory node)
		{
			return GetCategories(node).Select(n => n.Data).ToList();
		}
		
		public static List<Category> ToCategoryList(this TreeOfCategory tree)
		{
			List<Category> result = new List<Category>();

			foreach (TreeNodeOfCategory node in tree.Nodes)
			{
				result.AddRange(GetCategories(node).Select(n => n.Data).ToList());
			}

			return result;
		}

		public static List<TreeNodeOfCategory> ToNodeList(this TreeOfCategory tree)
		{
			List<TreeNodeOfCategory> result = new List<TreeNodeOfCategory>();

			foreach (TreeNodeOfCategory node in tree.Nodes)
			{
				result.AddRange(GetCategories(node));
			}

			return result;
		}

		private static TreeNodeOfCategory FindNode(TreeNodeOfCategory nodeToStartAt, int categoryIdtoFind)
		{
			if (nodeToStartAt.Data.Id == categoryIdtoFind)
				return nodeToStartAt;

			foreach (TreeNodeOfCategory subNode in nodeToStartAt.Children)
			{
				TreeNodeOfCategory found = FindNode(subNode, categoryIdtoFind);
				if (found != null)
					return found;
			}

			return null;
		}

		public static Category GetRootCategory(this TreeOfCategory tree, int categoryId)
		{
			foreach (TreeNodeOfCategory node in tree.Nodes)
			{
				if (FindNode(node, categoryId) != null)
					return node.Data;
			}

			return null;
		}

		private static string GetDropDownOptions(this TreeNodeOfCategory node, int selectedValue, int level)
		{
			string pre = "";
			for (int i = 0; i < level; ++i)
				pre += "&nbsp;&nbsp;";
			if(level > 0)
				pre += "- ";

			string result = string.Format("<option value=\"{0}\"{1}>" + pre + "{2}</option>", node.Data.Id, selectedValue == node.Data.Id ? " selected=\"selected\"" : "", level == 0 ? node.Data.Name.ToUpper() : node.Data.Name);

			foreach (TreeNodeOfCategory subNode in node.Children)
			{
				result += GetDropDownOptions(subNode, selectedValue, level + 1);
			}

			return result;
		}

		public static string GetDropDownOptions(this TreeOfCategory tree, int selectedValue)
		{
			string result = "";
			foreach (TreeNodeOfCategory node in tree.Nodes)
			{
				result += GetDropDownOptions(node, selectedValue, 0);
			}

			return result;
		}

		public static string GetMapPin(this Category cat)
		{
			switch (cat.Id)
			{
				case 3107:
					return "events";
				case 3127:
					return "accommodation";
				case 3071:
				default:
					return "activity";
			}
		}
	}
}
