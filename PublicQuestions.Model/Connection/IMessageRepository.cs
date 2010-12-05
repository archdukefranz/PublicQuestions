using System;
namespace PublicQuestions.Model.Connection
{
    public interface IMessageRepository
    {
        void Delete(string id);
        System.Collections.Generic.IList<PublicQuestions.Model.Message> GetMessages();
        PublicQuestions.Model.Message Load(string id);
        void Save(PublicQuestions.Model.Message message);
    }
}
