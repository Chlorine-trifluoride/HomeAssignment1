using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace HomeworkApp
{
    class MenuBuilder
    {
        private string title;
        private int selection = 0;
        private List<MenuItem> tempItemList = new List<MenuItem>();

        public void AddTitle(string title)
        {
            this.title = title;
        }

        public void SetSelection(int selection)
        {
            this.selection = selection;
        }

        public void AddItem(MenuItem item)
        {
            tempItemList.Add(item);
        }

        // Shorthand for new MenuItem(...)
        public void AddItem(string text, Action callback)
        {
            AddItem(new MenuItem(text, callback));
        }

        public MenuOptions Build()
        {
            return new MenuOptions(title, tempItemList.ToArray(), selection);
        }
    }
}
