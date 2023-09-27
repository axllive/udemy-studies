/* const helpers = require("./helpers.js"); */
const {sum} = require("./helpers.js");
//import package http
const http = require("http");

//variables types: var, let and const

/* const total = helpers.sum(1,2); */
const total = sum(1,2);

//creating http server std method
const server = http.createServer( (req, res) => {
    res.end("Hello world from node.js nodemonns");
} );

server.listen(3000);

console.log("Total is: " + total);