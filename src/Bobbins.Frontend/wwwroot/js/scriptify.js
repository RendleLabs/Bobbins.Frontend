$(() => {
    
    $(document).on("click", "a.scriptable", e => {
        e.preventDefault();
        const url = $(e.currentTarget).attr("href");
        $.ajax({
            url: `${url}?scripted=true`,
            method: "PUT"
        });
    });
    
});