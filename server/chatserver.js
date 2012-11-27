var http = require('http'),
url = require('url'),
fs = require('fs'),
server


server = http.createServer(function(req, res){

	var path = url.parse(req.url).pathname;

	switch(path){
		
		case '/':
		fs.readFile(__dirname + '/chatclient.html', function(err, data){
			if(err) return send404(res);
				res.writeHead(200, {'Content-Type': 'text/html'});
				res.write(data, 'utf8');
				res.end();
		});
		break;

		default: send404(res);
	}
}),

send404 = function(res){
	res.writeHead(404);
	res.write('404');
	res.end();
};

server.listen(8080, "172.16.0.108");
console.log('Server Running at http://127.0.0.1:8080/');

var io = require('socket.io').listen(server);
var usersArray = new Array();

//on 'connection' event
io.sockets.on('connection', function(socket){

	console.log("Connection "+ socket.id + " accepted.");

	socket.on('disconnect', function(){
		console.log("Connection "+ socket.id + "  terminated");

	});

	socket.on('message', function(message){
		console.log("Received Message: " + message.messsage + " - from client " + message.username);
		//Send the messages to all connected clients.
		io.sockets.emit("chat", socket.id , message);
	});

	socket.on('username', function(username){
		console.log("THE USERNAME IS " + username + " ON SOCKET ID" + socket.id);
		users(socket.id, username);
		//Send the messages to all connected clients.
		io.sockets.emit('username', socket.id , {"user": username, "online" : usersArray });
	})

	function users(socketId, username)
	{
		console.log("THERE ARE "+ usersArray.length + "items.");
		var user = new Object();
		user.socketid = socketId;
		user.username = username;

		usersArray.push(user);
		console.log(users);
	}

});



//});