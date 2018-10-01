
$(function() {

    $("#reload-contacts").unbind('click').bind('click', function(e) {
        e.preventDefault();
        loadContacts();
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



    $('#contacts-file').on('change', function(evt) {
        var file = this.files[0];
        $("#label-file").html(file.name);
        var reader = new FileReader();


        var files = evt.target.files;
        f = files[0];
        var reader = new FileReader();
        reader.onload = (function(theFile) {
            return function(e) {
                var text = e.target.result;
                jQuery( '#ms_word_filtered_html' ).val( e.target.result );
            };
        })(f);
        reader.readAsText(f);

        //if (file.size > 1024) {
        //    alert('max upload size is 1k')
        //}
        // Also see .name, .type
    });
    
    $("#upload-file-contacts").unbind('click').bind('click', function() {

        $(".custom-file").dmUploader("start");

    })
    
    loadEvents();
    $( document ).ajaxComplete(function() {
        loadEvents();
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

    $(".btn-edit-contact").unbind('click').bind('click', function() {
        $.post('/Home/Edit', { id: $(this).attr("data-obj") } )
            .done(function(data) {
                $("#edit-contact-content").html(data);
            }).fail(function(e) {
                toastr.error('Something was wrong, look at this ' + JSON.stringify( e ), 'Inconceivable!');
            }).always(function() {
                console.log( "finished" );
            })
    })

    setTimeout(function() {
        setIframeHeight("apidociframe");
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