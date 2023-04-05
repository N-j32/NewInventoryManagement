//  to check for session expiration in milliseconds 
var session_pollInterval = 60000;
// How many minutes the session is valid for
    var sess_expirationMinutes = 2;
//How many minutes before the warning prompt 
var session_warningMinutes = 1;
var session_intervalID;
var session_lastActivity;

//To initializes the session time when the page loads.

function initSession() {
    sess_lastActivity = new Date();
    sessSetInterval();
    $(document).bind('keypress.session', function (ed, e) {
        sessKeyPressed(ed, e);

    });
}
// to set session interval 

function sessSetInterval() {
    sess_intervalID = setInterval('sessInterval()', sess_pollInterval);
}
function sessClearInterval() {
    clearInterval(sess_intervalID);

}
function sessKeyPressed(ed, e) {
    sess_lastActivity = new Date();
}
function sessLogOut() {
    window.location.href = 'C:\Users\EI13056\Desktop\SPE\net core\LoginDemo\LoginDemo\Views\Home\Index.cshtml';
}
// to calculate session interval and to show a message whenever a session times out.
function sessInterval() {
    var now = new Date();
    //get milliseconds of differences
    var diff = now - session_lastActivity;
    //get minutes between differences

    var diffMins = (diff / 1000 / 60);
    if (diffMins >= session_warningMinutes) {
        //warn before expiring

        //stop the timer
        sessClearInterval();
        //prompt for attention
        var active = confirm('Your session will expire in ' + (session_expirationMinutes - session_warningMinutes) +
            ' minutes (as of ' + now.toTimeString() + '), press OK to remain logged in ' +
            'or press Cancel to log off. \nIf you are logged off any changes will be lost.');
        if (active == true) {
            now = new Date();

            diff = now - session_lastActivity;
            diffMins = (diff / 1000 / 60);
            if (diffMins > session_expirationMinutes) {
                sessLogOut();
            }
            else {
                initSession();
                sessSetInterval();
                session_lastActivity = new Date();
            }
        }
        else {
            sessLogOut();
        }
    }
}