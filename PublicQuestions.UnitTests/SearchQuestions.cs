using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PublicQuestions.Model.Questions;
using Raven.Client;
using Raven.Client.Linq;
using Rhino.Mocks;
using Raven.Client.Document;
using System.Configuration;
using System.Collections;

namespace PublicQuestions.UnitTests
{
    [TestFixture]
    public class SearchQuestions
    {
        MockRepository _mocks = new MockRepository();
        IDocumentSession _session = null;
        IRavenQueryable<Question> _mockQueryResults = null;
        private Question _mockrecord = new Question(); // single result
        private Question[] _mockrecords = new Question[2]; // Array of results

        [TestFixtureSetUp]
        public void Setup()
        {
            _mockrecord = new Question() { Id = 2, Name = "Mock User", Body = "Mock body", EMail = "mock.user@gmail.com", Posted = DateTime.Now, Title = "Title" };
            _mockrecords = new Question[] { _mockrecord, _mockrecord };
            _session = _mocks.StrictMock<IDocumentSession>();
        }

        [TestFixtureTearDown]
        public void ShutDown()
        { 
        
        }

        [Test(Description = "Search for a question in the database")]
        [TestCase(2, Description = "Simple search for one record")]
        public void Model_Questions_SearchForQuestion(int Id)
        {
            using (_mocks.Record())
            {
                Expect.Call(_session.Load<Question>(Id.ToString()))
                    .IgnoreArguments()
                    .Return(_mockrecord); 
            }

            using (_mocks.Playback())
            {
                QuestionRepository questionRepository = new QuestionRepository(_session);
                Question question = questionRepository.GetQuestion(Id);
                Assert.IsTrue(question != null, "Question should have been found!");
                Assert.IsTrue(question.Title == _mockrecord.Title, "Question Title should have been {0} but was {1}!", _mockrecord.Title, question.Title);
                Assert.IsTrue(question.Id == _mockrecord.Id, "Question Title should have been {0} but was {1}!", _mockrecord.Title, question.Title);
            }
        }

        [Test(Description = "Search for a question by Id but find nothing")]
        [TestCase(3, Description = "Simple search for one record, but exepct nothing")]
        public void Model_Questions_SearchForQuestionbutFindNone(int Id)
        {
            using (_mocks.Record())
            {
                Expect.Call(_session.Load<Question>(Id.ToString()))
                    .IgnoreArguments()
                    .Return(null);
            }

            using (_mocks.Playback())
            {
                QuestionRepository questionRepository = new QuestionRepository(_session);
                Question question = questionRepository.GetQuestion(Id);
                Assert.IsTrue(question == null, "Question should not have been found!");
            }
        }

        [Test(Description = "Search for all question")]
        public void Model_Questions_SearchForAllQuestion()
        {
            using (_mocks.Record())
            {
                IRavenQueryable<Question> stub = MockRepository.GenerateStub<IRavenQueryable<Question>>();
                MockResults mockResults = new MockResults();
                var result = (from q in _mockrecords select q);
                mockResults.Query = (from q in _mockrecords select q).AsQueryable<Question>();
                
                Expect.Call(_session.Query<Question>())
                    .IgnoreArguments()
                    .Return(mockResults);
            }

            using (_mocks.Playback())
            {
                QuestionRepository questionRepository = new QuestionRepository(_session);
                IRavenQueryable<Question> questions = questionRepository.GetQuestions();
                Assert.IsTrue(questions.Count() == _mockrecords.Count(), "Count of questions do not match!");
            }
            _mocks.ReplayAll();
        }


        private class MockResults : IRavenQueryable<Question>
        {
            public IQueryable<Question> Query { get; set; }
            public IRavenQueryable<Question> Customize(Action<IDocumentQueryCustomization> action)
            {   return this;   }

            public IRavenQueryable<Question> Statistics(out RavenQueryStatistics stats)
            {   stats = null; return this;  }

            public IEnumerator<Question> GetEnumerator()
            {  return Query.GetEnumerator(); }

            IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {   return Query.GetEnumerator(); }

            public Type ElementType
            { get { return Query.GetType(); } }

            public System.Linq.Expressions.Expression Expression
            {   get { return Query.Expression; }  }

            public IQueryProvider Provider
            {   get { return Query.Provider; }  }
        }

    }
}
