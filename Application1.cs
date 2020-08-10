using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace HomeworkApp
{
    class Application1 : ApplicationBase
    {

        protected override Age AgeVerification()
        {
            renderer.Rendermode = RENDERMODE.AGE_VERIFICATION;
            int birthYear = inputMgr.GetYear(renderer);
            int currentYear = DateTime.Now.Year;
            Age age = new Age { Years = currentYear - birthYear };

            return age;
        }

        protected override void PrintAgeReply(Age age)
        {
            // Pattern matching madness. 
            // Should be fixed in C# 9, sorry =(
            string message = age switch
            {
                { } i when i.Years < 0 => "You can not be negative years old",
                { } i when i.Years == 0 => "You are zero years old",
                { } i when i.Years < 18 => "You are too young for this application",
                { } i when i.Years > 120 => "You are suspiciously old",
                { } i when i.Years >= 18 => i.SetValid(),
                _ => throw new NotImplementedException(),
            };

            DisplayMessageToUser(message, 1000);
        }

        protected override void DisplayMessageToUser(string message, int milliseconds)
        {
            renderer.Rendermode = RENDERMODE.MESSAGE;
            renderer.MessageToRender = message;
            renderer.Render();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                renderer.CheckForResizeAndReRender();
                Thread.Sleep(16*2);
            } while (sw.ElapsedMilliseconds < milliseconds);

            sw.Stop();
        }

        protected override void PresentRandomQuestion()
        {
            Random random = new Random();
            int questionId = random.Next(0, QuestionLoader.Instance.Questions.Length);

            QuestionLoader.Instance.CurrentQuestion = QuestionLoader.Instance.Questions[questionId];
            renderer.Rendermode = RENDERMODE.QUESTION;
            renderer.SetSelectionIdAndRender(0); // reset selection

            int answerId = inputMgr.GetQuestionAnswer();

            if (answerId == QuestionLoader.Instance.CurrentQuestion.Answer)
                DisplayMessageToUser("Correct Answer", 1000);
            else
                DisplayMessageToUser("Wrong Answer", 1000);
        }
    }
}
