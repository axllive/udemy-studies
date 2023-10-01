//mongoose site connection example to node.js
/* const mongoose = require('mongoose');
const dotenv = require('dotenv');
dotenv.config();

//conexão no banco
mongoose.connect(
    process.env.MONGO_URI, {useNewUrlParser : true}
).then ( () => console.log('DB Connected') );

mongoose.connection.on('error', err => {
    console.log(`fail on db ${err.message}`);
}); */

//udemy course example to connection
const express = require('express');
const app = express();
const morgan = require('morgan');
const dotenv = require('dotenv');
const mongoose = require('mongoose');
const bodyParser = require('body-parser');
const expressValidator = require('express-validator');
const port = 3000;

dotenv.config();

mongoose
    .connect(
        process.env.MONGO_URI,
        {useNewUrlParser: true}
    )
    .then( () => { console.log('sucess on connection to db'); } );

mongoose.connection.on('error', err => {
    console.log(`db connection error: ${err.message}`);
});

const controller = require("./routes/post");

//middleware
app.use(morgan('dev'));
app.use(bodyParser.json());
app.use(expressValidator());
//app.use(myOwnMiddleware);

app.get("/", controller);
app.get("/all", controller);
app.post("/post/", controller);
app.get("/get/", controller);

app.listen(port, () => {console.log(`I'm listening on port: ${port}.`)});