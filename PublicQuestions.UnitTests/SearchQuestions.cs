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
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType);

        MockRepository _mocks = new MockRepository();
        IDocumentSession _session = null;
        IRavenQueryable<Question> _mockQueryResults = null;
        private Question _mockrecord = new Question(); // single result
        private Question[] _mockrecords = new Question[2]; // Array of results

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log.Info("Starting up for testing");
        }

        [SetUp]
        public void Setup()
        {
            _mockrecord = new Question() { Id = 2, Name = "Mock User", Body = "Mock body", EMail = "mock.user@gmail.com", Posted = DateTime.Now, Title = "Title" };
            _mockrecords = new Question[] { _mockrecord, _mockrecord };
            _session = _mocks.StrictMock<IDocumentSession>();
        }

        [TestFixtureTearDown]
        public void ShutDown()
        {
            _log.Info("Shutting down the tests");
        }

        /// <summary>
        /// Search for question and find one.
        /// </summary>
        /// <param name="Id">The id.</param>
        [Test(Description = "Search for a question in the database")]
        [TestCase(1, Description = "Simple search for one record")]
        [TestCase(2, Description = "Simple search for one record")]
        public void Model_Questions_SearchForQuestion(int Id)
        {
            using (_mocks.Record())
            {
                Expect.Call(_session.Load<Question>(Id.ToString()))
                    .IgnoreArguments()
                    .Return(_mockrecord); 
            }
            _mocks.ReplayAll();

            using (_mocks.Playback())
            {
                QuestionRepository questionRepository = new QuestionRepository(_session);
                Question question = questionRepository.GetQuestion(Id);
                Assert.IsTrue(question != null, "Question should have been found!");
                Assert.IsTrue(question.Title == _mockrecord.Title, "Question Title should have been {0} but was {1}!", _mockrecord.Title, question.Title);
                Assert.IsTrue(question.Id == _mockrecord.Id, "Question Title should have been {0} but was {1}!", _mockrecord.Title, question.Title);
            }
            _mocks.VerifyAll();
        }

        /// <summary>
        /// Search for questionbut find none.
        /// </summary>
        /// <param name="Id">The id of the question</param>
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
            _mocks.ReplayAll();

            using (_mocks.Playback())
            {
                QuestionRepository questionRepository = new QuestionRepository(_session);
                Question question = questionRepository.GetQuestion(Id);
                Assert.IsTrue(question == null, "Question should not have been found!");
            }
            _mocks.VerifyAll();
        }

        /// <summary>
        /// Search for all question and ensure they are returned.
        /// </summary>
        [Test(Description = "Search for all question")]
        public void Model_Questions_SearchForAllQuestion()
        {
            using (_mocks.Record())
            {
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
            _mocks.VerifyAll();
        }

        [Test(Description = "View a question, counter should increase")]
        public void Model_Questions_ViewAnswerVoteQuestion()
        {
            using (_mocks.Record())
            {
                Expect.Call(_session.Load<Question>("1"))
                    .IgnoreArguments()
                    .Return(_mockrecord);
                Expect.Call(delegate{ _session.Store(_mockrecord); })
                    .IgnoreArguments()
                    .Repeat.Times(3);
                Expect.Call(delegate { _session.SaveChanges(); })
                    .IgnoreArguments()
                    .Repeat.Times(3);
            }
            _mocks.ReplayAll();

            using (_mocks.Playback())
            {
                QuestionRepository questionRepository = new QuestionRepository(_session);
                Question question = questionRepository.GetQuestion(1);
                int previousView = question.Attributes.Views;
                int previousVotes = question.Attributes.Votes;
                int previousAnswers = question.Attributes.Answers;
                question.View();
                Assert.IsTrue(question.Attributes.Views == (previousView + 1), "the count of views is incorrect!");
                question.Answer(Answer);
                Assert.IsTrue(question.Attributes.Views == (previousAnswers + 1), "the count of answers is incorrect!");
                question.Vote(Answer);
                Assert.IsTrue(question.Attributes.Vote == (previousVotes + 1), "the count of answers is incorrect!");

                questionRepository.Save(question);
            
            }
            _mocks.VerifyAll();
        }


        #region - Private classes -

        /// <summary>
        /// Mock class to allow for control of the results
        /// </summary>
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

        #endregion
    }
}
