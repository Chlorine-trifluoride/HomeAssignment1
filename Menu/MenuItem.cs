using System;
using System.Collections.Generic;
using System.Text;

namespace HomeworkApp
{
    class MenuItem
    {
        public string Text { get; set; } = "Undefined";
        public Action Callback { get; set; }

        public MenuItem(string text, Action callback)
        {
            this.Text = text;
            this.Callback = callback;
        }
    }
}
