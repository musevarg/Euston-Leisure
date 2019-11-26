using System;
using Coursework1.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Coursework1_UnitTest
{
    [TestClass]
    public class MessageTest
    {
        //Check whether valid SMS is processed correctly
        [TestMethod]
        public void ProcessSmsTest()
        {
            Message sms1 = new Message()
            {
                Type = "SMS",
                ID = "S123456789",
                Sender = "+443339006040",
                Body = "The message LOL <Laughing out loud>."
            };

            ProcessData pd = new ProcessData();
            Message sms2 = pd.SanitizeSMS("S123456789", "+443339006040 The message LOL.");

            Assert.AreEqual(sms1.ID, sms2.ID);
            Assert.AreEqual(sms1.Type, sms2.Type);
            Assert.AreEqual(sms1.Sender, sms2.Sender);
            Assert.AreEqual(sms1.Body, sms2.Body);

        }

        //Check whether valid Tweet is processed correctly
        [TestMethod]
        public void ProcessTweetTest()
        {
            Message tweet1 = new Message()
            {
                Type = "Tweet",
                ID = "T123456789",
                Sender = "@tweetId",
                Body = "IMO <In my opinion>, impressive. #Awesome @Napier"
            };

            ProcessData pd = new ProcessData();
            Message tweet2 = pd.SanitizeTweet("T123456789", "@tweetId IMO, impressive. #Awesome @Napier");

            Assert.AreEqual(tweet1.ID, tweet2.ID);
            Assert.AreEqual(tweet1.Type, tweet2.Type);
            Assert.AreEqual(tweet1.Sender, tweet2.Sender);
            Assert.AreEqual(tweet1.Body, tweet2.Body);

        }

        //Check whether valid Email is processed correctly
        [TestMethod]
        public void ProcessEmailTest()
        {
            Message email1 = new Message()
            {
                Type = "Email",
                ID = "E123456789",
                Sender = "test@napier.ac.uk",
                Subject = "Email subject",
                Body = "This is the body of the email, <URL Quarantined>"
            };

            ProcessData pd = new ProcessData();
            Message email2 = pd.SanitizeEmail("E123456789", "test@napier.ac.uk Email subject        This is the body of the email, www.example.com");

            Assert.AreEqual(email1.ID, email2.ID);
            Assert.AreEqual(email1.Type, email2.Type);
            Assert.AreEqual(email1.Sender, email2.Sender);
            Assert.AreEqual(email1.Subject, email2.Subject);
            Assert.AreEqual(email1.Body, email2.Body);
        }

        //Email with valid data
        [TestMethod]
        public void ValidEmailMessage()
        {
            bool expected = true;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidEmail("test@napier.ac.uk Email subject        This is the body of the email, www.example.com"));
        }

        //Email with invalid email address
        [TestMethod]
        public void InvalidEmailAdress1()
        {
            bool expected = false;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidEmail("test@napier Email subject        This is the body of the email, www.example.com"));
        }

        //Email with invalid email address
        [TestMethod]
        public void InvalidEmailAdress2()
        {
            bool expected = false;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidEmail("testatnapier.ac.uk Email subject        This is the body of the email, www.example.com"));
        }

        //Email with body longer than 1028 characters
        [TestMethod]
        public void InvalidEmailBody()
        {
            bool expected = false;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidEmail("test@test.com test subject         A protoplanet is a large planetary embryo that originated within a protoplanetary disc and has undergone internal melting to produce a differentiated interior. Protoplanets are thought to form out of kilometer-sized planetesimals that gravitationally perturb each other's orbits and collide, gradually coalescing into the dominant planets. In the case of the Solar System, it is thought that the collisions of planetesimals created a few hundred planetary embryos. Such embryos were similar to Ceres and Pluto with masses of about 1022 to 1023 kg and were a few thousand kilometers in diameter. Over the course of hundreds of millions of years, they collided with one another. The exact sequence whereby planetary embryos collided to assemble the planets is not known, but it is thought that initial collisions would have replaced the first generation of embryos with a second generation consisting of fewer but larger embryos. These in their turn would have collided to create a third generation of fewer but even larger embryos. Eventually, only a handful of embryos were left, which collided to complete the assembly of the planets proper."));
        }

        //Twitter message with valid data
        [TestMethod]
        public void ValidTwitterMessage()
        {
            bool expected = true;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidTweet("@tweetId IMO, impressive. #Awesome @Napier"));
        }

        //Twitter message with invalid twitter ID
        [TestMethod]
        public void InvalidTwitterId1()
        {
            bool expected = false;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidTweet("John421 I'm happy I have lived long enough to see that! This is for you @JeanRochefort #ELM"));
        }

        //Twitter message with twitter ID longer than 15 characters
        [TestMethod]
        public void InvalidTwitterId2()
        {
            bool expected = false;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidTweet("@John421John4211 I'm happy I have lived long enough to see that! This is for you @JeanRochefort #ELM"));
        }

        //Twitter message with more than 140 characters
        [TestMethod]
        public void InvalidTwitterMessage()
        {
            bool expected = false;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidTweet("@John421 I'm happy I have lived long enough to see that! I'm happy I have lived long enough to see that! I'm happy I have lived long enough to see that!"));
        }

        //SMS message with valid data
        [TestMethod]
        public void ValidSMSMessage()
        {
            bool expected = true;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidSMS("+443339006040 The message LOL."));
        }

        //SMS with invalid phone number
        [TestMethod]
        public void InvalidPhoneNumber()
        {
            bool expected = false;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidSMS("3339006040 IMO this app is amazing. Good work."));
        }

        //SMS with message body longer than 140 characters
        [TestMethod]
        public void InvalidSMSMessage()
        {
            bool expected = false;
            ProcessData pd = new ProcessData();
            Assert.AreEqual(expected, pd.ValidSMS("+443339006040 IMO this app is amazing. Good work. A protoplanet is a large planetary embryo that originated within a protoplanetary disc and has undergone internal melting."));
        }
    }
}
