let connection;
(async () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .configureLogging(signalR.LogLevel.Trace)
        .build();

    try {
        await connection.start();
        console.info("Connected to Chat Hub!");
    } catch (error) {
        console.error(error);
    }

    try {
        const userInfo = JSON.parse(window.localStorage.getItem('userInfo'));
        await registerIntoHub(userInfo.email, userInfo.name);
    } catch {
        window.localStorage.removeItem('userInfo');
    }
})();

const registerIntoHub = async (email, name) => {
    await connection.send("Register", email, "Email", name);
}

const sendIntoHub = async (message) => {
    await connection.send("Send", message);
}