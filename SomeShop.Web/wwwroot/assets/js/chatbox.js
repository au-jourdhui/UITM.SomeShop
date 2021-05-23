(() => {
    const chatbox = jQuery;

    chatbox(() => {
        const onScroll = () => {
            const scroll = window.scrollY;
            const height = 84 - window.scrollY;
            chatbox(".chatbox-panel").css({bottom: -scroll + "px", top: scroll + "px"});
            chatbox(".chatbox-popup").css({bottom: height + "px"});
        }
        document.addEventListener('scroll', onScroll, true);

        chatbox(".chatbox-open").click(() => {
                onScroll()
                chatbox(".chatbox-popup, .chatbox-close").fadeIn()
            }
        );

        chatbox(".chatbox-close").click(() => {
                onScroll()
                chatbox(".chatbox-popup, .chatbox-close").fadeOut()
            }
        );

        chatbox(".chatbox-maximize").click(() => {
            onScroll()
            chatbox(".chatbox-popup, .chatbox-open, .chatbox-close").fadeOut();
            chatbox(".chatbox-panel").fadeIn();
            chatbox(".chatbox-panel").css({display: "flex"});
        });

        chatbox(".chatbox-minimize").click(() => {
            onScroll()
            chatbox(".chatbox-panel").fadeOut();
            chatbox(".chatbox-popup, .chatbox-open, .chatbox-close").fadeIn();
        });

        chatbox(".chatbox-panel-close").click(() => {
            onScroll()
            chatbox(".chatbox-panel").fadeOut();
            chatbox(".chatbox-open").fadeIn();
        });
    });

})()