$(document).ready(function () {
    // Defining a connection to the server hub.
    var myHub = $.connection.myHub;
    // Setting logging to true so that we can see whats happening in the browser console log. [OPTIONAL]
    $.connection.hub.logging = true;
    // Start the hub
    $.connection.hub.start();

    // This is the client method which is being called inside the MyHub constructor method every 3 seconds
    myHub.client.SendServerTime = function (serverTime) {
        // Set the received serverTime in the span to show in browser
        $("#newTime").text(serverTime);
    };

    // Client method to broadcast the message
    myHub.client.hello = function (message) {
      //  alert("něco se děje U");

        jQuery.ajax({
            type: "POST",
            url: '/Chat/RestoreChat',
            data: { name: "" },
            success: function (result) {
                //alert("pass2");
               // alert(result);
                $("#chatMain").html(result);
                $("#chatText").html('');
            }
        });

       // $("#chatMain").append("<p>" + message + "</p>");
    };

    //Button click jquery handler
    jQuery(document).on('click', '#btnClick22', function () {
        // Call SignalR hub method
        //alert("pass1");
        var text = jQuery("#chatText").val();
        var name = jQuery("#chatText").attr("data-test");

       

        myHub.server.helloServer(name, text);
    });


}());
