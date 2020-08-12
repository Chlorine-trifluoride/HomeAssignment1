using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HomeworkApp
{
    class QuestionLoader
    {
        private static readonly object instanceLock = new object();

        private static QuestionLoader instance = null;
        public static QuestionLoader Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = LoadQuestions();

                    return instance;
                }
            }
        }

        [JsonPropertyName("questions")]
        public Question[] Questions { get; set; }

        private const string path = "media/questions.json";

        public Question CurrentQuestion { get; set; }

        private QuestionLoader() { }

        private static QuestionLoader LoadQuestions()
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return JsonSerializer.Deserialize<QuestionLoader>(reader.ReadToEnd());
            }
        }
    }
}
