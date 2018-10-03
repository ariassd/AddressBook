
$(function() {

    $("#reload-contacts").unbind('click').bind('click', function(e) {
        e.preventDefault();
        loadContacts();
    });

    $("#btn-add-new-contact").unbind('click').bind('click', function() {
        $(".div-add-new-contact").html('')
        $("#edit-contact-content").html('');
        $.get("/Home/ContactForm", function(data) {
            $(".div-add-new-contact").html(data)
        });

    });

    $("#save-contact").unbind('click').bind('click', function() {
        $.post('/Home/AddContact', $("#add-new-contact-values").serialize())
            .done(function(data) {
                console.log( data );
                if ( data.Result == "OK" ) {
                    toastr.success('The contact was created!', 'Nice job');
                    $("#add-new-contact-values")[0].reset();
                    $("#add-new-contact").modal()
                    loadContacts();
                } else {
                    toastr.error('Something was wrong, look at this ' + data.Message, 'Inconceivable!');
                }
            }).fail(function(e) {
                toastr.error('Something was wrong, look at this ' + e, 'Inconceivable!');
            }).always(function() {
                console.log( "finished" );
            });
    })

    loadEvents();
    $( document ).ajaxComplete(function() {
        loadEvents();
    });

    $('.dropbox').filedrop({
        callback: function (fileEncryptedData, fileName) {
            var url = "/Home/UploadFile";
            toastr.success("We are processing your file", 'Well ...');
            $.post(url, { "fileName": fileName, "fileContent": fileEncryptedData.split("base64,")[1] }, function (data) {
                if (data.Result == "OK") {
                    toastr.success(data.Message, 'Nice job');
                    loadContacts();
                } else {
                    toastr.error('Something was wrong, look at this ' + data.Message, 'Inconceivable!');
                }
            });
        }
    });

    

})


function loadEvents() {

    $("#save-contact-changes").unbind('click').bind('click', function() {
        $.post('/Home/EditContact', $("#edit-contact-values").serialize())
            .done(function(data) {
                //console.log( data );
                if ( data.Result == "OK" ) {
                    toastr.success('The contact was modified!', 'Nice job');
                    loadContacts();
                } else {
                    toastr.error('Something was wrong, look at this ' + data.Message, 'Inconceivable!');
                }
            }).fail(function(e) {
                toastr.error('Something was wrong, look at this ' + e, 'Inconceivable!');
            }).always(function() {
                console.log( "finished" );
            });
    })

    $(".btn-delete-contact").unbind('click').bind('click', function() {
        $.post('/Home/Delete', { id: $(this).attr("data-obj") } )
            .done(function(data) {
                console.log( data );
                if ( data.Result == "OK" ) {
                    toastr.success('The contact was deleted!', 'Ready!');
                    loadContacts();
                } else {
                    toastr.error('Something was wrong, look at this ' + data.Message, 'Inconceivable!');
                }
            }).fail(function(e) {
                toastr.error('Something was wrong, look at this ' + e, 'Inconceivable!');
            }).always(function() {
                console.log( "finished" );
            });
    })

    $(".btn-edit-contact").unbind('click').bind('click', function () {
        $(".div-add-new-contact").html('')
        $("#edit-contact-content").html('');
        $.post('/Home/Edit', { id: $(this).attr("data-obj") })
            .done(function (data) {
                $("#edit-contact-content").html(data);
            }).fail(function (e) {
                toastr.error('Something was wrong, look at this ' + JSON.stringify(e), 'Inconceivable!');
            }).always(function () {
                console.log("finished");
            })
    });

    $("#add-email-field").unbind("click").bind("click", function (e) {
        e.preventDefault();
        if ($(".email-field").length < 3) {
            var newField = $(".email-field").first().clone();
            newField.val('');
            $(".email-field").closest("div.form-group").append(newField);
        }
        else {
            toastr.error('You reach the maximun of fields', 'Ooops!');
        }
    });

    $("#add-phone-field").unbind("click").bind("click", function (e) {
        e.preventDefault();
        if ($(".phone-field").length < 3) {
            var newField = $(".phone-field").first().clone();
            newField.val('');
            $(".phone-field").closest("div.form-group").append(newField);
        } else {
            toastr.error('You reach the maximun of fields', 'Ooops!');
        }
    });

    setTimeout(function () {
        if ($("#apidociframe").length >= 1) {
            setIframeHeight("apidociframe");
        }
    }, 2000);




}

function loadContacts() {
    $.get("/Home/GetList").done(function(data) {
        console.log( data );
        $("#contacts-room").html(data);

    }).fail(function(e) {
        toastr.error('Something was wrong, look at this ' + e, 'Inconceivable!');
    }).always(function() {
        console.log( "finished" );
    });
}

function getDocHeight(doc) {
    doc = doc || document;
    var body = doc.body, html = doc.documentElement;
    var height = Math.max( body.scrollHeight, body.offsetHeight, 
        html.clientHeight, html.scrollHeight, html.offsetHeight );
    return height;
}

function setIframeHeight(id) {
    var ifrm = document.getElementById(id);
    var doc = ifrm.contentDocument? ifrm.contentDocument: 
        ifrm.contentWindow.document;
    ifrm.style.visibility = 'hidden';
    ifrm.style.height = "10px"; 
    ifrm.style.height = getDocHeight( doc ) + 4 + "px";
    ifrm.style.visibility = 'visible';
}