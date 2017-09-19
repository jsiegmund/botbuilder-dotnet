﻿using Microsoft.Bot.Builder.Storage;
using Microsoft.Bot.Connector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Bot.Builder.Tests
{
    [TestClass]
    public class BotStateManagerTests
    {
        [TestMethod]
        public async Task DoNOTRememberContextState()
        {
            TestConnector connector = new TestConnector();

            Bot bot = new Bot(connector)
                .Use(new MemoryStorage())
                .Use(new BotStateManager())
                .OnReceive(
                    async (context, token) =>
                    {
                        Assert.IsNotNull(context.State, "context.state should exist");
                        switch (context.Request.Text)
                        {
                            case "set value":
                                context.State["value"] = "test";
                                context.Say("value saved");
                                break;
                            case "get value":
                                string state = context.State["value"];
                                context.Say(state);
                                break;
                        }
                        return new ReceiveResponse(true);
                    }
                );

            var runner = new TestRunner();

            await runner.Test(connector, "set value", (a) => Assert.IsTrue(a[0].Text == "value saved", "set value failed"));
            await runner.Test(connector, "get value", (a) => Assert.IsTrue(a[0].Text == null, "get value was incorrectly defined"));
        }

        [TestMethod]
        public async Task RememberUserState()
        {
            TestConnector connector = new TestConnector();

            Bot bot = new Bot(connector)
                .Use(new MemoryStorage())
                .Use(new BotStateManager())
                .OnReceive(
                    async (context, token) =>
                    {
                        Assert.IsNotNull(context.State.User, "state.user should exist");
                        switch (context.Request.Text)
                        {
                            case "set value":
                                context.State.User["value"] = "test";
                                context.Say("value saved");
                                break;
                            case "get value":
                                context.Say(context.State.User["value"]);
                                break;
                        }
                        return new ReceiveResponse(true);
                    }
                );

            var runner = new TestRunner();

            await runner.Test(connector, "set value", (a) => Assert.IsTrue(a[0].Text == "value saved"));
            await runner.Test(connector, "get value", (a) => Assert.IsTrue(a[0].Text == "test"));
        }

        [TestMethod]
        public async Task RememberConversationState()
        {
            TestConnector connector = new TestConnector();

            Bot bot = new Bot(connector)
                .Use(new MemoryStorage())
                .Use(new BotStateManager())
                .OnReceive(
                    async (context, token) =>
                    {
                        Assert.IsNotNull(context.State.Conversation, "state.conversation should exist");
                        switch (context.Request.Text)
                        {
                            case "set value":
                                context.State.Conversation["value"] = "test";
                                context.Say("value saved");
                                break;
                            case "get value":
                                context.Say(context.State.Conversation["value"]);
                                break;
                        }
                        return new ReceiveResponse(true);
                    }
                );

            var runner = new TestRunner();
            await runner.Test(connector, "set value", (a) => Assert.IsTrue(a[0].Text == "value saved"));
            await runner.Test(connector, "get value", (a) => Assert.IsTrue(a[0].Text == "test"));
        }
    }
}
