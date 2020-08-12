using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HomeworkApp
{
    class Question
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("options")]
        public string[] Options { get; set; }
        [JsonPropertyName("answer")]
        public int Answer { get; set; }
    }
}
