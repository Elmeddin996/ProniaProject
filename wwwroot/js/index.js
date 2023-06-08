$(document).on("click", ".modal-btn", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");

    fetch(url)
        .then(response => {

            if (response.ok) {
                return response.text()
            }
            else {
                alert("Error!")
            }
        })
        .then(modaldata => {
            $("#quickModal .modal-dialog").html(modaldata)
            $("#quickModal").modal('show');
        })
})



























$(document).on("click", ".addtobasket", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url)
        .then(response => {
            if (!response.ok) {
                alert("Error")
            }
            return response.text()
        })
        .then(data => {
            $(".cart-block").html(data)
        })
})

$(document).on("click", ".removefrombasket", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url)
        .then(response => {
            if (!response.ok) {
                alert("Error")
                return
            }
            return response.text()
        })
        .then(data => {
            $(".cart-block").html(data)
        })
})