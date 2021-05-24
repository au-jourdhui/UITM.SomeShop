(() => {
    const chatbox = jQuery;

    chatbox(() => {
        chatbox(".chatbox-open").click(async () => {
                let userInfo;
                try {
                    userInfo = JSON.parse(window.localStorage.getItem('userInfo'));
                } catch {
                    window.localStorage.removeItem('userInfo');
                }
                if (!userInfo || !userInfo.name || !userInfo.email) {
                    const {value: email} = await Swal.fire({
                        title: 'Enter email',
                        input: 'email',
                        inputLabel: 'Your email address',
                        inputPlaceholder: 'Enter your email address',
                        inputValue: userInfo?.email,
                    });
                    if (!email) {
                        return;
                    }

                    const {value: name} = await Swal.fire({
                        title: 'Enter name',
                        input: 'text',
                        inputLabel: 'Your name',
                        inputPlaceholder: 'Enter your name',
                        inputValue: userInfo?.name,
                    });

                    window.localStorage.setItem('userInfo', JSON.stringify({name, email}));
                    await registerIntoHub(email, name);
                }

                chatbox(".chatbox-popup, .chatbox-close").fadeIn()
            }
        );

        chatbox(".chatbox-close").click(() => {
                chatbox(".chatbox-popup, .chatbox-close").fadeOut()
            }
        );

        chatbox(".chatbox-maximize").click(() => {
            chatbox(".chatbox-popup, .chatbox-open, .chatbox-close").fadeOut();
            chatbox(".chatbox-panel").fadeIn();
            chatbox(".chatbox-panel").css({display: "flex"});
        });

        chatbox(".chatbox-minimize").click(() => {
            chatbox(".chatbox-panel").fadeOut();
            chatbox(".chatbox-popup, .chatbox-open, .chatbox-close").fadeIn();
        });

        chatbox(".chatbox-panel-close").click(() => {
            chatbox(".chatbox-panel").fadeOut();
            chatbox(".chatbox-open").fadeIn();
        });
        
        const textBox = chatbox(".text-to-chat-hub");
        const chatBody = chatbox(".chatbox-popup__main, .chatbox-panel__main");
        
        textBox.keyup(e => {
            if(e.keyCode === 13)
                textBox.trigger("enterKey");
        });
        
        let isFirst = true;
        const send = async () => {
            const message = textBox.val().trim() || textBox[1].value.trim();
            
            if (!message) {
                return;
            }
            
            if (isFirst) {
                chatBody.find(".chatbox-start-text").remove();
                isFirst = false;
            }
            
            chatBody.append(`<p class='text-right'>${message}: <b>You</b></p>`);
            await sendIntoHub(message);
        };
        chatbox(".send-to-chat-hub").click(send);
        textBox.bind("enterKey", send);
        textBox.bind("enterKey", () => textBox.val(null));

        connection.on("Receive", function (message, chatId, name) {
            chatBody.append(`<p class='text-left'><b class="text-primary">Operator</b>: ${message}</p>`);
        });
    });

})()