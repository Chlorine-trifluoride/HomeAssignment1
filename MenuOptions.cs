using System;
using System.Collections.Generic;
using System.Text;

namespace HomeworkApp
{
    public enum MAIN_MENUOPTION
    {
        QUESTIONS = 0,
        SLOTS = 1
    }

    class MenuOptions
    {
        public string Title { get; set; }
        public string[] OptionsTexts { get; set; }
        public int Selection { get; set; } = 0;
    }
}
