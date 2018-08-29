using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Quiz
{
	class Program
	{
		static void Main(string[] args)
		{
			var menu = new Menu();
			var menuItems = File.ReadAllLines("menu-items.txt");
			var ingredientItems = File.ReadAllLines("ingredient-items.txt");

			foreach (var item in menuItems) {
				menu.Add(item);
			}
			foreach (var item in ingredientItems) {
				var ingredientItem = GetIngredientMenuItem(item);
				if (menu.Exists(m => m.Item == ingredientItem.Item2)) {
					var menuItem = menu.Find(m => m.Item == ingredientItem.Item2);
					if (menuItem.Ingredients == null)
						menuItem.Ingredients = new List<string>();
					menuItem.Ingredients.Add(ingredientItem.Item1);
				}
			}
			Console.Write(menu.Display());
			Console.ReadLine();
		}
		private static Tuple<string, string> GetIngredientMenuItem(string imi)
		{
			var list = imi.Split("|");
			if (list.Length == 2)
				return new Tuple<string, string>(list[0], list[1]);
			throw new Exception("Invalid Ingredient|Menu");
		}

	}

	public class Menu : List<MenuItem>
	{
		public Menu() { }

		public void Add(string item)
		{
			var menuItem = new MenuItem();
			if (item.EndsWith("*")) {
				item = item.Replace("*", "");
				menuItem.Selected = true;
			}
			menuItem.Item = item;
			Add(menuItem);
		}

		public string Display()
		{
			var display = new StringBuilder("Selected Menu:\n");
			Sort((x, y) => x.Item.CompareTo(y.Item));
			foreach (var item in this) {
				if (item.Selected)
					display.AppendLine(item.Display());
			}
			return display.ToString();
		}
	}

	public class MenuItem
	{
		public string Item { get; set; }
		public bool Selected { get; set; } = false;
		public List<string> Ingredients { get; set; }

		public string Display()
		{
			var display = new StringBuilder();
			display.Append("\t");
			display.Append(Item);
			//display.Append($"\t{(Selected ? "*" : "")}");
			if (Ingredients != null) {
				display.Append("\n\tProvided Ingredients:\n");
				Ingredients.Sort((x, y) => x.CompareTo(y));
				foreach (var i in Ingredients) {
					display.Append("\t\t");
					display.AppendLine(i);
				}
			}
			return display.ToString();
		}
	}
}