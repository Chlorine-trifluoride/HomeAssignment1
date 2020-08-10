using System;
using System.Collections.Generic;
using System.Text;

namespace HomeworkApp
{
    abstract class ApplicationBase
    {
        protected Renderer renderer;
        protected InputManager inputMgr;

        public ApplicationBase()
        {
            renderer = Renderer.Instance;
            inputMgr = new InputManager();
        }

        public void Run()
        {
            Age age;
            do
            {
                age = AgeVerification();
                PrintAgeReply(age);
            } while (!age.IsValid);

            for (; ; )
            {
                PresentRandomQuestion();
            }
        }

        protected abstract Age AgeVerification();
        protected abstract void PrintAgeReply(Age age);
        protected abstract void DisplayMessageToUser(string message, int milliseconds);
        protected abstract void PresentRandomQuestion();
    }
}
