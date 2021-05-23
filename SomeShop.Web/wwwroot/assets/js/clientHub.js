const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Trace)
    .build();

hubConnection.on("Receive", function (message, chatId) {

});

connection
    .start()
    .then(() => {
        console.log("Connected to Chat Hub!");
    })
    .catch(error => console.log(error));
