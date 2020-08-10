using System.Diagnostics;

namespace HomeworkApp
{
    class Age
    {
        public int Years { get; set; }
        public bool IsValid { get; set; }

        // Lazy hack
        public string SetValid()
        {
            IsValid = true;
            return "Welcome to the secret club";
        }
    }
}
