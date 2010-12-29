using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Linq;

namespace PublicQuestions.Model.Questions
{
    public class QuestionRepository
    {
        private IDocumentSession _session;

        public QuestionRepository(IDocumentSession session)
        {
            _session = session;
        }

        public Question Load(string id)
        {
            return _session.Load<Question>(id);
        }

        public void Save(Question question)
        {
            _session.Store(question);    
            _session.SaveChanges();
        }


        public void Delete(string id)
        {
            var category = Load(id);
            _session.Delete<Question>(category);
            _session.SaveChanges();
        }


        //Get all questions
        public IRavenQueryable<Question> GetQuestions()
        {
            var result = _session.Query<Question>();
            return result;
        }

        //Get one question via Id
        public Question GetQuestion(int id)
        {
            return _session.Load<Question>(id.ToString());
        }

    }
}
