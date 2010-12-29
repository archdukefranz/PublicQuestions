using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PublicQuestions.Model.Questions
{
    public class Question
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
	    public string Title {get; set;}
	    public string Body {get; set;}
	    public DateTime Posted {get; set;}

        IList<MetaData> MetaData { get; set; }
        Attributes Attributes { get; set; }
    }
}
