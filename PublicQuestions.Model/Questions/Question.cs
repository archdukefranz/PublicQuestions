using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PublicQuestions.Model.Questions
{
    public class Question
    {
        public Question()
        {
            Attributes = new Attributes();
            MetaData = new List<MetaData>();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
	    public string Title {get; set;}
	    public string Body {get; set;}
	    public DateTime Posted {get; set;}

        public IList<MetaData> MetaData { get; set; }
        public Attributes Attributes { get; set; }

        /// <summary>
        /// View this questions and increase the counter.
        /// </summary>
        public void View()
        {
            Attributes.Views = Attributes.Views+1;
        }
    }
}
