using NUnit.Framework;
using System.IO;

namespace Test
{
    public class Test
    {
        [Test]
        public void ClientMessageTest()
        {
            var sample = new App.Message.ClientMessage { Data = new App.Data.SampleData { Id = 0, Name = "Client" } };

            byte[] serialized;

            using (var ms = new MemoryStream())
            using (var codedstream = new Google.Protobuf.CodedOutputStream(ms, true))
            {
                sample.WriteTo(codedstream);
                codedstream.Flush();

                serialized = ms.ToArray();
            }

            var deserialized = App.Message.ClientMessage.Parser.ParseFrom(serialized);

            Assert.AreEqual(sample.Data.Id, deserialized.Data.Id);
            Assert.AreEqual(sample.Data.Name, deserialized.Data.Name);
        }

        [Test]
        public void ServerMessageTest()
        {
            var sample = new App.Message.ServerMessage { Data = new App.Data.SampleData { Id = 1, Name = "Server" } };

            byte[] serialized;

            using (var ms = new MemoryStream())
            using (var codedstream = new Google.Protobuf.CodedOutputStream(ms, true))
            {
                sample.WriteTo(codedstream);
                codedstream.Flush();

                serialized = ms.ToArray();
            }

            var deserialized = App.Message.ServerMessage.Parser.ParseFrom(serialized);

            Assert.AreEqual(sample.Data.Id, deserialized.Data.Id);
            Assert.AreEqual(sample.Data.Name, deserialized.Data.Name);
        }
    }
}
