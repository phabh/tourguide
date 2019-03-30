var app = require('../services/appservice').get();

app.get('/', function (req, res) {
    res.send('Hello World!');
});
