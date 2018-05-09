var express = require('express');
var bodyParser = require('body-parser');
var cors = require('cors')
var Pusher = require('pusher');

var app = express();
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(cors())

// to serve our JavaScript, CSS and index.html
// app.use(express.static('./'));

var pusher = new Pusher({
  appId: '522655',
  key: 'a72e95caf97b5941ca0c',
  secret:  '834d61eb0ce5d489d543' 
});

app.post('/pusher/auth', function(req, res) {
  console.log("Inside POST to server.js")
  var socketId = req.body.socket_id;
  var channel = req.body.channel_name;
  var auth = pusher.authenticate(socketId, channel);
  res.send(auth);
});

// try listening to port 80
//var port = process.env.PORT || 8080;
var port = 8081;
// hardcore port 80 instead of 'port'
app.listen(port, () => console.log('Listening at http://localhost:8081'));