(function(){
    const findLastComment = (el) => {
        let last = null;
        for(let i = 0; i < el.childNodes.length; i++) {
            let node = el.childNodes[i];
            if(node.nodeType === 8) {
                last = node;
            }
        }
        return last;
    };
    const removeToEnd = (node) => {
        let next = node.nextSibling;
        if(!next) {
            return;
        }

        removeToEnd(next);

        next.remove();
    }


    (() => {
        let resilienceCounter = 25;
        const timer = setInterval(() => {
            let comment = findLastComment(document.body);
            if(comment.textContent === "SCRIPT GENERATED BY SERVER! PLEASE REMOVE") {
                resilienceCounter--;
                removeToEnd(comment);
            }
            if(resilienceCounter === 0) {
                clearInterval(timer);
            }
        }, 20);
    })();
})();