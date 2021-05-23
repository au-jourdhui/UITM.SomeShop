(async () => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .configureLogging(signalR.LogLevel.Trace)
        .build();

    connection.on("Receive", function (message, chatId) {
        console.info(message, chatId);
    });

    try {
        await connection.start();
        console.info("Connected to Chat Hub!");
        await connection.send("Register", "+1234567890", "Phone");
        await connection.send("Send", "Some message");
    } catch (error) {
        console.error(error);
    }
    
})();
