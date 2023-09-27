/* const helpers = require("./helpers.js"); */
const {sum} = require("./helpers.js");
//import package http
const http = require("http");

//variables types: var, let and const

/* const total = helpers.sum(1,2); */
/* const total = sum(1,2);

//creating http server std method
const server = http.createServer( (req, res) => {
    res.end("Hello world from node.js nodemonns");
} );

server.listen(3000);

console.log("Total is: " + total); */

/* const express = require('express');
const app = express();

app.get('/', (req, res) => { res.send("WHATSAPP LINDASS!?✨ 3000") } );


app.listen(3000); */

const fs = require('fs');
const fileName = "target.txt";

/* const data = fs.readFileSync(fileName);

console.log(data.toString()); */

const leitura = arquivo => new Promise((resolve, reject) =>{
     fs.readFile(arquivo, (err, data) =>  {
        if (err) {
            reject ( console.log(err) );
        }
        resolve(  console.log(data.toString()) );
    });
});
async function leu (arq){ 
    await leitura(arq); 
    console.log("async prgrmm");
}


leu(fileName);