var app = require('../services/appservice');
var newApp = app.getApp();

newApp.get('/', function (req, res) {
    res.send('Hello World!');
});
