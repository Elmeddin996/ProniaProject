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



$(document).on("click", ".addtocart", function (e) {
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
            $(".cart").html(data)
        })
})

$(document).on('click', '.button-close', function (e) {
    var dom = $('.main-wrapper').children();
    e.preventDefault();
    var $this = $(this);
    $this.parents('.open').removeClass('open');
    dom.find('.global-overlay').removeClass('overlay-open');
});


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
            $(".minicart-product").html(data)
        })
    
})


