using System;
using System.Collections.Generic;
using System.Text;

namespace HomeworkApp
{
    class MenuOptions
    {
        public string Title { get; private set; }
        public MenuItem[] MenuItems { get; private set; }
        public int Selection { get; private set; } = 0;

        public MenuOptions(string title, MenuItem[] menuItems, int selection = 0)
        {
            this.Title = title;
            this.MenuItems = menuItems;
            this.Selection = selection;
        }
    }
}
