var express = require('express');
var app = express();

module.exports = {
    init: function () {
        app.listen(3000, function () {
            console.log('App listening on port 3000!');
        });
    },
    get: function () {
        return app;
    }
};